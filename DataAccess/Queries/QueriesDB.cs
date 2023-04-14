using cochesApi.Logic.Models;
using DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using cochesApi.Logic.Interfaces;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesDB : IDBQueries
    {
        private readonly myAppContext _context;
        public QueriesDB(myAppContext context)
        {
            _context = context;
        }
        public async Task SaveChangesAsync(){
            await _context.SaveChangesAsync();
        }
        public void Update(Object obj){
            _context.Entry(obj).State = EntityState.Modified;
        }


























    }
}