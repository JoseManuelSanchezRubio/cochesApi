using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;


namespace cochesApi.Logic.Interfaces
{

    public interface ICustomerQueries
    {
        List<Customer> GetCustomers();
        Customer GetCustomer(int id);
        void RemoveCustomer(Customer customer);
        void AddCustomer(Customer customer);
        Customer GetCustomerByEmail(string email);
    }
    
}