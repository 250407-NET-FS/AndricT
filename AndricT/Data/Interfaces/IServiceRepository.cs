using Dealership.Models;

public interface IServiceRepository {
    List<Service> GetAllServices();
    Service AddService(Service serviceToAdd);
    List<Service> GetAllForVIN(string VIN);
    List<Service> GetAllByEmployeeID(int employeeId);
}