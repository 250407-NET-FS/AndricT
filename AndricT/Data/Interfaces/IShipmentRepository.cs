using Dealership.Models;

public interface IShipmentRepository {
    Task<List<Shipment>> GetAllShipmentsAsync();
    Task<int> GetCurrentLocationIdOfAsync(string VIN);
    Task<Shipment> AddShipmentAsync(Shipment shipmentToAdd);
    // Method to get shipments by date range? last 30 days?
}