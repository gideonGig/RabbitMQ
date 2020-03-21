﻿using MicroRabbit.Banking.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace MicroRabbit.Banking.Application.Interfaces
{
    public  interface IAccountService
    {
        IEnumerable<Account> GetAccounts();
    }
}
