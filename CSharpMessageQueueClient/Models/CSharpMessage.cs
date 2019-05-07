namespace CSharpMessageQueueClient.Models
{
    using System;

    public class CSharpMessage
    {
        public CSharpMessage()
        {
            Id = Guid.NewGuid();
            SendDate = DateTime.UtcNow;
           
        }

        public Guid Id { get; set; }
        public byte[] Body { get; set; }
        public string Label { get; set; }
        public string From { get; set; }
        public string[] Tos { get; set; }
        public string Version { get; set; }
        public DateTime SendDate { get;  set; }
    }
}
