namespace CSharpMessageQueue.Models
{
    public class JsCSharpMessage: CSharpMessageBase
    {
        public string Body { get; set; }
        public CSharpProfile FromProfile { get; set; }
        public bool SendToAll { get; set; } = false;
    }
}
