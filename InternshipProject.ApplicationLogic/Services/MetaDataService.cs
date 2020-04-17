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
        private readonly ICardMetaDataRepository cardMetaDataRepository;
        public MetaDataService(IPersistenceContext persistenceContext)
        {
            metaDataRepository = persistenceContext.BankAccountMetaDataRepository;
            cardMetaDataRepository = persistenceContext.CardMetaDataRepository;
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

        public CardMetaData GetMetaDataForCard(Guid cardId)
        {
            var metaData = cardMetaDataRepository.GetMetaData(cardId);

            if (metaData == null)
            {
                //#0094ff
                metaData = new CardMetaData()
                {
                    Color = 0x0094ff
                };
            }

            return metaData;
        }
    }
}
