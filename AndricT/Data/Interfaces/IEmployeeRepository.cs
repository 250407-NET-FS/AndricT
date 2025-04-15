using Dealership.Models;

public interface IEmployeeRepository {
    List<Employee> GetAllEmployees();
    Employee AddEmployee(Employee employeeToAdd);
    Employee? GetByID(int id);
    Employee? RemoveEmployee(int id);
    List<Employee> GetAllEmployeesAt(int locationId);
}