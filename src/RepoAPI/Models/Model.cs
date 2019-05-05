using System;
using System.Collections.Generic;

namespace RepoAPI.Models
{
    public class Model
    {
        public string Name { get; set; }
        public string MetamodelName { get; set; }
        public bool IsVisible { get; set; }
        public IEnumerable<ElementInfo> Elements { get; set; }
        public IEnumerable<ElementInfo> Nodes { get; set; }
        public IEnumerable<ElementInfo> Edges { get; set; }
    }
}
