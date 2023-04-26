namespace cochesApi.Logic.Models;
public class TypeCarResponse
{
    public TypeCarResponse(string name)
    {
        Name = name;
    }
    public int Id { get; set; }
    public string? Name { get; set; }

    //de momento no se usa
}