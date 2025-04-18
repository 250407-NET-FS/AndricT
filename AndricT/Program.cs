using Microsoft.EntityFrameworkCore;
using Dealership.Models;
using Dealership.Data;
using Microsoft.AspNetCore.Mvc;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
string conn_string = File.ReadAllText("./conn_string.env");
builder.Services.AddDbContext<DealershipContext>(options => options.UseSqlServer(conn_string));
builder.Services.AddScoped<ICarRepository, CarRepository>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
builder.Services.AddScoped<ILocationRepository, LocationRepository>();
builder.Services.AddScoped<IReceiptRepository, ReceiptRepository>();
builder.Services.AddScoped<IServiceRepository, ServiceRepository>();
builder.Services.AddScoped<IShipmentRepository, ShipmentRepository>();
builder.Services.AddScoped<ISalesService, SalesService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();

builder.Services.AddOpenApiDocument(config =>
{
    config.DocumentName = "DealershipAPI";
    config.Title = "DealershipAPI v1";
    config.Version = "v1";
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseOpenApi();
    app.UseSwaggerUi(config =>
    {
        config.DocumentTitle = "DealershipAPI";
        config.Path = "/swagger";
        config.DocumentPath = "/swagger/{documentName}/swagger.json";
        config.DocExpansion = "list";
    });
}

app.MapGet("/Cars", async (ISalesService salesService,
                          [FromQuery(Name = "make")] string make = "", 
                          [FromQuery(Name = "model")] string model = "",
                          [FromQuery(Name = "year")] int year = 0) => {
    return await salesService.GetCarStock(make, model, year);
});

app.MapGet("/Car/{VIN}", async (string VIN, ICarRepository carRepo) => {
    return await carRepo.GetByVINAsync(VIN);
});

app.MapPost("/Car", async (Car car, ICarRepository carRepo) => {
    await carRepo.AddCarAsync(car);
});

app.MapGet("/Customer/{ID}", async (int ID, ICustomerRepository customerRepo) => {
    return await customerRepo.GetByIDAsync(ID);
});

app.MapPost("/Customer", async (Customer customer, ICustomerRepository customerRepo) => {
    await customerRepo.AddCustomerAsync(customer);
});

app.MapGet("/Employee/{ID}", async (int ID, IEmployeeRepository employeeRepo) => {
    return await employeeRepo.GetByIDAsync(ID);
});

app.MapGet("/Locations", async (ILocationRepository locationRepo) => {
    return await locationRepo.GetAllLocationsAsync();
});

app.MapGet("/Receipt/{VIN}", async (string VIN, IReceiptRepository receiptRepo) => {
    return await receiptRepo.GetByVIN(VIN);
});

app.MapGet("/Service/Employee/{EmployeeID}", async (string EmployeeID, IServiceRepository serviceRepo) => {
    return await serviceRepo.GetAllByEmployeeIDAsync(Int32.Parse(EmployeeID));
});

app.MapGet("/Service/Car/{VIN}", async (string VIN, IServiceRepository serviceRepo) => {
    return await serviceRepo.GetAllForVINAsync(VIN);
});

app.MapPost("/Service", async (MaintenanceRequestDTO maintenanceRequest, IMaintenanceService maintenanceService) => {
    await maintenanceService.ScheduleCarMaintenance(maintenanceRequest);
});

app.MapGet("/Shipments", async (IShipmentRepository shipmentRepo) => {
    return await shipmentRepo.GetAllShipmentsAsync();
});

app.MapPost("/Shipment", async (Shipment shipment, IShipmentRepository shipmentRepo) => {
    await shipmentRepo.AddShipmentAsync(shipment);
});

app.MapDelete("/Shipment/{ShipmentID}", async (string ShipmentID, IShipmentRepository shipmentRepo) => {
    await shipmentRepo.CancelShipment(Int32.Parse(ShipmentID));
});

app.MapPost("/Sale", async (Receipt receipt, ISalesService salesService) => {
    return await salesService.MarkCarSold(receipt);
});

app.Run();
