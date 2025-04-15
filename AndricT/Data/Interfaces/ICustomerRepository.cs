using Dealership.Models;

public interface ICustomerRepository {
    List<Customer> GetAllCustomers();
    Customer AddCustomer(Customer customerToAdd);
    Customer? GetByEmail(string email);
    Customer? GetByPhone(string phone);
}