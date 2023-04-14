using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class Reservation
{
    public int Id { get; set; }
    public DateTime? InitialDate { get; set; }
    public DateTime? FinalDate { get; set; }
    public int CarId { get; set; }
    public int CustomerId { get; set; }
    public int BranchId { get; set; }

    
    public virtual Customer? Customer { get; set; }
    public virtual Car? Car { get; set; }
    public virtual Branch? Branch { get; set; }

}


