using Proll.Shared.Dtos;
using Refit;

[Headers("Authorization: Bearer ")]
public interface IUserApi
{
    [Post("/api/user/addresses")]
    Task<ApiResult> SaveAddressAsync(AddressDto dto);

    [Get("/api/user/addresses")]
    Task<AddressDto[]> GetAddressesAsync();

    [Post("/api/user/change-password")]
    Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto);
}