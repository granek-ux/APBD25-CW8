using APBD25_CW8;
using APBD25_CW8.Services;
using Swashbuckle.AspNetCore.SwaggerUI;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddScoped<ITripsService, TripsService>();
builder.Services.AddScoped<IClientsService, ClientsService>();
builder.Services.AddSwaggerGen();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "API v1");
        c.DocExpansion(DocExpansion.List);
        c.DefaultModelExpandDepth(0);
        c.DisplayRequestDuration();
        c.EnableFilter();
    });
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseAuthorization();

app.MapControllers();

app.Run();

// Console.Write(File.ReadAllText("script.sql"));

// TripsService tripsService = new TripsService();
// Console.WriteLine(
// await tripsService.SetupBase(CancellationToken.None));

// var tripDtos = await tripsService.GetTrips(CancellationToken.None);

// foreach (var tripDto in tripDtos)
// {
    // Console.WriteLine(tripDto.Id);
// }