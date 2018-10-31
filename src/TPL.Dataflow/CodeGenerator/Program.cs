using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RazorLight;

namespace CodeGenerator
{
	class Program
	{
		static void Main(string[] args)
		{
			var a = new Block("start", "int");
			var b = new Block("transform", "Random");
			var c = new Block("end", null);

			a.ConnectTo(b);
			b.ConnectTo(c);
			a.ConnectTo(c);

			var model = new Model();
			model.Blocks.Add(a);
			model.Blocks.Add(b);
			model.Blocks.Add(c);

			var engine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(@"..\..\"));
			string result = engine.Parse("template.cshtml", model);
			System.IO.File.WriteAllText("out.cs", result);
			//Console.WriteLine(result);
			//Console.ReadKey();
		}
	}
}
