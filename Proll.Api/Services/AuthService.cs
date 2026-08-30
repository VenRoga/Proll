using Proll.Api.Models.BaseModelsContext;
using Proll.Api.Models.BaseModels;
using Proll.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;


namespace Proll.Api.Services
{
    public class AuthService
    {
        private readonly BaseModelContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        public AuthService(BaseModelContext context, IPasswordHasher<User> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }
        public async Task<ApiResult> RegisterAsync(RegisterDto dto)
        {
            if (await _context.Users.AnyAsync(u => u.Email == dto.Email))
            {
                return ApiResult.Fail("Email already exist");
            }

            var user = new User
            {
                Email = dto.Email,
                Mobile = dto.Mobile,
                Name = dto.Name,
            };
            user.PaswdHash = _passwordHasher.HashPassword(user, dto.Password);

            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return ApiResult.Success();
            }
            catch (Exception ex)
            {
                return ApiResult.Fail(ex.Message);
            }
        }

        public async Task<ApiResult<LoggedInUser>> LoginAsync(LoginDto dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Username);
            if (user == null) { return ApiResult<LoggedInUser>.Fail("User does not exist"); }

            var verificationResult = _passwordHasher.VerifyHashedPassword(user, user.PaswdHash, dto.Password);
            if (verificationResult != PasswordVerificationResult.Success)
            {
                return ApiResult<LoggedInUser>.Fail("Incorrect password");
            }

            var jwt = "JWT _TTOKEN";

            var loggedInUser = new LoggedInUser(user.Id, user.Name, user.Email, jwt);
            return ApiResult<LoggedInUser>.Success(loggedInUser);
        }
    }
}