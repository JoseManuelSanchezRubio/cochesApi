using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Validations;
using Microsoft.AspNetCore.Cors;

namespace cochesApi.Logic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController
    {
        private IBranch branchValidation;
        public BranchController(IBranch _branch)
        {
            branchValidation = _branch;
        }

        [HttpGet]
        [DisableCors]
        public IEnumerable<BranchResponse> GetBranches()
        {
            return branchValidation.GetBranches();
        }

        [HttpGet("{id}")]
        public BranchResponseValidation GetBranch(int id)
        {
            return branchValidation.GetBranch(id);
        }

        [HttpPut("{id}")]
        public BranchResponseValidation PutBranch(int id, BranchRequest branchRequest)
        {
            return branchValidation.UpdateBranch(id, branchRequest);
        }

        [HttpPost]
        public BranchResponseValidation PostBranch(BranchRequest branchRequest)
        {
            return branchValidation.CreateBranch(branchRequest);
        }

        [HttpDelete("{id}")]
        public BranchResponseValidation DeleteBranch(int id)
        {
            return branchValidation.DeleteBranch(id);
        }
    }
}
