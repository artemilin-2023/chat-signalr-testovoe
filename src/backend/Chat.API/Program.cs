using Chat.API.Middlewares;
using Chat.API.ServiceRegistration;
using Chat.Application.ServiceRegistration;
using Chat.Common;
using Chat.Contracts;
using Chat.Infrastructure.ServiceRegistration;
using Microsoft.EntityFrameworkCore;


var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;
var configuration = builder.Configuration;

services
    .AddContracts()
    .AddApi(configuration)
    .AddApplication(configuration)
    .AddInfrastructure(configuration);

var app = builder.Build();
app.UseStaticLoggerInjecting();

// для скорости разработки оставлю это так
using var scope = app.Services.CreateScope();
var db = scope.ServiceProvider.GetRequiredService<DbContext>();
await db.Database.MigrateAsync();

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthentication();
app.UseAuthorization();

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.MapControllers();
app.MapHubsForAssemblyContains<Program>();

app.Run();