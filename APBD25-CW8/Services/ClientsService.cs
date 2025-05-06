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

    public Task<TripDTO> AddTripToClient(int id, int tripId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<TripDTO> DeleteTripFormClient(int id, int tripId, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}