using Proll.Api.Services;

namespace Proll.Api.Endpoints;

public static class ProductEndPoints
{
    public static IEndpointRouteBuilder MapProductEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapGet("/api/products", async (ProductService service)  
            => Results.Ok(await service.GetProductsAsync())
        ).Produces<ProductDto[]>()
        .WithName("Products");


        app.MapGet("/", async context =>
        {
            context.Response.Redirect("/api/products");
        })
        .WithName("Home")
        .AllowAnonymous();

        return app;
    }
}
