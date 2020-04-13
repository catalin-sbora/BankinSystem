﻿using InternshipProject.ApplicationLogic.Abstractions;
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
        public CustomerService(ICustomerRepository customerRepository , ICardRepository cardRepository, ICardColorRepository cardColorRepository )
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
        public void CreateAccountPayment(string userId, Guid account, decimal amount, string destinationName, string destinationIBAN, string details)
        {
            var sendingCustomer = GetCustomer(userId);
            var transaction = sendingCustomer.MakePayment(account, amount, destinationName, destinationIBAN, details);

            var sendingCurrency = GetCustomerBankAccount(sendingCustomer, transaction.BankAccountId).Currency;
            var receiverCustomer = GetCustomerWithIBAN(destinationIBAN, sendingCurrency);

            if (receiverCustomer != null)
            {
                receiverCustomer.NotifyTransaction(transaction);
            }

            customerRepository.Update(sendingCustomer);
        }

        private Customer GetCustomerWithIBAN(string currency, string destinationIBAN)
        {
            foreach (Customer customer in customerRepository.GetCustomerstWithBankAccounts())
            {
                var foundBankAccount = customer.GetBankAccountByIBAN(destinationIBAN);
                if (foundBankAccount != null)
                {
                    if (foundBankAccount.Currency.Equals(currency))
                    {
                        return customer;
                    }

                    throw new WrongCurrencyException(currency, foundBankAccount.Currency);
                }
            }
            throw new AccountIBANNotFoundException(destinationIBAN);
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

        public BankAccount GetCustomerBankAccount(string userId, Guid bankAccountId)
        {
            var customer = GetCustomer(userId);
            var bankAccount = customer.BankAccounts.Where(account => account.Id.Equals(bankAccountId))
                                 .SingleOrDefault();
            if (bankAccount == null)
            {
                throw new AccountNotFoundException(bankAccountId);
            }
            return bankAccount;
        }

        public BankAccount GetCustomerBankAccount(Customer customer, Guid accountId)
        {            
            var account = customer.BankAccounts
                    .Where(ba => accountId.Equals(ba.Id))
                    .SingleOrDefault();
            if (account == null)
            {
                throw new AccountNotFoundException(accountId);
            }

            return account;
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
