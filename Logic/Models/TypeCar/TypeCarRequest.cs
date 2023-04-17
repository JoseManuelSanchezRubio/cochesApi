namespace cochesApi.Logic.Models;
public class TypeCarRequest
{
    public TypeCarRequest(string name){
        Name=name;
    }
    public string? Name { get; set; }
}