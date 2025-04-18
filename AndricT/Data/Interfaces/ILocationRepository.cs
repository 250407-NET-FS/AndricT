using Dealership.Models;

public interface ILocationRepository {
    Task<List<Location>> GetAllLocationsAsync();
    Task<Location?> GetByIDAsync(int locationID);
    Task<List<Location>> GetAllInCityAsync(string city);
}