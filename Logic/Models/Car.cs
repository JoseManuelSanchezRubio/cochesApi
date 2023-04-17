using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class Car
{
    public int Id { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public bool isAutomatic { get; set; }
    public bool isGasoline { get; set; }
    public int BranchId { get; set; }
    public int TypeCarId { get; set; }
    public virtual ICollection<Reservation>? Reservations { get; set; } = new List<Reservation>();
    public virtual Branch? Branch { get; set; }
    public virtual TypeCar? TypeCar { get; set; }

    

}