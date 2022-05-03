using System;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
         IProductRepository Products { get; }
         Task<bool> SaveChangesAsync();
    }
}