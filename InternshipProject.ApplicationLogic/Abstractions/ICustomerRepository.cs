using InternshipProject.ApplicationLogic.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace InternshipProject.ApplicationLogic.Abstractions
{
    public interface ICustomerRepository
    {
        /*
         * Get methods
         */
        Customer GetCustomerByUserId(Guid userId);
        Customer GetCustomerById(Guid customerId);
        IEnumerable<Customer> FindByLastName(string lastName);
        IEnumerable<Customer> GetAll();

        /*
         * Write Methods
         */

        Customer Add(Customer customer);
        Customer Update(Guid customerId, Customer customerDetails);
        bool Remove(Guid customerId);
                
    }
}
