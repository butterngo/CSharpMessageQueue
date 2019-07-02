namespace CSharpMessageQueue
{
    using CSharpMessageQueue.Models;
    using log4net;
    using Microsoft.Extensions.Configuration;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class CSharpMessageHub: CSharpMessageHubBase<CSharpMessage>
    {
        protected override ILog Logger => LogManager.GetLogger(typeof(CSharpMessageHub));

        protected override string UniqueKey => context.Request.Query["uniqueKey"].ToString();

        protected override string Name => string.Empty;

        public CSharpMessageHub(IConfiguration configuration) : base(configuration)
        {
        }

        public override async Task SendAsync(CSharpMessage message)
        {
            var body = Encoding.UTF8.GetString(message.Body);

            var logMsg = $"messageId: {message.Id.ToString()} To: {string.Join(",", message.Tos)} Content: {body}";

            WriteLog($"Server received message: {logMsg}");

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
