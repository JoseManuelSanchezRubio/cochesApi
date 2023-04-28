namespace cochesApi.Logic.Models;
public class TypeCarRequest
{
    public string? Name { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public bool IsAutomatic { get; set; }
    public bool IsGasoline { get; set; }
    public double Price { get; set; }
}