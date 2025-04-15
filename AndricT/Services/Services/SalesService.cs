using Dealership.Models;

public class SalesService : ISalesService {
    private readonly ICarRepository _carRepo;
    private readonly ICustomerRepository _customerRepo;
    private readonly IReceiptRepository _receiptRepo;
    private readonly IShipmentRepository _shipementRepo;

    public SalesService(ICarRepository carRepo, ICustomerRepository customerRepo, IReceiptRepository receiptRepo, IShipmentRepository shipmentRepo) {
        _carRepo = carRepo;
        _customerRepo = customerRepo;
        _receiptRepo = receiptRepo;
        _shipementRepo = shipmentRepo;
    }

    public Car MarkCarSold(string vin, int customerId, int shipToId, decimal sellingPrice) {
        if (_receiptRepo.CheckIfSold(vin)) {
            throw new Exception("Car alread sold");
        }
        
        Receipt r = new Receipt() { VIN = vin, CustomerID = customerId, PickupLocation = shipToId, SellingPrice = sellingPrice };
        _receiptRepo.AddReceipt(r);

        int curLocation = _shipementRepo.GetCurrentLocationIdOf(vin);
        if (curLocation != shipToId) {
            Shipment s = new Shipment() { VIN = vin, SourceID = curLocation, DestinationID = shipToId };
            _shipementRepo.AddShipment(s);
        }

        return _carRepo.GetByVIN(vin)!;
    }
}