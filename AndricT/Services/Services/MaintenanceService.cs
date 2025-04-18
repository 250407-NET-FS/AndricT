using System.Threading.Tasks;
using Dealership.Models;

public class MaintenanceService : IMaintenanceService {
    private readonly ICarRepository _carRepo;
    private readonly IEmployeeRepository _employeeRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly IServiceRepository _serviceRepo;

    public MaintenanceService(ICarRepository carRepo, IEmployeeRepository employeeRepo, 
                            ILocationRepository locationRepo, IServiceRepository serviceRepo) {
        _carRepo = carRepo;
        _employeeRepo = employeeRepo;
        _locationRepo = locationRepo;
        _serviceRepo = serviceRepo;
    }

    public async Task<Service> ScheduleCarMaintenance(MaintenanceRequestDTO maintRequest) {
        Car? carCheck = await _carRepo.GetByVINAsync(maintRequest.VIN);
        Location? locationCheck = await _locationRepo.GetByIDAsync(maintRequest.LocationID);

        if (carCheck is null) {
            throw new Exception("Car not found");
        }
        if (locationCheck is null) {
            throw new Exception("Location not found");
        }

        List<Employee> empList = await _employeeRepo.GetAllEmployeesAtAsync(maintRequest.LocationID);
        if (empList.Count < 1) {
            throw new Exception("No employees available");
        }

        int? empId = empList.ElementAt(new Random().Next(empList.Count())).EmployeeID;

        Service s = new Service() { VIN = maintRequest.VIN, EmployeeID = (int)empId, Date = maintRequest.Date };
        await _serviceRepo.AddServiceAsync(s);

        return s;
    }
}