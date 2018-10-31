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
			Block A = new Block("start", "int");
			Block B = new Block("transform", "Random");
			Block C = new Block("end", null);

			A.ConnectTo(B);
			B.ConnectTo(C);
			A.ConnectTo(C);

			Model model = new Model();
			model.Blocks.Add(A);
			model.Blocks.Add(B);
			model.Blocks.Add(C);

			
			var engine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(@"..\..\"));

			string result = engine.Parse("template.cshtml", model);

			System.IO.File.WriteAllText("out.cs", result);

			//Console.WriteLine(result);
			//Console.ReadKey();
			
		}
	}
}
