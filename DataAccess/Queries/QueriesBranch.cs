using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesBranch : IBranchQueries
    {
        private readonly myAppContext _context;
        public QueriesBranch(myAppContext context)
        {
            _context = context;
        }


        public List<Branch> GetBranches(){
            return _context.Branches.ToList();
        }
        public Branch GetBranch(int id){
            return _context.Branches.Find(id)!;
        }
        public void RemoveBranch(Branch branch){
            _context.Branches.Remove(branch);
        }
        public void AddBranch(Branch branch){
            _context.Branches.Add(branch);
        }



    }
}