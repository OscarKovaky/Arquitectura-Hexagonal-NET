using System.Text;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using OfficeOpenXml;
using Prueba.API.Middlewares;
using Prueba.Application.Commands;
using Prueba.Core.Interfaces;
using Prueba.Infrastructure.Persistence;
using Prueba.Infrastructure.Repositories;
using Prueba.Infrastructure.Services;
using AutoMapper;
using Prueba.API.Configurations;
using Prueba.Application.Mappings;
using Prueba.Core.Services;
using Prueba.Infrastructure.Logging;
using StackExchange.Redis;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Prueba API", Version = "v1" });
});

var mappingConfig = new MapperConfiguration(mc => { mc.AddProfile(new AutoMapperProfile()); });
builder.Services.AddSingleton(mappingConfig.CreateMapper());

//Redis 
// Retrieve Redis connection string from environment variable or appsettings.json
// Bind the Redis configuration section to the RedisConfig class
var redisConfig = builder.Configuration.GetSection("Redis").Get<RedisConfig>();

// Override the connection string with the environment variable if available
var redisConnectionString = Environment.GetEnvironmentVariable("REDIS_CONNECTION_STRING");

if (!string.IsNullOrWhiteSpace(redisConnectionString))
{
    redisConfig.ConnectionString = redisConnectionString;
}

if (string.IsNullOrWhiteSpace(redisConfig.ConnectionString))
{
    throw new InvalidOperationException("Redis connection string is not configured properly.");
}

// Register Redis connection
var redis = ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
builder.Services.AddSingleton<IConnectionMultiplexer>(redis);
        
// Register RedisCacheService
builder.Services.AddSingleton<ICacheService>(new RedisCacheService(redis, redisConfig.InstanceName));


// Add Dapper Repositories
builder.Services.AddScoped<ITreeNodeRepository, TreeNodeRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();

builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();

// Registrar CategoriaRepository como la implementación principal de ICategoriaRepository
builder.Services.AddScoped<CategoriaRepository>();
builder.Services.AddScoped<ICategoriaRepository>(provider =>
{
    var repository = provider.GetRequiredService<CategoriaRepository>();
    var cacheService = provider.GetRequiredService<ICacheService>();
    return new CachedCategoriaRepository(repository, cacheService);
});

// Add Services
builder.Services.AddTransient<ITreeService, TreeService>();
builder.Services.AddTransient<IUserService, UserService>();

// Configurar Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
builder.Logging.AddFile(@"C:\ExamenVue\Back\Prueba.API\Logs\log.txt");


// Add Connection db
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSingleton(new DatabaseContext(connectionString));


// Other service registrations
// Configurar el contexto de la licencia
ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

// Add MediatR
builder.Services.AddMediatR(typeof(CreateTreeNodeCommand).Assembly);

builder.Services.AddSingleton<IPasswordHasher, PasswordHasher>();
builder.Services.AddSingleton<IJwtTokenGenerator, JwtTokenGenerator>();

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
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(builder.Configuration["Jwt:Secret"])),
            ClockSkew = TimeSpan.Zero // Opcional, para reducir la tolerancia de tiempo
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

}
else
{
    app.UseHttpsRedirection();
}

app.UseCors("AllowAll");

app.UseRouting();
app.UseMiddleware<ApiKeyMiddleware>();
app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseMiddleware<RequestLoggingMiddleware>();


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


