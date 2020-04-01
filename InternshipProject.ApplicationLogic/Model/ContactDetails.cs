using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Model
{
    public class ContactDetails
    {
        public Guid Id { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string PhoneNo { get; set; }
        public string AlternatePhoneNo { get; set; }
        public string Email { get; set; }
    }
}
