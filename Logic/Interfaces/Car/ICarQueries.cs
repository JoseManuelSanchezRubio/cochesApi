using cochesApi.Logic.Models;

namespace cochesApi.Logic.Interfaces
{
    public interface ICarQueries
    {
        List<Car> GetCars();
        Car GetCar(int id);
        void RemoveCar(Car car);
        void AddCar(Car car);
        List<Car> GetAvailableCarsByBranch(int id);
    }
}