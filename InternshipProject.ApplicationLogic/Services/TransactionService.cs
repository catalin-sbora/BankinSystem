using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class TransactionService
    {
        private readonly ITransactionRepository transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            this.transactionRepository = transactionRepository;
        }

        public Transaction Add(decimal amount, string externalName, string externalIBAN, string bankAccountId)
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

        public IEnumerable<Transaction> SearchedTransactionsByAmount(string amount, string userId)
        {
            Guid guidUserId = Guid.Empty;
            Guid.TryParse(userId, out guidUserId);

            if (guidUserId == Guid.Empty)
            {
                throw new Exception("Wrong guid");
            }

            var transactionList = transactionRepository.GetAllTransactionsForCustomer(guidUserId);
            var searchedTransactionList = transactionList.Where(transaction => transaction.Amount.ToString().Contains(amount));

            return searchedTransactionList;
        }
    }
}
