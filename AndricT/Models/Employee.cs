using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Dealership.Models;

public class Employee {
    public int EmployeeID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public int LocationID { get; set; }

    [Precision(10, 2)]
    public decimal Salary { get; set; }
}