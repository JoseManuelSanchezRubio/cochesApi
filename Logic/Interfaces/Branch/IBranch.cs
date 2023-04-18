using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Validations;


namespace cochesApi.Logic.Interfaces
{
    public interface IBranch
    {
        List<BranchRequest> GetBranches();
        BranchResponseValidation GetBranch(int id);
        BranchResponseValidation UpdateBranch(int id, BranchRequest branchRequest);
        BranchResponseValidation CreateBranch(BranchRequest branchRequest);
        BranchResponseValidation DeleteBranch(int id);
    }
}