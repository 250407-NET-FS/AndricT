using Dealership.Models;

public interface ICustomerRepository {
    Task AddCustomerAsync(Customer customerToAdd);
    Task<Customer?> GetByIDAsync(int id);
    Task<Customer?> GetByEmailAsync(string email);
    Task<Customer?> GetByPhoneAsync(string phone);
}