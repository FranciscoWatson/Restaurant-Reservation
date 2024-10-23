using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class UpdateOrderTotalHandling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP TRIGGER IF EXISTS dbo.OrderTotalAmount");

            migrationBuilder.Sql(@"
            CREATE PROCEDURE UpdateOrderTotal
                @OrderId INT
            AS
            BEGIN
                SET NOCOUNT ON;

                UPDATE Orders
                SET TotalAmount = (
                    SELECT ISNULL(SUM(oi.Quantity * mi.Price), 0)
                    FROM OrderItems oi
                    INNER JOIN MenuItems mi ON oi.ItemId = mi.ItemId
                    WHERE oi.OrderId = @OrderId
                )
                WHERE OrderId = @OrderId;
            END
        ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
            CREATE TRIGGER [dbo].[OrderTotalAmount]
            ON [dbo].[OrderItems]
            AFTER INSERT, UPDATE, DELETE
            AS
            BEGIN
                SET NOCOUNT ON;

                DECLARE @AffectedOrders TABLE (OrderId INT);

                INSERT INTO @AffectedOrders
                SELECT OrderId FROM INSERTED
                UNION
                SELECT OrderId FROM DELETED;

                UPDATE Orders
                SET TotalAmount = COALESCE((
                    SELECT SUM(oi.Quantity * mi.Price)
                    FROM OrderItems oi
                    INNER JOIN MenuItems mi ON oi.ItemId = mi.ItemId
                    WHERE oi.OrderId = Orders.OrderId
                ), 0)
                WHERE Orders.OrderId IN (SELECT OrderId FROM @AffectedOrders);
            END
        ");

            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS UpdateOrderTotal");
        }
    }
}
