using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeGenerator
{
	using Repo;

	public class ModelConverter
	{
		public static Model ConvertModelFromRepo(IModel repoModel)
		{
			var generatorModel = new Model();
			foreach (var node in repoModel.Nodes)
			{
				string name = "";
				string output_type = "";
				foreach (var attribute in node.Attributes)
				{
					switch (attribute.Name)
					{
						case "name":
							name = attribute.StringValue;
							break;
						case "output type":
							output_type = attribute.StringValue;
							break;
					}
				}
				var block = new Block(name, output_type);
				generatorModel.Blocks.Add(block);
			}

			foreach (var edge in repoModel.Edges)
			{
				string outputBlockName = "";
				string inputBlockName = "";
				foreach (var attribute in edge.From.Attributes)
				{
					if (attribute.Name == "name")
					{
						outputBlockName = attribute.StringValue;
					}
				}
				foreach (var attribute in edge.To.Attributes)
				{
					if (attribute.Name == "name")
					{
						inputBlockName = attribute.StringValue;
					}
				}
				Block inputBlock = null;
				Block outputBlock = null;
				foreach (var block in generatorModel.Blocks)
				{
					if (block.Name == outputBlockName)
					{
						outputBlock = block;
					}
					if (block.Name == inputBlockName)
					{
						inputBlock = block;
					}
				}
				outputBlock.ConnectTo(inputBlock);
			}
			return generatorModel;
		}
	}
}
