using Dealership.Models;
using Moq;
using System.Threading.Tasks;

namespace AndricT.Tests;

public class SalesTests
{
    private readonly Mock<ICarRepository> _mockCarRepo = new();
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ILocationRepository> _mockLocationRepo = new();
    private readonly Mock<IReceiptRepository> _mockReceiptRepo = new();
    private readonly Mock<IShipmentRepository> _mockShipmentRepo = new();
    private readonly ISalesService salesService;

    private const string VALID_VIN = "4Y1SL65848Z411439";
    private const int VALID_CUST_ID = 1;
    private const int VALID_LOCATION_ID = 2;
    private readonly Car _realCar = new Car() { VIN = VALID_VIN, Make = "Honda", Model = "Civic", Year = 2024 };
    private readonly Customer _realCustomer = new Customer() { CustomerID = VALID_CUST_ID };
    private readonly Location _realLocation = new Location() { LocationID = 2, City = "Tampa" };

    public SalesTests() {
        salesService = new SalesService(_mockCarRepo.Object, _mockCustomerRepo.Object, _mockLocationRepo.Object, _mockReceiptRepo.Object, _mockShipmentRepo.Object);
    }

    [Fact]
    public async Task MarkCarSold_Success_ExpectedBehavior() 
    {
        _mockCarRepo.Setup(r => r.GetByVINAsync(VALID_VIN)).ReturnsAsync(_realCar);
        _mockCustomerRepo.Setup(r => r.GetByIDAsync(VALID_CUST_ID)).ReturnsAsync(_realCustomer);
        _mockLocationRepo.Setup(r => r.GetByIDAsync(VALID_LOCATION_ID)).ReturnsAsync(_realLocation);
        _mockReceiptRepo.Setup(r => r.CheckIfSoldAsync(VALID_VIN)).ReturnsAsync(false);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOfAsync(VALID_VIN)).ReturnsAsync(1);

        Receipt receiptToAdd = new Receipt() { VIN = VALID_VIN, CustomerID = VALID_CUST_ID, PickupLocation = VALID_LOCATION_ID, SellingPrice = 0.99m };

        await salesService.MarkCarSold(receiptToAdd);
        _mockReceiptRepo.Verify(r => r.AddReceiptAsync(receiptToAdd), Times.Once);
    }

    [Fact]
    public async Task MarkCarSold_NonexistentVIN_ThrowsException()
    {
        _mockCustomerRepo.Setup(r => r.GetByIDAsync(VALID_CUST_ID)).ReturnsAsync(_realCustomer);
        _mockLocationRepo.Setup(r => r.GetByIDAsync(VALID_LOCATION_ID)).ReturnsAsync(_realLocation);

        Receipt receiptToAdd = new Receipt() { VIN = "bad vin", CustomerID = VALID_CUST_ID, PickupLocation = VALID_LOCATION_ID, SellingPrice = 0.99m };

        var ex = await Assert.ThrowsAsync<Exception>(() => salesService.MarkCarSold(receiptToAdd));

        Assert.Equal("Car not found", ex.Message);
    }

    [Fact]
    public async Task MarkCarSold_NonexistentCustID_ThrowsException()
    {
        _mockCarRepo.Setup(r => r.GetByVINAsync(VALID_VIN)).ReturnsAsync(_realCar);
        _mockLocationRepo.Setup(r => r.GetByIDAsync(VALID_LOCATION_ID)).ReturnsAsync(_realLocation);
        _mockReceiptRepo.Setup(r => r.CheckIfSoldAsync(VALID_VIN)).ReturnsAsync(false);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOfAsync(VALID_VIN)).ReturnsAsync(1);

        Receipt receiptToAdd = new Receipt() { VIN = VALID_VIN, CustomerID = 8008135, PickupLocation = VALID_LOCATION_ID, SellingPrice = 0.99m };
        
        var ex = await Assert.ThrowsAsync<Exception>(() => salesService.MarkCarSold(receiptToAdd));

        Assert.Equal("Customer profile not found", ex.Message);
    }

    [Fact]
    public async Task MarkCarSold_InvalidCity_ThrowsException()
    {
        _mockCarRepo.Setup(r => r.GetByVINAsync(VALID_VIN)).ReturnsAsync(_realCar);
        _mockCustomerRepo.Setup(r => r.GetByIDAsync(VALID_CUST_ID)).ReturnsAsync(_realCustomer);
        _mockReceiptRepo.Setup(r => r.CheckIfSoldAsync(VALID_VIN)).ReturnsAsync(false);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOfAsync(VALID_VIN)).ReturnsAsync(1);

        Receipt receiptToAdd = new Receipt() { VIN = VALID_VIN, CustomerID = VALID_CUST_ID, PickupLocation = 8008135, SellingPrice = 0.99m };

        var ex = await Assert.ThrowsAsync<Exception>(() => salesService.MarkCarSold(receiptToAdd));

        Assert.Equal("Location not found", ex.Message);
    }

    [Fact]
    public async Task MarkCarSold_CarAlreadySold_ThrowsException()
    {
        _mockCarRepo.Setup(r => r.GetByVINAsync(VALID_VIN)).ReturnsAsync(_realCar);
        _mockCustomerRepo.Setup(r => r.GetByIDAsync(VALID_CUST_ID)).ReturnsAsync(_realCustomer);
        _mockLocationRepo.Setup(r => r.GetByIDAsync(VALID_LOCATION_ID)).ReturnsAsync(_realLocation);
        _mockReceiptRepo.Setup(r => r.CheckIfSoldAsync(VALID_VIN)).ReturnsAsync(true);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOfAsync(VALID_VIN)).ReturnsAsync(1);

        Receipt receiptToAdd = new Receipt() { VIN = VALID_VIN, CustomerID = VALID_CUST_ID, PickupLocation = VALID_LOCATION_ID, SellingPrice = 0.99m };

        var ex = await Assert.ThrowsAsync<Exception>(() => salesService.MarkCarSold(receiptToAdd));

        Assert.Equal("Car already sold", ex.Message);
    }

    [Fact]
    public async Task ListAllCars_Success_ExpectedBehavior() 
    {
        List<Car> carList = new List<Car>();
        Car car1 = _realCar;
        Car car2 = new Car() { VIN = "2T1BR32E56C640079", Make = "Toyota", Model = "Corolla", Year = 2006 };
        carList.Add(car1);
        carList.Add(car2);
        _mockCarRepo.Setup(r => r.GetAllCarsAsync()).ReturnsAsync(carList);

        Assert.True(carList.SequenceEqual(await salesService.ListAllCars()));
    }
}
