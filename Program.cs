using LogisticsERP.API.Data;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Repositories;
using LogisticsERP.API.Services;
using Microsoft.EntityFrameworkCore;




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
    options.AddPolicy("AllowAll",
        policy =>
        {
            policy.AllowAnyOrigin()
                  .AllowAnyMethod()
                  .AllowAnyHeader();
        });
});


// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof
    (GenericRepo<>));
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IDriverService, DriverService>();

// Add AutoMapper
builder.Services.AddAutoMapper(_mapper => _mapper.AddProfile<MapperProfile>()
);
//registering connection string
string connectionString = builder.Configuration
    .GetConnectionString("defaultConnection") ?? throw new
    InvalidOperationException("Connection string 'defaultConnection' not found.");
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<AppDbContext>
(options => options.UseNpgsql(connectionString));


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

app.UseHttpsRedirection();
app.UseCors("AllowAll");
app.MapControllers();

app.Run();

