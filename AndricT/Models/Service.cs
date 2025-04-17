using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dealership.Models;

public class Service {
    public int ServiceID { get; set; }
    [ForeignKey(nameof(Car))]
    [Column(TypeName = "varchar(17)")]
    public string VIN { get; set; }
    public int EmployeeID { get; set; }
    public DateTime Date { get; set; }

    public virtual Car Car { get; set; }
    public virtual Employee Employee { get; set; }
}