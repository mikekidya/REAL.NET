using System.Collections.Generic;
using System.Linq;

namespace CodeGenerator
{
    public class Block
    {
        private IList<Block> inputBlocks = new List<Block>();
        private IList<Block> outputBlocks = new List<Block>();

        public string Name { get; set; }
        public string OutputType { get; private set; }

        public Block(string name, string outputType)
        {
            this.Name = name;
            this.OutputType = outputType;
        }

        public void ConnectTo(Block otherBlock)
        {
            this.outputBlocks.Add(otherBlock);
            otherBlock.inputBlocks.Add(this);
        }

        private string BlockType()
        {
            if (OutputType == null)
            {
                return "ActionBlock";
            }	
            if (inputBlocks.Count == 0)
            {
                return "BroadcastBlock";
            }
            return "TransformBlock";
        }

        public string GetDefinition()
        {
            string result = $"var {Name} = new {BlockType()}<";
            
            if (inputBlocks.Count == 1)
            {
                result += inputBlocks.First().OutputType;
            }
            else if (HasManyInputs())
            {
                result += "Tuple<";
                foreach (Block block in inputBlocks)
                {
                    result += block.OutputType;
                    if (block != inputBlocks.Last())
                    {
                        result +=", ";
                    }
                }
                result += ">";
            }
            if (inputBlocks.Count * outputBlocks.Count > 0)
            {
                result += ", ";
            }
            if (OutputType != null)
            {
                result += OutputType;
            }
            return result + ">(null);";
        }

        public bool HasManyInputs() => inputBlocks.Count > 1;

        public bool HasManyOutputs() => outputBlocks.Count > 1;

        public ICollection<string> GetAdditionalBlocks()
        {
            ICollection<string> result = new LinkedList<string>();

            if (HasManyInputs())
            {
                string current = $"var {Name}JoinBlock = new JoinBlock<";
                foreach (Block inputBlock in inputBlocks)
                {
                    current += inputBlock.OutputType;
                    if (inputBlock != inputBlocks.Last())
                    {
                        current += ", ";
                    }
                }
                result.Add(current + ">();");
            }
            if (HasManyOutputs())
            {
                result.Add($"var {Name}BroadcastBlock = new BroadcastBlock<{OutputType}>(null);");
            }
            return result;
            
        }

        public ICollection<string> GetConnections()
        {
            ICollection<string> result = new LinkedList<string>();

            if (HasManyInputs())
            {
                result.Add($"{Name}JoinBlock.LinkTo({Name});");
            }

            if (HasManyOutputs())
            {
                result.Add($"{Name}.LinkTo({Name}BroadcastBlock);");
            }

            foreach (Block output in outputBlocks)
            {
                string current = Name;
                if (HasManyOutputs())
                {
                    current += "BroadcastBlock";
                }
                current += $".LinkTo({output.Name}";
                if (output.HasManyInputs())
                {
                    current += "JoinBlock.Target" + (output.inputBlocks.IndexOf(this) + 1);
                }
                current += ");";
                result.Add(current);
            }
            return result;
        }
    }
}
