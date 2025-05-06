using APBD25_CW8.Models.DTOs;

namespace APBD25_CW8.Services;

public interface IClientsService
{
   public Task<int> CreateClient(ClientDTO clientDto, CancellationToken cancellationToken);
   
   public Task<int> AddTripToClient(int id,int tripId, CancellationToken cancellationToken);
   public Task<TripDTO> DeleteTripFormClient(int id,int tripId, CancellationToken cancellationToken);

    
}