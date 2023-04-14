using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Interfaces
{

    public interface ITypeCar
    {
        ActionResult<IEnumerable<TypeCarRequest>> GetTypeCars();
        ActionResult<TypeCarRequest> GetTypeCar(int id);
        TypeCarResponseValidation PutTypeCar(int id, TypeCarRequest typeCarRequest);
        TypeCarResponseValidation PostTypeCar(TypeCarRequest typeCarRequest);
        TypeCarResponseValidation DeleteTypeCar(int id);
    }
    
}