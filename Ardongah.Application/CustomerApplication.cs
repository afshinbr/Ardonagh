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

        // This method add a customer to the database
        public OperationResult Add(CreateCustomer command)
        {

            var operation = new OperationResult();
            // Check if the customer already exists.
            if (_customerRepository.Exists(x => x.Name.ToLower() == command.Name.ToLower() &&
                                                x.Age == command.Age &&
                                                x.PostCode.ToLower() == command.PostCode.ToLower())) 
                return operation.Failed("This user already exists.");
            // Create a new customer based on the command and save it to the database
            var customer = new Customer(command.Name, command.Age, command.PostCode, command.Height);
            _customerRepository.AddCustomer(customer);
            _customerRepository.SaveChanges();
            return operation.Succeeded($"Customer ({command.Name}) added successfully!");

        }

        // This method edit a customer from the database
        public OperationResult Edit(EditCustomer command)
        {
            var operation = new OperationResult();
            // Get the customer from database with the Id
            var customer = _customerRepository.GetCustomer(command.Id);
            if (customer == null)
                return operation.Failed("User cannot be found.");
            // Edit the customer based on the command and save it to the database
            customer.Edit(command.Name, command.Age, command.PostCode, command.Height);
            _customerRepository.SaveChanges();
            return operation.Succeeded($"Customer ({command.Name}) Edited successfully!");
            
        }

        // This method get the list of the customers from the database
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

        // This method get a specific customer from the database
        public EditCustomer GetDetails(long id)
        {
            var customer = _customerRepository.GetCustomer(id);
            // Check if the customer exist
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
