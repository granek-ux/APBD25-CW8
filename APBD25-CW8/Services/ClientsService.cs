using APBD25_CW8.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD25_CW8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public async Task<int> CreateClient(ClientDTO clientDto, CancellationToken cancellationToken)
    {
        string command =
            @"insert into Client (FirstName, LastName, Email, Telephone, Pesel) values (@FirstName, @LastName, @Email, @Telephone, @Pesel); SELECT CAST(SCOPE_IDENTITY() AS int);";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        using (SqlCommand cmd = new SqlCommand(command, conn))
        {
            cmd.Parameters.AddWithValue("@FirstName", clientDto.FirstName);
            cmd.Parameters.AddWithValue("@LastName", clientDto.LastName);
            cmd.Parameters.AddWithValue("@Email", clientDto.Email);
            cmd.Parameters.AddWithValue("@Telephone", clientDto.Telephone);
            cmd.Parameters.AddWithValue("@Pesel", clientDto.Pesel);
            await conn.OpenAsync(cancellationToken);
            using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
            {
                int id = -1;
                while (await reader.ReadAsync())
                {
                    id = reader.GetInt32(0);
                }

                return id;
            }
        }
    }

    public async Task<int> AddTripToClient(int id, int tripId, CancellationToken cancellationToken)
    {
        string checkClientCommand = @"SELECT 1 FROM Client c WHERE c.IdClient = @id ";
        string checkTripCommand = "SELECT MaxPeople FROM Trip WHERE IdTrip = @TripId";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);
            using (SqlCommand cmd = new SqlCommand(checkClientCommand, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tripId", tripId);
                
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    int tmpid = -1;
                    while (await reader.ReadAsync())
                    {
                        tmpid = reader.GetInt32(0);
                    }

                    if (tmpid == -1)
                        return tmpid;
                }
            }

            var maxpeople = -1;
            using (SqlCommand cmd = new SqlCommand(checkTripCommand, conn))
            {
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tripId", tripId);
              
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync())
                    {
                        maxpeople = reader.GetInt32(0);
                    }

                    if (maxpeople == -1)
                        return maxpeople;
                }
                
                
            }
        }

        return 200;
        throw new NotImplementedException();
    }

    public Task<TripDTO> DeleteTripFormClient(int id, int tripId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}