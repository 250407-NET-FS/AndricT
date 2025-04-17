using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class CustomerRepository : ICustomerRepository {
    private readonly DbSet<Customer> _dbSet;

    public CustomerRepository(DbContext context) {
        _dbSet = context.Set<Customer>();
    }

    public async Task AddCustomerAsync(Customer customerToAdd) {
        await _dbSet.AddAsync(customerToAdd);
    }

    public async Task<Customer?> GetByIDAsync(int id) {
        return await _dbSet.FindAsync(id);
    }
    public async Task<Customer?> GetByEmailAsync(string email) {
        return (await _dbSet.FirstAsync(c => c.Email == email))!;
    }
    public async Task<Customer?> GetByPhoneAsync(string phone) {
        return (await _dbSet.FirstAsync(c => c.PhoneNumber == phone))!;
    }
}