using Proll.Api.Models.BaseModelsContext;
using Proll.Api.Models.BaseModels;
using Proll.Shared.Dtos;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;


namespace Proll.Api.Services
{
    public class AuthService
    {
        private readonly BaseModelContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IConfiguration _configuration;
        public AuthService(BaseModelContext context, IPasswordHasher<User> passwordHasher, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _configuration = configuration;

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

            var jwt = GenerateToken(user);

            var loggedInUser = new LoggedInUser(user.Id, user.Name, user.Email, jwt);
            return ApiResult<LoggedInUser>.Success(loggedInUser);
        }

        private string GenerateToken(User user)
        {
            Claim[] claims = [
                new (ClaimTypes.NameIdentifier, user.Id.ToString()),
                new (ClaimTypes.Name, user.Name.ToString()),
                new (ClaimTypes.Email, user.Email.ToString())
                ];

            var secretKey = _configuration.GetValue<string>("Jwt:SecretKey");
            var securityKey = System.Text.Encoding.UTF8.GetBytes(secretKey);
            var symmetricKey = new SymmetricSecurityKey(securityKey);

            var signiingCreds = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256);

            var expiereInMinutes = _configuration.GetValue<int>("Jwt:ExpireInMinutes");

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.GetValue<string>("Jwt:Issuer"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiereInMinutes),
                signingCredentials: signiingCreds
                );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }
    }
}