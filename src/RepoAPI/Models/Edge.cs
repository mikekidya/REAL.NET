using System;
namespace RepoAPI.Models
{
    public class Edge : Element
    {
        public ElementInfo From { get; set; }
        public ElementInfo To { get; set; }
    }
}
