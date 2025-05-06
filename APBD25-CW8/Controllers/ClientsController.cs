using APBD25_CW8.Models.DTOs;
using APBD25_CW8.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD25_CW8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        ITripsService _tripsService;
        IClientsService _clientsService;

        [HttpGet("/{id}/trips")]
        public async Task<IActionResult> GetTrips(int id, CancellationToken cancellationToken)
        {
            var tripsById = await _tripsService.GetTripsByIdAsync(id, cancellationToken);
            return Ok(tripsById);
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip([FromBody] ClientDTO client, CancellationToken cancellationToken)
        {
            var id = _clientsService.CreateClient(client, cancellationToken);
            return Ok(id);
        }


        // PUT /api/clients/{id}/trips/{tripId
        [HttpPut("/{id}/trips/{tripId}")]
        public async Task<IActionResult> UpdateTrip(int id, int tripId, [FromBody] ClientDTO client,
            CancellationToken cancellationToken)
        {
            return Created();
        }

        [HttpDelete("/{id}/trips/{tripId}")]
        public async Task<IActionResult> DeleteTrip(int id, int tripId, CancellationToken cancellationToken)
        {
            return NoContent();
        }
    }
}