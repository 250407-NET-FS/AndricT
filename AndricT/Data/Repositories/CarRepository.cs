using Dealership.Data;
using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class CarRepository : ICarRepository {
    private readonly DealershipContext _dbContext;

    public CarRepository(DealershipContext context) {
        _dbContext = context;
    }
    
    public async Task<List<Car>> GetAllCarsAsync() {
        return await _dbContext.Cars.ToListAsync();
    }

    public async Task AddCarAsync(Car carToAdd) {
        await _dbContext.Cars.AddAsync(carToAdd);
        await _dbContext.SaveChangesAsync();
    }
    
    public async Task<Car?> GetByVINAsync(string VIN) {
        return (await _dbContext.Cars.FindAsync(VIN))!;
    }

    public async Task<List<Car>> GetFilteredCarsAsync(string make, string model, int year) {
        IQueryable<Car> query = _dbContext.Cars.Where(e => 1 == 1);

        if (!String.IsNullOrEmpty(make)) {
            query = query.Where(c => c.Make == make);
        }
        if (!String.IsNullOrEmpty(model)) {
            query = query.Where(c => c.Model == model);
        }
        if (year != 0) {
            query = query.Where(c => c.Year == year);
        }
        
        return await query.ToListAsync();
    }
}