using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Validations;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController
    {
        private readonly IConfiguration _configuration;
        private ICustomer customerValidation;
        public CustomerController(ICustomer _customer, IConfiguration configuration)
        {
            _configuration = configuration;
            customerValidation = _customer;
        }

        [HttpGet]
        public IEnumerable<CustomerResponse> GetCustomers()
        {
            return customerValidation.GetCustomers();
        }

        [HttpGet("{id}")]
        public CustomerResponseValidation GetCustomer(int id)
        {
            return customerValidation.GetCustomer(id);
        }

        [HttpPut("{id}")]
        public CustomerResponseValidation PutCustomer(int id, CustomerRequest customerRequest)
        {
            return customerValidation.UpdateCustomer(id, customerRequest);
        }

        [HttpPost]
        public CustomerResponseValidation PostCustomer(CustomerRequest customerRequest)
        {
            return customerValidation.CreateCustomer(customerRequest);
        }

        [HttpDelete("{id}")]
        public CustomerResponseValidation DeleteCustomer(int id)
        {
            return customerValidation.DeleteCustomer(id);
        }

        [HttpPost("login")]
        public TokenResponse GetToken(CustomerLoginRequest customerLoginRequest)
        {
            return customerValidation.GetToken(customerLoginRequest);

        }
    }
}
