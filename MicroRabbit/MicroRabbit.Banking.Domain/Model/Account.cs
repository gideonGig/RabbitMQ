﻿using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Domain.Model
{
    public class Account
    {
        public int ID { get; set; }
        public string AccountType { get; set; }
        public decimal AccountBalance { get; set; }
    }
}
