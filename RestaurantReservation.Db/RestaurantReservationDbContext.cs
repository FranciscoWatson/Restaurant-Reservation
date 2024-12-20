﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestaurantReservation.Db.Models;
using RestaurantReservation.Db.Views;

namespace RestaurantReservation.Db
{
    public class RestaurantReservationDbContext : DbContext
    {
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Restaurant> Restaurants { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Table> Tables { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<ReservationDetailsView> ReservationDetails { get; set; }
        public DbSet<EmployeeDetailsView> EmployeeDetails { get; set; }


        public RestaurantReservationDbContext(DbContextOptions<RestaurantReservationDbContext> options)
        : base(options) { }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<MenuItem>()
                .Property(m => m.Price)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<Order>()
                .Property(o => o.TotalAmount)
                .HasColumnType("decimal(18,2)");

            modelBuilder.Entity<ReservationDetailsView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("ReservationDetailsView");
            });

            modelBuilder.Entity<EmployeeDetailsView>(entity =>
            {
                entity.HasNoKey();
                entity.ToView("EmployeeDetailsView");

            });

            modelBuilder.HasDbFunction(typeof(RestaurantReservationDbContext)
              .GetMethod(nameof(CalculateTotalRevenue)))
              .HasName("CalculateTotalRevenue");

        }

        [DbFunction(Name = "CalculateTotalRevenue")]
        public decimal CalculateTotalRevenue(int restaurantId) => throw new NotSupportedException();

    }
}
