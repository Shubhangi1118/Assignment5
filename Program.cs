using Assignment5.Models;
using Assignment5.Services;
var builder = WebApplication.CreateBuilder(args);
builder.Services.Configure<UserSettings>(
    builder.Configuration.GetSection("UserDatabase"));
builder.Services.AddSingleton<UserService>();

// Add services to the container.

builder.Services.AddControllers()
.AddJsonOptions(
        options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
