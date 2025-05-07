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
        IClientsService _clientsService;

        public ClientsController(IClientsService clientsService)
        {
            _clientsService = clientsService;
        }

        [HttpGet("/{id}/trips")]
        public async Task<IActionResult> GetTrips(int id, CancellationToken cancellationToken)
        {
            var tripsById = await _clientsService.GetTripsByIdAsync(id, cancellationToken);
            if (tripsById.Count == 0)
                return NotFound();
            return Ok(tripsById);
        }

        [HttpPost]
        public async Task<IActionResult> AddTrip([FromBody] ClientDTO client, CancellationToken cancellationToken)
        {
            var id = await _clientsService.CreateClient(client, cancellationToken);
            if (id <= 0)
                return BadRequest();
            return Ok(id);
        }
        
        [HttpPut("/{id}/trips/{tripId}")]
        public async Task<IActionResult> UpdateTrip(int id, int tripId, CancellationToken cancellationToken)
        {
            var code = await _clientsService.AddTripToClient(id, tripId, cancellationToken);
            if (code < 0)
                return BadRequest();
            return Created();
        }

        [HttpDelete("/{id}/trips/{tripId}")]
        public async Task<IActionResult> DeleteTrip(int id, int tripId, CancellationToken cancellationToken)
        {
            var code = await _clientsService.DeleteTripFormClient(id, tripId, cancellationToken);
            if (code == 0)
                return NotFound();
            if(code <= -1)
                return BadRequest();
            
            return Ok();
        }
    }
}