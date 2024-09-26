using Application.WebApi;
using PersistenceLayer.DataAccess;
using Quartz;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerConfiguration();

builder.Services.AddInfrastructure(
    builder.Configuration.GetConnectionString("DefaultConnection"));

builder.Services.AddRepositories();

builder.Services.AddLogicServices(builder.Configuration);

builder.Services.AddAuthenticationConfiguration(builder.Configuration);

builder.Services.AddQuartz();
builder.Services.AddQuartzHostedService();

var app = builder.Build();

await app.AddJobs();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();

    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
