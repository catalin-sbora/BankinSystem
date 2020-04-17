using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class AccountNotFoundException: Exception
    {
        public Guid AccountId { get; private set; }
        public AccountNotFoundException(Guid id) : base($"Account with id {id.ToString()} was not found")
        {
            this.AccountId = id;
        }
    }
}
