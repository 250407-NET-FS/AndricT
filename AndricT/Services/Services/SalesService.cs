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

    public async Task<Receipt> MarkCarSold(Receipt receipt) {
        Car? vinCheck = await _carRepo.GetByVINAsync(receipt.VIN);
        Customer? customerCheck = await _customerRepo.GetByIDAsync(receipt.CustomerID);
        Location? locationCheck = await _locationRepo.GetByIDAsync(receipt.PickupLocation);

        if (vinCheck is null) {
            throw new Exception("Car not found");
        }
        if (customerCheck is null) {
            throw new Exception("Customer profile not found");
        }
        if (locationCheck is null) {
            throw new Exception("Location not found");
        }
        if (await _receiptRepo.CheckIfSoldAsync(receipt.VIN)) {
            throw new Exception("Car already sold");
        }

        await _receiptRepo.AddReceiptAsync(receipt);

        int curLocation = await _shipmentRepo.GetCurrentLocationIdOfAsync(receipt.VIN);
        if (curLocation != receipt.PickupLocation) {
            Shipment s = new Shipment() { VIN = receipt.VIN, SourceID = curLocation, DestinationID = receipt.PickupLocation };
            await _shipmentRepo.AddShipmentAsync(s);
        }

        return receipt;
    }

    public async Task<List<Car>> GetCarStock(string make, string model, int year) {
        List<Car> carList = await _carRepo.GetFilteredCarsAsync(make, model, year);
        List<Receipt> receiptList = await _receiptRepo.GetAllReceiptsAsync();
        return carList.Where(c => !receiptList.Where(r => r.VIN == c.VIN).Any()).ToList();
    }
}