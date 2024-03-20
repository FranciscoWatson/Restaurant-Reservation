using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContext : DbContext
    {
        private readonly string ConnectionString;

        public RestaurantReservationDbContext(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(ConnectionString);
        } 

    }
}
