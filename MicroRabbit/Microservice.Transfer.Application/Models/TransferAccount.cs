using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Transfer.Application.Models
{
    public  class TransferAccount
    {
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
        public decimal Amount { get; set; }
    }
}
