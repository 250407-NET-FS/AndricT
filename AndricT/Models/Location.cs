using System.ComponentModel.DataAnnotations.Schema;

namespace Dealership.Models;

public class Location {
    public int LocationID { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    public ICollection<Shipment> SourceForShipments;
    public ICollection<Shipment> DestinationForShipments;
}