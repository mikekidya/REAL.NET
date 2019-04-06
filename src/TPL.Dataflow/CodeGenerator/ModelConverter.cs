using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CodeGenerator
{
    using Repo;

    /// <summary>
    /// Class representing methods for converning Repo model to generator model
    /// </summary>
    public class ModelConverter
    {
        /// <summary>
        /// Converts Repo model to generator model
        /// </summary>
        public static Model ConvertModelFromRepo(IModel repoModel)
        {
            var generatorModel = new Model();
            foreach (var node in repoModel.Nodes)
            {
                string name = "";
                string outputType = "";
                foreach (var attribute in node.Attributes)
                {
                    switch (attribute.Name)
                    {
                        case "name":
                            name = attribute.StringValue;
                            break;
                        case "output type":
                            outputType = attribute.StringValue;
                            break;
                    }
                }
                if (outputType == "" || outputType == "null")
                {
                    outputType = null;
                }
                var block = new Block(name, outputType, node);
                generatorModel.Blocks.Add(block);
            }

            foreach (var edge in repoModel.Edges)
            {
                Block inputBlock = null;
                Block outputBlock = null;
                foreach (var block in generatorModel.Blocks)
                {
                    if (block.GetRepoNode().Equals(edge.From))
                    {
                        outputBlock = block;
                    }
                    if (block.GetRepoNode().Equals(edge.To))
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
