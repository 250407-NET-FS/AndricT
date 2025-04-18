using System.ComponentModel.DataAnnotations.Schema;

namespace Dealership.Models;

public class Customer {
    public int CustomerID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string? Email { get; set; }

    // 123-456-7890 - phone numbers should be stored without dashes
    [Column(TypeName = "varchar(10)")]
    public string? PhoneNumber { get; set; }
}