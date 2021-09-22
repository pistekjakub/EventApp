using System;

namespace EventApp.Data.Dtos
{
    public class EventDto
    {
        public long Id { get; set; } = (long)decimal.MinusOne;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public DateTime? StartTime { get; set; } = null;

        public DateTime? EndTime { get; set; } = null;
    }
}
