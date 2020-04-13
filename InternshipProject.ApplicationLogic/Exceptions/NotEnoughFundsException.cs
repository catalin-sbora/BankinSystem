using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class NotEnoughFundsException:Exception
    {
        public decimal AvailableFunds { get; private set; }
        public decimal PaymentAmount { get; private set; }

        public NotEnoughFundsException(decimal availableFunds, decimal paymentAmount) : base($"Payment with amount {paymentAmount} exceeds your funds: {availableFunds}")
        {
            AvailableFunds = availableFunds;
            PaymentAmount = paymentAmount;
        }
    }
}
