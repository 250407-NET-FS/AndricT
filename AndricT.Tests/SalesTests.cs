using Dealership.Models;
using Moq;

namespace AndricT.Tests;

public class SalesTests
{
    private readonly Mock<ICarRepository> _mockCarRepo = new();
    private readonly Mock<ICustomerRepository> _mockCustomerRepo = new();
    private readonly Mock<ILocationRepository> _mockLocationRepo = new();
    private readonly Mock<IReceiptRepository> _mockReceiptRepo = new();
    private readonly Mock<IShipmentRepository> _mockShipmentRepo = new();
    private readonly ISalesService salesService;

    private const string VALID_VIN = "2T3DK4DV8CW082696"; 
    private const string VALID_VIN2 = "JF1GR7E64DG203230";
    private const int VALID_CUST_ID = 1;
    private const int VALID_LOCATION_ID = 2;
    private const int INVALID_ID = 8008135;
    private readonly Car _realCar = new Car() { VIN = VALID_VIN, Make = "Toyota", Model = "Rav4", Year = 2012 };
    private readonly Car _realCar2 = new Car() { VIN = VALID_VIN2, Make = "Subaru", Model = "Impreza", Year = 2013 };
    private readonly Car _realCar3 = new Car() { VIN = "JT3HJ85J6T0133046", Make = "Toyota", Model = "Land Cruiser", Year = 1996 };
    private readonly Receipt _realReceipt = new Receipt { VIN = VALID_VIN, CustomerID = VALID_CUST_ID, PickupLocation = VALID_LOCATION_ID, SellingPrice = 0.99m };
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

        await salesService.MarkCarSold(_realReceipt);
        _mockReceiptRepo.Verify(r => r.AddReceiptAsync(_realReceipt), Times.Once);
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

        Receipt receiptToAdd = new Receipt() { VIN = VALID_VIN, CustomerID = INVALID_ID, PickupLocation = VALID_LOCATION_ID, SellingPrice = 0.99m };
        
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

        Receipt receiptToAdd = new Receipt() { VIN = VALID_VIN, CustomerID = VALID_CUST_ID, PickupLocation = INVALID_ID, SellingPrice = 0.99m };

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

        var ex = await Assert.ThrowsAsync<Exception>(() => salesService.MarkCarSold(_realReceipt));

        Assert.Equal("Car already sold", ex.Message);
    }

    [Fact]
    public async Task GetCarStock_Success_ExpectedBehavior()
    {
        List<Car> carList = new List<Car> { _realCar, _realCar2 };
        List<Receipt> receiptList = new List<Receipt> { _realReceipt };
        List<Car> expectedStock = new List<Car> { _realCar2 };
        _mockCarRepo.Setup(r => r.GetFilteredCarsAsync("", "", 0)).ReturnsAsync(carList);
        _mockReceiptRepo.Setup(r => r.GetAllReceiptsAsync()).ReturnsAsync(receiptList);

        List<Car> actualStock = await salesService.GetCarStock("", "", 0);

        Assert.Equal(expectedStock, actualStock);
    }
}
