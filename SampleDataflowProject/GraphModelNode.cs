using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SampleDataflowProject
{
    class GraphModelNode
    {
        public Type OutputType { get; private set; }
        private List<Type> inputTypes = new List<Type>();
        private List<GraphModelNode> inputs = new List<GraphModelNode>();
        private List<GraphModelNode> outputs = new List<GraphModelNode>();

        public GraphModelNode(Type outputType)
        {
            this.OutputType = outputType;
        }

        public void AddInput(GraphModelNode input)
        {
            inputs.Add(input);
            inputTypes.Add(input.OutputType);
        }

        public void AddOutput(GraphModelNode output)
        {
            outputs.Add(output);
        }

        
    }
}
