using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using cochesApi.Logic.Models;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using cochesApi.Logic.Validations;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private ICustomer customerValidation;
        public CustomerController(ICustomer _customer, IConfiguration configuration){
            _configuration = configuration;
            customerValidation=_customer;
        }




        // GET: api/Customer
        [HttpGet]
        public ActionResult<IEnumerable<CustomerRequest>> GetCustomers()
        {
            return customerValidation.GetCustomers();
        }

        // GET: api/Customer/5
        [HttpGet("{id}")]
        public ActionResult<CustomerRequest> GetCustomer(int id)
        {
            return customerValidation.GetCustomer(id);
        }

        // PUT: api/Customer/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public CustomerResponseValidation PutCustomer(int id, CustomerRequest customerRequest)
        {
            return customerValidation.PutCustomer(id, customerRequest);
        }

        // POST: api/Customer
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<CustomerRequest> PostCustomer(CustomerRequest customerRequest)
        {
            return customerValidation.PostCustomer(customerRequest);
        }

        // DELETE: api/Customer/5
        [HttpDelete("{id}")]
        public CustomerResponseValidation DeleteCustomer(int id)
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
