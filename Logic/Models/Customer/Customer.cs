using System.ComponentModel;

namespace cochesApi.Logic.Models;

public class Customer
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Age { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }

    public virtual ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
}
/* public enum CustomerAge
{
    [Description("19-24")]
    Age19_24 = 1,
    [Description("25-74")]
    Age25_34 = 2,
    [Description("74+")]
    Age75 = 3
} */
