using MicroRabbit.Transfer.Domain.Model;
using System.Collections.Generic;

namespace MicroRabbit.Transfer.Domain.Interface
{
    public interface ITransferRepository
    {
        IEnumerable<TransferLog> GetTransferLogs();

    }
}
