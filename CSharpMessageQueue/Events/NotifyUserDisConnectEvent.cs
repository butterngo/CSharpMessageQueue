using CSharpMessageQueue.Models;

namespace CSharpMessageQueue.Events
{
    public class NotifyUserDisConnectEvent : NotifyUserEventBase
    {
        public NotifyUserDisConnectEvent()
        {
        }

        public NotifyUserDisConnectEvent(string connectionId, string uniqueKey) : base(connectionId, uniqueKey)
        {
        }

        public NotifyUserDisConnectEvent(string connectionId, string uniqueKey, CSharpProfile profile) : base(connectionId, uniqueKey, profile)
        {
        }
    }
}
