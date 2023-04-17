using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using cochesApi.DataAccess.Queries;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class CustomerResponseValidation
    {
        public CustomerRequest? CustomerResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public CustomerResponseValidation(CustomerRequest? customerResponse)
        {
            CustomerResponse = customerResponse;
            Status = true;
            Message = "OK";
        }
    }

    public class CustomerValidation : ControllerBase, ICustomer
    {
        private IDBQueries queries;
        private ICustomerQueries queriesCustomer;
        private readonly IConfiguration _configuration;


        public CustomerValidation(IConfiguration configuration, IDBQueries _queries, ICustomerQueries _queriesCustomer)
        {
            _configuration = configuration;
            queries = _queries;
            queriesCustomer = _queriesCustomer;
        }
        public ActionResult<IEnumerable<CustomerRequest>> GetCustomers()
        {

            var customers = queriesCustomer.GetCustomers();
            List<CustomerRequest> customersRequest = new List<CustomerRequest>();
            foreach (Customer customer in customers)
            {
                CustomerRequest customerRequest = new CustomerRequest();
                customerRequest.Name = customer.Name;
                customerRequest.Surname = customer.Surname;
                customerRequest.Email = customer.Email;
                customersRequest.Add(customerRequest);
            }
            return customersRequest;
        }
        public ActionResult<CustomerRequest> GetCustomer(int id)
        {
            var customer = queriesCustomer.GetCustomer(id)!;

            if (customer == null) return Problem("Customer not found");

            CustomerRequest customerRequest = new CustomerRequest();
            customerRequest.Name = customer.Name;
            customerRequest.Surname = customer.Surname;
            customerRequest.Email = customer.Email;
            return customerRequest;
        }

        public CustomerResponseValidation PutCustomer(int id, CustomerRequest customerRequest)
        {
            var customer = queriesCustomer.GetCustomer(id);
            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(null); // Cambiar en un futuro

            if (customer == null)
            {
                customerResponseValidation.Status = false;
                customerResponseValidation.Message = "Customer does not exist";
                return customerResponseValidation;
            }

            customer.Name = customerRequest.Name;
            customer.Surname = customerRequest.Surname;
            customer.Email = customerRequest.Email;

            queries.Update(customer);

            queries.SaveChangesAsync();

            return customerResponseValidation;

        }
        public ActionResult<CustomerRequest> PostCustomer(CustomerRequest customerRequest)
        {
            Customer customer = new Customer();
            customer.Name = customerRequest.Name;
            customer.Surname = customerRequest.Surname;
            customer.Email = customerRequest.Email;
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customerRequest.Password);

            CustomerRequest customerResponse = new CustomerRequest();
            customerResponse.Name = customerRequest.Name;
            customerResponse.Surname = customerRequest.Surname;
            customerResponse.Email = customerRequest.Email;

            queriesCustomer.AddCustomer(customer);
            queries.SaveChangesAsync();

            return customerResponse;
        }
        public CustomerResponseValidation DeleteCustomer(int id)
        {
            var customer = queriesCustomer.GetCustomer(id);
            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(null); // Cambiar en un futuro
            if (customer == null)
            {
                customerResponseValidation.Status = false;
                customerResponseValidation.Message = "Customer does not exist";
                return customerResponseValidation;
            }

            queriesCustomer.RemoveCustomer(customer);
            queries.SaveChangesAsync();

            return customerResponseValidation;
        }
        public string GetToken(CustomerLoginRequest customerLoginRequest)
        {
            if (customerLoginRequest.Equals == null || customerLoginRequest.Password == null) return "Wrong Data";

            var customer = queriesCustomer.GetCustomerByEmail(customerLoginRequest.Email!);

            if (!BCrypt.Net.BCrypt.Verify(customerLoginRequest.Password, customer.Password)) return "Wrong password";

            string token = CreateToken(customer);
            return token;
        }
        private string CreateToken(Customer customer)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt!.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
            new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject!),
            new Claim(JwtRegisteredClaimNames.Aud, jwt.Audience!),
            new Claim(JwtRegisteredClaimNames.Iss, jwt.Issuer!),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Role, "customer")
        };

            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMonths(2),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}