using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Shared.Core.Filters;
using Shared.Core.Interfaces;
using Shared.Core.Middlewares;
using Shared.Core.Services;
using Shared.Data.Contexts;
using ToDoService.Application.Interfaces;
using ToDoService.Application.Services;

      
namespace ToDoService.API.Extensions;


public static class ServiceCollectionExtensions
{
    
    public static IServiceCollection AddInfrastructureAndWebServices(this IServiceCollection services, IConfiguration configuration)
    {
       
        services.AddDbContext<ToDoDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        
        services.AddScoped<IToDoService, ToDoManager>();
        services.AddControllers(options =>
        {
            options.Filters.Add<ValidationFilter>();
        });
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        var jwtSettings = configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options => {
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
        services.AddAuthorization();

        
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();

        
        services.AddOpenApi(options => {
            options.AddDocumentTransformer((document, context, cancellationToken) => {
                document.Info = new OpenApiInfo { Title = "ToDo Service API", Version = "v1" };
                document.Components ??= new OpenApiComponents();
                document.Components.SecuritySchemes.Add("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT"
                });
                document.SecurityRequirements.Add(new OpenApiSecurityRequirement {
                    { new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } }, Array.Empty<string>() }
                });
                return Task.CompletedTask;
            });
        });

        return services;
    }
}