using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
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
    public class CarValidation : ICar
    {
        private IDBQueries queriesDB;
        private IBranchQueries queriesBranch;
        private IPlanningQueries queriesPlanning;
        private ITypeCarQueries queriesTypeCar;
        private ICarQueries queriesCar;

        public CarValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries, ICarQueries _queriesCar)
        {
            queriesBranch = _queriesBranch;
            queriesPlanning = _queriesPlanning;
            queriesTypeCar = _queriesTypeCar;
            queriesDB = _queries;
            queriesCar = _queriesCar;
        }
        public List<CarResponse> GetCars()
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
        public CarResponseValidation GetCar(int id)
        {
            var car = queriesCar.GetCar(id);

            if (car == null)
            {
                CarResponseValidation crv = new CarResponseValidation(null);
                crv.Status = false;
                crv.Message = "Car not found";
                return crv;
            }

            CarResponse carResponse = new CarResponse();
            carResponse.Id = car.Id;
            carResponse.BranchId = car.BranchId;
            carResponse.Brand = car.Brand;
            carResponse.Model = car.Model;
            carResponse.isAutomatic = car.isAutomatic;
            carResponse.isGasoline = car.isGasoline;
            carResponse.TypeCarId = car.TypeCarId;

            CarResponseValidation carResponseValidation = new CarResponseValidation(carResponse);
            return carResponseValidation;
        }
        public CarResponseValidation UpdateCar(int id, CarRequest carRequest)
        {
            var car = queriesCar.GetCar(id);
            var firstBranchId = car.BranchId;
            var firstTypeCarId = car.TypeCarId;
            var branch = queriesBranch.GetBranch(carRequest.BranchId);
            var typeCar = queriesTypeCar.GetTypeCar(carRequest.TypeCarId);

            CarResponseValidation crv = new CarResponseValidation(null);

            if (branch == null)
            {
                crv.Status = false;
                crv.Message = "Branch does not exist";
                return crv;
            }
            if (typeCar == null)
            {
                crv.Status = false;
                crv.Message = "TypeCar does not exist";
                return crv;
            }
            if (car == null)
            {
                crv.Status = false;
                crv.Message = "Car does not exist";
                return crv;
            }

            car.Model = carRequest.Model;
            car.Brand = carRequest.Brand;
            car.isAutomatic = carRequest.isAutomatic;
            car.isGasoline = carRequest.isGasoline;
            car.BranchId = carRequest.BranchId;
            car.TypeCarId = carRequest.TypeCarId;
            car.Branch = branch;
            car.TypeCar = typeCar;

            CarResponse carResponse = new CarResponse();
            carResponse.Id = car.Id;
            carResponse.Model = car.Model;
            carResponse.Brand = car.Brand;
            carResponse.isAutomatic = car.isAutomatic;
            carResponse.isGasoline = car.isGasoline;
            carResponse.BranchId = car.BranchId;
            carResponse.TypeCarId = car.TypeCarId;

            CarResponseValidation carResponseValidation = new CarResponseValidation(carResponse);

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

            queriesDB.Update(car);

            queriesDB.SaveChangesAsync();

            return carResponseValidation;
        }
        public CarResponseValidation CreateCar(CarRequest carRequest)
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

            CarResponseValidation carResponseValidation = new CarResponseValidation(carResponse);

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

            var plannings = queriesPlanning.GetPlannings(); ;
            foreach (Planning planning in plannings)
            {
                if (planning.BranchId == carRequest.BranchId && planning.TypeCarId == carRequest.TypeCarId)
                {
                    planning.AvailableCars++;
                }
            }

            queriesCar.AddCar(car);
            queriesDB.SaveChangesAsync();


            return carResponseValidation;
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
            queriesDB.SaveChangesAsync();

            return carResponseValidation;
        }
        public ActionResult<int> GetNumberOfAvailableCarsByBranchAndDate(int branchId, int typeCarId, DateTime date)
        {
            if (queriesBranch.GetBranch(branchId) == null) return 0;
            if (queriesTypeCar.GetTypeCar(typeCarId) == null) return 0;

            return queriesPlanning.GetNumberOfAvailableCarsByBranchByTypeCarByDate(branchId, typeCarId, date);
        }
        public List<CarResponse> GetCarsByBranch(int id)
        {

            var branch = queriesBranch.GetBranch(id);
            var cars = queriesCar.GetCars();

            if (branch == null) return new List<CarResponse>();

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
        public List<CarResponse> GetCarsByTypeCar(int id)
        {
            var typeCar = queriesTypeCar.GetTypeCar(id);
            var cars = queriesCar.GetCars();

            if (typeCar == null) return new List<CarResponse>();

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
        public List<int> GetAvailableCarsByBranchAndDate(int branchId, DateTime initialDate, DateTime finalDate)
        {
            var branch = queriesBranch.GetBranch(branchId);

            if (branch == null) return new List<int>();

            if (initialDate > finalDate) return new List<int>();

            var plannings = queriesPlanning.GetPlanningsByBranchByDate(branchId, initialDate, finalDate);
            List<TypeCar> typeCars = queriesTypeCar.GetTypeCars();
            List<int> typeCarsIds = new List<int>();

            foreach (TypeCar typeCar in typeCars)
            {
                typeCarsIds.Add(typeCar.Id);
            }

            foreach (Planning planning in plannings)
            {
                foreach (TypeCar typeCar in typeCars)
                {
                    if (planning.TypeCarId == typeCar.Id)
                    {
                        if (planning.AvailableCars == 0)
                        {
                            typeCarsIds.Remove(typeCar.Id);
                        }
                    }

                }
            }


            return typeCarsIds;
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