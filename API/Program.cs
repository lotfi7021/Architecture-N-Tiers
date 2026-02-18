using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Projet.BLL;
using Projet.BLL.Contracts;
using Projet.Context;
using Projet.DAL;
using Projet.DAL.Contracts;
using Projet.DAL.Repos;
using Projet.Entities;
using Projet.Enums;
using Projet.Services;
using Projet.Services.Interfaces;
using System.Text;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// =====================
// Database
// =====================
var cnx = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<DataContext>(options =>
    options.UseSqlServer(cnx, b => b.MigrationsAssembly("API")));

// =====================
// Controllers
// =====================
builder.Services.AddControllers();

// =====================
// Unit of Work & Repositories
// =====================
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IRepository<User>, UserRepository>();
builder.Services.AddScoped<IRepository<Client>, ClientRepository>();

// =====================
// Services
// =====================
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<IOrderService, OrderService>();

// =====================
// BLL
// =====================
builder.Services.AddScoped<IOrderBLL, OrderBLL>();

// =====================
// Swagger
// =====================
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Projet DOTNET",
        Version = "v1"
    });
});


var jwtSettings = builder.Configuration.GetSection("Jwt");
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);

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
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(key)
    };
});

builder.Services.Configure<ApiBehaviorOptions>(options =>
{
    options.InvalidModelStateResponseFactory = context =>
    {
        var errors = context.ModelState
            .Where(e => e.Value!.Errors.Count > 0)
            .Select(e => new
            {
                Field = e.Key,
                Errors = e.Value!.Errors.Select(x => x.ErrorMessage)
            });

        return new BadRequestObjectResult(new
        {
            Message = "Validation failed",
            Errors = errors
        });
    };
});


var app = builder.Build();

// =============================================
//          SUPER ADMIN SEEDING
// =============================================
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    // Apply any pending migrations (good practice in startup)
    db.Database.Migrate();

    string superAdminEmail = "superadmin@yourcompany.com";   
    string superAdminUsername = "superadmin";                

    // Check if super admin already exists
    var exists = db.Users
        .Any(u => u.Email.ToLower() == superAdminEmail.ToLower());

    if (!exists)
    {
        var superAdmin = new User
        {
            Username = superAdminUsername,
            Email = superAdminEmail,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("K7$mP!v9qL2#xR8nT5@wZ"), 
            Role = UserRole.Admin,      
            CreatedAt = DateTime.UtcNow
        };

        db.Users.Add(superAdmin);
        db.SaveChanges();

        Console.WriteLine("=====================================");
        Console.WriteLine("  SUPER ADMIN CREATED SUCCESSFULLY   ");
        Console.WriteLine($"  Email    : {superAdminEmail}");
        Console.WriteLine($"  Username : {superAdminUsername}");
        Console.WriteLine("=====================================");
    }
    else
    {
        Console.WriteLine("Super admin already exists.");
    }
}

// =====================
// Middleware Pipeline
// =====================
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "Projet DOTNET v1");
});

app.UseAuthentication();
app.UseAuthorization();

app.UseAuthorization();

app.MapControllers();

app.Run();
