using cochesApi.Logic.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Interfaces;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesReservation : IReservationQueries
    {
        private readonly myAppContext _context;
        public QueriesReservation(myAppContext context)
        {
            _context = context;
        }


        public List<Reservation> GetReservations(){
            return _context.Reservations.ToList();
        }
        public Reservation GetReservation(int id){
            return _context.Reservations.Find(id)!;
        }
        public void RemoveReservation(Reservation reservation){
            _context.Reservations.Remove(reservation);
        }
        public void AddReservation(Reservation reservation){
            _context.Reservations.Add(reservation);
        }



    }
}