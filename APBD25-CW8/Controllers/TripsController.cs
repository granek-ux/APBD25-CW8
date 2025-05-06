using APBD25_CW8.Models.DTOs;
using APBD25_CW8.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APBD25_CW8.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TripsController : ControllerBase
    {
        private readonly ITripsService _tripsService;
        
        public TripsController(ITripsService tripsService)
        {
        _tripsService = tripsService;
        }
        
        [HttpGet]
        public async Task<IActionResult> GetTrips(CancellationToken cancellationToken)
        {
            var trips = await _tripsService.GetTripsAsync(cancellationToken);
            return Ok(trips);
        }
        
    }
}
