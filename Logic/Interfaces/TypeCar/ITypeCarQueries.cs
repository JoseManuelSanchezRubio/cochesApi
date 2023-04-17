using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;


namespace cochesApi.Logic.Interfaces
{

    public interface ITypeCarQueries
    {
        List<TypeCar> GetTypeCars();
        TypeCar GetTypeCar(int id);
        void RemoveTypeCar(TypeCar typeCar);
        void AddTypeCar(TypeCar typeCar);
    }
    
}