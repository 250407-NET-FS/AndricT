using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class ShipmentRepository : IShipmentRepository {
    private readonly DealershipContext _dbContext;

    public ShipmentRepository(DealershipContext context) {
        _dbContext = context;
    }

    public async Task<List<Shipment>> GetAllShipmentsAsync() {
        return await _dbContext.Shipments.ToListAsync();
    }

    public async Task<int> GetCurrentLocationIdOfAsync(string VIN) {
        var shipments = _dbContext.Shipments.Where(c => c.VIN == VIN);
        return shipments.Any() ? shipments.OrderByDescending(c => c.Date).First().DestinationID : 1;
    }

    public async Task AddShipmentAsync(Shipment shipmentToAdd) {
        await _dbContext.Shipments.AddAsync(shipmentToAdd);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Shipment>> GetAllBetweenDatesAsync(DateTime start, DateTime end) {
        return await _dbContext.Shipments.Where(s => s.Date >= start && s.Date <= end)?.ToListAsync()! ?? new List<Shipment>();
    }

    public async Task CancelShipment(int shipmentId) {
        _dbContext.Shipments.Remove(_dbContext.Shipments.Find(shipmentId)!);
        await _dbContext.SaveChangesAsync();
    }
}