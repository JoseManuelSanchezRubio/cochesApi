namespace cochesApi.Logic.Interfaces
{
    public interface IDBQueries
    {
        Task SaveChangesAsync();
        void Update(Object obj);
    }

}