using Microsoft.EntityFrameworkCore;
using SquareApi.Business;
using SquareApi.Business.Contract;
using SquareApi.Data;
using SquareApi.Data.Contracts;
using SquareApi.Middleware;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<SquareApiDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<ISquareService, SquareService>();
builder.Services.AddScoped<IPointService, PointService>();

builder.Services.AddScoped<IPointRepository, PointRepository>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// migrate any database changes on startup (includes initial db creation)
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<SquareApiDbContext>();
    dbContext.Database.Migrate();
}

app.MapControllers();

// Global exception handling middleware
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.Run();
