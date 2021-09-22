using EventApp.Data.Dtos;
using EventApp.Data.Repositories;
using EventApp.Models.Requests;
using EventApp.Models.Responses;
using System;
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
                Events = EntityMapper.MapEventsDtos(eventsDtos)
            };
            return result;
        }

        public async Task<InsertEventResponse> InsertEvent(InsertEventRequest request)
        {
            bool isUnique = await _eventRepository.CheckUniqueEvent(request.Name);
            if (!isUnique)
            {
                throw new ArgumentException($"Event name ${request.Name} is not unique");
            }

            var eventDto = new EventDto
            {
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                StartTime = request.StartTime,
                EndTime = request.EndTime,
            };

            var insertedDto = await _eventRepository.InsertEvent(eventDto);

            return new InsertEventResponse
            {
                Inserted = (insertedDto.Id != decimal.MinusOne)
            };
        }
    }
}
