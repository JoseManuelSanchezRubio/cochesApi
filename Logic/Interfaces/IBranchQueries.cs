using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Mvc;


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