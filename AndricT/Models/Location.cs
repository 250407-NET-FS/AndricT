using System.ComponentModel.DataAnnotations.Schema;

namespace Dealership.Models;

public class Location {
    public int LocationID { get; set; }
    public string Address { get; set; }
    public string City { get; set; }

    [InverseProperty(nameof(Shipment.Source))]
    public virtual ICollection<Shipment> SourceForShipments { get; set; }
    [InverseProperty(nameof(Shipment.Destination))]
    public virtual ICollection<Shipment> DestinationForShipments { get; set; }
}