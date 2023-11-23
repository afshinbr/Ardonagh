using System.Reflection.Emit;
using System.Xml.Linq;

namespace Ardonagh.Domain
{
    public class Customer
    {
        public long Id { get; private set; }
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string PostCode { get; private set; }
        public double Height { get; private set; }
        public DateTime Date { get; private set; }

        public Customer(string name, int age, string postCode, double height)
        {
            Name = name;
            Age = age;
            PostCode = postCode;
            Height = height;
            Date = DateTime.Now;
        }

        public Customer(int id, string name, int age, string postCode, double height)
        {
            Id = id;
            Name = name;
            Age = age;
            PostCode = postCode;
            Height = height;
            Date = DateTime.Now;
        }

        public void Edit(string name, int age, string postCode, double height)
        {
            Name = name;
            Age = age;
            PostCode = postCode;
            Height = height;
        }
    }
}
