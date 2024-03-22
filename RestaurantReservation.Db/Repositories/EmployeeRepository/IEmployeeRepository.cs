using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.EmployeeRepository
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task<Employee> GetByIdAsync(int employeeId);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee updatedEmployee);
        Task DeleteAsync(int employeeId);

    }
}
