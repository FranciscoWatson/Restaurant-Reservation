using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models
{
    public class Restaurant
    {
        public int RestaurantId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        public string Address { get; set; }

        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string PhoneNumber { get; set; }


        public string? OpeningHours { get; set; }
        public List<Reservation> Reservations { get; set; }
        public List<Employee> Employees { get; set; }
        public List<MenuItem> MenuItems { get; set; }
        public List <Table> Tables { get; set; }

    }
}
