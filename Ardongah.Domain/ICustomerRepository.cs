using Ardonagh.Application.Contracts;
using System.Linq.Expressions;

namespace Ardonagh.Domain
{
    public interface ICustomerRepository
    {
        List<Customer> GetCustomers();
        Customer? GetCustomer(long id);
        void AddCustomer(Customer command);
        void SaveChanges();
        bool Exists(Expression<Func<Customer, bool>> expression);
    }
}
