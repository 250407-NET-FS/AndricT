using Dealership.Models;

public interface IReceiptRepository {
    Task<List<Receipt>> GetAllReceiptsAsync();
    Task AddReceiptAsync(Receipt receiptToAdd);
    Task<bool> CheckIfSoldAsync(string VIN);
    Task<Receipt?> GetByVIN(string VIN);
}