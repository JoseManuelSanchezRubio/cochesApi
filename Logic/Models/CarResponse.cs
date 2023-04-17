using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class CarResponse
{
    public int Id { get; set; }
    public string? Brand { get; set; }
    public string? Model { get; set; }
    public int BranchId { get; set; }
    public int TypeCarId { get; set; }
    public bool isGasoline { get; set; }
    public bool isAutomatic { get; set; }


    

}