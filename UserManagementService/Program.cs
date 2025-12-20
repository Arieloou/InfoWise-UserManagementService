using Microsoft.EntityFrameworkCore;
using UserManagementService.Application.UserTools;
using UserManagementService.Infrastructure;
using UserManagementService.Infrastructure.JWT;
using UserManagementService.Infrastructure.Repositories;
using UserManagementService.Interfaces.JWT;
using UserManagementService.Interfaces.Repositories;
using UserManagementService.Interfaces.UserTools;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IJWTGenerator, JwtGenerator>();
builder.Services.AddScoped<IJWTResponseGenerator, JwtResponseGenerator>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    db.Database.Migrate();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
