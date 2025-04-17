using Microsoft.EntityFrameworkCore;
using Dealership.Models;

namespace Dealership.Data;

public class DealershipContext : DbContext
{
    public DealershipContext(DbContextOptions<DealershipContext> options) 
        : base(options) {}

    public DbSet<Car> Cars { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Receipt> Receipts { get; set; }
    public DbSet<Service> Services { get; set; }
    public DbSet<Shipment> Shipments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Shipment>()
                    .HasOne(e => e.Source)
                    .WithMany(e => e.SourceForShipments)
                    .HasForeignKey(e => e.SourceID)
                    .OnDelete(DeleteBehavior.NoAction);

        modelBuilder.Entity<Shipment>()
                    .HasOne(e => e.Destination)
                    .WithMany(e => e.DestinationForShipments)
                    .HasForeignKey(e => e.DestinationID)
                    .OnDelete(DeleteBehavior.NoAction);
    }

}   