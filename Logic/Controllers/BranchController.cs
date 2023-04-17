using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DataAccess.Data;
using cochesApi.Logic.Models;
using cochesApi.Logic.Validations;
using cochesApi.Logic.Interfaces;

namespace cochesApi.Logic.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private IBranch branchValidation;
        public BranchController(IBranch _branch){
            branchValidation=_branch;
        }

        // GET: api/Branch
        [HttpGet]
        public ActionResult<IEnumerable<BranchRequest>> GetBranches()
        {
            return branchValidation.GetBranches();
        }

        // GET: api/Branch/5
        [HttpGet("{id}")]
        public ActionResult<BranchRequest> GetBranch(int id)
        {
            return branchValidation.GetBranch(id);
        }

        // PUT: api/Branch/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public BranchResponseValidation PutBranch(int id, BranchRequest branchRequest)
        {
            return branchValidation.PutBranch(id, branchRequest);
        }

        // POST: api/Branch
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public ActionResult<BranchRequest> PostBranch(BranchRequest branchRequest)
        {
            return branchValidation.PostBranch(branchRequest);
        }

        // DELETE: api/Branch/5
        [HttpDelete("{id}")]
        public BranchResponseValidation DeleteBranch(int id)
        {
            return branchValidation.DeleteBranch(id);
        }

    }
}
