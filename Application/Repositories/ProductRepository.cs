using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Repositories
{
    public class ProductRepository : IProductRepository
    {
        protected readonly DataContext _context;
        public ProductRepository(DataContext context)
        {
            _context = context;
        }
        public void Add(Product product)
        {
            _context.Products.Add(product);
        }

        public async Task<Product> GetSingleById(int id)
        {
            return await _context.Products.FindAsync(id);
        }

        public async Task<List<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public void Remove(Product entity)
        {
            _context.Products.Remove(entity);
        }
    }
}