﻿using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicroRabbit.Infra.Bus
{
    public sealed class RabbitMQBus : IEventBus
    {
        private readonly IMediator _mediator;
        private readonly Dictionary<string, List<Type>> _handlers;
        private readonly List<Type> _eventTypes;
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public RabbitMQBus(IMediator mediator, IServiceScopeFactory serviceScopeFactory)
        {
             _mediator = mediator;
            _serviceScopeFactory = serviceScopeFactory;
            _handlers = new Dictionary<string, List<Type>>();
            _eventTypes = new List<Type>();
            
        }


        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public void Publish<T>(T @event) where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost" };
            using(var conn = factory.CreateConnection())
            using(var channel = conn.CreateModel())
            {
                var eventname = @event.GetType().Name;
                channel.QueueDeclare(eventname, false, false, false, null);

                var message = JsonConvert.SerializeObject(@event);
                var body = Encoding.UTF8.GetBytes(message);
                channel.BasicPublish("", eventname, null, body);

            }
        }

        public void Subscribe<T, Th>()
            where T : Event
            where Th : IEventHandler<T>
        {
            var eventname = typeof(T).Name;
            var handler = typeof(Th);

           if(! _eventTypes.Contains(typeof(T)))
           {
                _eventTypes.Add(typeof(T));
           }

           if(! _handlers.ContainsKey(eventname))
           {
                _handlers.Add(eventname, new List<Type>());
           }
          
            if(_handlers[eventname].Any(x => x.GetType() == handler))
            {
                throw new ArgumentException($"Handler Type : {handler} is already registered for event name '{eventname}' ");
            }

            _handlers[eventname].Add(handler);

            StartBasicConsume<T>();
        }

        private void StartBasicConsume<T>() where T : Event
        {
            var factory = new ConnectionFactory() { HostName = "localhost", DispatchConsumersAsync = true };
            using(var conn = factory.CreateConnection())
            using(var channel = conn.CreateModel())
            {
               var eventname = typeof(T).Name;
               channel.QueueDeclare(eventname,false,false,false,null);

                var consumer = new AsyncEventingBasicConsumer(channel);
                consumer.Received += Consumer_Received;  

                channel.BasicConsume(eventname, true, consumer);
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
        {
            var eventname = e.RoutingKey;
            var message = Encoding.UTF8.GetString(e.Body);

            try
            {
                await ProcessEvent(eventname, message).ConfigureAwait(false);
            }
            catch(Exception ex)
            {

            }
        }

        private async Task ProcessEvent(string eventname, string message)
        {
            if(_handlers.ContainsKey(eventname))
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var subscriptions = _handlers[eventname];
                    foreach (var subcription in subscriptions)
                    {
                        var handler = scope.ServiceProvider.GetService(subcription);
                        if (handler == null) continue;
                        var eventType = _eventTypes.SingleOrDefault(s => s.Name == eventname);
                        var @event = JsonConvert.DeserializeObject(message, eventType);
                        var concreteType = typeof(IEventHandler<>).MakeGenericType(eventType);
                        await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
                    }
                };



            }
        }
    }
}
