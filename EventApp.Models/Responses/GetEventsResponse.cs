using EventApp.Models.Models;
using System.Collections.Generic;

namespace EventApp.Models.Responses
{
    public class GetEventsResponse
    {
        public List<Event> Events { get; set; }
    }
}
