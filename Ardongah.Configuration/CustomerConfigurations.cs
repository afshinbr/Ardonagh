using Ardonagh.Application;
using Ardonagh.Application.Contracts;
using Ardonagh.Domain;
using Ardonagh.Infrastructure.EFCore;
using Ardonagh.Infrastructure.EFCore.Repository;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Ardonagh.Configuration
{
    public class CustomerConfigurations
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<ICustomerApplication, CustomerApplication>();
            services.AddTransient<ICustomerRepository, CustomerRepository>();
            var conn = new SqliteConnection(connectionString);
            conn.Open();
            services.AddDbContext<CustomerContext>(x => x.UseSqlite(conn));
        }

    }
}
