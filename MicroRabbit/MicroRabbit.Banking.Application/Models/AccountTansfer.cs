using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Application.Models
{
    public class AccountTansfer
    {
        public int ToAccount { get; set; }
        public int FromAccount { get; set; }
        public decimal AccountAmount { get; set; }
    }
}
