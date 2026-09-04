using Proll.Shared.Dtos;
using Refit;
using System;
using System.Collections.Generic;
using System.Text;

namespace Proll.Apis
{
    public interface IProductApi
    {
        [Get("/api/products")]
        Task<ProductDto[]> GetProductsAsync();
    }
}
