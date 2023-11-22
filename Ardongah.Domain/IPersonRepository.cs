using Ardonagh.Application.Contracts;
using System.Linq.Expressions;

namespace Ardonagh.Domain
{
    public interface IPersonRepository
    {
        List<Person> GetPeople();
        Person? GetPerson(long id);
        void AddPerson(Person command);
        void SaveChanges();
        bool Exists(Expression<Func<Person, bool>> expression);
    }
}
