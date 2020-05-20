using System;

namespace Domain
{
    public class Task
    {
        public Guid TaskId { get; set; }

        public string TaskName { get; set; }

        public string EndTime { get; set; }

        public Guid Channel { get; set; }
    }
}
