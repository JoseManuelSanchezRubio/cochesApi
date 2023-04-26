using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Validations;

namespace cochesApi.Logic.Interfaces
{
    public interface ICar
    {
        List<CarResponse> GetCars();
        CarResponseValidation GetCar(int id);
        CarResponseValidation UpdateCar(int id, CarRequest carRequest);
        CarResponseValidation CreateCar(CarRequest carRequest);
        CarResponseValidation DeleteCar(int id);
        List<CarResponse> GetCarsByBranch(int id);
        List<CarResponse> GetCarsByTypeCar(int id);
        List<int> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate);
    }
}