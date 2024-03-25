using RestaurantReservation.Db.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantReservation.Db.Views
{
    public class ReservationDetailsView
    {
        public int ReservationId { get; set; }
        public int TableId { get; set; }
        public DateTime ReservationDate { get; set; }
        public int ReservationPartySize { get; set; }
        public int CustomerId { get; set; }
        public string CustomerFirstName { get; set; }
        public string CustomerLastName { get; set; }
        public string CustomerEmail { get; set; }
        public string? CustomerPhoneNumber { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantPhoneNumber { get; set; }
        public string? RestaurantOpeningHours { get; set; }

    }
}
