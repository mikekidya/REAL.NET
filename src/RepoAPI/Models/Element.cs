using System;
using System.Collections.Generic;

namespace RepoAPI.Models
{
    public class Element
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ElementInfo Class { get; set; }
        public IEnumerable<Attribute> Attributes { get; set; } 
        public bool IsAbstract { get; set; }
        public Metatype Metatype { get; set; }
        public Metatype InstanceMetatype { get; set; }
        public string Shape { get; set; }
    }
}
