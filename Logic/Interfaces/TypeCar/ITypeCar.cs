using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Interfaces
{
    public interface ITypeCar
    {
        ActionResult<IEnumerable<TypeCarRequest>> GetTypeCars();
        ActionResult<TypeCarRequest> GetTypeCar(int id);
        ActionResult<TypeCarRequest> PutTypeCar(int id, TypeCarRequest typeCarRequest);
        ActionResult<TypeCarRequest> PostTypeCar(TypeCarRequest typeCarRequest);
        ActionResult<TypeCarRequest> DeleteTypeCar(int id);
    }
}