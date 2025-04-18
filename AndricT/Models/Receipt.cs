using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dealership.Models;

public class Receipt {
    [Key]
    [ForeignKey(nameof(Car))]
    [Column(TypeName = "varchar(17)")]
    public string VIN { get; set; }
    public int CustomerID { get; set; }
    [ForeignKey(nameof(Location))]
    public int PickupLocation { get; set; }
    [Precision(10, 2)]
    public decimal SellingPrice { get; set; }

    public Car Car;
    public Customer Customer;
    public Location Location;
}