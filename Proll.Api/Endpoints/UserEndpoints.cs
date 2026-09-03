using Proll.Api.Services;
using Proll.Shared;
using Proll.Shared.Dtos;
using System.Security.Claims;

namespace Proll.Api.Endpoints;

public static class UserEndpoints
{
    public static IEndpointRouteBuilder MapUserEndpoints(this IEndpointRouteBuilder app)
    {
        var userGroup = app.MapGroup("/api/user")
            .RequireAuthorization()
            .WithTags("User");

        userGroup.MapPost("/addresses", async (AddressDto dto, UserService service, ClaimsPrincipal principal) =>
            {
                return Results.Ok(await service.SaveAddressAsync(dto, principal.GetUserId()));
            })
            .Produces<ApiResult>()
            .WithName("Save-Address");

        userGroup.MapGet("/addresses", async (UserService service, ClaimsPrincipal principal) =>
        {
            return Results.Ok(await service.GetAddressesAsync(principal.GetUserId()));
        })
            .Produces<AddressDto[]>()
            .WithName("Get-Address");

        userGroup.MapPost("/change-password", async (ChangePasswordDto dto, UserService service, ClaimsPrincipal principal) =>
        {
            return Results.Ok(await service.ChangePasswordAsync(dto, principal.GetUserId()));
        })
            .Produces<ApiResult>()
            .WithName("Change-password");

        return app;
    }
}
