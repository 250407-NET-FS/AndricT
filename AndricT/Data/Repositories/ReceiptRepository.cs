using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class ReceiptRepository : IReceiptRepository {
    private readonly DealershipContext _dbContext;

    public ReceiptRepository(DealershipContext context) {
        _dbContext = context;
    }

    public async Task<List<Receipt>> GetAllReceiptsAsync() {
        return await _dbContext.Receipts.ToListAsync();
    }

    public async Task AddReceiptAsync(Receipt receiptToAdd) {
        await _dbContext.Receipts.AddAsync(receiptToAdd);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<bool> CheckIfSoldAsync(string VIN) {
        return await _dbContext.Receipts.AnyAsync(r => r.VIN == VIN);
    }

    public async Task<Receipt?> GetByVIN(string VIN) {
        return await _dbContext.Receipts.FindAsync(VIN);
    }
}