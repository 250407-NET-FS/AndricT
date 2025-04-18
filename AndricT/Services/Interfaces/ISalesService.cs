using Dealership.Models;

public interface ISalesService {
    Task<Receipt> MarkCarSold(Receipt receipt);
    Task<List<Car>> GetCarStock(string make, string model, int year);
}