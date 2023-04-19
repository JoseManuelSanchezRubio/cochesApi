using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class CustomerResponseValidation
    {
        public CustomerResponse? CustomerResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public CustomerResponseValidation(CustomerResponse? customerResponse)
        {
            CustomerResponse = customerResponse;
            Status = true;
            Message = "OK";
        }
    }
    public class CustomerValidation : ICustomer
    {
        private IDBQueries queriesDB;
        private ICustomerQueries queriesCustomer;
        private readonly IConfiguration _configuration;

        public CustomerValidation(IConfiguration configuration, IDBQueries _queries, ICustomerQueries _queriesCustomer)
        {
            _configuration = configuration;
            queriesDB = _queries;
            queriesCustomer = _queriesCustomer;
        }
        public List<CustomerResponse> GetCustomers()
        {
            var customers = queriesCustomer.GetCustomers();
            List<CustomerResponse> customersResponse = new List<CustomerResponse>();
            foreach (Customer customer in customers)
            {
                CustomerResponse customerResponse = new CustomerResponse();
                customerResponse.Id = customer.Id;
                customerResponse.Name = customer.Name;
                customerResponse.Surname = customer.Surname;
                customerResponse.Age = customer.Age;
                customerResponse.Photo = customer.Photo;
                customerResponse.Email = customer.Email;
                customersResponse.Add(customerResponse);
            }
            return customersResponse;
        }
        public CustomerResponseValidation GetCustomer(int id)
        {
            var customer = queriesCustomer.GetCustomer(id)!;

            if (customer == null)
            {
                CustomerResponseValidation crv = new CustomerResponseValidation(null);
                crv.Status = false;
                crv.Message = "Customer not found";
                return crv;
            }

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customer.Name;
            customerResponse.Surname = customer.Surname;
            customerResponse.Age = customer.Age;
            customerResponse.Photo = customer.Photo;
            customerResponse.Email = customer.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            return customerResponseValidation;
        }
        public CustomerResponseValidation UpdateCustomer(int id, CustomerRequest customerRequest)
        {
            bool ageOk = false;
            var ages = Enum.GetValues(typeof(CustomerAge));
            foreach (CustomerAge age in ages)
            {
                if (customerRequest.Age == age)
                {
                    ageOk = true;
                }
            }
            CustomerResponseValidation crv = new CustomerResponseValidation(null);

            if (!ageOk)
            {
                crv.Status = false;
                crv.Message = "Invalid Age";
                return crv;
            }

            var customer = queriesCustomer.GetCustomer(id);

            if (customer == null)
            {
                crv.Status = false;
                crv.Message = "Customer not found";
                return crv;
            }

            customer.Name = customerRequest.Name;
            customer.Surname = customerRequest.Surname;
            customer.Age = customerRequest.Age;
            customer.Photo = customerRequest.Photo;
            customer.Email = customerRequest.Email;

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customer.Name;
            customerResponse.Surname = customer.Surname;
            customerResponse.Age = customer.Age;
            customerResponse.Photo = customer.Photo;
            customerResponse.Email = customer.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            queriesDB.Update(customer);

            queriesDB.SaveChangesAsync();

            return customerResponseValidation;

        }
        public CustomerResponseValidation CreateCustomer(CustomerRequest customerRequest)
        {
            bool ageOk = false;
            var ages = Enum.GetValues(typeof(CustomerAge));
            foreach (CustomerAge age in ages)
            {
                if (customerRequest.Age == age)
                {
                    ageOk = true;
                }
            }
            CustomerResponseValidation crv = new CustomerResponseValidation(null);

            if (!ageOk)
            {
                crv.Status = false;
                crv.Message = "Invalid Age";
                return crv;
            }

            Customer customer = new Customer();
            customer.Name = customerRequest.Name;
            customer.Surname = customerRequest.Surname;
            customer.Age = customerRequest.Age;
            customer.Photo = customerRequest.Photo;
            customer.Email = customerRequest.Email;
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customerRequest.Password);

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customerRequest.Name;
            customerResponse.Surname = customerRequest.Surname;
            customerResponse.Age = customerRequest.Age;
            customerResponse.Photo = customerRequest.Photo;
            customerResponse.Email = customerRequest.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            queriesCustomer.AddCustomer(customer);
            queriesDB.SaveChangesAsync();

            return customerResponseValidation;
        }
        public CustomerResponseValidation DeleteCustomer(int id)
        {
            var customer = queriesCustomer.GetCustomer(id);

            if (customer == null)
            {
                CustomerResponseValidation crv = new CustomerResponseValidation(null);
                crv.Status = false;
                crv.Message = "Customer not found";
                return crv;
            }

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customer.Name;
            customerResponse.Surname = customer.Surname;
            customerResponse.Age = customer.Age;
            customerResponse.Photo = customer.Photo;
            customerResponse.Email = customer.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            queriesCustomer.RemoveCustomer(customer);
            queriesDB.SaveChangesAsync();

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