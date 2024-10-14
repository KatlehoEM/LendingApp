using System.Text;
using API.BlockchainStructure;
using API.CustomLoggers;
using API.Data;
using API.Interfaces;
using API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

 AllocConsole();
// Add services to the container.

builder.Services.AddControllers();

// Configure SQLite
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));


builder.Services.AddCors();
// Register services
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<ILoanOfferRepository, LoanOfferRepository>();
builder.Services.AddScoped<IRatingRepository, RatingRepository>();
builder.Services.AddScoped<ILoanRepository, LoanRepository>();
builder.Services.AddScoped<ILoanApplicationRepository, LoanApplicationRepository>();
builder.Services.AddScoped<IPaymentRepository, PaymentRepository>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddTransient<IBlockchainService, BlockchainService>();
builder.Services.AddScoped<IJwtService, JwtService>();


// Add custom file logger provider and configure filtering
builder.Logging.ClearProviders();
builder.Logging.AddProvider(new FileLoggerProvider("Logs/blockchain_node_logs.txt"));
 builder.Logging.AddConsole(); 
// // Set up logging to capture only logs from the Blockchain and Node classes
// builder.Logging.AddFilter((category, level) =>
// {
//     return category == "" || 
//     category == "API.BlockchainStructure.Node" ||
//     category == "API.BlockchainStructure.Network" ||
//     category == "API.BlockchainStructure.Block";
// });

builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // Suppress ASP.NET Core Information logs
builder.Logging.AddFilter("Microsoft", LogLevel.Warning); // Suppress ASP.NET Core Information logs
builder.Logging.AddFilter("Microsoft.AspNetCore", LogLevel.Warning); // Suppress ASP.NET Core Information logs
builder.Logging.AddFilter("API.BlockchainStructure.Blockchain", LogLevel.Information);
builder.Logging.AddFilter("API.BlockchainStructure.Node", LogLevel.Information);
builder.Logging.AddFilter("API.BlockchainStructure.Network", LogLevel.Information);
builder.Logging.AddFilter("API.BlockchainStructure.Block", LogLevel.Information);

builder.Services.AddTransient<Blockchain>();
builder.Services.AddTransient<Node>();
builder.Services.AddTransient<Network>();
builder.Services.AddTransient<Block>();


// Configure logging
//builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
// builder.Logging.AddConsole();
// builder.Logging.AddDebug();



// Configure JWT authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("RequireAdminRole", policy => policy.RequireRole("Admin"));
    options.AddPolicy("RequireLenderRole", policy => policy.RequireRole("Lender"));
    options.AddPolicy("RequireBorrowerRole", policy => policy.RequireRole("Borrower"));
    options.AddPolicy("RequireLenderOrBorrowerRole", policy => policy.RequireRole("Lender", "Borrower"));
});

var app = builder.Build();
app.UseCors(builder => builder
    .AllowAnyHeader()
    .AllowAnyMethod()
    .AllowCredentials()
    .WithOrigins("https://localhost:4200"));
app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Ensure the database is created and seeded
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<DataContext>();
        context.Database.Migrate(); // This will apply any pending migrations
        SeedData.SeedUsers(services); // This will seed the data
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}
app.Run();

 [System.Runtime.InteropServices.DllImport("kernel32.dll")]
 static extern bool AllocConsole();

