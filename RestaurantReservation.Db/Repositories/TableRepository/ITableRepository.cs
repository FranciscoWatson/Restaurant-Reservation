using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Repositories.TableRepository
{
    public interface ITableRepository
    {
        Task<IEnumerable<Table>> GetAllAsync();
        Task<Table> GetByIdAsync(int tableId);
        Task AddAsync(Table table);
        Task UpdateAsync(Table updatedTable);
        Task DeleteAsync(int tableId);
    }
}
