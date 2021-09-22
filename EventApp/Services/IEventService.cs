using EventApp.Models.Requests;
using EventApp.Models.Responses;
using System.Threading.Tasks;

namespace EventApp.Services
{
    public interface IEventService
    {
        public Task<GetEventsResponse> GetEvents(GetEventsRequest request);
    }
}
