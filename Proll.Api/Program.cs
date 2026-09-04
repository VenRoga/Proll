using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Proll.Api.Endpoints;
using Proll.Api.Models;
using Proll.Api.Models.BaseModels;
using Proll.Api.Models.BaseModelsContext;
using Proll.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddOpenApi();

builder.Services.AddDbContext<BaseModelContext>(options => 
{
    var connectionString = builder.Configuration.GetConnectionString("Default");
    options.UseSqlite(connectionString);
});

builder.Services.AddTransient<AuthService>()
    .AddTransient<OrderService>()
    .AddTransient<ProductService>()
    .AddTransient<UserService>()
    .AddTransient<IPasswordHasher<User>, PasswordHasher<User>>();


builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var issuer = builder.Configuration.GetValue<string>("Jwt:Issuer");

    var secretKey = builder.Configuration.GetValue<string>("Jwt:SecretKey");
    var securityKey = System.Text.Encoding.UTF8.GetBytes(secretKey);
    var symmetricKey = new SymmetricSecurityKey(securityKey);

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = issuer,
        ValidateIssuer = true,
        IssuerSigningKey = symmetricKey,
        ValidateIssuerSigningKey = true,
        ValidateAudience = false
    };
});  

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    AuthoMigrateDb(app.Services);
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapAuthEndpoints()
   .MapUserEndpoints()
   .MapProductEndpoints()
   .MapOrderEndpoints();

app.Run();


static void AuthoMigrateDb(IServiceProvider sp)//автоматическая миграция в бд
{
    using var scope = sp.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<BaseModelContext>();
    if(context.Database.GetPendingMigrations().Any())
        context.Database.Migrate();
}