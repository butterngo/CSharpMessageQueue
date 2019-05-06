namespace CSharpMessageQueue.Models
{
    public class CompletedMessageReceived
    {
        public string ClientId { get; set; }

        public string From { get; set; }

        public CSharpMessage Message { get; set; }
    }
}
