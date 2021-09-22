using EventApp.Models.Requests;
using EventApp.Models.Responses;
using EventApp.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;

namespace EventApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;

        private readonly ILogger<EventController> _logger;

        public EventController(IEventService eventService, ILogger<EventController> logger)
        {
            _eventService = eventService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<GetEventsResponse> Get()
        {
            return await _eventService.GetEvents(new GetEventsRequest());
        }

        [HttpPost]
        public async Task<ActionResult<InsertEventResponse>> Post([FromBody] InsertEventRequest request)
        {
            try
            {
                return await _eventService.InsertEvent(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.StackTrace);
                return new InsertEventResponse
                {
                    Error = ex.Message
                };
            }
        }
    }
}
