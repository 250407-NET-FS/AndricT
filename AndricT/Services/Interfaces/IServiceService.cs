using Dealership.Models;

public interface IMaintenanceService {
    Service ScheduleCarMaintenance(string VIN, string city, DateTime date);
}