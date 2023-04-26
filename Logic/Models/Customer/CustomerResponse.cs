using System.ComponentModel;

namespace cochesApi.Logic.Models;

public class CustomerResponse
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
    public string? Age { get; set; }
    public string? Email { get; set; }
    //public string? Password { get; set; }
}