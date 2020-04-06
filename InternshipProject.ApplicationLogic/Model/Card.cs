using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class Card
    {
        public Guid Id { get; private set; }
        public string SerialNumber { get; private set; }
        public string CVV { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime ExpiryDate { get; private set; }
        public string OwnerName { get; private set; }
        public BankAccount BankAccount { get; private set; }

        private Card()
        { 
        }
        public static Card Create(string serialNumber, string cvv, string ownerName, BankAccount account)
        {
            if (string.IsNullOrEmpty(serialNumber))
                throw new ArgumentException("");

            if (string.IsNullOrEmpty(cvv))
                throw new ArgumentException();

            if (string.IsNullOrEmpty(ownerName))
                throw new ArgumentException();

            if (account == null)
                throw new ArgumentNullException("account");

            var card = new Card
            {
                Id = Guid.NewGuid(),
                CVV = cvv,
                SerialNumber=serialNumber,
                CreateDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddYears(4),
                OwnerName = ownerName,
                BankAccount = account
            };
            return card;
        }
    }
}
