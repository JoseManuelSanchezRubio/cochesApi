using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class Branch
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Location { get; set; }


    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
    public virtual ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();


}