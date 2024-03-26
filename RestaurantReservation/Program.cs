using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Repositories.CustomerRepository;
using RestaurantReservation.Db.Repositories.EmployeeRepository;
using RestaurantReservation.Db.Repositories.MenuItemRepository;
using RestaurantReservation.Db.Repositories.OrderItemRepository;
using RestaurantReservation.Db.Repositories.OrderRepository;
using RestaurantReservation.Db.Repositories.ReservationRepository;
using RestaurantReservation.Db.Repositories.RestaurantRepository;

IConfigurationRoot configuration = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json")
               .Build();

var contextOptions = new DbContextOptionsBuilder<RestaurantReservationDbContext>()
    .UseSqlServer(configuration.GetConnectionString("SqlServerConnection"))
    .Options;



while (true)
{
    Console.WriteLine("\nRestaurant Reservation System");
    Console.WriteLine("1. Manage Entities (Create/Update/Delete)");
    Console.WriteLine("2. List Managers");
    Console.WriteLine("3. Get Reservations By Customer");
    Console.WriteLine("4. List Orders And Menu Items");
    Console.WriteLine("5. List Ordered Menu Items");
    Console.WriteLine("6. Calculate Average Order Amount");
    Console.WriteLine("7. Query Reservation Details View");
    Console.WriteLine("8. Query Employee Details View");
    Console.WriteLine("9. Calculate Total Revenue By Restaurant");
    Console.WriteLine("10. Find Customers By Party Size");
    Console.WriteLine("0. Exit");
    Console.Write("Select an option: ");

    int option = Convert.ToInt32(Console.ReadLine());

    switch (option)
    {
        case 1:
            // CRUD methods for each repository
            break;
        case 2:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                var employeeRepository = new EmployeeRepository(dbContext);

                var employeeManagers = await employeeRepository.ListManagersAsync();

                Console.WriteLine("Managers:");
                foreach (var manager in employeeManagers)
                {
                    Console.WriteLine($"- {manager.FirstName} {manager.LastName}");
                }
            }
            break;
       
        case 3:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                Console.Write("Enter Customer ID: ");
                int customerId = Convert.ToInt32(Console.ReadLine());

                var reservationRepository = new ReservationRepository(dbContext);
                var reservations = await reservationRepository.GetReservationsByCustomer(customerId);

                Console.WriteLine($"Reservations for Customer {customerId}:");
                foreach (var r in reservations)
                {
                    Console.WriteLine($"- {r.ReservationId} {r.ReservationDate} {r.PartySize} {r.RestaurantId} {r.TableId}");
                }
            }

            
            break;

        case 4:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                Console.Write("Enter Reservation ID: ");
                int reservationIdForOrders = Convert.ToInt32(Console.ReadLine());

                var orderRepository = new OrderRepository(dbContext);
                var orders = await orderRepository.ListOrdersAndMenuItems(reservationIdForOrders);
                if (!orders.Any())
                {
                    Console.WriteLine($"No orders found for reservation ID {reservationIdForOrders}.");
                    break;
                }

                Console.WriteLine($"Orders for Reservation ID {reservationIdForOrders}:");
                foreach (var order in orders)
                {
                    Console.WriteLine($"\nOrder ID: {order.OrderId}, Total Amount: {order.TotalAmount}");
                    Console.WriteLine("Menu Items:");
                    foreach (var item in order.OrderItems)
                    {
                        Console.WriteLine($"- {item.MenuItem.Name}: {item.MenuItem.Description}, Price: {item.MenuItem.Price}, Order: {item.Quantity}");
                    }
                }
            }


                
            break;

        case 5:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                Console.Write("Enter Reservation ID: ");
                int reservationIdForMenuItems = Convert.ToInt32(Console.ReadLine());

                var menuItemRepository = new MenuItemRepository(dbContext);
                var menuItems = await menuItemRepository.ListOrderedMenuItems(reservationIdForMenuItems);
                if (!menuItems.Any())
                {
                    Console.WriteLine($"No menu items found for reservation ID {reservationIdForMenuItems}.");
                    break;
                }

                Console.WriteLine($"Menu Items Ordered for Reservation ID {reservationIdForMenuItems}:");
                foreach (var item in menuItems)
                {
                    Console.WriteLine($"- {item.Name}: {item.Description}, Price: {item.Price}");
                }
            }
            break;

        case 6:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                Console.Write("Enter Employee ID: ");
                int employeeIdForAverage = Convert.ToInt32(Console.ReadLine());

                var orderItemRepository = new OrderItemRepository(dbContext);
                var averageAmount = await orderItemRepository.CalculateAverageOrderAmountAsync(employeeIdForAverage);
                Console.WriteLine($"The average order amount for employee ID {employeeIdForAverage} is: {averageAmount:C}");
            }
                
            break;
        case 7:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                var reservationDetails = await dbContext.ReservationDetails.ToListAsync();
                Console.WriteLine("Reservation Details:");
                foreach (var rd in reservationDetails)
                {
                    Console.WriteLine($"Reservation ID: {rd.ReservationId}, Table ID: {rd.TableId}, " +
                                      $"Date: {rd.ReservationDate}, Party Size: {rd.ReservationPartySize}, " +
                                      $"Customer ID: {rd.CustomerId}, Customer Name: {rd.CustomerFirstName} {rd.CustomerLastName}, " +
                                      $"Customer Email: {rd.CustomerEmail}, Customer Phone: {rd.CustomerPhoneNumber}, " +
                                      $"Restaurant ID: {rd.RestaurantId}, Restaurant Name: {rd.RestaurantName}, " +
                                      $"Restaurant Phone: {rd.RestaurantPhoneNumber}, Opening Hours: {rd.RestaurantOpeningHours}");
                }
            }
                
            break;

        case 8:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {
                var employeeDetails = await dbContext.EmployeeDetails.ToListAsync();
                Console.WriteLine("Employee Details:");
                foreach (var ed in employeeDetails)
                {
                    Console.WriteLine($"Employee ID: {ed.EmployeeId}, Name: {ed.EmployeeFirstName} {ed.EmployeeLastName}, " +
                                      $"Position: {ed.EmployeePosition}, Restaurant ID: {ed.RestaurantId}, " +
                                      $"Restaurant Name: {ed.RestaurantName}, Address: {ed.RestaurantAddress}, " +
                                      $"Phone: {ed.RestaurantPhoneNumber}, Opening Hours: {ed.RestaurantOpeningHours}");
                }
            }
            break;

        case 9:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {

                Console.Write("Enter Restaurant ID: ");
                int restaurantId = Convert.ToInt32(Console.ReadLine());
                var restaurantRepository = new RestaurantRepository(dbContext);
                var totalRevenue = await restaurantRepository.CalculateTotalRevenueAsync(restaurantId);
                Console.WriteLine($"Total revenue for restaurant ID {restaurantId}: {totalRevenue:C}");
            }
            break;


        case 10:
            using (var dbContext = new RestaurantReservationDbContext(contextOptions))
            {

                Console.Write("Enter PartySize: ");
                int partySize = Convert.ToInt32(Console.ReadLine());
                var customerRespository = new CustomerRepository(dbContext);
                var customers = await customerRespository.FindCustomersByPartySizeAsync(partySize);
                Console.WriteLine($"Customers with party size {partySize}:");
                foreach (var customer in customers)
                {
                    Console.WriteLine($"ID: {customer.CustomerId}, Name: {customer.FirstName} {customer.LastName}, Email: {customer.Email}, Phone: {customer.PhoneNumber ?? "N/A"}");
                }
            }
            break;
        case 0:
            return;
        default:
            Console.WriteLine("Invalid option, please try again.");
            break;
    }
}