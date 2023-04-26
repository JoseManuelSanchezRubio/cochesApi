using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Validations;

namespace cochesApi.Logic.Interfaces
{
    public interface ICustomer
    {
        List<CustomerResponse> GetCustomers();
        CustomerResponseValidation GetCustomer(int id);
        CustomerResponseValidation UpdateCustomer(int id, CustomerRequest customerRequest);
        CustomerResponseValidation CreateCustomer(CustomerRequest customerRequest);
        CustomerResponseValidation DeleteCustomer(int id);
        TokenResponse GetToken(CustomerLoginRequest customerLoginRequest);
    }
}