using APBD25_CW8.Models.DTOs;

namespace APBD25_CW8.Services;

public interface IClientsService
{
   Task<List<TripForClientDTO>>  GetTripsByIdAsync(int id, CancellationToken cancellationToken);
   public Task<int> CreateClient(ClientDTO clientDto, CancellationToken cancellationToken);
   public Task<int> AddTripToClient(int id,int tripId, CancellationToken cancellationToken);
   public Task<int> DeleteTripFormClient(int id,int tripId, CancellationToken cancellationToken);
    
}