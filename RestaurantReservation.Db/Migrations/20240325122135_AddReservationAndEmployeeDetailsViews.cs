using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class AddReservationAndEmployeeDetailsViews : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    CREATE VIEW ReservationDetailsView AS
                    SELECT
                        r.ReservationId,
                        r.TableId,
                        r.ReservationDate,
                        r.PartySize AS ReservationPartySize,
                        c.CustomerId,
                        c.FirstName AS CustomerFirstName,
                        c.LastName AS CustomerLastName,
                        c.Email AS CustomerEmail,
                        c.PhoneNumber AS CustomerPhoneNumber,
                        rest.RestaurantId,
                        rest.Name AS RestaurantName,
                        rest.PhoneNumber AS RestaurantPhoneNumber,
                        rest.OpeningHours AS RestaurantOpeningHours
                    FROM Reservations r
                    JOIN Customers c ON r.CustomerId = c.CustomerId
                    JOIN Restaurants rest ON r.RestaurantId = rest.RestaurantId
                    ");

            migrationBuilder.Sql(@"
                    CREATE VIEW EmployeeDetailsView AS
                    SELECT
                        e.EmployeeId,
                        e.FirstName AS EmployeeFirstName,
                        e.LastName AS EmployeeLastName,
                        e.Position AS EmployeePosition,
                        r.RestaurantId,
                        r.Name AS RestaurantName,
                        r.Address AS RestaurantAddress,
                        r.PhoneNumber AS RestaurantPhoneNumber,
                        r.OpeningHours AS RestaurantOpeningHours
                    FROM Employees e
                    JOIN Restaurants r ON e.RestaurantId = r.RestaurantId;
                    ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP VIEW IF EXISTS ReservationDetailsView");
            migrationBuilder.Sql("DROP VIEW IF EXISTS EmployeeDetailsView");
        }
    }
}
