namespace Ardonagh.Application.Contracts
{
    public interface IPersonApplication
    {
        OperationResult Add(CreatePerson command);
        OperationResult Edit(EditPerson command);
        List<PersonViewModel> GetPeople();
        EditPerson GetDetails(long id);
    }
}
