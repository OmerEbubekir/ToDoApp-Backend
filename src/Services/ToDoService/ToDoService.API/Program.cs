using Scalar.AspNetCore;
using ToDoService.API.Extensions;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddInfrastructureAndWebServices(builder.Configuration);

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler(); 
app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();