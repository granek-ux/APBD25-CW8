using APBD25_CW8.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD25_CW8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString =
        "Data Source=localhost, 1433; User=SA; Password=yourStrong(!)Password; Initial Catalog=apbd; Integrated Security=False;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False";

    public async Task<List<TripDTO>> GetTripsById(int id, CancellationToken cancellationToken)
    {
        var trips = new List<TripDTO>();

        string command = "SELECT t.Name AS TripName,t.DateFrom,    t.DateTo,   ct.RegisteredAt,    ct.PaymentDateFROM Client_Trip ctJOIN Trip t ON ct.IdTrip = t.IdTripWHERE ct.IdClient = @id;";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@id", id);
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
        

        throw new NotImplementedException();
    }

    public Task<int> CreateClient(ClientDTO clientDto, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TripDTO> AddTripToClient(int id, int tripId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TripDTO> DeleteTripFormClient(int id, int tripId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}