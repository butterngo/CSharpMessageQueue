namespace CSharpEventBus
{
    using System;

    public class IntegrationEvent
    {
        public IntegrationEvent()
        {
            Id = Guid.NewGuid();
            SentDate = DateTime.UtcNow;
        }

        public Guid Id { get; }
        public DateTime SentDate { get; }
    }
}
