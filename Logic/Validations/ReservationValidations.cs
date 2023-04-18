using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class ReservationResponseValidation
    {
        public ReservationResponse? ReservationResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public ReservationResponseValidation(ReservationResponse? reservationResponse)
        {
            ReservationResponse = ReservationResponse;
            Status = true;
            Message = "OK";
        }
    }
    public class ReservationValidation : IReservation
    {
        private IDBQueries queriesDB;
        private IBranchQueries queriesBranch;
        private IPlanningQueries queriesPlanning;
        private ITypeCarQueries queriesTypeCar;
        private ICarQueries queriesCar;
        private IReservationQueries queriesReservation;
        private ICustomerQueries queriesCustomer;
        public ReservationValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries, ICarQueries _queriesCar, IReservationQueries _queriesReservation, ICustomerQueries _queriesCustomer)
        {
            queriesBranch = _queriesBranch;
            queriesPlanning = _queriesPlanning;
            queriesTypeCar = _queriesTypeCar;
            queriesDB = _queries;
            queriesCar = _queriesCar;
            queriesReservation = _queriesReservation;
            queriesCustomer = _queriesCustomer;
        }

        public List<ReservationResponse> GetReservations()
        {
            var reservations = queriesReservation.GetReservations();
            List<ReservationResponse> reservationsList = new List<ReservationResponse>();
            foreach (Reservation reservation in reservations)
            {
                ReservationResponse reservationResponse = new ReservationResponse();
                reservationResponse.Id = reservation.Id;
                reservationResponse.BranchId = reservation.BranchId;
                reservationResponse.CarId = reservation.CarId;
                reservationResponse.CustomerId = reservation.CustomerId;
                reservationResponse.InitialDate = reservation.InitialDate;
                reservationResponse.FinalDate = reservation.FinalDate;
                reservationResponse.isInternational = reservation.isInternational;
                reservationResponse.hasGPS = reservation.hasGPS;
                reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
                reservationResponse.ReturnBranchId = reservation.ReturnBranchId;
                reservationsList.Add(reservationResponse);
            }
            return reservationsList;
        }
        public ReservationResponseValidation GetReservation(int id)
        {
            var reservation = queriesReservation.GetReservation(id);

            if (reservation == null){
                ReservationResponseValidation rrv = new ReservationResponseValidation(null);
                rrv.Status = false;
                rrv.Message = "Reservation not found";
            }

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.Id = reservation!.Id;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;
            reservationResponse.isInternational = reservation.isInternational;
            reservationResponse.hasGPS = reservation.hasGPS;
            reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
            reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservationResponse);

            return reservationResponseValidation;
        }
        public ReservationResponseValidation UpdateReservation(int id, ReservationRequest reservationRequest)
        {
            var reservation = queriesReservation.GetReservation(id);

            ReservationResponseValidation rrv = new ReservationResponseValidation(null);
            if (reservation == null){
                rrv.Status = false;
                rrv.Message = "Reservation not found";
            }

            if (reservationRequest.numberOfDrivers > 3){
                rrv.Status = false;
                rrv.Message = "Number of drivers must be less than 3";
            }

            queriesDB.Update(reservation!);

            queriesDB.SaveChangesAsync();

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.Id = reservation!.Id;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;
            reservationResponse.isInternational = reservation.isInternational;
            reservationResponse.hasGPS = reservation.hasGPS;
            reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
            reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservationResponse);

            return reservationResponseValidation;
        }

        public ReservationResponseValidation CreateReservation(ReservationRequest reservationRequest)
        {
            //Comprobaciones
            var typeCar = queriesTypeCar.GetTypeCar(reservationRequest.TypeCarId);
            var branch = queriesBranch.GetBranch(reservationRequest.BranchId);
            var customer = queriesCustomer.GetCustomer(reservationRequest.CustomerId);

            ReservationResponseValidation rrv = new ReservationResponseValidation(null);

            if (reservationRequest.InitialDate > reservationRequest.FinalDate){
                rrv.Status = false;
                rrv.Message = "Initial Date must be less than Final Date";
                return rrv;
            }

            if (reservationRequest.numberOfDrivers > 3){
                rrv.Status = false;
                rrv.Message = "Number of drivers must be less than 3";
                return rrv;
            }

            if (typeCar?.Cars == null){
                rrv.Status = false;
                rrv.Message = "TypeCar empty";
                return rrv;
            }

            if (branch == null){
                rrv.Status = false;
                rrv.Message = "Branch does not exist";
                return rrv;
            }

            if (customer == null){
                rrv.Status = false;
                rrv.Message = "Customer does not exist";
                return rrv;
            }

            var plannings = queriesPlanning.GetPlanningsByBranchByTypeCarByDate(reservationRequest.BranchId, reservationRequest.TypeCarId, reservationRequest.InitialDate, reservationRequest.FinalDate);

            foreach (Planning planning in plannings)
            {
                if (planning.AvailableCars == 0){
                    rrv.Status = false;
                    rrv.Message = "Planning available cars is 0";
                    return rrv;
                }
            }

            var cars = (from c in typeCar.Cars
                        where c.BranchId == reservationRequest.BranchId
                        select c);

            Car? carToBook = null;
            //Buscamos cuál es el primer coche disponible para asignarlo a la reserva
            foreach (Car car in cars)
            {
                if (isCarAvailable(car, reservationRequest.InitialDate, reservationRequest.FinalDate))
                {
                    carToBook = car;
                }
            }

            Reservation reservation = new Reservation();
            reservation.InitialDate = reservationRequest.InitialDate;
            reservation.FinalDate = reservationRequest.FinalDate;
            reservation.CarId = carToBook!.Id;
            reservation.CustomerId = reservationRequest.CustomerId;
            reservation.BranchId = reservation.BranchId;
            reservation.isInternational = reservationRequest.isInternational;
            reservation.hasGPS = reservationRequest.hasGPS;
            reservation.numberOfDrivers = reservationRequest.numberOfDrivers;
            reservation.ReturnBranchId = reservationRequest.BranchId;
            reservation.Car = carToBook;
            reservation.Customer = customer;
            reservation.Branch = branch;

            customer.Reservations?.Add(reservation);
            branch.Reservations?.Add(reservation);
            carToBook.Reservations?.Add(reservation);

            queriesReservation.AddReservation(reservation);

            foreach (Planning planning in plannings)
            {
                planning.AvailableCars--;
            }

            queriesDB.SaveChangesAsync();

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.InitialDate = reservationRequest.InitialDate;
            reservationResponse.FinalDate = reservationRequest.FinalDate;
            reservationResponse.CarId = carToBook.Id;
            reservationResponse.CustomerId = reservationRequest.CustomerId;
            reservationResponse.BranchId = reservationRequest.BranchId;
            reservationResponse.isInternational = reservationRequest.isInternational;
            reservationResponse.hasGPS = reservationRequest.hasGPS;
            reservationResponse.numberOfDrivers = reservationRequest.numberOfDrivers;
            reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservationResponse);

            return reservationResponseValidation;
        }
        public ReservationResponseValidation CreateReservationOnDifferentBranch(ReservationRequestDifferentBranch reservationRequestDifferentBranch)
        {
            var typeCar = queriesTypeCar.GetTypeCar(reservationRequestDifferentBranch.TypeCarId);
            var pickUpBranch = queriesBranch.GetBranch(reservationRequestDifferentBranch.PickUpBranchId);
            var returnBranch = queriesBranch.GetBranch(reservationRequestDifferentBranch.ReturnBranchId);
            var customer = queriesCustomer.GetCustomer(reservationRequestDifferentBranch.CustomerId);

            ReservationResponseValidation rrv = new ReservationResponseValidation(null);

            if (reservationRequestDifferentBranch.InitialDate > reservationRequestDifferentBranch.FinalDate){
                rrv.Status = false;
                rrv.Message = "Initial Date must be less than Final Date";
                return rrv;
            }

            if (reservationRequestDifferentBranch.numberOfDrivers > 3){
                rrv.Status = false;
                rrv.Message = "Number of drivers must be less than 3";
                return rrv;
            }

            if (typeCar?.Cars == null){
                rrv.Status = false;
                rrv.Message = "TypeCar empty";
                return rrv;
            }

            if (pickUpBranch == null){
                rrv.Status = false;
                rrv.Message = "PickUpBranch does not exist";
                return rrv;
            }
            if (returnBranch == null){
                rrv.Status = false;
                rrv.Message = "ReturnBranch does not exist";
                return rrv;
            }

            if (customer == null){
                rrv.Status = false;
                rrv.Message = "Customer does not exist";
                return rrv;
            }

            if (queriesPlanning.GetNumberOfAvailableCarsByBranchByTypeCarByDate(reservationRequestDifferentBranch.PickUpBranchId, reservationRequestDifferentBranch.TypeCarId, reservationRequestDifferentBranch.InitialDate) == 0){
                rrv.Status = false;
                rrv.Message = "Planning available cars is 0";
                return rrv;
            }

            var cars = (from c in typeCar.Cars
                        where c.BranchId == reservationRequestDifferentBranch.PickUpBranchId
                        select c);

            Car? carToBook = null;
            //Buscamos cuál es el primer coche disponible para asignarlo a la reserva
            foreach (Car car in cars)
            {
                if (isCarAvailable(car, reservationRequestDifferentBranch.InitialDate, reservationRequestDifferentBranch.FinalDate))
                {
                    carToBook = car;
                }
            }

            Reservation reservation = new Reservation();
            reservation.InitialDate = reservationRequestDifferentBranch.InitialDate;
            reservation.FinalDate = reservationRequestDifferentBranch.FinalDate;
            reservation.CarId = carToBook!.Id;
            reservation.CustomerId = reservationRequestDifferentBranch.CustomerId;
            reservation.BranchId = reservation.BranchId;
            reservation.isInternational = reservationRequestDifferentBranch.isInternational;
            reservation.hasGPS = reservationRequestDifferentBranch.hasGPS;
            reservation.numberOfDrivers = reservationRequestDifferentBranch.numberOfDrivers;
            reservation.ReturnBranchId = reservationRequestDifferentBranch.ReturnBranchId;
            reservation.Car = carToBook;
            reservation.Customer = customer;
            reservation.Branch = pickUpBranch;

            customer.Reservations?.Add(reservation);
            pickUpBranch.Reservations?.Add(reservation);
            carToBook.Reservations?.Add(reservation);

            queriesReservation.AddReservation(reservation);

            pickUpBranch.Cars.Remove(carToBook);
            returnBranch.Cars.Add(carToBook);

            var olderPlanningsPickUpBranch = queriesPlanning.GetOlderPlanningsByBranchByTypeCarByDate(reservationRequestDifferentBranch.PickUpBranchId, reservationRequestDifferentBranch.TypeCarId, reservationRequestDifferentBranch.InitialDate.AddDays(-1));

            foreach (Planning planning in olderPlanningsPickUpBranch)
            {
                planning.AvailableCars--;
            }

            var newerPlanningsReturnBranch = queriesPlanning.GetOlderPlanningsByBranchByTypeCarByDate(reservationRequestDifferentBranch.ReturnBranchId, reservationRequestDifferentBranch.TypeCarId, reservationRequestDifferentBranch.FinalDate);

            foreach (Planning planning in newerPlanningsReturnBranch)
            {
                planning.AvailableCars++;
            }

            queriesDB.SaveChangesAsync();

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.InitialDate = reservationRequestDifferentBranch.InitialDate;
            reservationResponse.FinalDate = reservationRequestDifferentBranch.FinalDate;
            reservationResponse.CarId = carToBook.Id;
            reservationResponse.CustomerId = reservationRequestDifferentBranch.CustomerId;
            reservationResponse.BranchId = reservationRequestDifferentBranch.PickUpBranchId;
            reservationResponse.isInternational = reservationRequestDifferentBranch.isInternational;
            reservationResponse.hasGPS = reservationRequestDifferentBranch.hasGPS;
            reservationResponse.numberOfDrivers = reservationRequestDifferentBranch.numberOfDrivers;
            reservationResponse.ReturnBranchId = reservationRequestDifferentBranch.ReturnBranchId;

            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservationResponse);

            return reservationResponseValidation;
        }
        public ReservationResponseValidation DeleteReservation(int id)
        {
            var reservation = queriesReservation.GetReservation(id);

            if (reservation == null){
                ReservationResponseValidation rrv = new ReservationResponseValidation(null);
                rrv.Status = false;
                rrv.Message = "Reservation does not exist";
                return rrv;
            }

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;
            reservationResponse.Id = reservation.Id;
            reservationResponse.isInternational = reservation.isInternational;
            reservationResponse.hasGPS = reservation.hasGPS;
            reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
            reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservationResponse);

            queriesReservation.RemoveReservation(reservation);
            queriesDB.SaveChangesAsync();

            return reservationResponseValidation;
        }
        public List<ReservationResponse> GetReservationsByBranch(int id)
        {
            var reservations = queriesReservation.GetReservations();
            var branch = queriesBranch.GetBranch(id);

            if (branch == null) return null!;

            List<ReservationResponse> reservationsList = new List<ReservationResponse>();
            foreach (Reservation reservation in reservations)
            {
                if (reservation.BranchId == branch.Id)
                {
                    ReservationResponse reservationResponse = new ReservationResponse();
                    reservationResponse.Id = reservation.Id;
                    reservationResponse.BranchId = reservation.BranchId;
                    reservationResponse.CarId = reservation.CarId;
                    reservationResponse.CustomerId = reservation.CustomerId;
                    reservationResponse.InitialDate = reservation.InitialDate;
                    reservationResponse.FinalDate = reservation.FinalDate;
                    reservationResponse.isInternational = reservation.isInternational;
                    reservationResponse.hasGPS = reservation.hasGPS;
                    reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
                    reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
        }
        public List<ReservationResponse> GetReservationsByCar(int id)
        {
            var reservations = queriesReservation.GetReservations();
            var car = queriesCar.GetCar(id);

            if (car == null) return null!;

            List<ReservationResponse> reservationsList = new List<ReservationResponse>();
            foreach (Reservation reservation in reservations)
            {
                if (reservation.CarId == car.Id)
                {
                    ReservationResponse reservationResponse = new ReservationResponse();
                    reservationResponse.Id = reservation.Id;
                    reservationResponse.BranchId = reservation.BranchId;
                    reservationResponse.CarId = reservation.CarId;
                    reservationResponse.CustomerId = reservation.CustomerId;
                    reservationResponse.InitialDate = reservation.InitialDate;
                    reservationResponse.FinalDate = reservation.FinalDate;
                    reservationResponse.isInternational = reservation.isInternational;
                    reservationResponse.hasGPS = reservation.hasGPS;
                    reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
                    reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
        }
        public List<ReservationResponse> GetReservationsByCustomer(int id)
        {
            var reservations = queriesReservation.GetReservations();
            var customer = queriesCustomer.GetCustomer(id);

            if (customer == null) return null!;

            List<ReservationResponse> reservationsList = new List<ReservationResponse>();
            foreach (Reservation reservation in reservations)
            {
                if (reservation.CustomerId == customer.Id)
                {
                    ReservationResponse reservationResponse = new ReservationResponse();
                    reservationResponse.Id = reservation.Id;
                    reservationResponse.BranchId = reservation.BranchId;
                    reservationResponse.CarId = reservation.CarId;
                    reservationResponse.CustomerId = reservation.CustomerId;
                    reservationResponse.InitialDate = reservation.InitialDate;
                    reservationResponse.FinalDate = reservation.FinalDate;
                    reservationResponse.isInternational = reservation.isInternational;
                    reservationResponse.hasGPS = reservation.hasGPS;
                    reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
                    reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
        }
        public List<ReservationResponse> GetReservationsByDate(DateTime date)
        {
            var reservations = queriesReservation.GetReservations();

            List<ReservationResponse> reservationsList = new List<ReservationResponse>();
            foreach (Reservation reservation in reservations)
            {
                if (reservation.InitialDate < date && reservation.FinalDate > date)
                {
                    ReservationResponse reservationResponse = new ReservationResponse();
                    reservationResponse.Id = reservation.Id;
                    reservationResponse.BranchId = reservation.BranchId;
                    reservationResponse.CarId = reservation.CarId;
                    reservationResponse.CustomerId = reservation.CustomerId;
                    reservationResponse.InitialDate = reservation.InitialDate;
                    reservationResponse.FinalDate = reservation.FinalDate;
                    reservationResponse.isInternational = reservation.isInternational;
                    reservationResponse.hasGPS = reservation.hasGPS;
                    reservationResponse.numberOfDrivers = reservation.numberOfDrivers;
                    reservationResponse.ReturnBranchId = reservation.ReturnBranchId;

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
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