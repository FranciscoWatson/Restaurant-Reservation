using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class CreateStoredProcedureFindCustomersByPartySize : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                    CREATE OR ALTER PROCEDURE FindCustomersByPartySize
                        @PartySize INT
                    AS
                    BEGIN
                        SELECT DISTINCT c.*
                        FROM Customers c
                        INNER JOIN Reservations r ON c.CustomerId = r.CustomerId
                        WHERE r.PartySize > @PartySize
                    END
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DROP PROCEDURE IF EXISTS FindCustomersByPartySize");

        }
    }
}
