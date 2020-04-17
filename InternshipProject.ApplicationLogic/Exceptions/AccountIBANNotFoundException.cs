using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class AccountIBANNotFoundException : Exception
    {
        public string IBANAccount { get; private set; }
        public AccountIBANNotFoundException(string ibanAccount) : base($"No customer with such IBAN exists {ibanAccount}")
        {
            IBANAccount = ibanAccount;
        }
    }
}
