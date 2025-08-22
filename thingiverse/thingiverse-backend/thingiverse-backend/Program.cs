using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using Thingiverse.Application.Abstractions.Interfaces;
using Thingiverse.Application.Contracts.Repository;
using Thingiverse.Application.Interfaces;
using Thingiverse.Application.Services;
using Thingiverse.Domain.Models;
using Thingiverse.Infrastructure.Persistence.Identity;
using Thingiverse.Infrastructure.Repositories;
using thingiverse_backend.Interfaces;
using thingiverse_backend.Services;
using Thingiverse.Integration.Services;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Text.Json;
using System.Threading.RateLimiting;
using Microsoft.AspNetCore.RateLimiting;
using Thingiverse.Application.Options;
using Thingiverse.Application.Contracts.Config;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

// --- DbContext (Identity için) ---
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));




// --- Identity ---
builder.Services.AddIdentity<AppUser, IdentityRole>(options =>
{
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 10;
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// --- Authentication (JWT) ---
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
        ValidIssuer = configuration["JWT:Issuer"],
        ValidateAudience = true,
        ValidAudiences = new[] { configuration["JWT:Audience"], "http://localhost:5173" },
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(configuration["JWT:SigningKey"])
        ),
        NameClaimType = ClaimTypes.Name,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.FromMinutes(5)
    };
});

// --- CORS ---
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:5173", "https://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithExposedHeaders("Content-Disposition"));
});

// --- HttpClient & Services ---
builder.Services.AddHttpClient("Thingiverse", client =>
{
    client.BaseAddress = new Uri("https://api.thingiverse.com/");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddScoped<ThingiverseService>(provider =>
{
    var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("Thingiverse");
    var repo = provider.GetRequiredService<IThingRepository>();
    var options = provider.GetRequiredService<IOptions<ThingiverseOptions>>();
    var apiOptions = provider.GetRequiredService<IOptions<ApiSettings>>();
    return new ThingiverseService(client, repo, options,apiOptions);
});

builder.Services.AddScoped<IDownloadService>(provider =>
{
    var client = provider.GetRequiredService<IHttpClientFactory>().CreateClient("Thingiverse");
    var options = provider.GetRequiredService<IOptions<ThingiverseOptions>>();
    return new DownloadService(client, options);
});

// --- Dapper için IDbConnection ---
builder.Services.AddScoped<IDbConnection>(sp =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    return new SqlConnection(connectionString);
});

// --- DI Servisler ---
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<ILikeRepository, LikeRepository>();
builder.Services.AddScoped<ILikeService, LikeService>();
builder.Services.AddScoped<IAccountRepository, AccountRepository>();
builder.Services.AddScoped<ICommentRepository, CommentRepository>();
builder.Services.AddScoped<IItemRepository, ItemRepository>();
builder.Services.AddScoped<IMakeRepository, MakeRepository>();
builder.Services.AddScoped<INewestRepository, NewestRepository>();
builder.Services.AddScoped<IPopularRepository, PopularRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IThingRepository, ThingRepository>();
builder.Services.AddHttpContextAccessor();

// --- Controllers & JSON Options ---
builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        opt.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
    });

builder.Services.AddControllers()
    .AddJsonOptions(opt =>
    {
        opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });


// --- Swagger ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(option =>
{
    option.SwaggerDoc("v1", new OpenApiInfo { Title = "Demo API", Version = "v1" });
    option.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "JWT Authorization header using the Bearer scheme. Example: 'Bearer {token}'",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT"
    });
    option.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});
builder.Services.AddRateLimiter(options =>  //rate limit dakikada 5 istek
{
    options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(httpContext =>
    {
        // IP adresini al
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";

        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 5,                
            Window = TimeSpan.FromMinutes(1), 
            QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
            QueueLimit = 2
        });
    });

    options.RejectionStatusCode = 429; // çok request
});
builder.Services.Configure<ThingiverseOptions>(
    builder.Configuration.GetSection("Thingiverse"));


var app = builder.Build();

// --- Middleware Pipeline ---
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowReactApp");

app.UseRouting();

// --- Static Files ---
// wwwroot (varsayýlan)
app.UseStaticFiles();

// Ekstra upload klasörü (eðer ileride lazým olursa)
var uploadPath = Path.Combine(Directory.GetCurrentDirectory(), "upload");
if (!Directory.Exists(uploadPath))
    Directory.CreateDirectory(uploadPath);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadPath),
    RequestPath = "/upload"
});
var uploadway = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "upload");
if (!Directory.Exists(uploadway))
    Directory.CreateDirectory(uploadway);

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(uploadway),
    RequestPath = "/upload"
});


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
