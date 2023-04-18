using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Validations;

namespace cochesApi.Logic.Interfaces
{
    public interface IReservation
    {
        List<ReservationResponse> GetReservations();
        ReservationResponseValidation GetReservation(int id);
        ReservationResponseValidation UpdateReservation(int id, ReservationRequest reservationRequest);
        ReservationResponseValidation CreateReservation(ReservationRequest reservationRequest);
        ReservationResponseValidation CreateReservationOnDifferentBranch(ReservationRequestDifferentBranch reservationRequestDifferentBranch);
        ReservationResponseValidation DeleteReservation(int id);
        List<ReservationResponse> GetReservationsByBranch(int id);
        List<ReservationResponse> GetReservationsByCar(int id);
        List<ReservationResponse> GetReservationsByCustomer(int id);
        List<ReservationResponse> GetReservationsByDate(DateTime date);
    }
}