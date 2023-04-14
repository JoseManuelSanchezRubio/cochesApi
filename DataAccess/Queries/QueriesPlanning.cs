using cochesApi.Logic.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Interfaces;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesPlanning : IPlanningQueries
    {
        private readonly myAppContext _context;
        public QueriesPlanning(myAppContext context)
        {
            _context = context;
        }


        public List<Planning> GetPlannings(){
            return _context.Plannings.ToList();
        }
        public Planning GetPlanning(int id){
            return _context.Plannings.Find(id)!;
        }
        public void RemovePlanning(Planning planning){
            _context.Plannings.Remove(planning);
        }
        public void AddPlanning(Planning planning){
            _context.Plannings.Add(planning);
        }
        public int GetAvailableCarsByBranchByTypeCarByDate(int branchId, int typeCarId, DateTime date){
            return (from p in _context.Plannings
                    where p.BranchId == branchId && p.TypeCarId == typeCarId && p.Day.Date == date.Date
                    select p.AvailableCars).First();
        }
        public IQueryable GetPlanningsByBranchByTypeCar(int branchId, int typeCarId){
            return (from p in _context.Plannings
                            where p.BranchId == branchId && p.TypeCarId == typeCarId
                            select p);
        }
    }
}