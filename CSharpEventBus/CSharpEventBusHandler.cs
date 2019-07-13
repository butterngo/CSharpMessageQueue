namespace CSharpEventBus
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using CSharpMessageQueueClient;
    using CSharpMessageQueueClient.Models;
    using Newtonsoft.Json;

    public class CSharpEventBusHandler : IEventBus, IDisposable
    {
        private readonly ICSharpClientConnectionFactory _connectionFactory;

        private readonly Dictionary<string, List<IIntegrationEventHandler>> _handlers
             = new Dictionary<string, List<IIntegrationEventHandler>>();

        private readonly List<Type> _eventTypes = new List<Type>();

        public CSharpEventBusHandler(ICSharpClientConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));

            _connectionFactory.OnHandleReceivedMessage += HandleReceivedMessage;

            _connectionFactory.OnHandleCompletedMessageReceived += HandleCompletedMessageReceived;
        }

        public async Task PublishAsync(IntegrationEvent @event, string[] tos)
        {
            var message = JsonConvert.SerializeObject(@event);

            var body = Encoding.UTF8.GetBytes(message);

            await _connectionFactory.SendAsync(new CSharpMessage
            {
                Body = body,
                Tos = tos,
                Label = @event.GetType().Name
            });

        }

        public async Task PublishAsync(IntegrationEvent @event) 
            => await InvokeAsync(@event.GetType().Name, @event);

        public void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            var eventName = typeof(T).Name;

            if (_handlers.ContainsKey(eventName))
            {
                _handlers[eventName].Add(handler);
            }
            else
            {
                _handlers.Add(eventName, new List<IIntegrationEventHandler>());

                _handlers[eventName].Add(handler);

                _eventTypes.Add(typeof(T));

            }
        }

        public void Unsubscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent
        {
            var eventName = typeof(T).Name;

            if (!_handlers.ContainsKey(eventName) || !_handlers[eventName].Contains(handler))
            {
                return;
            }

            _handlers[eventName].Remove(handler);

            if (_handlers[eventName].Count != 0)
            {
                return;
            }

            _handlers.Remove(eventName);

            var eventType = _eventTypes.SingleOrDefault(e => e.Name == eventName);

            if (eventType != null)
            {
                _eventTypes.Remove(eventType);
            }
        }

        private async void HandleReceivedMessage(CSharpMessage message)
        {
            try
            {
                var eventName = message.Label;

                var body = Encoding.UTF8.GetString(message.Body);

                if (_handlers.ContainsKey(eventName))
                {
                    await InvokeAsync(eventName, body);

                    await _connectionFactory.ProcessCompletedMessageAsync(message);
                }
                else
                {
                    throw new Exception($"Not found event {eventName}");
                }

            }
            catch (Exception ex)
            {
               //TODO Some logic
            }

        }

        private void HandleCompletedMessageReceived(CompletedMessageReceived args)
        {
            //TODO
        }

        private async Task InvokeAsync(string eventName, object obj)
        {
            Type eventType = _eventTypes.Single(t => t.Name == eventName);

            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);

            var handlers = _handlers[eventName];

            foreach (var handler in handlers)
            {
                await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { obj });
            }

        }

        public void Dispose()
        {
            //TODO
        }
    }
}
