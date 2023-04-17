using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Interfaces
{

    public interface ICustomer
    {
        ActionResult<IEnumerable<CustomerRequest>> GetCustomers();
        ActionResult<CustomerRequest> GetCustomer(int id);
        CustomerResponseValidation PutCustomer(int id, CustomerRequest customerRequest);
        ActionResult<CustomerRequest> PostCustomer(CustomerRequest customerRequest);
        CustomerResponseValidation DeleteCustomer(int id);
        string GetToken(CustomerLoginRequest customerLoginRequest);
    }
    
}