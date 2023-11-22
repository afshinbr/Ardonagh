using Ardonagh.Domain;
using Ardonagh.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Ardonagh.Infrastructure.EFCore
{
    public class PersonContext : DbContext
    {

        public DbSet<Person> People { get; set; }

        public PersonContext(DbContextOptions<PersonContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(PersonMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
