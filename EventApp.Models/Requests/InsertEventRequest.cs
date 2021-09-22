using System;

namespace EventApp.Models.Requests
{
    public class InsertEventRequest
    {
        public string Login { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime? StartTime { get; set; } = null;

        public DateTime? EndTime { get; set; } = null;
    }
}
