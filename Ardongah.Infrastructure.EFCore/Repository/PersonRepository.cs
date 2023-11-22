using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using Ardonagh.Domain;

namespace Ardonagh.Infrastructure.EFCore.Repository
{
    public class PersonRepository : IPersonRepository
    {
        private readonly PersonContext _context;

        public PersonRepository(PersonContext context)
        {
            _context = context;
        }

        public List<Person> GetPeople()
        {
            return _context.People.ToList();
        }

        public Person? GetPerson(long id)
        {
            return _context.People.Find(id);
        }

        public void AddPerson(Person command)
        {
            _context.Add(command);
        }

        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        public bool Exists(Expression<Func<Person, bool>> expression)
        {
            return _context.People.Any(expression);
        }
    }
}
