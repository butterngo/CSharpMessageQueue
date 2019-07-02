using CSharpMessageQueue.Models;

namespace CSharpMessageQueue.Events
{
    public class NotifyUserConnectEvent : NotifyUserEventBase
    {
        public NotifyUserConnectEvent()
        {
        }

        public NotifyUserConnectEvent(string connectionId, string uniqueKey) : base(connectionId, uniqueKey)
        {
        }

        public NotifyUserConnectEvent(string connectionId, string uniqueKey, CSharpProfile profile) : base(connectionId, uniqueKey, profile)
        {
        }
    }
}
