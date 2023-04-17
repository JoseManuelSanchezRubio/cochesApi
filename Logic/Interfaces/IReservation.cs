using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Interfaces
{

    public interface IReservation
    {
        ActionResult<IEnumerable<ReservationResponse>> GetReservations();
        ActionResult<ReservationResponse> GetReservation(int id);
        ActionResult<ReservationResponse> PutReservation(int id, ReservationRequest reservationRequest);
        ActionResult<ReservationResponse> PostReservation(ReservationRequest reservationRequest);
        ActionResult<ReservationResponse> PostReservationOnDifferentBranch(ReservationRequestDifferentBranch reservationRequestDifferentBranch);
        ActionResult<ReservationResponse> DeleteReservation(int id);
        ActionResult<List<ReservationResponse>> GetReservationsByBranch(int id);
        ActionResult<List<ReservationResponse>> GetReservationsByCar(int id);
        ActionResult<List<ReservationResponse>> GetReservationsByCustomer(int id);
        ActionResult<List<ReservationResponse>> GetReservationsByDate(DateTime date);
    }
    
}