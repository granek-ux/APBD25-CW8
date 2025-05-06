using Microsoft.Data.SqlClient;
using APBD25_CW8.Models.DTOs;

namespace APBD25_CW8.Services;

public class TripsService : ITripsService
{
    // private readonly string _connectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=APBD;Integrated Security=True;";
    private readonly string _connectionString =
        "Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Initial Catalog=apbd; Integrated Security=False;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False";

    private readonly string _connectionStringAdmin =
        "Data Source=localhost,1433;User=SA;Password=yourStrong(!)Password;Initial Catalog=master;Encrypt=False;TrustServerCertificate=True";

    private readonly string sqlFileName = "script.sql";

    public async Task<List<TripDTO>> GetTripsAsync(CancellationToken cancellationToken)
    {
        var trips = new List<TripDTO>();

        string command = "SELECT IdTrip, Name FROM Trip";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync(cancellationToken);

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(1),
                    });
                }
            }
        }

        Console.WriteLine(trips.Count);
        return trips;
    }

    public async Task<List<TripDTO>> GetTripsByIdAsync(int id, CancellationToken cancellationToken)
    {
        var trips = new List<TripDTO>();

        string command = "SELECT IdTrip, Name FROM Trip where Id=@id";

        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
            // anty sql incjet
            await conn.OpenAsync(cancellationToken);

            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                while (await reader.ReadAsync())
                {
                    int idOrdinal = reader.GetOrdinal("IdTrip");
                    trips.Add(new TripDTO()
                    {
                        Id = reader.GetInt32(idOrdinal),
                        Name = reader.GetString(1),
                    });
                }
            }
        }

        foreach (var trip in trips)
            Console.WriteLine($"Id: {trip.Id}, Name: {trip.Name}");
        return trips;
    }

    public async Task<int> SetupBase(CancellationToken cancellationToken)
    {
        var command = await File.ReadAllTextAsync(sqlFileName, cancellationToken);
        using (SqlConnection conn = new SqlConnection(_connectionStringAdmin))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            await conn.OpenAsync(cancellationToken);
            var row = await cmd.ExecuteNonQueryAsync(cancellationToken);
            return row;
        }
    }
}