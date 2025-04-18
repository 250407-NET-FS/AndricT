using Dealership.Models;

public interface IShipmentRepository {
    Task<List<Shipment>> GetAllShipmentsAsync();
    Task<int?> GetCurrentLocationIdOfAsync(string VIN);
    Task AddShipmentAsync(Shipment shipmentToAdd);
    Task<List<Shipment>> GetAllBetweenDatesAsync(DateTime start, DateTime end);
    Task CancelShipment(int shipmentId);
}