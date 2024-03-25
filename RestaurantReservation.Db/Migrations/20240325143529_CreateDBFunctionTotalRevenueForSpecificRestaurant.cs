using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateDBFunctionTotalRevenueForSpecificRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    CREATE OR ALTER FUNCTION CalculateTotalRevenue(@RestaurantId INT)
                    RETURNS DECIMAL(18,2)
                    AS
                    BEGIN
                        RETURN (
                            SELECT SUM(o.TotalAmount) 
                            FROM Orders o
                            INNER JOIN Reservations r ON o.ReservationId = r.ReservationId
                            WHERE r.RestaurantId = @RestaurantId
                        )
                    END
            ");
        }


        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP FUNCTION IF EXISTS CalculateTotalRevenue");
        }
    }
}
