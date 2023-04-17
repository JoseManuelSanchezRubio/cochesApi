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
        public CarController(ICar _car)
        {
            carValidation = _car;
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
            return carValidation.GetCar(id);
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
        public ActionResult<CarResponse> PostCar(CarRequest carRequest)
        {
            return carValidation.PostCar(carRequest);
        }

        // DELETE: api/Car/5
        [HttpDelete("{id}")]
        public CarResponseValidation DeleteCar(int id)
        {
            return carValidation.DeleteCar(id);
        }


        [HttpGet("availability/{branchId}/{initialDate}/{finalDate}")]
        public ActionResult<List<CarResponse>> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate)
        {
            return carValidation.GetAvailableCarsByBranchAndDate(branchId, initialDate, finalDate);
        }
    }
}
