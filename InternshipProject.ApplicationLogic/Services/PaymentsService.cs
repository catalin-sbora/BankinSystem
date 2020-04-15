using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class PaymentsService
    {
        private readonly ITransactionRepository transactionRepository;
        private readonly ICustomerRepository customerRepository;

        public PaymentsService(ITransactionRepository transactionRepository, ICustomerRepository customerRepository)
        {
            this.transactionRepository = transactionRepository;
            this.customerRepository = customerRepository;
        }

        public Transaction AddPayment(string bankAccountId, decimal amount, string externalName, string externalIBAN)
        {
            Guid guidBankAccountId = Guid.Empty;
            Guid.TryParse(bankAccountId, out guidBankAccountId);

            if (guidBankAccountId == Guid.Empty)
            {
                throw new Exception("Wrong guid");
            }

            return transactionRepository.NewTransaction(amount, externalName, externalIBAN, guidBankAccountId);
        }
        public Transaction GetById(string Id)
        {
            Guid transactionId = Guid.Empty;
            Guid.TryParse(Id, out transactionId);

            if (transactionId == Guid.Empty)
            {
                throw new Exception("Wrong guid");
            }
            return transactionRepository.GetTransactionById(transactionId);
        }
        public IEnumerable<Transaction> GetFilteredPayments(string userId, string searchedString)
        {
            Guid guidUserId = Guid.Empty;
            Guid.TryParse(userId, out guidUserId);

            if (guidUserId == Guid.Empty)
            {
                throw new Exception("Wrong guid");
            }


            var paymentsList = GetCustomerPayments(userId);

            var searchedPaymentsList = paymentsList.Where(transaction => (transaction.Amount.ToString().Contains(searchedString)) ||
                                                                           (transaction.ExternalIBAN.ToLower().Contains(searchedString)) ||
                                                                           (transaction.ExternalName.ToLower().Contains(searchedString)) ||
                                                                           (transaction.Time.ToString().Contains(searchedString)));
            return searchedPaymentsList;
        }

        public IEnumerable<Transaction> GetCustomerPayments(string userId)
        {
            var customer = customerRepository?.GetCustomerByUserId(Guid.Parse(userId));
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
