using cochesApi.Logic.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Interfaces;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesCar : ICarQueries
    {
        private readonly myAppContext _context;
        public QueriesCar(myAppContext context)
        {
            _context = context;
        }

        public List<Car> GetCars(){
            return _context.Cars.ToList();
        }
        public Car GetCar(int id){
            return _context.Cars.Find(id)!;
        }
        public void RemoveCar(Car car){
            _context.Cars.Remove(car);
        }
        public void AddCar(Car car){
            _context.Cars.Add(car);
        }
        




    }
}