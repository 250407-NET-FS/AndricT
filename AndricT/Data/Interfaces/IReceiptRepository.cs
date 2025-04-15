using Dealership.Models;

public interface IReceiptRepository {
    List<Receipt> GetAllReceipts();
    Receipt AddReceipt(Receipt receiptToAdd);
    bool CheckIfSold(string VIN);
}