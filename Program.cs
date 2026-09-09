

using VoteLinhTinh.Models;
using VoteLinhTinh.Repositories;
using VoteLinhTinh.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddScoped<ISongRepository, SongRepository>();
builder.Services.AddScoped<IMatchRepository, MatchRepository>();
builder.Services.AddScoped<IRoundRepository, RoundRepository>();
builder.Services.AddHttpClient<ISongService, SongService>();
builder.Services.AddScoped<IRoundService, RoundService>();
builder.Services.AddScoped<IMatchService, MatchService>();



builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy
            .WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var initializer = new DatabaseInitializer(configuration);
    await initializer.InitializeAsync();
}

//app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthorization();

app.MapControllers();

app.Run();
