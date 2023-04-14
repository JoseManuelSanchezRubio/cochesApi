using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using cochesApi.DataAccess.Queries;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class BranchResponseValidation
    {
        public Branch? Branch { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public BranchResponseValidation(Branch? branch)
        {
            Branch = branch;
            Status = true;
            Message = "OK";
        }
    }

    public class BranchValidation : IBranch
    {
        private IBranchQueries queriesBranch;
        private IPlanningQueries queriesPlanning;
        private ITypeCarQueries queriesTypeCar;
        private IDBQueries queries;

        public BranchValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries)
        {
            queriesBranch = _queriesBranch;
            queriesPlanning = _queriesPlanning;
            queriesTypeCar = _queriesTypeCar;
            queries = _queries;
        }

        public ActionResult<IEnumerable<BranchRequest>> GetBranches()
        {
            var branches = queriesBranch.GetBranches();
            List<BranchRequest> branchesRequest = new List<BranchRequest>();
            foreach (Branch branch in branches)
            {
                BranchRequest branchRequest = new BranchRequest();
                branchRequest.Name = branch.Name;
                branchRequest.Location = branch.Location;
                branchesRequest.Add(branchRequest);
            }
            return branchesRequest;
        }
        public ActionResult<BranchRequest> GetBranch(int id)
        {
            var branch = queriesBranch.GetBranch(id);
            if (branch == null) return null!;
            BranchRequest branchRequest = new BranchRequest();
            branchRequest.Name = branch.Name;
            branchRequest.Location = branch.Location;

            return branchRequest;
        }

        public BranchResponseValidation PutBranch(int id, BranchRequest branchRequest)
        {
            var branch = queriesBranch.GetBranch(id);
            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(branch);

            if (branch == null)
            {
                branchResponseValidation.Status = false;
                branchResponseValidation.Message = "Branch does not exist";
                return branchResponseValidation;
            }
            branch.Name = branchRequest.Name;
            branch.Location = branchRequest.Location;

            queries.Update(branch);

            queries.SaveChangesAsync();

            return branchResponseValidation;

        }
        public BranchResponseValidation PostBranch(BranchRequest branchRequest)
        {
            List<TypeCar> typeCars = queriesTypeCar.GetTypeCars();
            Branch branch = new Branch();
            branch.Name = branchRequest.Name;
            branch.Location = branchRequest.Location;
            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(branch);

            queriesBranch.AddBranch(branch);
            queries.SaveChangesAsync();

            var branchh = queriesBranch.GetBranch(branch.Id); //esto es para poder coger el id del branch


            for (int i = 0; i < 365; i++)
            {
                for (int j = 1; j < typeCars.Count + 1; j++)
                {
                    Planning planning = new Planning();
                    DateTime date = DateTime.Now;
                    date = date.AddDays(i);
                    planning.Day = date;
                    planning.BranchId = branchh!.Id;
                    planning.TypeCarId = j;
                    planning.AvailableCars = 0;

                    queriesPlanning.AddPlanning(planning);
                }
            }

            queries.SaveChangesAsync();
            return branchResponseValidation;
        }
        public BranchResponseValidation DeleteBranch(int id)
        {
            var branch = queriesBranch.GetBranch(id);
            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(branch);

            if (branch == null)
            {
                branchResponseValidation.Status = false;
                branchResponseValidation.Message = "Branch does not exist";
                return branchResponseValidation;
            }

            queriesBranch.RemoveBranch(branch);
            var plannings = queriesPlanning.GetPlannings();
            foreach (Planning planning in plannings)
            {
                if (planning.BranchId == id)
                {
                    queriesPlanning.RemovePlanning(planning);
                }
            }
            queries.SaveChangesAsync();

            return branchResponseValidation;
        }
    }
}