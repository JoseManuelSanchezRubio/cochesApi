namespace cochesApi.Logic.Models;
public class TypeCar
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public bool IsAutomatic { get; set; }
    public bool IsGasoline { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}