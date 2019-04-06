using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CodeGenerator;
using NSubstitute;
using NUnit.Framework;
using Repo;

namespace Dataflow.Tests
{
    [TestFixture]
    public class GenerationTest
    {
        [Test]
        public void GenerationFailTest()
        {
            var model = new Model();
            var aBlock = new Block("first", "int");
            var bBlock = new Block("second", "None");
            aBlock.ConnectTo(bBlock);
            model.Blocks.Add(aBlock);
            model.Blocks.Add(bBlock);

			Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);

            var generatedText = RazorGenerator.GenerateFromModel(model);
        }
    }
}
