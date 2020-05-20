using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace CSC1451_TaskWorker.Handlers
{
    public class ClearEventsHandler : IMessageHandler
    {
        private readonly ILogger<ClearEventsHandler> _logger;
        private readonly EventLog _eventLog;

        public ClearEventsHandler(ILogger<ClearEventsHandler> logger, EventLog eventLog)
        {
            _logger = logger;
            _eventLog = eventLog;
        }

        public async Task Handle(string messageBody)
        {
            _logger.LogInformation("Clearing Events");

            await _eventLog.ClearEvents();
        }
    }
}
