using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private ICustomer customerValidation;
        public CustomerController(ICustomer _customer, IConfiguration configuration)
        {
            _configuration = configuration;
            customerValidation = _customer;
        }

        [HttpGet]
        public ActionResult<IEnumerable<CustomerRequest>> GetCustomers()
        {
            return customerValidation.GetCustomers();
        }

        [HttpGet("{id}")]
        public ActionResult<CustomerRequest> GetCustomer(int id)
        {
            return customerValidation.GetCustomer(id);
        }

        [HttpPut("{id}")]
        public ActionResult<CustomerRequest> PutCustomer(int id, CustomerRequest customerRequest)
        {
            return customerValidation.PutCustomer(id, customerRequest);
        }

        [HttpPost]
        public ActionResult<CustomerRequest> PostCustomer(CustomerRequest customerRequest)
        {
            return customerValidation.PostCustomer(customerRequest);
        }

        [HttpDelete("{id}")]
        public ActionResult<CustomerRequest> DeleteCustomer(int id)
        {
            return customerValidation.DeleteCustomer(id);
        }

        [HttpPost("login")]
        public string GetToken(CustomerLoginRequest customerLoginRequest)
        {
            return customerValidation.GetToken(customerLoginRequest);

        }
    }
}
