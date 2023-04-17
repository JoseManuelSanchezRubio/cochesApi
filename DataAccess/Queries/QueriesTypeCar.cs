using cochesApi.Logic.Models;
using DataAccess.Data;
using cochesApi.Logic.Interfaces;

namespace cochesApi.DataAccess.Queries
{
    public class QueriesTypeCar : ITypeCarQueries
    {
        private readonly myAppContext _context;
        public QueriesTypeCar(myAppContext context)
        {
            _context = context;
        }

        public List<TypeCar> GetTypeCars()
        {
            return _context.TypeCars.ToList();
        }
        public TypeCar GetTypeCar(int id)
        {
            return _context.TypeCars.Find(id)!;
        }
        public void RemoveTypeCar(TypeCar typeCar)
        {
            _context.TypeCars.Remove(typeCar);
        }
        public void AddTypeCar(TypeCar typeCar)
        {
            _context.TypeCars.Add(typeCar);
        }
    }
}