namespace CSharpMessageQueueClient.Models
{
    public class CompletedMessageReceived
    {
        public string From { get; set; }

        public CSharpMessage Message { get; set; }
    }
}
