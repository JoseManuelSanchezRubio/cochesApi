using cochesApi.Logic.Models;

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