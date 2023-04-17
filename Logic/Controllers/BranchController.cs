using Microsoft.AspNetCore.Mvc;
using cochesApi.Logic.Models;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Logic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private IBranch branchValidation;
        public BranchController(IBranch _branch)
        {
            branchValidation = _branch;
        }

        [HttpGet]
        public ActionResult<IEnumerable<BranchRequest>> GetBranches()
        {
            return branchValidation.GetBranches();
        }

        [HttpGet("{id}")]
        public ActionResult<BranchRequest> GetBranch(int id)
        {
            return branchValidation.GetBranch(id);
        }
        
        [HttpPut("{id}")]
        public ActionResult<BranchRequest> PutBranch(int id, BranchRequest branchRequest)
        {
            return branchValidation.PutBranch(id, branchRequest);
        }
        
        [HttpPost]
        public ActionResult<BranchRequest> PostBranch(BranchRequest branchRequest)
        {
            return branchValidation.PostBranch(branchRequest);
        }

        [HttpDelete("{id}")]
        public ActionResult<BranchRequest> DeleteBranch(int id)
        {
            return branchValidation.DeleteBranch(id);
        }
    }
}
