namespace Dealership.Models;

public class Employee {
    public int ID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }

    public int LocationId { get; set; }
    public decimal Salary { get; set; }
}