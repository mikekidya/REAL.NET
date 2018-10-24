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
			var engine = EngineFactory.CreatePhysical(System.IO.Path.GetFullPath(@"..\..\"));

			string result = engine.Parse("template.cshtml", new Model());
			Console.WriteLine(result);
			Console.ReadKey();
		}
	}
}
