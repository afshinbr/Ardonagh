using System.Security.Cryptography.X509Certificates;
using Ardonagh.Application.Contracts;
using Ardonagh.Domain;

namespace Ardonagh.Application
{
    public class PersonApplication : IPersonApplication
    {
        private readonly IPersonRepository _personRepository;

        public PersonApplication(IPersonRepository personRepository)
        {
            _personRepository = personRepository;
        }

        public OperationResult Add(CreatePerson command)
        {
            var operation = new OperationResult();
            if (_personRepository.Exists(x=> x.Name == command.Name &&  x.Age == command.Age & x.Height == command.Height))
                return operation.Failed("This user already exists.");
            var person = new Person(command.Name, command.Age, command.PostCode, command.Height);
            _personRepository.AddPerson(person);
            _personRepository.SaveChanges();
            return operation;

        }

        public OperationResult Edit(EditPerson command)
        {
            var operation = new OperationResult();
            var person = _personRepository.GetPerson(command.Id);
            if (person == null)
                return operation.Failed("User cannot be found.");
            person.Edit(command.Name, command.Age, command.PostCode, command.Height);
            _personRepository.SaveChanges();
            return operation;


        }

        public List<PersonViewModel> GetPeople()
        {
            var people = _personRepository.GetPeople();
            return people.OrderByDescending(x=>x.Date).Select(x => new PersonViewModel()
            {
                Name = x.Name,
                Age = x.Age,
                Height = x.Height,
                PostCode = x.PostCode,
                Id = x.Id
            }).ToList();
        }

        public EditPerson GetDetails(long id)
        {
            var person = _personRepository.GetPerson(id);
            if (person == null)
                throw new NullReferenceException("User does not exist.");
            return new EditPerson
            {
                Name = person.Name,
                Age = person.Age,
                Height = person.Height,
                PostCode = person.PostCode,
                Id = person.Id,
            };
        }
    }
}
