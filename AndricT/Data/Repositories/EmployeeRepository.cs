using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class EmployeeRepository : IEmployeeRepository {
    private readonly DealershipContext _dbContext;

    public EmployeeRepository(DealershipContext context) {
        _dbContext = context;
    }

    public async Task AddEmployeeAsync(Employee employeeToAdd) {
        await _dbContext.Employees.AddAsync(employeeToAdd);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<Employee?> GetByIDAsync(int id) {
        return await _dbContext.Employees.FindAsync(id);
    }

    public async Task<List<Employee>> GetAllEmployeesAtAsync(int locationId) {
        return await _dbContext.Employees.Where(e => e.LocationID == locationId)?.ToListAsync()! ?? new List<Employee>();
    }
}