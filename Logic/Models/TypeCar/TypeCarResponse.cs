namespace cochesApi.Logic.Models;
public class TypeCarResponse
{

    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public bool IsAutomatic { get; set; }
    public bool IsGasoline { get; set; }

    //de momento no se usa
}