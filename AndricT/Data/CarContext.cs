using Microsoft.EntityFrameworkCore;
using Dealership.Models;

namespace Dealership.Data;

public class CarContext : DbContext
{
    public CarContext(DbContextOptions<CarContext> options) 
        : base(options) {}

    public DbSet<Car> Cars { get; set; }
    public DbSet<Customer> Customer { get; set; }

}   