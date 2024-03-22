using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.ReservationRepository
{
    public interface IReservationRepository
    {
        Task<IEnumerable<Reservation>> GetAllAsync();
        Task<Reservation> GetByIdAsync(int reservationId);
        Task AddAsync(Reservation reservation);
        Task UpdateAsync(Reservation updatedReservation);
        Task DeleteAsync(int reservationId);
        Task<IEnumerable<Reservation>> GetReservationsByCustomer(int customerId);
    }
}
