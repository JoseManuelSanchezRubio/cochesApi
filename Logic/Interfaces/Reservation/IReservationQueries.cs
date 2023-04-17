using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;


namespace cochesApi.Logic.Interfaces
{

    public interface IReservationQueries
    {
        List<Reservation> GetReservations();
        Reservation GetReservation(int id);
        void RemoveReservation(Reservation reservation);
        void AddReservation(Reservation reservation);
    }
    
}