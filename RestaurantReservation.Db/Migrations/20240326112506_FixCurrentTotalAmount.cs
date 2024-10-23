using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class FixCurrentTotalAmount : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE Orders
                SET TotalAmount = COALESCE((
                    SELECT SUM(oi.Quantity * mi.Price)
                    FROM OrderItems oi
                    INNER JOIN MenuItems mi ON oi.ItemId = mi.ItemId
                    WHERE oi.OrderId = Orders.OrderId
                ), 0);
            ");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
