using System.Reflection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using RestaurantReservation.API.Authentication;
using RestaurantReservation.API.Validators;
using RestaurantReservation.Db;
using RestaurantReservation.Db.Repositories.CustomerRepository;
using RestaurantReservation.Db.Repositories.EmployeeRepository;
using RestaurantReservation.Db.Repositories.MenuItemRepository;
using RestaurantReservation.Db.Repositories.OrderItemRepository;
using RestaurantReservation.Db.Repositories.OrderRepository;
using RestaurantReservation.Db.Repositories.ReservationRepository;
using RestaurantReservation.Db.Repositories.RestaurantRepository;
using RestaurantReservation.Db.Repositories.TableRepository;
using RestaurantReservation.Db.Repositories.UserRepository;

var builder = WebApplication.CreateBuilder(args);

// Configure services
builder.Services
    .AddControllers()
    .AddFluentValidation(v =>  v.RegisterValidatorsFromAssembly(Assembly.GetExecutingAssembly()))
    .AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());

builder.Services.AddScoped<EntityValidator>();

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
builder.Services.AddScoped<ITableRepository, TableRepository>();


//Authentication
var jwtConfig = builder.Configuration.GetSection("JwtAuthentication").Get<JwtAuthenticationConfig>();

builder.Services.AddSingleton(jwtConfig);

builder.Services.AddScoped<IJwtTokenGenerator, JwtTokenGenerator>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtConfig.Issuer,
            ValidAudience = jwtConfig.Audience,
            IssuerSigningKey = new SymmetricSecurityKey(Convert.FromBase64String(jwtConfig.SecretKey)),
            ClockSkew = TimeSpan.Zero

        };
    });


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