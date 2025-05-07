using Microsoft.Data.SqlClient;
using APBD25_CW8.Models.DTOs;

namespace APBD25_CW8.Services;

public class TripsService : ITripsService
{
    private readonly string _connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public async Task<List<TripDTO>> GetTripsAsync(CancellationToken cancellationToken)
    {
        var trips = new List<TripDTO>();

        string command = "Select t.IdTrip, t.Name AS TripName, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, C.Name As CountrName from Trip t join dbo.Country_Trip CT on t.IdTrip = CT.IdTrip join dbo.Country C on C.IdCountry = CT.IdCountry";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync(cancellationToken);

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync())
                {
                    int id = (int)reader["IdTrip"];
                    if (trips.All(t => t.Id != id))
                    {
                        trips.Add(new TripDTO()
                        {
                            Id = id,
                            Name = (string)reader["TripName"],
                            Description = reader["Description"] as string,
                            DateFrom = (DateTime)reader["DateFrom"],
                            DateTo = (DateTime)reader["DateTo"],
                            MaxPeople = (int)reader["MaxPeople"],
                            Countries = []
                        });
                    }
                    
                    trips.Find(t => t.Id == id)
                        ?.Countries.Add(new CountryDTO()
                        {
                            Name = (string)reader["CountrName"]
                        });
                }
            }
        }
        return trips;
    }

    
}