using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;


namespace cochesApi.Logic.Interfaces
{
    public interface IBranch
    {
        ActionResult<IEnumerable<BranchRequest>> GetBranches();
        ActionResult<BranchRequest> GetBranch(int id);
        ActionResult<BranchRequest> PutBranch(int id, BranchRequest branchRequest);
        ActionResult<BranchRequest> PostBranch(BranchRequest branchRequest);
        ActionResult<BranchRequest> DeleteBranch(int id);
    }
}