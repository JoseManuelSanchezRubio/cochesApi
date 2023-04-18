using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Validations;

namespace cochesApi.Logic.Interfaces
{
    public interface ICustomer
    {
        List<CustomerRequest> GetCustomers();
        CustomerResponseValidation GetCustomer(int id);
        CustomerResponseValidation UpdateCustomer(int id, CustomerRequest customerRequest);
        CustomerResponseValidation CreateCustomer(CustomerRequest customerRequest);
        CustomerResponseValidation DeleteCustomer(int id);
        string GetToken(CustomerLoginRequest customerLoginRequest);
    }
}