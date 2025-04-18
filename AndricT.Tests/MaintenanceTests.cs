using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace AndricT.Tests;

public class MaintenanceTests
{
    private readonly Mock<ICarRepository> _mockCarRepo = new();
    private readonly Mock<IEmployeeRepository> _mockEmployeeRepo = new();
    private readonly Mock<ILocationRepository> _mockLocationRepo = new();
    private readonly Mock<IServiceRepository> _mockServiceRepo = new();
    private readonly IMaintenanceService maintService;

    private const string VALID_VIN = "4Y1SL65848Z411439";
    private const int VALID_EMPLOYEE_ID = 1;
    private const int VALID_LOCATION_ID = 2;
    private const int INVALID_ID = 8008135;
    private readonly Car _realCar = new Car() { VIN = VALID_VIN, Make = "Honda", Model = "Civic", Year = 2024 };
    private readonly Employee _realEmployee = new Employee() { EmployeeID = VALID_EMPLOYEE_ID, FirstName = "FName", LastName = "LName", LocationID = VALID_LOCATION_ID };
    private readonly Location _realLocation = new Location() { LocationID = 2, City = "Tampa" };

    public MaintenanceTests() {
        maintService = new MaintenanceService(_mockCarRepo.Object, _mockEmployeeRepo.Object, _mockLocationRepo.Object, _mockServiceRepo.Object);
    }

    [Fact]
    public async Task ScheduleCarMaintenance_Success_ExpectedBehavior() 
    {
        _mockCarRepo.Setup(r => r.GetByVINAsync(VALID_VIN)).ReturnsAsync(_realCar);
        _mockEmployeeRepo.Setup(r => r.GetAllEmployeesAtAsync(VALID_LOCATION_ID)).ReturnsAsync(new List<Employee> { _realEmployee });
        _mockLocationRepo.Setup(r => r.GetByIDAsync(VALID_LOCATION_ID)).ReturnsAsync(_realLocation);
        DateTime realDate = DateTime.Now;

        Service serviceToAdd = new Service() { ServiceID = 0, VIN = VALID_VIN, EmployeeID = VALID_EMPLOYEE_ID, Date = realDate };
        MaintenanceRequestDTO maintenanceRequest = new MaintenanceRequestDTO() { VIN = VALID_VIN, LocationID = VALID_LOCATION_ID, Date = realDate};

        Service s = await maintService.ScheduleCarMaintenance(maintenanceRequest);
        Assert.NotNull(s);
        Assert.Equal(s.VIN, serviceToAdd.VIN);
    }

    [Fact]
    public async Task ScheduleCarMaintenance_NoEmployeesFound_ThrowsException() 
    {
        _mockCarRepo.Setup(r => r.GetByVINAsync(VALID_VIN)).ReturnsAsync(_realCar);
        _mockEmployeeRepo.Setup(r => r.GetAllEmployeesAtAsync(VALID_LOCATION_ID)).ReturnsAsync(new List<Employee>());
        _mockLocationRepo.Setup(r => r.GetByIDAsync(VALID_LOCATION_ID)).ReturnsAsync(_realLocation);
        DateTime realDate = DateTime.Now;

        MaintenanceRequestDTO maintenanceRequest = new MaintenanceRequestDTO() { VIN = VALID_VIN, LocationID = VALID_LOCATION_ID, Date = realDate};

        var ex = await Assert.ThrowsAsync<Exception>(() => maintService.ScheduleCarMaintenance(maintenanceRequest));
        Assert.Equal("No employees available", ex.Message);
    }
}