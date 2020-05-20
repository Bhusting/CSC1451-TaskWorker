using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace CSC1451_TaskWorker.Handlers
{
    public interface IMessageHandler
    {
        Task Handle(string messageBody);
    }
}
