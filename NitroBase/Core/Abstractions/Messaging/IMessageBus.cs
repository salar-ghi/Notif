using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Abstractions.Messaging
{
    public interface IMessageBus
    {
        Task SubscribeAsync(string channel, Action<string, string> handler);
        Task PublishAsync(string channel, string message);
    }
}
