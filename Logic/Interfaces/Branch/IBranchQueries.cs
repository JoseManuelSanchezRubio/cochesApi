using cochesApi.Logic.Models;

namespace cochesApi.Logic.Interfaces
{
    public interface IBranchQueries
    {
        List<Branch> GetBranches();
        Branch GetBranch(int id);
        void RemoveBranch(Branch branch);
        void AddBranch(Branch branch);
    }
}