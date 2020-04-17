using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Exceptions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class PaymentsService
    {
        private readonly IPersistenceContext persistenceContext;
        private readonly ICustomerRepository customerRepository;
        private readonly CustomerService customerService;

        public PaymentsService(IPersistenceContext persistenceContext, CustomerService customerService)
        {            
            this.persistenceContext = persistenceContext;
            customerRepository = this.persistenceContext.CustomerRepository;
            this.customerService = customerService;
        }      

        public Transaction CreateAccountPayment(string userId, Guid account, decimal amount, string destinationName, string destinationIBAN, string details)
        {
            var sendingCustomer = customerService.GetCustomerFromUserId(userId);
            var sendingAccount = sendingCustomer.GetAccount(account);
            Transaction transaction = null;
            using (var transactionScope = persistenceContext.BeginTransaction())
            {
                var receiverCustomer = customerRepository.GetCustomerThatOwnsIban(destinationIBAN);
                if (receiverCustomer != null)
                {
                    var destAccount = receiverCustomer.GetBankAccountByIBAN(destinationIBAN);
                    if (!destAccount.Currency.Equals(sendingAccount.Currency))
                    {
                        throw new WrongCurrencyException(sendingAccount.Currency, destAccount.Currency);
                    }
                }
                transaction = sendingCustomer.MakePayment(account, amount, destinationName, destinationIBAN, details);

                customerRepository.Update(sendingCustomer);

                if (receiverCustomer != null)
                {
                    receiverCustomer.NotifyTransaction(transaction, sendingCustomer);
                    customerRepository.Update(receiverCustomer);
                }
                persistenceContext.SaveChanges();                
            }

            return transaction;
        }
        public Transaction GetPaymentById(string userId, Guid id)
        {
            var customer = customerService.GetCustomerFromUserId(userId);
            var bankAccount = customer.BankAccounts.Where(ba => ba.Transactions
                                                .Where(transaction => transaction.Amount < 0 && transaction.Id == id)
                                                .Any()
                            ).Single();

            var payment = bankAccount.Transactions
                        .Where(transaction => transaction.Id == id )
                        .Single();

            return payment;
        }
        public IEnumerable<Transaction> GetFilteredPayments(string userId, string searchedString)
        {
            Guid guidUserId = Guid.Parse(userId);
            var paymentsList = GetCustomerPayments(userId);
            var searchedPaymentsList = paymentsList.Where(transaction => (transaction.Amount.ToString().Contains(searchedString)) ||
                                                                           (transaction.ExternalIBAN.ToLower().Contains(searchedString)) ||
                                                                           (transaction.ExternalName.ToLower().Contains(searchedString)) ||
                                                                           (transaction.Time.ToString().Contains(searchedString)));
            return searchedPaymentsList;
        }

        public IEnumerable<Transaction> GetCustomerPayments(string userId)
        {
            var customer = customerService.GetCustomerFromUserId(userId);
            var paymentsList = new List<Transaction>();
            foreach (var account in customer.BankAccounts)
            {
                foreach (var transaction in account.Transactions)
                {
                    if (transaction.Amount < 0)
                    {
                        paymentsList.Add(transaction);
                    }
                }
            }

            return paymentsList.AsEnumerable();
        }
    }
}
