using Ardonagh.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Ardonagh.Infrastructure.EFCore.Mapping
{
    public class CustomerMapping : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Name).HasMaxLength(50).IsRequired();
            builder.Property(x => x.Age).IsRequired();
            builder.Property(x => x.Height).HasPrecision(2).IsRequired();
            builder.Property(x => x.PostCode).IsRequired();

        }
    }
}
