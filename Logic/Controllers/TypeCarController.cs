using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Validations;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeCarController
    {
        private ITypeCar typeCarValidation;
        public TypeCarController(ITypeCar _typeCar)
        {
            typeCarValidation = _typeCar;
        }

        [HttpGet]
        public List<TypeCarRequest> GetTypeCars()
        {
            return typeCarValidation.GetTypeCars();
        }

        [HttpGet("{id}")]
        public TypeCarResponseValidation GetTypeCar(int id)
        {
            return typeCarValidation.GetTypeCar(id);
        }

        [HttpPut("{id}")]
        public TypeCarResponseValidation PutTypeCar(int id, TypeCarRequest typeCarRequest)
        {
            return typeCarValidation.UpdateTypeCar(id, typeCarRequest);
        }

        [HttpPost]
        public TypeCarResponseValidation PostTypeCar(TypeCarRequest typeCarRequest)
        {
            return typeCarValidation.CreateTypeCar(typeCarRequest);
        }

        [HttpDelete("{id}")]
        public TypeCarResponseValidation DeleteTypeCar(int id)
        {
            return typeCarValidation.DeleteTypeCar(id);
        }
    }
}
