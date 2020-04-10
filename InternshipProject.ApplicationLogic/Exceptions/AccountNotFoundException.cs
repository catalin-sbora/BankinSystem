using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class AccountNotFoundException: Exception
    {
        public string AccountId { get; private set; }
        public AccountNotFoundException(string id) : base($"Account with id {id} was not found")
        {
            this.AccountId = id;
        }
    }
}
