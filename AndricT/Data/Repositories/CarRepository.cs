using Dealership.Models;
using Microsoft.EntityFrameworkCore;

public class CarRepository : ICarRepository {
    private readonly DbSet<Car> _dbSet;

    public CarRepository(DbContext context) {
        _dbSet = context.Set<Car>();
    }
    
    public async Task<List<Car>> GetAllCarsAsync() {
        return await _dbSet.ToListAsync();
    }
    public async Task AddCarAsync(Car carToAdd) {
        await _dbSet.AddAsync(carToAdd);
    }
    public async Task<Car?> GetByVINAsync(string VIN) {
        return (await _dbSet.FindAsync(VIN))!;
    }
}