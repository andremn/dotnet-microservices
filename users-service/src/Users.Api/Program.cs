using System.Reflection;
using Users.Api.Configurations;
using Users.Application.Extensions;
using Users.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSwaggerGen(options =>
{
    var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";

    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
});

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddUsersDbContext(builder.Configuration.GetConnectionString("UsersDb"));
builder.Services.AddEndpointsApiExplorer();

builder.Services.Configure<JwtConfiguration>(builder.Configuration.GetSection("Jwt"));

var app = builder.Build();

app.UseExecuteMigrations();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
