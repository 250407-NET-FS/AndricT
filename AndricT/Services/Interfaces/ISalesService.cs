using Dealership.Models;

public interface ISalesService {
    Car MarkCarSold(string VIN, int customerId, int locationId, decimal sellingPrice);
}