namespace Dealership.Models;

public class Receipt {
    public string VIN { get; set; }
    public int CustomerID { get; set; }
    public int PickupLocation { get; set; }
    public decimal SellingPrice { get; set; }
}