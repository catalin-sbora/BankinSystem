using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class Card
    {
        public Guid Id { get; set; }
        public string SerialNumber { get; set; }
        public string CVV { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string OwnerName { get; set; }
       
    }
}
