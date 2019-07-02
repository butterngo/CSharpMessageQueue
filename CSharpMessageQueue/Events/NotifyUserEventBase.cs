using CSharpMessageQueue.Models;

namespace CSharpMessageQueue.Events
{
    public abstract class NotifyUserEventBase
    {
        public NotifyUserEventBase() { }

        public NotifyUserEventBase(string connectionId, string uniqueKey)
        {
            ConnectionId = connectionId;
            UniqueKey = uniqueKey;
        }

        public NotifyUserEventBase(string connectionId, string uniqueKey, CSharpProfile profile)
        {
            ConnectionId = connectionId;
            UniqueKey = uniqueKey;
            Profile = profile;
        }

        public string UniqueKey { get; set; }

        public string ConnectionId { get; set; }

        public CSharpProfile Profile { get; set; }
    }
}
