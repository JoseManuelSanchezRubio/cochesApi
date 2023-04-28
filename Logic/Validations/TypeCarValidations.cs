using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class TypeCarResponseValidation
    {
        public TypeCarResponse? TypeCarResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public TypeCarResponseValidation(TypeCarResponse? typeCarResponse)
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

        public List<TypeCarResponse> GetTypeCars()
        {
            var types = queriesTypeCar.GetTypeCars();
            List<TypeCarResponse> typesResponse = new List<TypeCarResponse>();
            foreach (TypeCar type in types)
            {
                TypeCarResponse t = new TypeCarResponse();
                t.Id = type.Id;
                t.Name = type.Name;
                t.Brand = type.Brand;
                t.Model = type.Model;
                t.IsAutomatic = type.IsAutomatic;
                t.IsGasoline = type.IsGasoline;
                t.Price = type.Price;
                typesResponse.Add(t);
            }
            return typesResponse;
        }
        public TypeCarResponseValidation GetTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id)!;

            if (typeCar == null)
            {
                TypeCarResponseValidation trv = new TypeCarResponseValidation(null);
                trv.Status = false;
                trv.Message = "TypeCar does not exist";
            }

            TypeCarResponse typeCarResponse = new TypeCarResponse();
            typeCarResponse.Id = typeCar!.Id;
            typeCarResponse.Name = typeCar.Name;
            typeCarResponse.Brand = typeCar.Brand;
            typeCarResponse.Model = typeCar.Model;
            typeCarResponse.IsAutomatic = typeCar.IsAutomatic;
            typeCarResponse.IsGasoline = typeCar.IsGasoline;
            typeCarResponse.Price = typeCar.Price;

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            return typeCarResponseValidation;
        }

        public TypeCarResponseValidation UpdateTypeCar(int id, TypeCarRequest typeCarRequest)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);

            if (typeCar == null)
            {
                TypeCarResponseValidation trv = new TypeCarResponseValidation(null);
                trv.Status = false;
                trv.Message = "TypeCar does not exist";
                return trv;
            }

            typeCar.Name = typeCarRequest.Name;
            typeCar.Brand = typeCarRequest.Brand;
            typeCar.Model = typeCarRequest.Model;
            typeCar.IsAutomatic = typeCarRequest.IsAutomatic;
            typeCar.IsGasoline = typeCarRequest.IsGasoline;
            typeCar.Price = typeCarRequest.Price;


            TypeCarResponse typeCarResponse = new TypeCarResponse();
            typeCarResponse.Id = typeCar!.Id;
            typeCarResponse.Name = typeCar.Name;
            typeCarResponse.Brand = typeCar.Brand;
            typeCarResponse.Model = typeCar.Model;
            typeCarResponse.IsAutomatic = typeCar.IsAutomatic;
            typeCarResponse.IsGasoline = typeCar.IsGasoline;
            typeCarResponse.Price = typeCar.Price;

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            queriesDB.Update(typeCar);

            queriesDB.SaveChangesAsync();

            return typeCarResponseValidation;
        }
        public TypeCarResponseValidation CreateTypeCar(TypeCarRequest typeCarRequest)
        {
            TypeCar typeCar = new TypeCar();
            typeCar.Name = typeCarRequest.Name;
            typeCar.Brand = typeCarRequest.Brand;
            typeCar.Model = typeCarRequest.Model;
            typeCar.IsAutomatic = typeCarRequest.IsAutomatic;
            typeCar.IsGasoline = typeCarRequest.IsGasoline;
            typeCar.Price = typeCarRequest.Price;


            TypeCarResponse typeCarResponse = new TypeCarResponse();
            typeCarResponse.Id = typeCar.Id;
            typeCarResponse.Name = typeCar.Name;
            typeCarResponse.Brand = typeCar.Brand;
            typeCarResponse.Model = typeCar.Model;
            typeCarResponse.IsAutomatic = typeCar.IsAutomatic;
            typeCarResponse.IsGasoline = typeCar.IsGasoline;
            typeCarResponse.Price = typeCar.Price;

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            queriesTypeCar.AddTypeCar(typeCar);
            queriesDB.SaveChangesAsync();

            return typeCarResponseValidation;
        }
        public TypeCarResponseValidation DeleteTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);

            if (typeCar == null) return new TypeCarResponseValidation(null);

            TypeCarResponse typeCarResponse = new TypeCarResponse();
            typeCarResponse.Id = typeCar!.Id;
            typeCarResponse.Name = typeCar.Name;
            typeCarResponse.Brand = typeCar.Brand;
            typeCarResponse.Model = typeCar.Model;
            typeCarResponse.IsAutomatic = typeCar.IsAutomatic;
            typeCarResponse.IsGasoline = typeCar.IsGasoline;
            typeCarResponse.Price = typeCar.Price;

            TypeCarResponseValidation typeCarResponseValidation = new TypeCarResponseValidation(typeCarResponse);

            queriesTypeCar.RemoveTypeCar(typeCar);
            queriesDB.SaveChangesAsync();

            return typeCarResponseValidation;
        }
    }
}