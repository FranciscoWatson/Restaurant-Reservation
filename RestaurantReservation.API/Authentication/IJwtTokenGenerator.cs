using RestaurantReservation.Db.Models;

namespace RestaurantReservation.API.Authentication;

public interface IJwtTokenGenerator
{
    string GenerateToken(User user);
}