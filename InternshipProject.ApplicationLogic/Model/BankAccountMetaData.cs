using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class BankAccountMetaData:DataEntity
    {        
        public int Color { get; set; }
        public string Name { get; set; }
        public Guid BankAccountId { get; set; }
    }
}
