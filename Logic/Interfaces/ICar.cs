using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Interfaces
{
    public interface ICar
    {
        ActionResult<IEnumerable<CarResponse>> GetCars();
        ActionResult<CarResponse> GetCar(int id);
        CarResponseValidation PutCar(int id, CarRequest carRequest);
        CarResponseValidation PostCar(CarRequest carRequest);
        CarResponseValidation DeleteCar(int id);
        ActionResult<List<CarResponse>> GetCarsByBranch(int id);
        ActionResult<List<CarResponse>> GetCarsByTypeCar(int id);
        ActionResult<List<CarResponse>> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate);
    }
    
}