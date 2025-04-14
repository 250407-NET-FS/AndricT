using Microsoft.EntityFrameworkCore;
using Dealership.Models;
using Dealership.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<CarContext>(opt => opt.UseInMemoryDatabase("carDb"));
var app = builder.Build();

app.MapGet("/Cars", async (CarContext db) => {
    return await db.Cars.ToListAsync<Car>();
});

app.MapPost("/Car", async (Car car, CarContext db) => {
    db.Cars.Add(car);
    await db.SaveChangesAsync();
});

app.Run();
