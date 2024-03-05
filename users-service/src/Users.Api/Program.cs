using Users.Api.Configurations;
using Users.Application.Extensions;
using Users.Infrastructure.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices();
builder.Services.AddUsersDbContext(builder.Configuration.GetConnectionString("UsersDb"));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
