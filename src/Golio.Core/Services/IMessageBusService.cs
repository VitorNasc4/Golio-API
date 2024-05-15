using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Golio.Core.Services
{
    public interface IMessageBusService
    {
        Task SendMessageQueueAsync<T>(T message);
    }
}