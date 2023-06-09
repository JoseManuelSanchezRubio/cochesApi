using cochesApi.Logic.Models;

namespace cochesApi.Logic.Interfaces
{
    public interface IPlanningQueries
    {
        List<Planning> GetPlannings();
        Planning GetPlanning(int id);
        void RemovePlanning(Planning planning);
        void AddPlanning(Planning planning);
        int GetNumberOfAvailableCarsByBranchByTypeCarByDate(int branchId, int typeCarId, DateTime date);
        IQueryable GetPlanningsByBranchByTypeCarByDate(int branchId, int typeCarId, DateTime initialDate, DateTime finalDate);
        IQueryable GetOlderPlanningsByBranchByTypeCarByDate(int branchId, int typeCarId, DateTime date);
        IQueryable GetPlanningsByBranchByDate(int branchId, DateTime initialDate, DateTime finalDate);
    }
}