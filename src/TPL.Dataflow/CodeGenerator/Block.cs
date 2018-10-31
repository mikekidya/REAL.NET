using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
	public class Block
	{
		public string Name { get; set; }

		private IList<Block> inputBlocks = new List<Block>();
		public string OutputType { get; private set; }
		private IList<Block> outputBlocks = new List<Block>();

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
				return "ActionBlock";
			if (inputBlocks.Count == 0)
				return "BroadcastBlock";
			return "TransformBlock";
		}

		public string GetDefinition()
		{
			var resultBuilder = new StringBuilder();
			resultBuilder.Append("var ");
			resultBuilder.Append(Name);
			resultBuilder.Append(" = new ");
			resultBuilder.Append(BlockType());
			resultBuilder.Append("<");
			
			if (inputBlocks.Count == 1)
			{
				resultBuilder.Append(inputBlocks.First().OutputType);
			}
			else if (HasManyInputs())
			{
				resultBuilder.Append("Tuple<");
				foreach (Block block in inputBlocks)
				{
					resultBuilder.Append(block.OutputType);
					if (block != inputBlocks.Last())
						resultBuilder.Append(", ");
				}
				resultBuilder.Append(">");
			}

			if (inputBlocks.Count * outputBlocks.Count > 0)
				resultBuilder.Append(", ");

			if (OutputType != null)
				resultBuilder.Append(OutputType);

			resultBuilder.Append(">(null);");
			return resultBuilder.ToString();
		}

		public bool HasManyInputs()
		{
			return inputBlocks.Count > 1;
		}

		public bool HasManyOutputs()
		{
			return outputBlocks.Count > 1;
		}

		public ICollection<string> GetAdditionalBlocks()
		{
			ICollection<string> result = new LinkedList<string>();
			StringBuilder resultBuilder = new StringBuilder();

			if (HasManyInputs())
			{
				resultBuilder.Append("var ");
				resultBuilder.Append(Name);
				resultBuilder.Append("JoinBlock = new JoinBlock<");
				foreach (Block inputBlock in inputBlocks)
				{
					resultBuilder.Append(inputBlock.OutputType);
					if (inputBlock != inputBlocks.Last())
						resultBuilder.Append(", ");
				}
				resultBuilder.Append(">();");
				result.Add(resultBuilder.ToString());
				resultBuilder.Clear();
			}
			if (HasManyOutputs())
			{
				resultBuilder.Append("var ");
				resultBuilder.Append(Name);
				resultBuilder.Append("BroadcastBlock = new BroadcastBlock<");
				resultBuilder.Append(OutputType);
				resultBuilder.Append(">(null);");
				result.Add(resultBuilder.ToString());
				resultBuilder.Clear();
			}
			return result;
			
		}

		public ICollection<string> GetConnections()
		{
			ICollection<string> result = new LinkedList<string>();
			StringBuilder resultBuilder = new StringBuilder();

			if (HasManyInputs())
			{
				resultBuilder.Append(Name);
				resultBuilder.Append("JoinBlock.LinkTo(");
				resultBuilder.Append(Name);
				resultBuilder.Append(");");
				result.Add(resultBuilder.ToString());
				resultBuilder.Clear();
			}

			if (HasManyOutputs())
			{
				resultBuilder.Append(Name);
				resultBuilder.Append(".LinkTo(");
				resultBuilder.Append(Name);
				resultBuilder.Append("BroadcastBlock);");
				result.Add(resultBuilder.ToString());
				resultBuilder.Clear();
			}

			foreach (Block output in outputBlocks)
			{
				resultBuilder.Append(Name);
				if (HasManyOutputs())
					resultBuilder.Append("BroadcastBlock");
				resultBuilder.Append(".LinkTo(");
				resultBuilder.Append(output.Name);
				if (output.HasManyInputs())
				{
					resultBuilder.Append("JoinBlock.Target");
					resultBuilder.Append(output.inputBlocks.IndexOf(this) + 1);
				}
				resultBuilder.Append(");");
				result.Add(resultBuilder.ToString());
				resultBuilder.Clear();
			}
			return result;
		}
	}
}
