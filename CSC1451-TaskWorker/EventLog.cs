using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CSC1451_TaskWorker.Settings;
using CSC1451_TaskWorker.Time;
using PusherServer;

namespace CSC1451_TaskWorker
{
    public class EventLog
    {
        private readonly List<Domain.Task> _eventLog = new List<Domain.Task>();
        private readonly Pusher _pusher;

        public EventLog(PusherSettings settings)
        {
            _pusher = new Pusher(settings.AppId, settings.Key, settings.Secret, new PusherOptions() {Cluster = settings.Cluster });
        }

        public async Task AddEvent(Domain.Task ev)
        {

            if (_eventLog.Count == 0)
            {
                _eventLog.Add(ev);
            }
            else
            {
                var newEvTs = TimeSpan.Parse(ev.EndTime);

                for (var x = _eventLog.Count - 1; x > -1; x--)
                {
                    var ts = _eventLog[x].EndTime.ParseEndTime();
                    if (newEvTs.Hours >= ts.Hours && newEvTs.Minutes >= ts.Minutes)
                    {
                        _eventLog.Insert(x + 1, ev);
                    }

                    if (x == 0)
                    {
                        _eventLog.Insert(0, ev);
                    }
                }
            }
        }

        public async Task<bool> CheckTime()
        {

            var time = DateTime.Now.TimeOfDay;

            var evTime = TimeSpan.Parse(_eventLog[0].EndTime);

            if (time.Hours >= evTime.Hours && time.Minutes >= evTime.Minutes)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task TriggerEvent()
        {
            var res = await _pusher.TriggerAsync(_eventLog[0].Channel.ToString(), _eventLog[0].TaskName, null);
            _eventLog.RemoveAt(0);
        }

        public async Task ClearEvents()
        {
            _eventLog.Clear();
        }

    }
}
