using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class TypeCarResponseValidation
    {
        public TypeCarRequest? TypeCarResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public TypeCarResponseValidation(TypeCarRequest? typeCarResponse)
        {
            TypeCarResponse = typeCarResponse;
            Status = true;
            Message = "OK";
        }
    }
    public class TypeCarValidation : ITypeCar
    {
        private IDBQueries queriesDB;
        private ITypeCarQueries queriesTypeCar;
        public TypeCarValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries, ICarQueries _queriesCar, IReservationQueries _queriesReservation, ICustomerQueries _queriesCustomer)
        {
            queriesTypeCar = _queriesTypeCar;
            queriesDB = _queries;
        }

        public List<TypeCarRequest> GetTypeCars()
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
        public TypeCarResponseValidation GetTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id)!;

            if (typeCar == null){
                TypeCarResponseValidation trv = new TypeCarResponseValidation(null);
                trv.Status = false;
                trv.Message = "TypeCar does not exist";
            }

            TypeCarRequest typeCarRequest = new TypeCarRequest(typeCar!.Name!);

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarRequest);

            return typeCarResponseValidation;
        }

        public TypeCarResponseValidation UpdateTypeCar(int id, TypeCarRequest typeCarRequest)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);

            if (typeCar == null){
                TypeCarResponseValidation trv = new TypeCarResponseValidation(null);
                trv.Status = false;
                trv.Message = "TypeCar does not exist";
                return trv;
            }

            typeCar.Name = typeCarRequest.Name;

            TypeCarRequest typeCarResponse = new TypeCarRequest(typeCarRequest.Name!);

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            queriesDB.Update(typeCar);

            queriesDB.SaveChangesAsync();

            return typeCarResponseValidation;
        }
        public TypeCarResponseValidation CreateTypeCar(TypeCarRequest typeCarRequest)
        {
            TypeCar typeCar = new TypeCar();
            typeCar.Name = typeCarRequest.Name;

            TypeCarRequest typeCarResponse = new TypeCarRequest(typeCarRequest.Name!);

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            queriesTypeCar.AddTypeCar(typeCar);
            queriesDB.SaveChangesAsync();

            return typeCarResponseValidation;
        }
        public TypeCarResponseValidation DeleteTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);

            if (typeCar == null) return null!;

            TypeCarRequest typeCarResponse = new TypeCarRequest(typeCar.Name!);

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            queriesTypeCar.RemoveTypeCar(typeCar);
            queriesDB.SaveChangesAsync();

            return typeCarResponseValidation;
        }
    }
}