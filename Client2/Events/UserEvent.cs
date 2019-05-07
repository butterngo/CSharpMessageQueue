namespace Client2.Events
{
    using CSharpEventBus;

    public class UserEvent: IntegrationEvent
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}
