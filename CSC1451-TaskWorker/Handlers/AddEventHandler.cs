using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace CSC1451_TaskWorker.Handlers
{
    public class AddEventHandler : IMessageHandler
    {
        private readonly ILogger<AddEventHandler> _logger;
        private readonly EventLog _eventLog;

        public AddEventHandler(ILogger<AddEventHandler> logger, EventLog eventLog)
        {
            _logger = logger;
            _eventLog = eventLog;
        }

        public async System.Threading.Tasks.Task Handle(string messageBody)
        {
            var ev = JsonConvert.DeserializeObject<Domain.Task>(messageBody);

            _logger.LogInformation($"Adding Task {ev.EventName}");

            await _eventLog.AddEvent(ev);
        }
    }
}
