using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Email { get; set; }
    public string? Password {get; set; }
    public virtual ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();

}
