namespace Dealership.Models;

public class Shipment {
    public string VIN { get; set; }
    public int SourceID { get; set; }
    public int DestinationID { get; set; }
}