using Dealership.Models;

public interface IEmployeeRepository {
    Task<List<Employee>> GetAllEmployeesAsync();
    Task AddEmployeeAsync(Employee employeeToAdd);
    Task<Employee?> GetByIDAsync(int id);
    Task<List<Employee>> GetAllEmployeesAtAsync(int locationId);
}