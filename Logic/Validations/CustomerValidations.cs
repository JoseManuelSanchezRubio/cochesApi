using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
    public class TokenResponse
    {
        public string Token { get; set; }
        public CustomerResponse Customer { get; set; }
        public bool Status { get; set; }
        public TokenResponse(string token, CustomerResponse customer, bool status)
        {
            Token = token;
            Customer = customer;
            Status = status;
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

            customerResponse.Email = customer.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            return customerResponseValidation;
        }
        public CustomerResponseValidation UpdateCustomer(int id, CustomerRequest customerRequest)
        {

            CustomerResponseValidation crv = new CustomerResponseValidation(null);


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

            customer.Email = customerRequest.Email;

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customer.Name;
            customerResponse.Surname = customer.Surname;
            customerResponse.Age = customer.Age;

            customerResponse.Email = customer.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            queriesDB.Update(customer);

            queriesDB.SaveChangesAsync();

            return customerResponseValidation;

        }
        public CustomerResponseValidation CreateCustomer(CustomerRequest customerRequest)
        {

            CustomerResponseValidation crv = new CustomerResponseValidation(null);



            Customer customer = new Customer();
            customer.Name = customerRequest.Name;
            customer.Surname = customerRequest.Surname;
            customer.Age = customerRequest.Age;

            customer.Email = customerRequest.Email;
            customer.Password = BCrypt.Net.BCrypt.HashPassword(customerRequest.Password);

            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customerRequest.Name;
            customerResponse.Surname = customerRequest.Surname;
            customerResponse.Age = customerRequest.Age;

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

            customerResponse.Email = customer.Email;

            CustomerResponseValidation customerResponseValidation = new CustomerResponseValidation(customerResponse);

            queriesCustomer.RemoveCustomer(customer);
            queriesDB.SaveChangesAsync();

            return customerResponseValidation;
        }
        public TokenResponse GetToken(CustomerLoginRequest customerLoginRequest)
        {
            if (customerLoginRequest.Email == "" || customerLoginRequest.Password == "") return new TokenResponse("There are empty fields", null!, false);

            Customer customer;

            try
            {
                customer = queriesCustomer.GetCustomerByEmail(customerLoginRequest.Email!);
            }
            catch
            {
                return new TokenResponse("Email not found", null!, false);
            }


            if (!BCrypt.Net.BCrypt.Verify(customerLoginRequest.Password, customer.Password)) return new TokenResponse("Wrong password", null!, false);


            CustomerResponse customerResponse = new CustomerResponse();
            customerResponse.Id = customer.Id;
            customerResponse.Name = customer.Name;
            customerResponse.Surname = customer.Surname;
            customerResponse.Age = customer.Age;
            customerResponse.Email = customer.Email;


            string token = CreateToken(customer);
            return new TokenResponse(token, customerResponse, true);
        }
        private string CreateToken(Customer customer)
        {
            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt!.Key!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[] { new Claim(JwtRegisteredClaimNames.Sub, customer.Id.ToString()) };

            if (customer.Role == 0)
            {
                claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject!),
                new Claim(JwtRegisteredClaimNames.Aud, jwt.Audience!),
                new Claim(JwtRegisteredClaimNames.Iss, jwt.Issuer!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "customer")
            };
            }
            else
            {
                claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject!),
                new Claim(JwtRegisteredClaimNames.Aud, jwt.Audience!),
                new Claim(JwtRegisteredClaimNames.Iss, jwt.Issuer!),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, "admin")
            };
            }

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