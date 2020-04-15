using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class WrongCurrencyException : Exception
    {
        public string SendingCurrency { get; private set; }
        public string DestinationCurrency { get; private set; }
        public WrongCurrencyException(string sendingCurrency, string destinationCurrency) 
            : base($"Trying to send {sendingCurrency} to a {destinationCurrency} account")
        {
            SendingCurrency = sendingCurrency;
            DestinationCurrency = destinationCurrency;
        }
    }
}
