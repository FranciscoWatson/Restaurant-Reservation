using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.ReservationRepository
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public ReservationRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(Reservation reservation)
        {
            await _restaurantReservationDbContext.Reservations.AddAsync(reservation);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int reservationId)
        {
            var reservation = await _restaurantReservationDbContext.Reservations.FindAsync(reservationId);
            if (reservation != null)
            {
                _restaurantReservationDbContext.Reservations.Remove(reservation);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.Reservations.ToListAsync();
        }

        public async Task<Reservation> GetByIdAsync(int reservationId)
        {
            return await _restaurantReservationDbContext.Reservations.FindAsync(reservationId);
        }

        public async Task UpdateAsync(Reservation updatedReservation)
        {
            _restaurantReservationDbContext.Reservations.Update(updatedReservation);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }
    }
}
