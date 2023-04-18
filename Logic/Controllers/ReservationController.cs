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
    public class ReservationController
    {

        private IReservation reservationValidation;
        public ReservationController(IReservation _reservation)
        {
            reservationValidation = _reservation;
        }


        // GET: api/Reservation
        [HttpGet]
        public List<ReservationResponse> GetReservations()
        {
            return reservationValidation.GetReservations();
        }

        // GET: api/Reservation/5
        [HttpGet("{id}")]
        public ReservationResponseValidation GetReservation(int id)
        {
            return reservationValidation.GetReservation(id);
        }

        // PUT: api/Reservation/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public ReservationResponseValidation PutReservation(int id, ReservationRequest reservationRequest)
        {
            return reservationValidation.UpdateReservation(id, reservationRequest);
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "customer")]
        public ReservationResponseValidation PostReservation(ReservationRequest reservationRequest)
        {
            return reservationValidation.CreateReservation(reservationRequest);
        }
        [HttpPost("reservationOnDifferentBranch")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "customer")]
        public ReservationResponseValidation PostReservationOnDifferentBranch(ReservationRequestDifferentBranch reservationRequestDifferentBranch)
        {
            return reservationValidation.CreateReservationOnDifferentBranch(reservationRequestDifferentBranch);
        }
        // DELETE: api/Reservation/5
        [HttpDelete("{id}")]
        public ReservationResponseValidation DeleteReservation(int id)
        {
            return reservationValidation.DeleteReservation(id);
        }

        [HttpGet("branch/{id}")]
        public List<ReservationResponse> GetReservationsByBranch(int id)
        {
            if (reservationValidation.GetReservationsByBranch(id) == null) return new List<ReservationResponse>();

            return reservationValidation.GetReservationsByBranch(id);

        }
        [HttpGet("car/{id}")]
        public List<ReservationResponse> GetReservationsByCar(int id)
        {
            if (reservationValidation.GetReservationsByCar(id) == null) return new List<ReservationResponse>();

            return reservationValidation.GetReservationsByCar(id);
        }

        [HttpGet("customer/{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [Authorize(Roles = "customer")]
        public List<ReservationResponse> GetReservationsByCustomer(int id)
        {
            if (reservationValidation.GetReservationsByCustomer(id) == null) return new List<ReservationResponse>();

            return reservationValidation.GetReservationsByCustomer(id);
        }
        [HttpGet("date/{date}")]
        public List<ReservationResponse> GetReservationsByDate(DateTime date)
        {
            return reservationValidation.GetReservationsByDate(date);
        }
    }
}
