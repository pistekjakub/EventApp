using EventApp.Data.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventApp.Data.Repositories
{
    public interface IEventRepository
    {
        public Task<List<EventDto>> GetEvents();

        public Task<bool> CheckUniqueEvent(string eventName);

        public Task<EventDto> InsertEvent(EventDto eventDto);
    }
}
