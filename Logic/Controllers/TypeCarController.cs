using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TypeCarController : ControllerBase
    {
        private ITypeCar typeCarValidation;
        public TypeCarController(ITypeCar _typeCar)
        {
            typeCarValidation = _typeCar;
        }

        [HttpGet]
        public ActionResult<IEnumerable<TypeCarRequest>> GetTypeCars()
        {
            return typeCarValidation.GetTypeCars();
        }

        [HttpGet("{id}")]
        public ActionResult<TypeCarRequest> GetTypeCar(int id)
        {
            return typeCarValidation.GetTypeCar(id);
        }

        [HttpPut("{id}")]
        public ActionResult<TypeCarRequest> PutTypeCar(int id, TypeCarRequest typeCarRequest)
        {
            return typeCarValidation.PutTypeCar(id, typeCarRequest);
        }

        [HttpPost]
        public ActionResult<TypeCarRequest> PostTypeCar(TypeCarRequest typeCarRequest)
        {
            return typeCarValidation.PostTypeCar(typeCarRequest);
        }

        [HttpDelete("{id}")]
        public ActionResult<TypeCarRequest> DeleteTypeCar(int id)
        {
            return typeCarValidation.DeleteTypeCar(id);
        }
    }
}
