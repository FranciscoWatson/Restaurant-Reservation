using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Models
{
    public class Customer
    {
        public int CustomerId { get; set; }

        [MaxLength(50)]
        public string FirstName { get; set; }

        [MaxLength(50)]
        public string LastName { get; set; }

        [MaxLength(255)]
        [Column(TypeName = "varchar(255)")]
        public string Email { get; set; }

        [MaxLength(30)]
        [Column(TypeName = "varchar(30)")]
        public string? PhoneNumber { get; set; }

        public List<Reservation> Reservations { get; set; }
    }
}
