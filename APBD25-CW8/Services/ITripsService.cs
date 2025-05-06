using APBD25_CW8.Models.DTOs;

namespace APBD25_CW8.Services;

public interface ITripsService
{
    Task<List<TripDTO>> GetTripsAsync(CancellationToken cancellationToken);
    Task<List<TripDTO>>  GetTripsByIdAsync(int id, CancellationToken cancellationToken);
}