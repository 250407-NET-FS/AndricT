using Dealership.Models;

public interface ICarRepository {
    Task<List<Car>> GetAllCarsAsync();
    Task AddCarAsync(Car carToAdd);
    Task<Car?> GetByVINAsync(string VIN);
    Task<List<Car>> GetFilteredCarsAsync(string make, string model, int year);
}