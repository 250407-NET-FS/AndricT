using Microsoft.EntityFrameworkCore;
using Dealership.Models;

namespace Dealership.Data;

public class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options) 
        : base(options) {}

    public DbSet<Car> Cars { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Shipment> Shipments { get; set; }

}   