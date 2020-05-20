using System;

namespace Domain
{
    public class Task
    {
        public Guid EventId { get; set; }

        public string EventName { get; set; }

        public string EventTime { get; set; }

        public Guid Channel { get; set; }
    }
}
