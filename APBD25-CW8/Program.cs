using APBD25_CW8;
using APBD25_CW8.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITripsService, TripsService>();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

Console.Write(ReadFromFiele.FileRead("script.sql"));

// TripsService tripsService = new TripsService();
// Console.WriteLine(
// await tripsService.SetupBase(CancellationToken.None));

// var tripDtos = await tripsService.GetTrips(CancellationToken.None);

// foreach (var tripDto in tripDtos)
// {
    // Console.WriteLine(tripDto.Id);
// }