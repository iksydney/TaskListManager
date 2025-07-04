using AutoMapper;
using HealthChecks.UI.Client;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Serilog;
using TaskListManager.API.Extensions;
using TaskListManager.API.MappingConfigurations;
using TaskListManager.Business.Factory;
using TaskListManager.Business.Implementation;

var builder = WebApplication.CreateBuilder(args);

var services = builder.Services;
var config = builder.Configuration;

Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

try
{
    #region
    var mappingConfiguration = new MapperConfiguration(c =>
    {
        c.AddProfile(new MappingConfiguration());
    });
    var mapper = mappingConfiguration.CreateMapper();
    services.AddSingleton(mapper);

    #endregion
    services.AddHealthChecks()
        .AddCheck<RandomHealthChecks>("random-check")
        .AddCheck<SqlHealthCheck>("sql-check", HealthStatus.Unhealthy);

    // Add services to the container.

    services.AddControllers();

    services.RegisterApplicationService<TaskListManagerService>();

    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();
    services.SwaggerExtensions();
    services.AddJwtSecurityKey(config);
    services.AddHttpClient();
    services.AddScoped<IHttpService, HttpService>();
    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    services.AddEndpointsApiExplorer();
    services.AddSwaggerGen();

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        app.UseDeveloperExceptionPage();
    }
    else
    {
        app.UseExceptionHandler("/Home/Error");
        app.UseHsts();
    }
    app.SwaggerDocumentation();
    app.UseSwagger();
    app.UseCors(c => c.AllowAnyHeader().AllowAnyOrigin().AllowAnyMethod());

    app.UseHttpsRedirection();

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();
    app.MapHealthChecks("/health",
        new HealthCheckOptions
        {
            ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
        });

    Log.Information("Starting application");

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex.Message, "Host terminated unexpectedly");
}
finally
{
    Log.CloseAndFlush();
}
