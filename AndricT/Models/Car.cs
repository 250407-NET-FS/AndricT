using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dealership.Models;

public class Car {
    [Key]
    [Column(TypeName = "varchar(17)")]
    public string VIN { get; set; }
    public string Make { get; set; }
    public string Model { get; set; }
    public int Year { get; set; }
    [Precision(10, 2)]
    public decimal? StartingPrice { get; set; }
}