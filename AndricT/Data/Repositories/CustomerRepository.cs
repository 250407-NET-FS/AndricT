using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository {
    private readonly DealershipContext _dbContext;

    public CustomerRepository(DealershipContext context) {
        _dbContext = context;
    }

    public async Task AddCustomerAsync(Customer customerToAdd) {
        await _dbContext.Customers.AddAsync(customerToAdd);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Customer?> GetByIDAsync(int id) {
        return await _dbContext.Customers.FindAsync(id);
    }

    public async Task<Customer?> GetByEmailAsync(string email) {
        return (await _dbContext.Customers.FirstAsync(c => c.Email == email))!;
    }
    
    public async Task<Customer?> GetByPhoneAsync(string phone) {
        return (await _dbContext.Customers.FirstAsync(c => c.PhoneNumber == phone))!;
    }
}