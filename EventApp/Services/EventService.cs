using EventApp.Data.Repositories;
using EventApp.Models.Requests;
using EventApp.Models.Responses;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public class EventService : IEventService
    {
        private readonly IEventRepository _eventRepository;

        public EventService(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        public async Task<GetEventsResponse> GetEvents(GetEventsRequest request)
        {
            var eventsDtos = await _eventRepository.GetEvents();
            var result = new GetEventsResponse
            {
                Events = EventApp.EntityMapper.EntityMapper.MapEventsDtos(eventsDtos)
            };
            return result;
        }
    }
}
