using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class CustomerService
    {
        private readonly ICustomerRepository customerRepository;
        private readonly ICardRepository cardRepository;
        private readonly ICardColorRepository cardColorRepository;
        public CustomerService(ICustomerRepository customerRepository , ICardRepository cardRepository,ICardColorRepository cardColorRepository )
        {
            this.customerRepository = customerRepository;
            this.cardRepository = cardRepository;
            this.cardColorRepository = cardColorRepository;
        }

        public Guid GetCustomerIdFromUserId(string userId)
        {
            var idToSearch = Guid.Parse(userId);
            var foundCustomer =  customerRepository?.GetCustomerByUserId(idToSearch);
            if (foundCustomer == null)
            {
                throw new CustomerNotFoundException(userId);
            }
            return foundCustomer.Id;
        }
        
        public IEnumerable<BankAccount> GetCustomerBankAccounts(Guid customerId)
        {
            var customer = customerRepository?.GetById(customerId);
            if (customer == null)
            {
                throw new CustomerNotFoundException(customerId);
            }
            
            return customer.BankAccounts
                            .AsEnumerable();           
        }
        public IEnumerable<BankAccount> GetCustomerBankAccounts(string userId)
        {
            Guid idToSearch = Guid.Empty;            
            Guid.TryParse(userId, out idToSearch);
            var customer = customerRepository?.GetCustomerByUserId(idToSearch);
            
            if (customer == null)
            {
                throw new CustomerNotFoundException(userId);
            }

            return customer.BankAccounts
                            .AsEnumerable();
        }
        public CardColor GetCardColor(Guid cardId)
        {
            return cardColorRepository.GetColor(cardId);
        }
        public Customer GetCustomer(string userId)
        {
            Guid idToSearch = Guid.Empty;
            Guid.TryParse(userId, out idToSearch);
            var customer = customerRepository?.GetCustomerByUserId(idToSearch);
            if (customer == null)
            {
                throw new CustomerNotFoundException(userId);
            }

            return customer;
        }

        public IEnumerable<Transaction> GetAllTransaction(Customer customer)
        {
            var transactionList = new List<Transaction>();
            foreach(var account in customer.BankAccounts)
            {
                transactionList.AddRange(account.Transactions);
            }

            return transactionList.AsEnumerable();
        }

        public BankAccount GetCustomerBankAccount(Customer customer, string accountId)
        {            
            if (string.IsNullOrEmpty(accountId))
            {
                throw new ArgumentException("accountId");
            }
            var account = customer.BankAccounts
                    .Where(ba => accountId.Equals(ba.Id.ToString()))
                    .SingleOrDefault();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            return account;
        }

        public IEnumerable<Transaction> SearchTransactions(Customer customer, string accountId, string stringToSearch)
        {
            if (string.IsNullOrEmpty(accountId))
            {
                throw new ArgumentException("accountId");
            }
            var account = customer.BankAccounts
                    .Where(ba => accountId.Equals(ba.Id.ToString()))
                    .SingleOrDefault();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            IEnumerable<Transaction> transactions;
            if (string.IsNullOrEmpty(stringToSearch))
            {
                transactions = account.Transactions;
            }
            else
            {

                transactions = account.Transactions
                                      .Where(t =>
                                      t.Amount.ToString().Contains(stringToSearch) ||
                                      (t.ExternalIBAN != null && t.ExternalIBAN.ToString().Contains(stringToSearch)) ||
                                      (t.ExternalName != null && t.ExternalName.ToString().Contains(stringToSearch)) ||
                                      (t.Details != null && t.Details.ToString().Contains(stringToSearch))
                                      );
            }
            return transactions.AsEnumerable();
        }

        public IEnumerable<Card> GetCardsByUserID(string userID)
        {
            Guid idToSearch = Guid.Empty;
            Guid.TryParse(userID, out idToSearch);
            var cards = cardRepository.GetByUserId(idToSearch);
            return cards;
        }



    }
}
