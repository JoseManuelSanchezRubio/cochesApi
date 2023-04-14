using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;


namespace cochesApi.Logic.Interfaces
{

    public interface ICarQueries
    {
        List<Car> GetCars();
        Car GetCar(int id);
        void RemoveCar(Car car);
        void AddCar(Car car);
    }
    
}