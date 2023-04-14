using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class Planning
{
    public int Id { get; set; }
    public DateTime Day { get; set; }
    public int BranchId { get; set; }
    public int TypeCarId { get; set; }
    public int AvailableCars { get; set; }
}