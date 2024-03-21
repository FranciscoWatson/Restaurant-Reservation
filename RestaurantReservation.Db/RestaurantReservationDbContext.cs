using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {

            IConfigurationRoot configuration = new ConfigurationBuilder()
                        .SetBasePath(Directory.GetCurrentDirectory())
                        .AddJsonFile("appsettings.json")
                        .Build();

            var connectionString = configuration.GetConnectionString("SqlServerConnection");

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}
