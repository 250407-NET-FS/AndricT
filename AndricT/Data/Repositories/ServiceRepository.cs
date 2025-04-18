using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class ServiceRepository : IServiceRepository {
    private readonly DealershipContext _dbContext;

    public ServiceRepository(DealershipContext context) {
        _dbContext = context;
    }

    public async Task<List<Service>> GetAllServicesAsync() {
        return await _dbContext.Services.ToListAsync();
    }

    public async Task AddServiceAsync(Service serviceToAdd) {
        await _dbContext.Services.AddAsync(serviceToAdd);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<Service>> GetAllForVINAsync(string VIN) {
        return await _dbContext.Services.Where(s => s.VIN == VIN)?.ToListAsync()! ?? new List<Service>();
    }

    public async Task<List<Service>> GetAllByEmployeeIDAsync(int employeeId) {
        return await _dbContext.Services.Where(s => s.EmployeeID == employeeId)?.ToListAsync()! ?? new List<Service>();
    }

    public async Task<List<Service>> GetAllBetweenDatesAsync(DateTime start, DateTime end) {
        return await _dbContext.Services.Where(s => s.Date <= end && s.Date >= start)?.ToListAsync()! ?? new List<Service>();
    }

    public async Task CancelService(int serviceId) {
        _dbContext.Services.Remove(_dbContext.Services.Find(serviceId)!);
        await _dbContext.SaveChangesAsync();
    }
}