using Microsoft.EntityFrameworkCore;
using Dealership.Models;
using Dealership.Data;

var builder = WebApplication.CreateBuilder(args);
/* builder.Services.AddEndpointsApiExplorer();
builder.Services.AddDbContext<CarContext>(opt => opt.UseInMemoryDatabase("carDb"));*/
var app = builder.Build();

/*app.MapGet("/Cars", async (CarContext db) => {
    return await db.Cars.ToListAsync<Car>();
});

app.MapPost("/Car", async (Car car, CarContext db) => {
    db.Cars.Add(car);
    await db.SaveChangesAsync();
});

app.MapGet("/Customer/{Email}", async (string email, CarContext db) => {
    return await db.Customers.Find(c => c.Email == email);
});

app.MapGet("/Employees", async (CarContext db) => {
    return await db.Employees.ToListAsync<Employee>();
});

app.MapPost("/Employee", async (Employee employee, CarContext db) => {
    db.Employees.Add(employee);
    await db.SaveChangesAsync();
});

app.MapGet("/Locations", async (CarContext db) => {
    return await db.Locations.ToListAsync<Location>();
});
 */

app.Run();
