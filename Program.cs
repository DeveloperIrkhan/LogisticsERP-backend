using LogisticsERP.API.Data;
using LogisticsERP.API.Helpers;
using LogisticsERP.API.interfaces;
using LogisticsERP.API.Models;
using LogisticsERP.API.Repositories;
using LogisticsERP.API.Services;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using System.Text.Json;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;



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
// ── JWT Authentication ──────────────────────────────────────
var jwtSecret = builder.Configuration["JwtSettings:Secret"]
    ?? throw new InvalidOperationException("JwtSettings:Secret is not configured.");
var jwtIssuer = builder.Configuration["JwtSettings:Issuer"] ?? "LogisticsERP.API";
var jwtAudience = builder.Configuration["JwtSettings:Audience"] ?? "LogisticsERP.Client";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidIssuer = jwtIssuer,
        ValidateAudience = true,
        ValidAudience = jwtAudience,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(1),
    };
});
builder.Services.AddAuthorization();


// Add services to the container.
builder.Services.AddScoped(typeof(IGenericRepo<>), typeof
    (GenericRepo<>));
builder.Services.AddScoped<IVehicleService, VehicleService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IDriverService, DriverService>();
builder.Services.AddScoped<ICloudinaryService, CloudinaryService>();
builder.Services.AddScoped<IDocumentService, DocumentService>();
builder.Services.AddScoped<IFuelService, FuelService>();
builder.Services.AddScoped<IDutyLogService, DutyLogService>();
builder.Services.AddScoped<IOvertimeService, OvertimeService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportService, ReportService>();
builder.Services.AddScoped<IItemService, ItemService>();
builder.Services.AddScoped<IItemPurchaseService, ItemPurchaseService>();
builder.Services.AddScoped<ItemSaleService, ItemSaleService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
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
builder.Services.AddScoped<IItemRepo, ItemRepo>();
builder.Services.AddScoped<IItemPurchaseRepo, ItemPurchaseRepo>();
builder.Services.AddScoped<IItemSaleRepo, ItemSaleRepo>();

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
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
// Apply any pending EF migrations automatically, then seed default roles + a
// default Admin account so there's someone who can log in and approve the
// first sign-ups. Change the DefaultAdmin password after first login.
// Wrapped in try/catch + logging so a DB problem is visible in the console
// instead of silently crashing the app before it starts listening.

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    try
    {
        var pendingMigrations = (await db.Database.GetPendingMigrationsAsync()).ToList();
        if (pendingMigrations.Count > 0)
        {
            logger.LogInformation("Applying {Count} pending migration(s): {Migrations}",
                pendingMigrations.Count, string.Join(", ", pendingMigrations));
            await db.Database.MigrateAsync();
        }

        await DataSeeder.SeedAsync(db, app.Configuration, logger);
        logger.LogInformation("Startup seeding completed (roles + default admin check).");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Startup migration/seeding failed. The API will still start, " +
            "but auth-related tables/data may be missing until this is fixed.");
    }

}
app.Run();

