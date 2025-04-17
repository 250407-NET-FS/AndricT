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

    public virtual Car Car { get; set; }
    public virtual Customer Customer { get; set; }
    public virtual Location Location { get; set; }
}