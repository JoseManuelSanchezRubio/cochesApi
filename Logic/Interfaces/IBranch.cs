using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;


namespace cochesApi.Logic.Interfaces
{

    public interface IBranch
    {
        ActionResult<IEnumerable<BranchRequest>> GetBranches();
        ActionResult<BranchRequest> GetBranch(int id);
        BranchResponseValidation PutBranch(int id, BranchRequest branchRequest);
        ActionResult<BranchRequest> PostBranch(BranchRequest branchRequest);
        BranchResponseValidation DeleteBranch(int id);
    }
    
}