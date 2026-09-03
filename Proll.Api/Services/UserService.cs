using Proll.Api.Models.BaseModelsContext;
using Proll.Api.Models.BaseModels;
using Proll.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Identity;


namespace Proll.Api.Services
{
    public class UserService
    {
        private readonly BaseModelContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public UserService(BaseModelContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            
        }
        public async Task<ApiResult> SaveAddressAsync(AddressDto dto, int userId)
        {
            UserAddress? userAddress = null;
            if (dto.Id == 0)
            {
                userAddress = new UserAddress
                {
                    Address = dto.Address,
                    Id = dto.Id,
                    IsDefault = dto.IsDefault,
                    Name = dto.Name,
                    UserId = userId
                };
                _context.UserAddresses.Add(userAddress);
            }
            else
            {
                userAddress = await _context.UserAddresses.FindAsync(dto.Id);
                if (userAddress is null)
                {
                    return ApiResult.Fail("Invalid request");
                }

                userAddress.Address = dto.Address;
                userAddress.Name = dto.Name;
                userAddress.IsDefault = dto.IsDefault;

                _context.UserAddresses.Update(userAddress);
            }
            try
            {
                if (dto.IsDefault)
                {
                    var defaultAddress = await _context.UserAddresses
                        .FirstOrDefaultAsync(a => a.UserId == userId && a.IsDefault && a.Id != dto.Id);
                    if (defaultAddress is not null)
                    {
                        defaultAddress.IsDefault = false;
                    }
                }

                await _context.SaveChangesAsync();
                return ApiResult.Success();
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ex.Message);
            }

        }
        public async Task<AddressDto[]> GetAddressesAsync(int userId) =>
                await _context.UserAddresses
                    .AsNoTracking().Where(a => a.Id == userId)
                    .Select(a => new AddressDto
                    {
                        Id = a.Id,
                        Address = a.Address,
                        IsDefault = a.IsDefault,
                        Name = a.Name
                    })
                    .ToArrayAsync();
        public async Task<ApiResult> ChangePasswordAsync(ChangePasswordDto dto, int userId) 
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);

                if (user is null)
                {
                    return ApiResult.Fail("User does not exist");
                }

                var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PaswdHash, dto.CurrentPassword);
                if (verificationResult != PasswordVerificationResult.Success)
                {
                    return ApiResult.Fail("Incorrect password");
                }

                user.PaswdHash = _passwordHasher.HashPassword(user, dto.NewPassword);
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return ApiResult.Success();
            }
            catch(Exception ex) 
            {
                return ApiResult.Fail(ex.Message);
            }
        }
    }
}