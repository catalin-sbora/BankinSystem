using InternshipProject.ApplicationLogic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class Customer:DataEntity
    {         
        public Guid UserId { get; private set; }
        public string FirstName { get; private set; }
        public string LastName { get; private set; }
        public string SocialId { get; private set; }
        public virtual ContactDetails ContactDetails { get; private set; }
        public virtual ICollection<BankAccount> BankAccounts { get; private set; }

        protected Customer()
        { 
        }
        public static Customer Create(Guid userId, string firstName, string lastName, string socialId)
        {
            var newCustomer = new Customer() {
                Id = Guid.NewGuid(),
                FirstName = firstName,
                LastName = lastName,
                SocialId = socialId,
                UserId = userId,
                ContactDetails = new ContactDetails(),
                BankAccounts = new List<BankAccount>()
            };
            return newCustomer;
        }
        public void SetContactDetails(string address, string city, string country, string phoneNo, string altPhone, string email)
        {
            ContactDetails.Address = address;
            ContactDetails.City = city;
            ContactDetails.Country = country;
            ContactDetails.PhoneNo = phoneNo;
            ContactDetails.AlternatePhoneNo = altPhone;
            ContactDetails.Email = email;
        }

        public IEnumerable<Transaction> GetFilteredAccountTransactions(Guid accountId, string filter)
        {
            var account = BankAccounts
                    .Where(ba => accountId.Equals(ba.Id))
                    .SingleOrDefault();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            IEnumerable<Transaction> transactions;
            if (string.IsNullOrEmpty(filter))
            {
                transactions = account.Transactions;
            }
            else
            {
                filter = filter.ToLower();
                transactions = account.Transactions
                                      .Where(t =>
                                      t.Amount.ToString().Contains(filter) ||
                                      (t.ExternalIBAN != null && t.ExternalIBAN.ToLower().Contains(filter)) ||
                                      (t.ExternalName != null && t.ExternalName.ToLower().Contains(filter)) ||
                                      (t.Details != null && t.Details.ToLower().Contains(filter))
                                      );
            }
            return transactions.OrderByDescending(t => t.Time)
                               .AsEnumerable();
        }

        public BankAccount GetAccount(Guid accountId)
        {
            var account = BankAccounts.Where(ba => ba.Id == accountId)
                                      .SingleOrDefault();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            return account;
        }


    }
}
