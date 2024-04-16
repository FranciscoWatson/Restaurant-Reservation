using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Serialization;
using RestaurantReservation.API.DTOs;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories.CustomerRepository;
using RestaurantReservation.Db.Repositories.EmployeeRepository;
using RestaurantReservation.Db.Repositories.MenuItemRepository;
using RestaurantReservation.Db.Repositories.OrderItemRepository;
using RestaurantReservation.Db.Repositories.OrderRepository;
using RestaurantReservation.Db.Repositories.ReservationRepository;
using RestaurantReservation.Db.Repositories.RestaurantRepository;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services
    .AddControllers()
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddDbContext<RestaurantReservationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlServerConnection")));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IReservationRepository, ReservationRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<IMenuItemRepository, MenuItemRepository>();
builder.Services.AddScoped<IOrderRepository, OrderRepository>();
builder.Services.AddScoped<IOrderItemRepository, OrderItemRepository>();
builder.Services.AddScoped<IRestaurantRepository, RestaurantRepository>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();