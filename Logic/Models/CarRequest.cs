using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class CarRequest
{
    public string? Model { get; set; }
    public string? Brand { get; set; }
    public int BranchId { get; set; }
    public int TypeCarId { get; set; }
    public bool isAutomatic { get; set; }
    public bool isGasoline { get; set; }


    

}