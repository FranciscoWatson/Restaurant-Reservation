using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RestaurantReservation.Db.Migrations
{
    /// <inheritdoc />
    public partial class SeedTables : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Customers",
                columns: new[] { "CustomerId", "FirstName", "LastName", "Email", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, "John", "Doe", "johndoe@example.com", "123-456-7890" },
                    { 2, "Jane", "Doe", "janedoe@example.com", "123-456-7891" },
                    { 3, "Jim", "Beam", "jimbeam@example.com", "123-456-7892" },
                    { 4, "Jack", "Daniels", "jackdaniels@example.com", "123-456-7893" },
                    { 5, "Johnny", "Walker", "johnnywalker@example.com", "123-456-7894" }
                });

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantId", "Name", "Address", "PhoneNumber", "OpeningHours" },
                values: new object[,]
                {
                    { 1, "Tasty Bites", "123 Flavor St, Foodville", "555-1234", "8:00 AM - 10:00 PM" },
                    { 2, "Gourmet Hub", "456 Gourmet Ave, Culinary City", "555-5678", "9:00 AM - 11:00 PM" },
                    { 3, "Fast Feast", "789 Quick Meal Blvd, Speedy Town", "555-9012", "7:00 AM - 9:00 PM" },
                    { 4, "Veggie Paradise", "321 Green Leaf Rd, Plantville", "555-3456", "10:00 AM - 8:00 PM" },
                    { 5, "Seafood Delight", "654 Ocean Wave Way, Coastline", "555-7890", "11:00 AM - 10:00 PM" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "EmployeeId", "RestaurantId", "FirstName", "LastName", "Position" },
                values: new object[,]
                {
                    { 1, 1, "Alice", "Johnson", "Manager" },
                    { 2, 1, "Bob", "Smith", "Chef" },
                    { 3, 1, "Charlie", "Brown", "Waiter" },
                    { 4, 3, "David", "Green", "Bartender" },
                    { 5, 2, "Eve", "White", "Hostess" }
                });

            migrationBuilder.InsertData(
                table: "MenuItems",
                columns: new[] { "ItemId", "RestaurantId", "Name", "Description", "Price" },
                values: new object[,]
                {
                    { 1, 1, "Cheeseburger", "A juicy cheeseburger with lettuce, tomato, and pickle", 9.99m },
                    { 2, 1, "Veggie Burger", "A delicious and healthy veggie burger with all the fixings", 8.99m },
                    { 3, 1, "Chicken Sandwich", "Grilled chicken sandwich with avocado and bacon", 10.99m },
                    { 4, 3, "Salad", "Fresh garden salad with your choice of dressing", 7.99m },
                    { 5, 2, "Pizza", "Large pepperoni pizza", 12.99m }
                });

            migrationBuilder.InsertData(
                table: "Tables",
                columns: new[] { "TableId", "RestaurantId", "Capacity" },
                values: new object[,]
                {
                    { 1, 1, 4 },
                    { 2, 1, 4 },
                    { 3, 2, 6 },
                    { 4, 2, 6 },
                    { 5, 3, 2 },
                    { 6, 3, 2 },
                    { 7, 4, 4 },
                    { 8, 4, 4 },
                    { 9, 5, 6 },
                    { 10, 5, 6 }
                });

            migrationBuilder.InsertData(
                table: "Reservations",
                columns: new[] { "ReservationId", "CustomerId", "RestaurantId", "TableId", "ReservationDate", "PartySize" },
                values: new object[,]
                {
                    {1, 1, 1, 1, new DateTime(2024, 3, 25, 19, 0, 0), 2 },
                    {2, 2, 1, 2, new DateTime(2024, 3, 26,20, 0, 0), 4 },
                    {3, 3, 2, 3, new DateTime(2024, 3, 27, 19, 0, 0), 3 },
                    {4, 4, 2, 4, new DateTime(2024, 3, 28, 19, 0, 0), 2 },
                    {5, 1, 1, 5, new DateTime(2024, 3, 29, 19, 0, 0), 5 }
                });


            migrationBuilder.InsertData(
                table: "Orders",
                columns: new[] { "OrderId", "ReservationId", "EmployeeId", "OrderDate", "TotalAmount" },
                values: new object[,]
                {
                    // Ensure these IDs are realistic and match existing Reservations and Employees
                    { 1, 1, 1, new DateTime(2024, 3, 25, 20, 0, 0), 100m },
                    { 2, 2, 2, new DateTime(2024, 3, 26, 20, 30, 0), 150m },
                    { 3, 3, 3, new DateTime(2024, 3, 27, 21, 0, 0), 120m },
                    { 4, 4, 4, new DateTime(2024, 3, 28, 19, 45, 0), 80m },
                    { 5, 5, 5, new DateTime(2024, 3, 29, 20, 15, 0), 200m }
                });

            migrationBuilder.InsertData(
                table: "OrderItems",
                columns: new[] { "OrderItemId", "OrderId", "ItemId", "Quantity" },
                values: new object[,]
                {
                    { 1, 1, 1, 2 },
                    { 2, 1, 2, 1 },
                    { 3, 2, 3, 3 },
                    { 4, 3, 4, 2 },
                    { 5, 4, 5, 1 },
                    { 6, 5, 1, 4 },
                    { 7, 5, 2, 1 },
                    { 8, 5, 3, 1 }
                });

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "OrderItems",
                keyColumn: "OrderItemId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8 });

            migrationBuilder.DeleteData(
                table: "Orders",
                keyColumn: "OrderId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Reservations",
                keyColumn: "ReservationId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Tables",
                keyColumn: "TableId",
                keyValues: new object[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });

            migrationBuilder.DeleteData(
                table: "MenuItems",
                keyColumn: "ItemId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "EmployeeId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });

            migrationBuilder.DeleteData(
                table: "Customers",
                keyColumn: "CustomerId",
                keyValues: new object[] { 1, 2, 3, 4, 5 });
        }

    }
}
