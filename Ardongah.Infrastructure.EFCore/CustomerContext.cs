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
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var assembly = typeof(CustomerMapping).Assembly;
            modelBuilder.ApplyConfigurationsFromAssembly(assembly);
            base.OnModelCreating(modelBuilder);



        }
    }
}
