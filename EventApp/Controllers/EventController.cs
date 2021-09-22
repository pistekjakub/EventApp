using EventApp.Models.Requests;
using EventApp.Models.Responses;
using EventApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        public EventController(IEventService eventService)
        {
            _eventService = eventService;
        }

        //[EnableCors]
        [HttpGet]
        public async Task<GetEventsResponse> Get()
        {
            return await _eventService.GetEvents(new GetEventsRequest());
        }
    }
}
