using InternshipProject.ApplicationLogic.Abstractions;
using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace InternshipProject.ApplicationLogic.Services
{
    public class MetaDataService
    {
        private readonly IBankAccountMetaDataRepository metaDataRepository;
        public MetaDataService(IBankAccountMetaDataRepository metaDataRepository)
        {
            this.metaDataRepository = metaDataRepository;
        }

        public BankAccountMetaData GetMetaDataForBankAccount(Guid bankAccountId)
        {
            var metaData = metaDataRepository.GetMetadataForBankAccount(bankAccountId);

            if (metaData == null)
            {
                //#0094ff
                metaData = new BankAccountMetaData() 
                { 
                    Color =  0x0094ff, 
                    BankAccountId = bankAccountId, 
                    Name = "Account Name"
                };       
            }

            return metaData;
        }
    }
}
