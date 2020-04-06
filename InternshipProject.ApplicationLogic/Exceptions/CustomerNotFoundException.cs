using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Exceptions
{
    public class CustomerNotFoundException: Exception
    {
        public Guid CustomerId { get; private set; }
        public string UserId { get; private set; }
        public CustomerNotFoundException(Guid customerId): base($"Customer with id {customerId} was not found")
        {
            CustomerId = customerId;
        }

        public CustomerNotFoundException(string userId) : base($"Customer with id {userId} was not found")
        {
            CustomerId = Guid.Empty;
            UserId = userId;
        }
    }
}
