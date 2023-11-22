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
    public class PersonConfigurations
    {
        public static void Configure(IServiceCollection services, string connectionString)
        {
            services.AddTransient<IPersonApplication, PersonApplication>();
            services.AddTransient<IPersonRepository, PersonRepository>();
            var conn = new SqliteConnection(connectionString);
            services.AddDbContext<PersonContext>(x => x.UseSqlite(conn));
        }

    }
}
