using Microsoft.EntityFrameworkCore;
using Dealership.Models;
using Dealership.Data;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();

string conn_string = File.ReadAllText("./conn_string.env");
builder.Services.AddDbContext<DealershipContext>(options => options.UseSqlServer(conn_string));
Console.WriteLine(conn_string);
//builder.Services.AddSingleton<ISalesService, SalesService>();
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

app.MapGet("/Cars", (ISalesService service) => {
    return Results.Ok(service.ListAllCars());
});

app.Run();
