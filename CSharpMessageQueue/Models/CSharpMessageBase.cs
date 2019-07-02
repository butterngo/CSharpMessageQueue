namespace CSharpMessageQueue.Models
{
    using System;

    public abstract class CSharpMessageBase
    {
        public CSharpMessageBase()
        {
            Id = Guid.NewGuid();
            SendDate = DateTime.UtcNow;
            Tos = new string[] { };
        }

        public Guid Id { get; set; }
        public string Label { get; set; }
        public string From { get; set; }
        public string[] Tos { get; set; }
        public string Version { get; set; }
        public DateTime SendDate { get; set; }
    }
}
