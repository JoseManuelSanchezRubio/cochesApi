using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using cochesApi.Logic.Validations;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {

        private IReservation reservationValidation;
        public ReservationController(IReservation _reservation){
            reservationValidation=_reservation;
        }


        // GET: api/Reservation
        [HttpGet]
        public ActionResult<IEnumerable<ReservationResponse>> GetReservations()
        {
            return reservationValidation.GetReservations();
        }

        // GET: api/Reservation/5
        [HttpGet("{id}")]
        public ActionResult<ReservationResponse> GetReservation(int id)
        {
            var reservation = reservationValidation.GetReservation(id);
            if (reservation == null) return Problem("Reservation does not exist");
            return reservation;
        }

        // PUT: api/Reservation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ReservationResponseValidation PutReservation(int id, ReservationRequest reservationRequest)
        {
            return reservationValidation.PutReservation(id, reservationRequest);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "customer")]
        public ActionResult<ReservationResponse> PostReservation(ReservationRequest reservationRequest)
        {
            return reservationValidation.PostReservation(reservationRequest);
        }
        [HttpPost("reservationOnDifferentBranch")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "customer")]
        public ActionResult<ReservationResponse> PostReservationOnDifferentBranch(ReservationRequestDifferentBranch reservationRequestDifferentBranch){
            return reservationValidation.PostReservationOnDifferentBranch(reservationRequestDifferentBranch);
        }
        // DELETE: api/Reservation/5
        [HttpDelete("{id}")]
        public ReservationResponseValidation DeleteReservation(int id)
        {
            return reservationValidation.DeleteReservation(id);
        }

        [HttpGet("branch/{id}")]
        public ActionResult<List<ReservationResponse>> GetReservationsByBranch(int id)
        {
            if (reservationValidation.GetReservationsByBranch(id) == null) return Problem("Branch does not exist");

            return reservationValidation.GetReservationsByBranch(id);

        }
        [HttpGet("car/{id}")]
        public ActionResult<List<ReservationResponse>> GetReservationsByCar(int id)
        {
            if (reservationValidation.GetReservationsByCar(id) == null) return Problem("Car does not exist");

            return reservationValidation.GetReservationsByCar(id);
        }

        [HttpGet("customer/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "customer")]
        public ActionResult<List<ReservationResponse>> GetReservationsByCustomer(int id)
        {
            if (reservationValidation.GetReservationsByCustomer(id) == null) return Problem("Customer does not exist");

            return reservationValidation.GetReservationsByCustomer(id);
        }
        [HttpGet("date/{date}")]
        public ActionResult<List<ReservationResponse>> GetReservationsByDate(DateTime date)
        {
            return reservationValidation.GetReservationsByDate(date);
        }


    }
}
