using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class ReservationResponse
{
    public int Id { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? FinalDate { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }

}


