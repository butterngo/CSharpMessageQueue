namespace CSharpMessageQueue
{
    using CSharpMessageQueue.Models;
    using log4net;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class JsCSharpMessageHub : CSharpMessageHubBase<JsCSharpMessage>
    {
        protected override ILog Logger => LogManager.GetLogger(typeof(JsCSharpMessageHub));

        protected override string UniqueKey => context.Request.Query["uniqueKey"].ToString();

        protected override string Name
        {
            get
            {
                if(context.Request.Query.ContainsKey("name"))
                    return context.Request.Query["name"].ToString();

                return string.Empty;
            }
        }

        public JsCSharpMessageHub(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task SendAsync(JsCSharpMessage message)
        {
            var logMsg = $"messageId: {message.Id.ToString()} To: {string.Join(",", message.Tos)} Content: {message.Body}";

            WriteLog($"Server received message: {logMsg}");

            if (_client.TryGetValue(UniqueKey, out CSharpProfile from)) message.FromProfile = from;

            if (message.SendToAll)
            {
                await HandleSendMessage(message);

                return;
            }

            var listTask = new List<Task>();

            foreach (var to in message.Tos)
            {
                if (_client.TryGetValue(to, out CSharpProfile profile))
                {
                    listTask.AddRange(profile.ConnectionIds
                        .Select(connectionId => HandleSendMessage(connectionId, message)));
                    
                    WriteLog($"Send to {to} message: {logMsg}");
                }
                else
                {
                    WriteLog($"Not found {to} message: {logMsg}");
                }
            }

            if (listTask.Count() == 0) return;

            await Task.WhenAll(listTask);
        }
    }
}
