using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class LocationRepository : ILocationRepository {
    private readonly DealershipContext _dbContext;

    public LocationRepository(DealershipContext context) {
        _dbContext = context;
    }

    public async Task<List<Location>> GetAllLocationsAsync() {
        return await _dbContext.Locations.ToListAsync();
    }

    public async Task<Location?> GetByIDAsync(int locationID) {
        return await _dbContext.Locations.FindAsync(locationID);
    }

    public async Task<List<Location>> GetAllInCityAsync(string city) {
        return await _dbContext.Locations.Where(l => l.City == city)?.ToListAsync()! ?? new List<Location>();
    }
}