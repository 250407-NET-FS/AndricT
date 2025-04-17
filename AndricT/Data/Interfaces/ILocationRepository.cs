using Dealership.Models;

public interface ILocationRepository {
    Task<List<Location>> GetAllLocationsAsync();
    Task<Location?> GetByCityAsync(string city);
}