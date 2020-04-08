using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface IBankAccountMetaDataRepository: IRepository<BankAccountMetaData>
    {
        BankAccountMetaData GetMetadataForBankAccount(Guid bankAccountId);   
    }
}
