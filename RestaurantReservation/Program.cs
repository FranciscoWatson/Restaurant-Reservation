using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db;

IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

var contextOptions = new DbContextOptionsBuilder<RestaurantReservationDbContext>()
    .UseSqlServer(configuration.GetConnectionString("SqlServerConnection"))
    .Options;

using var context = new RestaurantReservationDbContext(contextOptions);