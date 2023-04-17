using Microsoft.EntityFrameworkCore;

namespace cochesApi.Logic.Models;

public class BranchRequest
{
    public string? Name { get; set; }
    public string? Location { get; set; }


}