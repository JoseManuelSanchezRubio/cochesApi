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
        public CarResponse? CarResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public CarResponseValidation(CarResponse? carResponse)
        {
            CarResponse = carResponse;
            Status = true;
            Message = "OK";
        }
    }

    public class CarValidation : ControllerBase, ICar
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
                carResponse.isAutomatic = car.isAutomatic;
                carResponse.isGasoline = car.isGasoline;
                carResponse.TypeCarId = car.TypeCarId;
                carsResponse.Add(carResponse);
            }
            return carsResponse;
        }
        public ActionResult<CarResponse> GetCar(int id)
        {
            var car = queriesCar.GetCar(id);

            if(car==null) return Problem("Car does not exist");

            CarResponse carResponse = new CarResponse();
            carResponse.Id = car.Id;
            carResponse.BranchId = car.BranchId;
            carResponse.Brand = car.Brand;
            carResponse.Model = car.Model;
            carResponse.isAutomatic = car.isAutomatic;
            carResponse.isGasoline = car.isGasoline;
            carResponse.TypeCarId = car.TypeCarId;
            return carResponse;
        }

        public CarResponseValidation PutCar(int id, CarRequest carRequest)
        {
            var car = queriesCar.GetCar(id);
            var firstBranchId = car.BranchId;
            var firstTypeCarId = car.TypeCarId;
            var branch = queriesBranch.GetBranch(carRequest.BranchId);
            var typeCar = queriesTypeCar.GetTypeCar(carRequest.TypeCarId);

            CarResponseValidation carResponseValidation = new CarResponseValidation(null); // Cambiar en un futuro

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
            car.isAutomatic = carRequest.isAutomatic;
            car.isGasoline = carRequest.isGasoline;
            car.BranchId = carRequest.BranchId;
            car.TypeCarId = carRequest.TypeCarId;
            car.Branch = branch;
            car.TypeCar = typeCar;

            

            var oldPlannings = queriesPlanning.GetPlanningsByBranchByTypeCarByDate(firstBranchId, firstTypeCarId, DateTime.Now.Date, queriesPlanning.GetPlanning(queriesPlanning.GetPlannings().Count).Day.Date);
            foreach (Planning planning in oldPlannings)
            {
                planning.AvailableCars--;
            }
            var newPlannings = queriesPlanning.GetPlanningsByBranchByTypeCarByDate(carRequest.BranchId, carRequest.TypeCarId, DateTime.Now, queriesPlanning.GetPlanning(queriesPlanning.GetPlannings().Count).Day);
            foreach (Planning planning in newPlannings)
            {
                planning.AvailableCars++;
            }


            queries.Update(car);

            queries.SaveChangesAsync();

            return carResponseValidation;

        }
        public ActionResult<CarResponse> PostCar(CarRequest carRequest)
        {
            var branch = queriesBranch.GetBranch(carRequest.BranchId);
            var typeCar = queriesTypeCar.GetTypeCar(carRequest.TypeCarId);

            Car car = new Car();
            car.Model = carRequest.Model;
            car.Brand = carRequest.Brand;
            car.isAutomatic = carRequest.isAutomatic;
            car.isGasoline = carRequest.isGasoline;
            car.BranchId = carRequest.BranchId;
            car.TypeCarId = carRequest.TypeCarId;
            car.Branch = branch;
            car.TypeCar = typeCar;

            CarResponse carResponse = new CarResponse();
            carResponse.BranchId = carRequest.BranchId;
            carResponse.TypeCarId = carRequest.TypeCarId;
            car.Model = carRequest.Model;
            car.Brand = carRequest.Brand;
            car.isAutomatic = carRequest.isAutomatic;
            car.isGasoline = carRequest.isGasoline;


            if (branch == null) return Problem("Branch does not exist");

            if (typeCar == null) return Problem("TypeCar does not exist");


            var plannings = queriesPlanning.GetPlannings(); ;
            foreach (Planning planning in plannings)
            {
                if (planning.BranchId == carRequest.BranchId && planning.TypeCarId == carRequest.TypeCarId)
                {
                    planning.AvailableCars++;
                }
            }

            queriesCar.AddCar(car);
            queries.SaveChangesAsync();


            return carResponse;
        }
        public CarResponseValidation DeleteCar(int id)
        {
            var car = queriesCar.GetCar(id);

            CarResponseValidation carResponseValidation = new CarResponseValidation(null); // Cambiar en un futuro

            if (car == null)
            {
                carResponseValidation.Status = false;
                carResponseValidation.Message = "Car does not exist";
                return carResponseValidation;
            }

            var plannings = queriesPlanning.GetPlannings(); ;

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
        public ActionResult<int> GetNumberOfAvailableCarsByBranchAndDate(int branchId, int typeCarId, DateTime date)
        {
            if (queriesBranch.GetBranch(branchId) == null) return Problem("Branch does not exist");
            if (queriesTypeCar.GetTypeCar(typeCarId) == null) return Problem("TypeCar does not exist");

            return queriesPlanning.GetNumberOfAvailableCarsByBranchByTypeCarByDate(branchId, typeCarId, date);
        }

        public ActionResult<List<CarResponse>> GetCarsByBranch(int id)
        {

            var branch = queriesBranch.GetBranch(id);
            var cars = queriesCar.GetCars();

            if (branch == null) return Problem("Branch does not exist");

            List<CarResponse> carsList = new List<CarResponse>();
            foreach (Car car in cars)
            {
                if (car.BranchId == branch!.Id)
                {
                    CarResponse carResponse = new CarResponse();
                    carResponse.Id = car.Id;
                    carResponse.Brand = car.Brand;
                    carResponse.Model = car.Model;
                    carResponse.isAutomatic = car.isAutomatic;
                    carResponse.isGasoline = car.isGasoline;
                    carResponse.BranchId = car.BranchId;
                    carResponse.TypeCarId = car.TypeCarId;
                    carsList.Add(carResponse);
                }
            }
            return carsList;
        }
        public ActionResult<List<CarResponse>> GetCarsByTypeCar(int id)
        {

            var typeCar = queriesTypeCar.GetTypeCar(id);
            var cars = queriesCar.GetCars();

            if (typeCar == null) return Problem("TypeCar does not exist");

            List<CarResponse> carsList = new List<CarResponse>();
            foreach (Car car in cars)
            {
                if (car.TypeCarId == typeCar!.Id)
                {
                    CarResponse carResponse = new CarResponse();
                    carResponse.Id = car.Id;
                    carResponse.Brand = car.Brand;
                    carResponse.Model = car.Model;
                    carResponse.isAutomatic = car.isAutomatic;
                    carResponse.isGasoline = car.isGasoline;
                    carResponse.BranchId = car.BranchId;
                    carResponse.TypeCarId = car.TypeCarId;
                    carsList.Add(carResponse);
                }
            }
            return carsList;
        }
        public ActionResult<List<CarResponse>> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate)
        {
            var branch = queriesBranch.GetBranch(branchId);

            if (branch == null) return Problem("Branch does not exist");

            if (initialDate > finalDate) return Problem("Initial date must be before final date");

            var cars = queriesCar.GetAvailableCarsByBranch(branchId);

            List<CarResponse> availableCars = new List<CarResponse>();
            foreach (Car car in cars)
            {
                if (isCarAvailable(car, initialDate, finalDate))
                {
                    CarResponse carResponse = new CarResponse();
                    carResponse.Id = car.Id;
                    carResponse.Brand = car.Brand;
                    carResponse.Model = car.Model;
                    carResponse.isAutomatic = car.isAutomatic;
                    carResponse.isGasoline = car.isGasoline;
                    carResponse.BranchId = car.BranchId;
                    carResponse.TypeCarId = car.TypeCarId;
                    availableCars.Add(carResponse);
                }
            }

            return availableCars;

        }
        private bool isCarAvailable(Car car, DateTime? initialDate, DateTime? finalDate)
        {
            if (car.Reservations == null) return true;
            foreach (Reservation reservation in car.Reservations)
            {
                if (initialDate < reservation.InitialDate && finalDate > reservation.FinalDate ||
                    initialDate < reservation.InitialDate && finalDate < reservation.FinalDate && initialDate < reservation.FinalDate && finalDate > reservation.InitialDate ||
                    initialDate > reservation.InitialDate && initialDate < reservation.FinalDate)
                {
                    return false;
                }
            }
            return true;
        }
    }
}