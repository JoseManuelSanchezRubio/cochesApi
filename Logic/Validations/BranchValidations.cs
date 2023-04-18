using cochesApi.Logic.Interfaces;
using cochesApi.Logic.Models;
using Microsoft.AspNetCore.Mvc;

namespace cochesApi.Logic.Validations
{
    public class BranchResponseValidation
    {
        public BranchRequest? BranchResponse { get; set; }
        public bool Status { get; set; }
        public string? Message { get; set; }

        public BranchResponseValidation(BranchRequest? branchResponse)
        {
            BranchResponse = branchResponse;
            Status = true;
            Message = "OK";
        }
    }
    public class BranchValidation : IBranch
    {
        private IBranchQueries queriesBranch;
        private IPlanningQueries queriesPlanning;
        private ITypeCarQueries queriesTypeCar;
        private IDBQueries queriesDB;

        public BranchValidation(IBranchQueries _queriesBranch, IPlanningQueries _queriesPlanning, ITypeCarQueries _queriesTypeCar, IDBQueries _queries)
        {
            queriesBranch = _queriesBranch;
            queriesPlanning = _queriesPlanning;
            queriesTypeCar = _queriesTypeCar;
            queriesDB = _queries;
        }
        public List<BranchRequest> GetBranches()
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
        public BranchResponseValidation GetBranch(int id)
        {
            var branch = queriesBranch.GetBranch(id);
            if (branch == null)
            {
                BranchResponseValidation brv = new BranchResponseValidation(null);
                brv.Status = false;
                brv.Message = "Branch not found";
            }
            BranchRequest branchRequest = new BranchRequest();
            branchRequest.Name = branch?.Name;
            branchRequest.Location = branch?.Location;
            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(branchRequest);

            return branchResponseValidation;
        }
        public BranchResponseValidation UpdateBranch(int id, BranchRequest branchRequest)
        {
            var branch = queriesBranch.GetBranch(id);
            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(null); // Cambiar en un futuro

            if (branch == null)
            {
                branchResponseValidation.Status = false;
                branchResponseValidation.Message = "Branch does not exist";
                return branchResponseValidation;
            }
            branch.Name = branchRequest.Name;
            branch.Location = branchRequest.Location;

            queriesDB.Update(branch);

            queriesDB.SaveChangesAsync();

            return branchResponseValidation;

        }
        public BranchResponseValidation CreateBranch(BranchRequest branchRequest)
        {
            List<TypeCar> typeCars = queriesTypeCar.GetTypeCars();
            Branch branch = new Branch();
            branch.Name = branchRequest.Name;
            branch.Location = branchRequest.Location;

            queriesBranch.AddBranch(branch);
            queriesDB.SaveChangesAsync();


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

            queriesDB.SaveChangesAsync();

            BranchRequest branchResponse = new BranchRequest();
            branchResponse.Name = branchRequest.Name;
            branchResponse.Location = branchRequest.Location;

            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(branchResponse);

            return branchResponseValidation;
        }
        public BranchResponseValidation DeleteBranch(int id)
        {
            var branch = queriesBranch.GetBranch(id);
            BranchResponseValidation branchResponseValidation = new BranchResponseValidation(null); // Cambiar en un futuro

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
            queriesDB.SaveChangesAsync();

            return branchResponseValidation;
        }
    }
}