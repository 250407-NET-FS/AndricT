using Dealership.Models;

public interface ICarRepository {
    List<Car> GetAllCars();
    Car AddCar(Car carToAdd);
    Car? GetByVIN(string VIN);
}