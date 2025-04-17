using Dealership.Models;

public interface IServiceRepository {
    Task<List<Service>> GetAllServicesAsync();
    Task AddServiceAsync(Service serviceToAdd);
    Task<List<Service>> GetAllForVINAsync(string VIN);
    Task<List<Service>> GetAllByEmployeeIDAsync(int employeeId);
}