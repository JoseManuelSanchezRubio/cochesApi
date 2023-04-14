using cochesApi.Logic.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Interfaces;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesCustomer : ICustomerQueries
    {
        private readonly myAppContext _context;
        public QueriesCustomer(myAppContext context)
        {
            _context = context;
        }

        public List<Customer> GetCustomers(){
            return _context.Customers.ToList();
        }
        public Customer GetCustomer(int id){
            return _context.Customers.Find(id)!;
        }
        public void RemoveCustomer(Customer customer){
            _context.Customers.Remove(customer);
        }
        public void AddCustomer(Customer customer){
            _context.Customers.Add(customer);
        }
        public Customer GetCustomerByEmail(string email){
            return (from c in _context.Customers
                            where c.Email == email
                            select c).First();
        }


    }
}