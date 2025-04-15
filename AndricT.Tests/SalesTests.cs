using Dealership.Models;
using Dealership.Data;
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

    private const string VALID_VIN = "4Y1SL65848Z411439";
    private const int VALID_CUST_ID = 1;
    private const string VALID_CITY = "Tampa";
    private readonly Car _realCar = new Car() { VIN = VALID_VIN, Make = "Honda", Model = "Civic", Year = 2024 };
    private readonly Customer _realCustomer = new Customer() { ID = VALID_CUST_ID };
    private readonly Location _realLocation = new Location() { ID = 2, City = VALID_CITY };

    public SalesTests() {
        salesService = new SalesService(_mockCarRepo.Object, _mockCustomerRepo.Object, _mockLocationRepo.Object, _mockReceiptRepo.Object, _mockShipmentRepo.Object);
    }

    [Fact]
    public void MarkCarSold_Success_ExpectedBehavior()
    {
        Receipt expectedReceipt = new Receipt() { VIN = VALID_VIN, CustomerID = VALID_CUST_ID, PickupLocation = 2, SellingPrice = 0.99m };

        _mockCarRepo.Setup(r => r.GetByVIN(VALID_VIN)).Returns(_realCar);
        _mockCustomerRepo.Setup(r => r.GetByID(VALID_CUST_ID)).Returns(_realCustomer);
        _mockLocationRepo.Setup(r => r.GetByCity(VALID_CITY)).Returns(_realLocation);
        _mockReceiptRepo.Setup(r => r.CheckIfSold(VALID_VIN)).Returns(false);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOf(VALID_VIN)).Returns(1);

        var actualReceipt = salesService.MarkCarSold(VALID_VIN, VALID_CUST_ID, VALID_CITY, 0.99m);

        Assert.NotNull(actualReceipt);
        Assert.Equal(expectedReceipt.VIN, actualReceipt.VIN);
        Assert.Equal(expectedReceipt.CustomerID, actualReceipt.CustomerID);
        Assert.Equal(expectedReceipt.PickupLocation, actualReceipt.PickupLocation);
        Assert.Equal(expectedReceipt.SellingPrice, actualReceipt.SellingPrice);
    }

    [Fact]
    public void MarkCarSold_NonexistentVIN_ThrowsException()
    {
        _mockCustomerRepo.Setup(r => r.GetByID(VALID_CUST_ID)).Returns(_realCustomer);
        _mockLocationRepo.Setup(r => r.GetByCity(VALID_CITY)).Returns(_realLocation);

        var ex = Assert.Throws<Exception>(() => salesService.MarkCarSold("bad vin", VALID_CUST_ID, VALID_CITY, 0.99m));
        Assert.Equal("Car not found", ex.Message);
    }

    [Fact]
    public void MarkCarSold_NonexistentCustID_ThrowsException()
    {
        _mockCarRepo.Setup(r => r.GetByVIN(VALID_VIN)).Returns(_realCar);
        _mockLocationRepo.Setup(r => r.GetByCity(VALID_CITY)).Returns(_realLocation);
        _mockReceiptRepo.Setup(r => r.CheckIfSold(VALID_VIN)).Returns(false);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOf(VALID_VIN)).Returns(1);

        var ex = Assert.Throws<Exception>(() => salesService.MarkCarSold(VALID_VIN, 651651, VALID_CITY, 0.99m));
        Assert.Equal("Customer profile not found", ex.Message);
    }

    [Fact]
    public void MarkCarSold_InvalidCity_ThrowsException()
    {
        _mockCarRepo.Setup(r => r.GetByVIN(VALID_VIN)).Returns(_realCar);
        _mockCustomerRepo.Setup(r => r.GetByID(VALID_CUST_ID)).Returns(_realCustomer);
        _mockReceiptRepo.Setup(r => r.CheckIfSold(VALID_VIN)).Returns(false);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOf(VALID_VIN)).Returns(1);

        var ex = Assert.Throws<Exception>(() => salesService.MarkCarSold(VALID_VIN, VALID_CUST_ID, "Atlantis", 0.99m));
        Assert.Equal("No location at this city", ex.Message);
    }

    [Fact]
    public void MarkCarSold_CarAlreadySold_ThrowsException()
    {
        _mockCarRepo.Setup(r => r.GetByVIN(VALID_VIN)).Returns(_realCar);
        _mockLocationRepo.Setup(r => r.GetByCity(VALID_CITY)).Returns(_realLocation);
        _mockCustomerRepo.Setup(r => r.GetByID(VALID_CUST_ID)).Returns(_realCustomer);
        _mockReceiptRepo.Setup(r => r.CheckIfSold(VALID_VIN)).Returns(true);
        _mockShipmentRepo.Setup(r => r.GetCurrentLocationIdOf(VALID_VIN)).Returns(1);

        var ex = Assert.Throws<Exception>(() => salesService.MarkCarSold(VALID_VIN, VALID_CUST_ID, VALID_CITY, 0.99m));
        Assert.Equal("Car already sold", ex.Message);
    }
}
