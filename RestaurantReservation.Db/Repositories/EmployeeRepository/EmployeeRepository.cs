using Microsoft.EntityFrameworkCore;
using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.EmployeeRepository
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly RestaurantReservationDbContext _restaurantReservationDbContext;

        public EmployeeRepository(RestaurantReservationDbContext restaurantReservationDbContext)
        {
            _restaurantReservationDbContext = restaurantReservationDbContext;

        }
        public async Task AddAsync(Employee employee)
        {
            await _restaurantReservationDbContext.Employees.AddAsync(employee);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(int employeeId)
        {
            var employee = await _restaurantReservationDbContext.Employees.FindAsync(employeeId);
            if (employee != null)
            {
                _restaurantReservationDbContext.Employees.Remove(employee);
                await _restaurantReservationDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Employee>> GetAllAsync()
        {
            return await _restaurantReservationDbContext.Employees.ToListAsync();
        }

        public async Task<Employee> GetByIdAsync(int employeeId)
        {
            return await _restaurantReservationDbContext.Employees.FindAsync(employeeId);
        }

        public async Task UpdateAsync(Employee updatedEmployee)
        {
            _restaurantReservationDbContext.Employees.Update(updatedEmployee);
            await _restaurantReservationDbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Employee>> ListManagersAsync()
        {
            return await _restaurantReservationDbContext.Employees.Where(e => e.Position == "Manager").ToListAsync();
        }
    }
}
