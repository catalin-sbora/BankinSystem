using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface ICustomerRepository: IRepository<Customer>
    {
        /*
         * Get methods
         */
        Customer GetCustomerByUserId(Guid userId);
        
        IEnumerable<Customer> FindByLastName(string lastName);
        /*
         * Write Methods   */      
        Customer UpdateCustomerDetails(Guid customerId, Customer customerDetails);

    }
}
