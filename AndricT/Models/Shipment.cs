using System.ComponentModel.DataAnnotations.Schema;

namespace Dealership.Models;

public class Shipment {
    public int ShipmentID { get; set; }

    [ForeignKey(nameof(Car))]
    [Column(TypeName = "varchar(17)")]
    public string VIN { get; set; }

    public int SourceID { get; set; }

    public int DestinationID { get; set; }
    public DateTime Date { get; set; }

    // Navigators
    public virtual Location Source { get; set; }
    public virtual Location Destination { get; set; }
}