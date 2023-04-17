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

    public class ReservationValidation : ControllerBase, IReservation
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

            if (reservation == null) return Problem("Reservation does not exist");

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.Id = reservation.Id;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;

            return reservationResponse;
        }

        public ActionResult<ReservationResponse> PutReservation(int id, ReservationRequest reservationRequest)
        {
            var reservation = queriesReservation.GetReservation(id);

            if (reservation == null) return Problem("Reservation does not exist");

            queries.Update(reservation);

            queries.SaveChangesAsync();

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.Id = reservation.Id;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;
            


            return reservationResponse;

        }

        public ActionResult<ReservationResponse> PostReservation(ReservationRequest reservationRequest)
        {
            //Comprobaciones
            var typeCar = queriesTypeCar.GetTypeCar(reservationRequest.TypeCarId);
            var branch = queriesBranch.GetBranch(reservationRequest.BranchId);
            var customer = queriesCustomer.GetCustomer(reservationRequest.CustomerId);


            ReservationResponseValidation rrv = new ReservationResponseValidation(null);

            if (reservationRequest.InitialDate > reservationRequest.FinalDate) return Problem("Initial date must be before final date");

            if (typeCar?.Cars == null) return Problem("Type car does not exist");

            if (branch == null) return Problem("Branch does not exist");

            if (customer == null) return Problem("Customer does not exist");

            var plannings = queriesPlanning.GetPlanningsByBranchByTypeCarByDate(reservationRequest.BranchId, reservationRequest.TypeCarId, reservationRequest.InitialDate, reservationRequest.FinalDate);

            foreach (Planning planning in plannings)
            {
                if (planning.AvailableCars == 0) return Problem("No cars available");
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

            queries.SaveChangesAsync();

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.InitialDate = reservationRequest.InitialDate;
            reservationResponse.FinalDate = reservationRequest.FinalDate;
            reservationResponse.CarId = carToBook.Id;
            reservationResponse.CustomerId = reservationRequest.CustomerId;
            reservationResponse.BranchId = reservationRequest.BranchId;



            return reservationResponse;

        }

        public ActionResult<ReservationResponse> PostReservationOnDifferentBranch(ReservationRequestDifferentBranch reservationRequestDifferentBranch)
        {

            var typeCar = queriesTypeCar.GetTypeCar(reservationRequestDifferentBranch.TypeCarId);
            var pickUpBranch = queriesBranch.GetBranch(reservationRequestDifferentBranch.PickUpBranchId);
            var returnBranch = queriesBranch.GetBranch(reservationRequestDifferentBranch.ReturnBranchId);
            var customer = queriesCustomer.GetCustomer(reservationRequestDifferentBranch.CustomerId);

            ReservationResponseValidation rrv = new ReservationResponseValidation(null);

            if (reservationRequestDifferentBranch.InitialDate > reservationRequestDifferentBranch.FinalDate) return Problem("Initial date must be before final date");

            if (typeCar?.Cars == null) return Problem("Type car does not exist");

            if (pickUpBranch == null) return Problem("Pick up branch does not exist");

            if (returnBranch == null) return Problem("Return branch does not exist");

            if (customer == null) return Problem("Customer does not exist");

            if (queriesPlanning.GetNumberOfAvailableCarsByBranchByTypeCarByDate(reservationRequestDifferentBranch.PickUpBranchId, reservationRequestDifferentBranch.TypeCarId, reservationRequestDifferentBranch.InitialDate) == 0) return Problem("No cars available");


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

            var olderPlanningsReturnBranch = queriesPlanning.GetOlderPlanningsByBranchByTypeCarByDate(reservationRequestDifferentBranch.ReturnBranchId, reservationRequestDifferentBranch.TypeCarId, reservationRequestDifferentBranch.FinalDate);

            foreach (Planning planning in olderPlanningsReturnBranch)
            {
                planning.AvailableCars++;
            }

            queries.SaveChangesAsync();

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.InitialDate = reservationRequestDifferentBranch.InitialDate;
            reservationResponse.FinalDate = reservationRequestDifferentBranch.FinalDate;
            reservationResponse.CarId = carToBook.Id;
            reservationResponse.CustomerId = reservationRequestDifferentBranch.CustomerId;
            reservationResponse.BranchId = reservationRequestDifferentBranch.PickUpBranchId;


            return reservationResponse;
        }
        public ActionResult<ReservationResponse> DeleteReservation(int id)
        {
            var reservation = queriesReservation.GetReservation(id);

            if (reservation == null) return Problem("Reservation not found");

            ReservationResponse reservationResponse = new ReservationResponse();
            reservationResponse.CustomerId = reservation.CustomerId;
            reservationResponse.BranchId = reservation.BranchId;
            reservationResponse.CarId = reservation.CarId;
            reservationResponse.InitialDate = reservation.InitialDate;
            reservationResponse.FinalDate = reservation.FinalDate;
            reservationResponse.Id = reservation.Id;


            queriesReservation.RemoveReservation(reservation);
            queries.SaveChangesAsync();

            return reservationResponse;
        }
        public ActionResult<List<ReservationResponse>> GetReservationsByBranch(int id)
        {

            var reservations = queriesReservation.GetReservations();
            var branch = queriesBranch.GetBranch(id);

            if (branch == null) return  Problem("Branch does not exist");

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

        public ActionResult<List<ReservationResponse>> GetReservationsByCar(int id)
        {

            var reservations = queriesReservation.GetReservations();
            var car = queriesCar.GetCar(id);

            if (car == null) return Problem("Car does not exist");

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

        public ActionResult<List<ReservationResponse>> GetReservationsByCustomer(int id)
        {

            var reservations = queriesReservation.GetReservations();
            var customer = queriesCustomer.GetCustomer(id);

            if (customer == null) return Problem("Customer does not exist");

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
        public ActionResult<List<ReservationResponse>> GetReservationsByDate(DateTime date)
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