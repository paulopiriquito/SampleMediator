using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Patterns.Mediator.Interfaces
{
    public interface IPublisher : IDisposable
    {
        void Publish(object message, string routingKey, IDictionary<string, object> messageAttributes);
        Task PublishAsync(object message, string routingKey, IDictionary<string, object> messageAttributes);
    }
}