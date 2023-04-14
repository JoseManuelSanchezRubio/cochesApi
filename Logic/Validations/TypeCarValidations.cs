using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using cochesApi.DataAccess.Queries;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class TypeCarResponseValidation
    {
        public TypeCar? TypeCar { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public TypeCarResponseValidation(TypeCar? typeCar)
        {
            TypeCar = typeCar;
            Status = true;
            Message = "OK";
        }
    }

    public class TypeCarValidation : ITypeCar
    {

        private IDBQueries queries;
        private ITypeCarQueries queriesTypeCar;
        

        public TypeCarValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries, ICarQueries _queriesCar, IReservationQueries _queriesReservation, ICustomerQueries _queriesCustomer)
        {
            queriesTypeCar = _queriesTypeCar;
            queries = _queries;
        }

        public ActionResult<IEnumerable<TypeCarRequest>> GetTypeCars()
        {
            var types = queriesTypeCar.GetTypeCars();
            List<TypeCarRequest> typesRequest = new List<TypeCarRequest>();
            foreach (TypeCar type in types)
            {
                TypeCarRequest t = new TypeCarRequest(type.Name!);
                typesRequest.Add(t);
            }
            return typesRequest;
        }
        public ActionResult<TypeCarRequest> GetTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id)!;

            if (typeCar == null) return null!;

            TypeCarRequest typeCarRequest = new TypeCarRequest(typeCar.Name!);

            return typeCarRequest;
        }

        public TypeCarResponseValidation PutTypeCar(int id, TypeCarRequest typeCarRequest)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);
            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCar);

            if (typeCar == null)
            {
                typeCarResponseValidation.Status = false;
                typeCarResponseValidation.Message = "TypeCar does not exist";
                return typeCarResponseValidation;
            }

            typeCar.Name = typeCarRequest.Name;

            queries.Update(typeCar);

            queries.SaveChangesAsync();

            return typeCarResponseValidation;

        }
        public TypeCarResponseValidation PostTypeCar(TypeCarRequest typeCarRequest)
        {
            TypeCar typeCar = new TypeCar();
            typeCar.Name = typeCarRequest.Name;

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCar);

            queriesTypeCar.AddTypeCar(typeCar);
            queries.SaveChangesAsync();

            return typeCarResponseValidation;
        }
        public TypeCarResponseValidation DeleteTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);
            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCar);
            if (typeCar == null)
            {
                typeCarResponseValidation.Status = false;
                typeCarResponseValidation.Message = "TypeCar does not exist";
                return typeCarResponseValidation;
            }

            queriesTypeCar.RemoveTypeCar(typeCar);
            queries.SaveChangesAsync();

            return typeCarResponseValidation;
        }
    }
}