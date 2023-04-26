using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Validations;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController
    {
        private ICar carValidation;
        public CarController(ICar _car)
        {
            carValidation = _car;
        }

        [HttpGet]
        public IEnumerable<CarResponse> GetCars()
        {
            return carValidation.GetCars();
        }

        [HttpGet("{id}")]
        public CarResponseValidation GetCar(int id)
        {
            return carValidation.GetCar(id);
        }
        [HttpGet("branch/{id}")]
        public List<CarResponse> GetCarsByBranch(int id)
        {
            return carValidation.GetCarsByBranch(id);
        }

        [HttpGet("typeCar/{id}")]
        public List<CarResponse> GetCarsByTypeCar(int id)
        {
            return carValidation.GetCarsByTypeCar(id);
        }

        [HttpPut("{id}")]
        public CarResponseValidation PutCar(int id, CarRequest carRequest)
        {
            return carValidation.UpdateCar(id, carRequest);
        }

        [HttpPost]
        public CarResponseValidation PostCar(CarRequest carRequest)
        {
            return carValidation.CreateCar(carRequest);
        }

        [HttpDelete("{id}")]
        public CarResponseValidation DeleteCar(int id)
        {
            return carValidation.DeleteCar(id);
        }


        [HttpGet("availability/{branchId}/{initialDate}/{finalDate}")]
        public List<int> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate)
        {
            return carValidation.GetAvailableCarsByBranchAndDate(branchId, initialDate, finalDate);
        }
    }
}
