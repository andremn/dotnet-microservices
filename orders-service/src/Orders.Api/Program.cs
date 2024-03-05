using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Orders.Api.Extensions;
using Orders.Api.Services;
using Orders.Application.Extensions;
using Orders.Application.Messaging.Configurations;
using Orders.Extensions;
using Orders.Infrastructure.Extensions;
using System.Reflection;
using System.Text;

var builder = WebApplication.CreateBuilder(args);
var jtwIssuer = builder.Configuration.GetSection("Jwt:Issuer").Value ?? string.Empty;
var jtwKey = builder.Configuration.GetSection("Jwt:Key").Value ?? string.Empty;

builder.Services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMq"));

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jtwIssuer,
            ValidAudience = jtwIssuer,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jtwKey))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.DefaultPolicy = new AuthorizationPolicyBuilder(JwtBearerDefaults.AuthenticationScheme)
        .RequireAuthenticatedUser()
        .Build();
});

builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
    {
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Name = "Authorization",
        Description = "Bearer Authentication with JWT Token",
        Type = SecuritySchemeType.Http
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Id = "bearer",
                    Type = ReferenceType.SecurityScheme
                }
            },
            new List<string>()
        }
    });

    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddApiServices();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddDbContext(builder.Configuration.GetConnectionString("OrdersDb"));
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddProductsServiceRefitClient(options =>
{
    var productsServiceAddress = builder.Configuration.GetSection("ProductsService:BaseAddress").Value
        ?? throw new InvalidOperationException("Invalid address for the Products service");

    options
        .ConfigureHttpClient(c => c.BaseAddress = new Uri(productsServiceAddress))
        .AddHttpMessageHandler<AuthorizationHeaderHandler>();
});

var app = builder.Build();

app.UseExecuteMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseLoggedUserProvider();

app.UseRabbitMqConsumers();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
