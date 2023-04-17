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
    public class TypeCarController : ControllerBase
    {
        private ITypeCar typeCarValidation;
        public TypeCarController(ITypeCar _typeCar){
            typeCarValidation=_typeCar;
        }


        // GET: api/TypeCar
        [HttpGet]
        public ActionResult<IEnumerable<TypeCarRequest>> GetTypeCars()
        {
            return typeCarValidation.GetTypeCars();
        }

        // GET: api/TypeCar/5
        [HttpGet("{id}")]
        public ActionResult<TypeCarRequest> GetTypeCar(int id)
        {
            return typeCarValidation.GetTypeCar(id);
        }

        // PUT: api/TypeCar/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public  ActionResult<TypeCarRequest> PutTypeCar(int id, TypeCarRequest typeCarRequest)
        {
            return typeCarValidation.PutTypeCar(id, typeCarRequest);
        }

        // POST: api/TypeCar
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<TypeCarRequest> PostTypeCar(TypeCarRequest typeCarRequest)
        {
          return typeCarValidation.PostTypeCar(typeCarRequest);
        }

        // DELETE: api/TypeCar/5
        [HttpDelete("{id}")]
        public  ActionResult<TypeCarRequest> DeleteTypeCar(int id)
        {
            return typeCarValidation.DeleteTypeCar(id);
        }
    }
}
