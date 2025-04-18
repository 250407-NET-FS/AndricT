using Dealership.Models;

public interface IServiceRepository {
    Task<List<Service>> GetAllServicesAsync();
    Task AddServiceAsync(Service serviceToAdd);
    Task<List<Service>> GetAllForVINAsync(string VIN);
    Task<List<Service>> GetAllByEmployeeIDAsync(int employeeId);
    Task<List<Service>> GetAllBetweenDatesAsync(DateTime start, DateTime end);
    Task CancelService(int serviceId);
}