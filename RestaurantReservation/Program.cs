using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db;

IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

var connectionString = configuration.GetConnectionString("SqlServerConnection");

using (var dbContext = new RestaurantReservationDbContext(connectionString!))
{

}