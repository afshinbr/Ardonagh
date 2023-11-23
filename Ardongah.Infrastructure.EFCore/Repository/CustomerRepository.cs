using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Ardonagh.Domain;

namespace Ardonagh.Infrastructure.EFCore.Repository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly CustomerContext _context;

        public CustomerRepository(CustomerContext context)
        {
            _context = context;
        }

        public List<Customer> GetCustomers()
        {
            return _context.Customers.ToList();
        }

        public Customer? GetCustomer(long id)
        {
            return _context.Customers.Find(id);
        }

        public void AddCustomer(Customer command)
        {
            _context.Add(command);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool Exists(Expression<Func<Customer, bool>> expression)
        {
            return _context.Customers.Any(expression);
        }
    }
}
