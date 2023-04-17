using System.ComponentModel;

namespace cochesApi.Logic.Models;

public class CustomerRequest
{
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public CustomerAge? Age { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
}
