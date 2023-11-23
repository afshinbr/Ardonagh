using System.Security.Cryptography.X509Certificates;
using Ardonagh.Application.Contracts;
using Ardonagh.Domain;

namespace Ardonagh.Application
{
    public class CustomerApplication : ICustomerApplication
    {
        private readonly ICustomerRepository _customerRepository;

        public CustomerApplication(ICustomerRepository customerRepository)
        {
            _customerRepository = customerRepository;
        }

        public OperationResult Add(CreateCustomer command)
        {

            var operation = new OperationResult();
            if (_customerRepository.Exists(x => x.Name.ToLower() == command.Name.ToLower() &&
                                                x.Age == command.Age &&
                                                x.PostCode.ToLower() == command.PostCode.ToLower())) 
                return operation.Failed("This user already exists.");
            var customer = new Customer(command.Name, command.Age, command.PostCode, command.Height);
            _customerRepository.AddCustomer(customer);
            _customerRepository.SaveChanges();
            return operation.Succeeded($"Customer ({command.Name}) added successfully!");

        }

        public OperationResult Edit(EditCustomer command)
        {
            var operation = new OperationResult();
            var customer = _customerRepository.GetCustomer(command.Id);
            if (customer == null)
                return operation.Failed("User cannot be found.");
            customer.Edit(command.Name, command.Age, command.PostCode, command.Height);
            _customerRepository.SaveChanges();
            return operation.Succeeded($"Customer ({command.Name}) Edited successfully!");


        }

        public List<CustomerViewModel> GetCustomers()
        {
            var customers = _customerRepository.GetCustomers();
            return customers.OrderByDescending(x=>x.Date).Select(x => new CustomerViewModel()
            {
                Name = x.Name,
                Age = x.Age,
                Height = x.Height,
                PostCode = x.PostCode,
                Id = x.Id
            }).ToList();
        }

        public EditCustomer GetDetails(long id)
        {
            var customer = _customerRepository.GetCustomer(id);
            if (customer == null)
                throw new NullReferenceException("User does not exist.");
            return new EditCustomer
            {
                Name = customer.Name,
                Age = customer.Age,
                Height = customer.Height,
                PostCode = customer.PostCode,
                Id = customer.Id,
            };
        }
    }
}
