namespace cochesApi.Logic.Models;
public class ReservationRequest
{
    public DateTime InitialDate { get; set; }
    public DateTime FinalDate { get; set; }
    public bool isInternational { get; set; }
    public bool hasGPS { get; set; }
    public int numberOfDrivers { get; set; }
    public int TypeCarId { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }
}


