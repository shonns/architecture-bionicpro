using System.IdentityModel.Tokens.Jwt; // Для JwtSecurityToken
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyHeader()
              .AllowAnyMethod()
    );
});

// Добавляем аутентификацию
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = "http://keycloak:8080/realms/reports-realm";
        options.RequireHttpsMetadata = false;
        
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidIssuers = new[]
            {
                "http://localhost:8080/realms/reports-realm",
                "http://keycloak:8080/realms/reports-realm"
            },
            ValidateAudience = false,
            RoleClaimType = "http://schemas.microsoft.com/ws/2008/06/identity/claims/role", // Тип claim для ролей "roles"

        };

        options.Events = new JwtBearerEvents
        {
            OnTokenValidated = context =>
            {
                Console.WriteLine("Token validated successfully");
                var claims = context.Principal?.Claims;
                if (claims != null)
                {
                    foreach (var claim in claims)
                    {
                        Console.WriteLine($"{claim.Type}: {claim.Value}");
                    }
                }
                return Task.CompletedTask;
            },
            OnAuthenticationFailed = context =>
            {
                Console.WriteLine($"Authentication failed: {context.Exception}");
                return Task.CompletedTask;
            }
        };
    });

// Добавляем авторизацию
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("ProtheticOnly", policy =>
    {
        policy.RequireRole("prothetic_user");
    });
});

var app = builder.Build();


app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers().RequireAuthorization();


app.Run();