using Dealership.Models;

public class MaintenanceService : IMaintenanceService {
    private readonly ICarRepository _carRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly IServiceRepository _serviceRepo;

    public Service ScheduleCarMaintenance(string VIN, string city, DateTime date) {
        int? locationID =_locationRepo.GetByCity(city)?.LocationID;
        if (locationID is null) {
            throw new Exception("No location at this city");
        }

        IQueryable<Employee> empList = GetAllEmployeesAt((int)locationID);
        int? empId = empList.ElementAt(new Random().Next(empList.Count())).EmployeeID;
        if (empId is null) {
            throw new Exception("No employees found in this city");
        }

        Service s = new Service() { VIN = VIN, EmployeeID = (int)empId, Date = date };
        return _serviceRepo.AddService(s);
    }

    IQueryable<Employee> GetAllEmployeesAt(int locationID) {
        return _employeeRepo.GetAllEmployees().AsQueryable().Where(e => e.LocationID == locationID);
    }
}