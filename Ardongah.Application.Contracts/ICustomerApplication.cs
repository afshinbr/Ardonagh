namespace Ardonagh.Application.Contracts
{
    public interface ICustomerApplication
    {
        OperationResult Add(CreateCustomer command);
        OperationResult Edit(EditCustomer command);
        List<CustomerViewModel> GetCustomers();
        EditCustomer GetDetails(long id);
    }
}
