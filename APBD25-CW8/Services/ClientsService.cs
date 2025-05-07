using APBD25_CW8.Models.DTOs;
using Microsoft.Data.SqlClient;

namespace APBD25_CW8.Services;

public class ClientsService : IClientsService
{
    private readonly string _connectionString =
        "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=apbd;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

    public async Task<List<TripForClientDTO>> GetTripsByIdAsync(int id, CancellationToken cancellationToken)
    {
        var trips = new List<TripForClientDTO>();

        string command = @"Select t.IdTrip, t.Name AS TripName, t.Description, t.DateFrom, t.DateTo, t.MaxPeople, C.Name AS CountryName, CT2.PaymentDate, CT2.RegisteredAt from Trip t 
                            join dbo.Country_Trip CT on t.IdTrip = CT.IdTrip 
                            join dbo.Country C on C.IdCountry = CT.IdCountry 
                            join dbo.Client_Trip CT2 on CT.IdTrip = CT2.IdTrip 
                            join dbo.Client C2 on C2.IdClient = CT2.IdClient where C2.IdClient=@id";

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
                    int sqlid = (int)reader["IdTrip"];
                    if (trips.All(t => t.TripDto.Id != sqlid))
                    {
                        trips.Add(new TripForClientDTO()
                        {
                            TripDto = new TripDTO()
                            {
                                Id = sqlid,
                                Name = (string)reader["TripName"],
                                Description = reader["Description"] as string,
                                DateFrom = (DateTime)reader["DateFrom"],
                                DateTo = (DateTime)reader["DateTo"],
                                MaxPeople = (int)reader["MaxPeople"],
                                Countries = []  
                            },
                                // PaymentDate = (int)reader["PaymentDate"],
                                PaymentDate = reader["PaymentDate"] == DBNull.Value ? (int?)null : Convert.ToInt32(reader["PaymentDate"]),
                                RegisteredAt = (int)reader["RegisteredAt"],
                        });
                    }
                    
                    trips.Find(t => t.TripDto.Id == sqlid)
                        ?.TripDto.Countries.Add(new CountryDTO()
                        {
                            Name = (string)reader["CountryName"]
                        });
                }
            }
        }
        return trips;
    }
    
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
        string checkPeopleCommand = "SELECT COUNT(*) FROM Client join dbo.Client_Trip CT on Client.IdClient = CT.IdClient where Ct.IdTrip = @TripId";
        string insertCommand = "insert into Client_Trip (IdClient, IdTrip, RegisteredAt, PaymentDate) values (@id,@tripId,@registeredAt,@PaymentDate)";
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
            
            using (SqlCommand cmd = new SqlCommand(checkPeopleCommand,conn))
            {
                cmd.Parameters.AddWithValue("@tripId", tripId);
                int people = -1;
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync())
                    {
                        people = reader.GetInt32(0);
                    }
                }

                if (people == -1 || people > maxpeople)
                    return -1;
            }

            using (SqlCommand cmd = new SqlCommand(insertCommand, conn))
            {
                // DateTime now = DateTime.Now;
                var now = 07052025;
                cmd.Parameters.AddWithValue("@id", id);
                cmd.Parameters.AddWithValue("@tripId", tripId);
                cmd.Parameters.AddWithValue("@registeredAt", now);
                cmd.Parameters.AddWithValue("@PaymentDate", DBNull.Value);

                await cmd.ExecuteNonQueryAsync(cancellationToken);
            }
        }
        return 1;
    }

    public async Task<int> DeleteTripFormClient(int id, int tripId, CancellationToken cancellationToken)
    {
        string checkCommand = @"SELECT COUNT(*) FROM Client_Trip where IdTrip =@tripID and IdClient =@id";
        string deleteCommand = "DELETE FROM Client_Trip WHERE IdTrip = @tripID and IdClient = @id";
        using (SqlConnection conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync(cancellationToken);
            using (SqlCommand cmd = new SqlCommand(checkCommand, conn))
            {
                cmd.Parameters.AddWithValue("@tripId", tripId);
                cmd.Parameters.AddWithValue("@id", id);
                int people = -1;
                using (var reader = await cmd.ExecuteReaderAsync(cancellationToken))
                {
                    while (await reader.ReadAsync())
                    {
                        people = reader.GetInt32(0);
                    }
                }
                if (people<= 0)
                    return people;
            }

            using (SqlCommand cmd = new SqlCommand(deleteCommand, conn))
            {
                cmd.Parameters.AddWithValue("@tripId", tripId);
                cmd.Parameters.AddWithValue("@id", id);

                var del = await cmd.ExecuteNonQueryAsync(cancellationToken);
                
                return del;
            }
        }
    }
}