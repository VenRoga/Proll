using Proll.Api.Models.BaseModelsContext;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using SQLitePCL;


namespace Proll.Api.Services
{
    public class ProductService
    {
        private readonly BaseModelContext _context;
        public ProductService(BaseModelContext context)
        {
            _context = context;
        }

        public async Task<ProductDto[]> GetProductsAsync() =>
            await _context.Products
            .AsNoTracking()
            .Select(p => new ProductDto
            {
                Id = p.Id,
                ImageUrl = p.ImageUrl,
                Name = p.Name,
                Price = p.Price,
                Unit = p.Unit
            }).ToArrayAsync();
    }
}