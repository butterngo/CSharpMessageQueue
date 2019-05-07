namespace CSharpEventBus
{
    using System.Threading.Tasks;

    public interface IEventBus
    {
        void Subscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;

        void Unsubscribe<T>(IIntegrationEventHandler<T> handler) where T : IntegrationEvent;

        Task PublishAsync(IntegrationEvent @event, string[] tos);

    }
}
