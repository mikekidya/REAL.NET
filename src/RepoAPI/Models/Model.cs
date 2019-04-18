using System;
using System.Collections.Generic;

namespace RepoAPI.Models
{
    public class Model
    {
        public string Name { get; set; }
        public string MetamodelName { get; set; }
        public IEnumerable<Node> Nodes { get; set; }
    }
}
