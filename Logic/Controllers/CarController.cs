using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private ICar carValidation;
        public CarController(ICar _car){
            carValidation=_car;
        }

        // GET: api/Car
        [HttpGet]
        public ActionResult<IEnumerable<CarResponse>> GetCars()
        {
            return carValidation.GetCars();
        }

        // GET: api/Car/5
        [HttpGet("{id}")]
        public ActionResult<CarResponse> GetCar(int id)
        {
            var carRequest = carValidation.GetCar(id);

            if (carRequest == null) return NotFound();

            return carRequest;
        }
        [HttpGet("branch/{id}")]
        public ActionResult<List<CarResponse>> GetCarsByBranch(int id)
        {
            if (carValidation.GetCarsByBranch(id) == null) return Problem("Branch does not exist");

            return carValidation.GetCarsByBranch(id);
        }
        [HttpGet("typeCar/{id}")]
        public ActionResult<List<CarResponse>> GetCarsByTypeCar(int id)
        {
            if (carValidation.GetCarsByTypeCar(id) == null) return Problem("TypeCar does not exist");

            return carValidation.GetCarsByTypeCar(id);
        }

        // PUT: api/Car/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public CarResponseValidation PutCar(int id, CarRequest carRequest)
        {
            return carValidation.PutCar(id, carRequest);
        }

        // POST: api/Car
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public CarResponseValidation PostCar(CarRequest carRequest)
        {
            return carValidation.PostCar(carRequest);
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        public CarResponseValidation DeleteCar(int id)
        {
            return carValidation.DeleteCar(id);
        }



        [HttpGet("availability/{branchId}/{typeCarId}/{date}")]
        public ActionResult<int> GetAvailableCarsByBranchAndDate(int branchId, int typeCarId, DateTime date)
        {
            /* if (carValidation.GetAvailableCarsByBranchAndDate(branchId, typeCarId, date) == -1) return Problem("Branch does not exist");

            if (carValidation.GetAvailableCarsByBranchAndDate(branchId, typeCarId, date) == -2) return Problem("TypeCar does not exist"); */

            return carValidation.GetAvailableCarsByBranchAndDate(branchId, typeCarId, date);
        }
    }
}
