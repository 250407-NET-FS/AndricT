using Dealership.Models;

public interface ISalesService {
    Receipt MarkCarSold(string VIN, int customerId, string shipToCity, decimal sellingPrice);
    Task<List<Car>> ListAllCars();
}