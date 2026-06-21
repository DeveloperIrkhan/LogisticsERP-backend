using LogisticsERP.API.Data;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using LogisticsERP.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;



var builder = WebApplication.CreateBuilder(args);


// configuring swagger UI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.OpenApiInfo
    {
        Version = "v1",
        Title = "Logestics ERP API",
        Description = "API for Logestics ERP System"
    });
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        policy =>
        {
            policy.WithOrigins(
                "http://localhost:3000",
                "https://localhost:3000")
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof
    (GenericRepo<>));
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IFuelService,FuelService>();
builder.Services.AddScoped<IDutyLogService, DutyLogService>();
builder.Services.AddScoped<IOvertimeService, OvertimeService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();

builder.Services.AddScoped<IRosterService, RosterService>();
builder.Services.AddScoped<IGenericRepo<DutyRoster>, GenericRepo<DutyRoster>>();
// Add repositories to the container
builder.Services.AddScoped<IGenericRepo<DutyRosterEntry>, GenericRepo<DutyRosterEntry>>();
builder.Services.AddScoped<IRosterRepo, RosterRepo>();
builder.Services.AddScoped<IExpenseRepo, ExpenseRepo>();
builder.Services.AddScoped<IDutyRepo, DutyLogRepo>();
builder.Services.AddScoped<IVehicleRepo, VehicleRepo>();
builder.Services.AddScoped<IDriverRepo, DriverRepo>();
builder.Services.AddScoped<IMaintenanceRepo, MaintenanceRepo>();
builder.Services.AddScoped<IFuelRepo, FuelRepo>();
builder.Services.AddScoped<IOvertimeRepo, OvertimeRepo>();

// Add AutoMapper
builder.Services.AddAutoMapper(_mapper => _mapper.AddProfile<MapperProfile>()
);
//registering connection string
string connectionString = builder.Configuration
    .GetConnectionString("defaultConnection") ?? throw new
    InvalidOperationException("Connection string 'defaultConnection' not found.");
builder.Services.AddOpenApi();
builder.Services.AddControllers()
      .AddJsonOptions(options =>
      {
          options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
          options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
          options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); 
      });
builder.Services.AddDbContext<AppDbContext>
(options => options.UseNpgsql(connectionString));
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(e =>
    {
        e.SwaggerEndpoint("/swagger/v1/swagger.json", "Logestics ERP API V1");
        e.RoutePrefix = string.Empty; // Set Swagger UI at the app's root
    });
    app.MapOpenApi();
}
app.UseCors("AllowFrontend");
app.UseHttpsRedirection();
app.MapControllers();

app.Run();

