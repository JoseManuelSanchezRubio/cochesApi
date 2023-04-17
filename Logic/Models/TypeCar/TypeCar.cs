namespace cochesApi.Logic.Models;
public class TypeCar
{
    public int Id { get; set; }
    public string? Name { get; set; }

    public virtual ICollection<Car> Cars { get; set; } = new List<Car>();
}