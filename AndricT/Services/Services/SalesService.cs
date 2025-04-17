using Dealership.Models;

public class SalesService : ISalesService {
    private readonly ICarRepository _carRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly ILocationRepository _locationRepo;
    private readonly IReceiptRepository _receiptRepo;
    private readonly IShipmentRepository _shipmentRepo;

    public SalesService(ICarRepository carRepo, ICustomerRepository customerRepo, 
                        ILocationRepository locationRepo, IReceiptRepository receiptRepo,
                        IShipmentRepository shipmentRepo) {
        _carRepo = carRepo;
        _customerRepo = customerRepo;
        _locationRepo = locationRepo;
        _receiptRepo = receiptRepo;
        _shipmentRepo = shipmentRepo;
    }

    public Receipt MarkCarSold(string vin, int customerId, string shipToCity, decimal sellingPrice) {
        int? locationID;

        if (_carRepo.GetByVINAsync(vin) is null) {
            throw new Exception("Car not found");
        }
        if (_customerRepo.GetByID(customerId) is null) {
            throw new Exception("Customer profile not found");
        }
        if ((locationID = _locationRepo.GetByCity(shipToCity)?.LocationID) is null) {
            throw new Exception("No location at this city");
        }
        if (_receiptRepo.CheckIfSold(vin)) {
            throw new Exception("Car already sold");
        }

        Receipt r = new Receipt() { VIN = vin, CustomerID = customerId, PickupLocation = (int)locationID, SellingPrice = sellingPrice };
        _receiptRepo.AddReceipt(r);

        int curLocation = _shipmentRepo.GetCurrentLocationIdOf(vin);
        if (curLocation != locationID) {
            Shipment s = new Shipment() { VIN = vin, SourceID = curLocation, DestinationID = (int)locationID };
            _shipmentRepo.AddShipment(s);
        }

        return r;
    }

    public Task<List<Car>> ListAllCars() {
        return _carRepo.GetAllCarsAsync();
    }
}