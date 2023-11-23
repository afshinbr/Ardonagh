using Ardonagh.Domain;
using Ardonagh.Infrastructure.EFCore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace Ardonagh.Infrastructure.EFCore
{
    public class CustomerContext : DbContext
    {

        public DbSet<Customer> Customers { get; set; }

        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {
            // It create the sql database first.
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // it apply any assembly of type of mapping into the modelBuilder
            var assembly = typeof(CustomerMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);



        }
    }
}
