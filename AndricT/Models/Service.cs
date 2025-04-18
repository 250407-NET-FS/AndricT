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

    public Car Car;
    public Employee Employee;
}