using Dealership.Models;

public interface IMaintenanceService {
    Task<Service> ScheduleCarMaintenance(MaintenanceRequestDTO maintenanceRequest);
}