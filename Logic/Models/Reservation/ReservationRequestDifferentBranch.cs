namespace cochesApi.Logic.Models;
public class ReservationRequestDifferentBranch
{
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public bool isInternational { get; set; }
    public bool hasGPS { get; set; }
    public int numberOfDrivers { get; set; }
    public int TypeCarId { get; set; }
    public int CustomerId { get; set; }
    public int PickUpBranchId { get; set; }
    public int ReturnBranchId { get; set; }
}


