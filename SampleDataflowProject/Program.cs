using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks.Dataflow;

namespace SampleDataflowProject
{
    class Program
    {

        static void Main(string[] args)
        {
            ISamplePathPrinter manually = new ManuallyWrittenPrinter();
            manually.printPath();

			ISamplePathPrinter automatically = new AutomaticallyWrittenPrinter();
			automatically.printPath();

			Console.ReadKey();
        }
    }
}
