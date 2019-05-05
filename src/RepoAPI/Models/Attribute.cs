using System;
namespace RepoAPI.Models
{
    public class Attribute
    {
        public string Name { get; set; }
        public AttributeKind Kind { get; set; }
        public bool IsInstantiable { get; set; }
        public ElementInfo Type { get; set; }
        public string StringValue { get; set; }
        public ElementInfo ReferenceValue { get; set; }
    }
}
