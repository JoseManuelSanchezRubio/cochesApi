using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Validations;

namespace cochesApi.Logic.Interfaces
{
    public interface ITypeCar
    {
        List<TypeCarResponse> GetTypeCars();
        TypeCarResponseValidation GetTypeCar(int id);
        TypeCarResponseValidation UpdateTypeCar(int id, TypeCarRequest typeCarRequest);
        TypeCarResponseValidation CreateTypeCar(TypeCarRequest typeCarRequest);
        TypeCarResponseValidation DeleteTypeCar(int id);
    }
}