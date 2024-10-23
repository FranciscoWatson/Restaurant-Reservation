using RestaurantReservation.Db.Models;

namespace RestaurantReservation.Db.Repositories.UserRepository;

public interface IUserRepository
{
    Task<User?> Get(string username, string password);
}