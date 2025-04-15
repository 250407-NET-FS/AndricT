using Dealership.Models;

public interface IShipmentRepository {
    List<Shipment> GetAllShipments();
    int GetCurrentLocationIdOf(string VIN);
    Shipment AddShipment(Shipment shipmentToAdd);
    // add function to get 30 days worth of shipments before specified date
}