using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContextFactory : IDesignTimeDbContextFactory<RestaurantReservationDbContext>
    {
        public RestaurantReservationDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();

            // Get connection string from appsettings.json
            var connectionString = configuration.GetConnectionString("SqlServerConnection");

            // Create options builder
            var optionsBuilder = new DbContextOptionsBuilder<RestaurantReservationDbContext>();
            optionsBuilder.UseSqlServer(connectionString);

            // Create and return DbContext
            return new RestaurantReservationDbContext(optionsBuilder.Options);
        }
    }
}
