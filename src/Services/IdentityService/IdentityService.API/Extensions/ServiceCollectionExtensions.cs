using System.Text;
using FluentValidation;
using IdentityService.Business.Interfaces;
using IdentityService.Business.Services;
using IdentityService.API.Validators;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Core.Filters;
using Shared.Core.Middlewares;
using Shared.Data.Contexts;
using Shared.Data.Entities;
using IdentityService.API.Mappings;
using Microsoft.Extensions.DependencyInjection;
using IdentityService.Business.Mappings;
namespace IdentityService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddIdentityInfrastructureAndWebServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 1. Veritabanı
        services.AddDbContext<AuthDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // 2. Identity Ayarları
        services.AddIdentity<AppUser, IdentityRole>(options => {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.User.RequireUniqueEmail = true;
        }).AddEntityFrameworkStores<AuthDbContext>().AddDefaultTokenProviders();

        // 3. DI (Servisler)
        services.AddScoped<IAuthService, AuthService>();

        // 4. FluentValidation Kurallarını Sisteme Tanıtma
        services.AddValidatorsFromAssemblyContaining<RegisterRequestValidator>();

        // 5. Controller'lar ve ValidationFilter (Araya giren güvenlik duvarımız)
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });

        // 6. Authentication (Token Ayarları)
        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(options => {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidAudience = jwtSettings["Audience"],
                IssuerSigningKey = new SymmetricSecurityKey(secretKey)
            };
        });

        // 7. Global Hata Yönetimi & Swagger
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        services.AddAutoMapper(cfg => {
            cfg.AddProfile<ApiMappingProfile>();
            cfg.AddProfile<MappingProfile>();
        });

        services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info = new OpenApiInfo { Title = "Identity Service API", Version = "v1" };

                // Arayüze JWT Bearer butonu ekliyoruz
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = "JWT Authorization header using the Bearer scheme."
                });

                document.SecurityRequirements.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
                },
                Array.Empty<string>()
            }
        });

                return Task.CompletedTask;
            });
        });


        return services;
    }
}