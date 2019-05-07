namespace Client2
{
    using Client2.Events;
    using CSharpEventBus;
    using System.Threading.Tasks;

    public class CSharpEventHandler : IIntegrationEventHandler<UserEvent>
    {
        public Task Handle(UserEvent @event)
        {
            //TODO Some logic
            return Task.FromResult(0);
        }
    }
}
