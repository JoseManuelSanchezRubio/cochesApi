using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using cochesApi.DataAccess.Queries;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class CarResponseValidation
    {
        public Car? Car { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public CarResponseValidation(Car? car)
        {
            Car = car;
            Status = true;
            Message = "OK";
        }
    }

    public class CarValidation : ICar
    {
        private IDBQueries queries;
        private IBranchQueries queriesBranch;
        private IPlanningQueries queriesPlanning;
        private ITypeCarQueries queriesTypeCar;
        private ICarQueries queriesCar;
        

        public CarValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries, ICarQueries _queriesCar)
        {
            queriesBranch = _queriesBranch;
            queriesPlanning = _queriesPlanning;
            queriesTypeCar = _queriesTypeCar;
            queries = _queries;
            queriesCar = _queriesCar;
        }


        public ActionResult<IEnumerable<CarResponse>> GetCars()
        {
            var cars = queriesCar.GetCars();
            List<CarResponse> carsResponse = new List<CarResponse>();
            foreach (Car car in cars)
            {
                CarResponse carResponse = new CarResponse();
                carResponse.Id = car.Id;
                carResponse.BranchId = car.BranchId;
                carResponse.Brand = car.Brand;
                carResponse.Model = car.Model;
                carResponse.TypeCarId = car.TypeCarId;
                carsResponse.Add(carResponse);
            }
            return carsResponse;
        }
        public ActionResult<CarResponse> GetCar(int id)
        {
            var car = queriesCar.GetCar(id);

            CarResponse carResponse = new CarResponse();
            carResponse.Id = car.Id;
            carResponse.BranchId = car.BranchId;
            carResponse.Brand = car.Brand;
            carResponse.Model = car.Model;
            carResponse.TypeCarId = car.TypeCarId;
            return carResponse;
        }

        public CarResponseValidation PutCar(int id, CarRequest carRequest)
        {
            var car = queriesCar.GetCar(id); // por aqui
            var branch = queriesBranch.GetBranch(carRequest.BranchId);
            var typeCar = queriesTypeCar.GetTypeCar(carRequest.TypeCarId);

            CarResponseValidation carResponseValidation = new CarResponseValidation(car);

            if (branch == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "Branch does not exist";
                return carResponseValidation;
            }
            if (typeCar == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "TypeCar does not exist";
                return carResponseValidation;
            }
            if (car == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "Car does not exist";
                return carResponseValidation;
            }

            car.Model = carRequest.Model;
            car.Brand = carRequest.Brand;
            car.BranchId = carRequest.BranchId;
            car.TypeCarId = carRequest.TypeCarId;
            car.Branch = branch;

            if (!branch.Cars.Contains(car)) branch.Cars.Add(car); //Falta hacer cambios en planning si cambia de sucursal y/o tipo
            if (!typeCar.Cars.Contains(car)) typeCar.Cars.Add(car);

            queries.Update(car);

            queries.SaveChangesAsync();

            return carResponseValidation;

        }
        public CarResponseValidation PostCar(CarRequest carRequest)
        {
            var branch = queriesBranch.GetBranch(carRequest.BranchId);
            var typeCar = queriesTypeCar.GetTypeCar(carRequest.TypeCarId);
            
            Car car = new Car();
            car.Model = carRequest.Model;
            car.Brand = carRequest.Brand;
            car.BranchId = carRequest.BranchId;
            car.TypeCarId = carRequest.TypeCarId;
            car.Branch = branch;
            branch!.Cars.Add(car);
            typeCar!.Cars.Add(car);

            CarResponseValidation carResponseValidation = new CarResponseValidation(car);

            if (branch == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "Branch does not exist";
                return carResponseValidation;
            }
            if (typeCar == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "TypeCar does not exist";
                return carResponseValidation;
            }

            var plannings = queriesPlanning.GetPlannings();;
            foreach (Planning planning in plannings)
            {
                if (planning.BranchId == carRequest.BranchId && planning.TypeCarId == carRequest.TypeCarId)
                {
                    planning.AvailableCars++;
                }
            }

            queriesCar.AddCar(car);
            queries.SaveChangesAsync();

            //return CreatedAtAction("GetCar", new { id = car.Id }, car);
            return carResponseValidation;
        }
        public CarResponseValidation DeleteCar(int id)
        {
            var car = queriesCar.GetCar(id);

            CarResponseValidation carResponseValidation = new CarResponseValidation(car);

            if (car == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "Car does not exist";
                return carResponseValidation;
            }

            var plannings = queriesPlanning.GetPlannings();;

            foreach (Planning planning in plannings)
            {
                if (planning.BranchId == car.BranchId && planning.TypeCarId == car.TypeCarId)
                {
                    planning.AvailableCars--;
                }
            }

            queriesCar.RemoveCar(car);
            queries.SaveChangesAsync();

            return carResponseValidation;
        }
        public ActionResult<int> GetAvailableCarsByBranchAndDate(int branchId, int typeCarId, DateTime date){
            if(queriesBranch.GetBranch(branchId)==null) return -1;
            if(queriesTypeCar.GetTypeCar(typeCarId)==null) return -2;

            return queriesPlanning.GetAvailableCarsByBranchByTypeCarByDate(branchId, typeCarId, date);
        }

        public ActionResult<List<CarResponse>> GetCarsByBranch(int id){

            var branch = queriesBranch.GetBranch(id);
            var cars = queriesCar.GetCars();

            if(branch == null) return null!;

            List<CarResponse> carsList = new List<CarResponse>();
            foreach (Car car in cars)
            {
                if (car.BranchId == branch!.Id)
                {
                    CarResponse carResponse = new CarResponse();
                    carResponse.Id = car.Id;
                    carResponse.Brand = car.Brand;
                    carResponse.Model = car.Model;
                    carResponse.BranchId = car.BranchId;
                    carResponse.TypeCarId = car.TypeCarId;
                    carsList.Add(carResponse);
                }
            }
            return carsList;
        }
        public ActionResult<List<CarResponse>> GetCarsByTypeCar(int id){

            var typeCar = queriesTypeCar.GetTypeCar(id);
            var cars = queriesCar.GetCars();

            if(typeCar == null) return null!;

            List<CarResponse> carsList = new List<CarResponse>();
            foreach (Car car in cars)
            {
                if (car.TypeCarId == typeCar!.Id)
                {
                    CarResponse carResponse = new CarResponse();
                    carResponse.Id = car.Id;
                    carResponse.Brand = car.Brand;
                    carResponse.Model = car.Model;
                    carResponse.BranchId = car.BranchId;
                    carResponse.TypeCarId = car.TypeCarId;
                    carsList.Add(carResponse);
                }
            }
            return carsList;
        }
    }
}