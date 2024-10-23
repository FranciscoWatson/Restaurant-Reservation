using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.CustomerRepository
{
    public interface ICustomerRepository
    {
        Task<IEnumerable<Customer>> GetAllAsync();
        Task<Customer> GetByIdAsync(int customerId);
        Task AddAsync(Customer customer);
        Task UpdateAsync(Customer updatedCustomer);
        Task DeleteAsync(int customerId);
    }
}
