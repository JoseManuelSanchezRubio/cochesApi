using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Interfaces
{
    public interface ICar
    {
        ActionResult<IEnumerable<CarResponse>> GetCars();
        ActionResult<CarResponse> GetCar(int id);
        ActionResult<CarResponse> PutCar(int id, CarRequest carRequest);
        ActionResult<CarResponse> PostCar(CarRequest carRequest);
        ActionResult<CarResponse> DeleteCar(int id);
        ActionResult<List<CarResponse>> GetCarsByBranch(int id);
        ActionResult<List<CarResponse>> GetCarsByTypeCar(int id);
        ActionResult<List<CarResponse>> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate);
    }
    
}