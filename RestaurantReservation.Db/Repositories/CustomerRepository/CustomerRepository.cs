using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.CustomerRepository
{
    public class CustomerRepository : ICustomerRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public CustomerRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(Customer customer)
        {
            await _restaurantReservationDbContext.Customers.AddAsync(customer);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int customerId)
        {
            var customer = await _restaurantReservationDbContext.Customers.FindAsync(customerId);
            if (customer != null)
            {
                _restaurantReservationDbContext.Customers.Remove(customer);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Customer>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.Customers.ToListAsync();
        }

        public async Task<Customer> GetByIdAsync(int id)
        {
            return await _restaurantReservationDbContext.Customers.FindAsync(id);
        }

        public async Task UpdateAsync(Customer updatedCustomer)
        {
            _restaurantReservationDbContext.Customers.Update(updatedCustomer);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }
    }
}
