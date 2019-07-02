using System.Collections.Generic;

namespace CSharpMessageQueue.Models
{
    public class CSharpProfile
    {
        public CSharpProfile() { }
        public CSharpProfile(string id, string connectionId, string name)
        {
            Id = id;
            ConnectionIds = new List<string> { connectionId };
            Groups = new List<string>();
            Name = name;
        }
        public string Id { get; set; }
        public List<string> ConnectionIds { get; set; }
        public List<string> Groups { get; set; }
        public string Name { get; set; }
    }
}
