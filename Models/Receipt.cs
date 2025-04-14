namespace Dealership.Models;

public class Receipt {
    public string VIN { get; set; }
    public int CustomerId { get; set; }
    public int PickupLocation { get; set; }
    public decimal SellingPrice { get; set; }
}