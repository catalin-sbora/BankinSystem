using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class InvalidIBANAccountException: Exception
    {
        public string IBANAccount { get; private set; }
        public InvalidIBANAccountException(string ibanAccount):base($"Invalid IBAN account format {ibanAccount}")
        {
            IBANAccount = ibanAccount;
        }
    }
}
