using MicroRabbit.Transfer.Application.Models;
using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interface;
using MicroRabbit.Transfer.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Data.Repository
{
    public class TransferRepository : ITransferRepository
    {
        private readonly TransferDbContext _tdx;

        public TransferRepository(TransferDbContext tdx)
        {
            _tdx = tdx;
        }

        public IEnumerable<TransferLog> GetTransferLogs()
        {
            return _tdx.TransferLogs;
        }
    }
}
