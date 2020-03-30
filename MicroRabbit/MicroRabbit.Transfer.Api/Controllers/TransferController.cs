using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Application.Models;
using MicroRabbit.Transfer.Domain.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace MicroRabbit.Transfer.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TransferController : ControllerBase
    {
        private readonly ITransferService _transferService;

        public TransferController(ITransferService transferservice)
        {
            _transferService = transferservice;
        }
        [HttpGet]
      
        public ActionResult<IEnumerable<TransferLog>> Get()
        {
            return Ok(_transferService.GetTransferLogs());
        }

        [HttpPost]
        public IActionResult Post([FromBody] TransferAccount transferAccount)
        {
            _transferService.Transfer(transferAccount);
            return Ok(transferAccount);
        }
    }
}
