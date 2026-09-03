using Proll.Api.Services;

namespace Proll.Api.Endpoints;

public static class ProductEndPoints
{
    public static IEndpointRouteBuilder MapProductEnpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (ProductService service)  
            => Results.Ok(await service.GetProductsAsync())
        ).Produces<ProductDto[]>()
        .WithName("Products");

        return app;
    }
}
