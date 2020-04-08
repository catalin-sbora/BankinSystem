﻿using InternshipProject.ApplicationLogic.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.EFDataAccess
{
    public class BankingDbContext: DbContext
    {
        public BankingDbContext(DbContextOptions<BankingDbContext> options): base(options)
        { 
        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<BankAccount> BankAccounts { get; set; }
        public DbSet<Card> Cards { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<ContactDetails> ContactDetails { get; set; }
        public DbSet<CardTransaction> CardTransactions { get; set; }
        public DbSet<BankAccountMetaData> BankAccountMetaDatas { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<BankAccount>()
                    .Property(ba => ba.Balance)
                    .HasColumnType("decimal(18, 4)");

            builder.Entity<Transaction>()
                    .Property(transaction => transaction.Amount)
                    .HasColumnType("decimal(18, 4)");

            builder.Entity<BankAccountMetaData>()
                    .HasOne<BankAccount>();
                    
        }


            
    }
}
