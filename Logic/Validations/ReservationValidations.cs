using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using cochesApi.DataAccess.Queries;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class ReservationResponseValidation
    {
        public Reservation? Reservation { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public ReservationResponseValidation(Reservation? reservation)
        {
            Reservation = reservation;
            Status = true;
            Message = "OK";
        }
    }

    public class ReservationValidation : IReservation
    {
        private IDBQueries queries;
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
            queries = _queries;
            queriesCar = _queriesCar;
            queriesReservation = _queriesReservation;
            queriesCustomer = _queriesCustomer;
        }

        public ActionResult<IEnumerable<ReservationResponse>> GetReservations()
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
                reservationsList.Add(reservationResponse);
            }
            return reservationsList;
        }
        public ActionResult<ReservationResponse> GetReservation(int id)
        {
            var reservation = queriesReservation.GetReservation(id);

            if (reservation == null) return null!;

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.Id = reservation.Id;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;

            return reservationResponse;
        }

        public ReservationResponseValidation PutReservation(int id, ReservationRequest reservationRequest)
        {
            var reservation = queriesReservation.GetReservation(id);
            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservation);

            if (reservation == null)
            {
                reservationResponseValidation.Status = false;
                reservationResponseValidation.Message = "Reservation does not exist";
                return reservationResponseValidation;
            }

            queries.Update(reservation);


            queries.SaveChangesAsync();


            return reservationResponseValidation;

        }
        public ReservationResponseValidation PostReservation(ReservationRequest reservationRequest)
        {
            //Comprobaciones
            var typeCar = queriesTypeCar.GetTypeCar(reservationRequest.TypeCarId);
            var branch = queriesBranch.GetBranch(reservationRequest.BranchId);
            var customer = queriesCustomer.GetCustomer(reservationRequest.CustomerId);
            

            ReservationResponseValidation rrq = new ReservationResponseValidation(null);

            if (reservationRequest.InitialDate > reservationRequest.FinalDate){
                rrq.Status=false;
                rrq.Message="Wrong dates";
                return rrq;
            }
            if (typeCar?.Cars == null){
                rrq.Status=false;
                rrq.Message="TypeCar empty";
                return rrq;
            }
            if (branch == null){
                rrq.Status=false;
                rrq.Message="Branch does not exist";
                return rrq;
            }
            if (customer == null){
                rrq.Status=false;
                rrq.Message="Customer does not exist";
                return rrq;
            }
            if (queriesPlanning.GetAvailableCarsByBranchByTypeCarByDate(reservationRequest.BranchId, reservationRequest.TypeCarId, reservationRequest.InitialDate)==0){
                rrq.Status=false;
                rrq.Message="No typeCar available";
                return rrq;
            }


            var cars = (from t in typeCar.Cars
                        where t.BranchId == reservationRequest.BranchId
                        select t);

            if (cars == null){
                rrq.Status=false;
                rrq.Message="No cars available in that branch";
                return rrq;
            }

            Car? carToBook = null;
            //Buscamos cuál es el primer coche disponible para asignarlo a la reserva
            foreach (Car car in cars)
            {
                if (isCarAvailable(car, reservationRequest.InitialDate, reservationRequest.FinalDate))
                {
                    carToBook = car;
                }
            }

            var plannings = queriesPlanning.GetPlanningsByBranchByTypeCar(reservationRequest.BranchId, reservationRequest.TypeCarId);


            bool booked = false;
            bool stop = false;
            Reservation reservation = new Reservation();
            foreach (Planning planning in plannings) //Recorremos el planning para ver si hay coches disponibles
            {
                if (planning.Day.Date == reservationRequest.InitialDate.Date && planning.AvailableCars > 0 && !stop)
                {
                    reservation.InitialDate = reservationRequest.InitialDate;
                    reservation.FinalDate = reservationRequest.FinalDate;
                    reservation.CarId = carToBook!.Id;
                    reservation.CustomerId = reservationRequest.CustomerId;
                    reservation.BranchId = reservation.BranchId;
                    reservation.Car = carToBook;
                    reservation.Customer = customer;
                    reservation.Branch = branch;

                    customer.Reservations?.Add(reservation);
                    branch.Reservations?.Add(reservation);
                    carToBook.Reservations?.Add(reservation);

                    queriesReservation.AddReservation(reservation);

                    booked = true;
                    stop = true;

                    queries.SaveChangesAsync();

                }
            }


            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservation);

            if (booked)
            {
                foreach (Planning planning in plannings) //Restamos un coche al planning
                {
                    if (planning.Day.Date >= reservationRequest.InitialDate.Date && planning.Day.Date <= reservationRequest.FinalDate.Date)
                    {
                        planning.AvailableCars--;
                    }
                }
                queries.SaveChangesAsync();
                
                return reservationResponseValidation;
            }
            else
            {
                reservationResponseValidation.Status=false;
                reservationResponseValidation.Message="No cars available";
                return reservationResponseValidation;
            }
        }
        public ReservationResponseValidation DeleteReservation(int id)
        {
            var reservation = queriesReservation.GetReservation(id);
            ReservationResponseValidation reservationResponseValidation = new ReservationResponseValidation(reservation);
            if (reservation == null)
            {
                reservationResponseValidation.Status=false;
                reservationResponseValidation.Message="Reservation does not exist";
                return reservationResponseValidation;
            }

            queriesReservation.RemoveReservation(reservation);
            queries.SaveChangesAsync();

            return reservationResponseValidation;
        }
        public ActionResult<List<ReservationResponse>> GetReservationsByBranch(int id){

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

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
        }

        public ActionResult<List<ReservationResponse>> GetReservationsByCar(int id){

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

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
        }

        public ActionResult<List<ReservationResponse>> GetReservationsByCustomer(int id){

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

                    reservationsList.Add(reservationResponse);
                }
            }
            return reservationsList;
        }
        public ActionResult<List<ReservationResponse>> GetReservationsByDate(DateTime date){

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