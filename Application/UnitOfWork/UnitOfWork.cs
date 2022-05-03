using System.Threading.Tasks;
using Application.Interfaces;
using Application.Repositories;
using Persistence;

namespace Application.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DataContext _context;
        public UnitOfWork(DataContext context)
        {
            _context = context;
            Products = new ProductRepository(_context);
        }

        public IProductRepository Products {get; private set;}

        public void Dispose()
        {
            _context.Dispose();
        }
        public async Task<bool> SaveChangesAsync()
        {
             return await _context.SaveChangesAsync() > 0;
        }
    }
}