using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace MicroRabbit.Transfer.Domain.Model
{
    public class TransferLog
    {
        public int ID { get; set; }
        public int FromAccount { get; set; }
        public int ToAccount { get; set; }
      
        [Column(TypeName = "decimal(18,10)")]
        public decimal Amount { get; set; }
    }
}
