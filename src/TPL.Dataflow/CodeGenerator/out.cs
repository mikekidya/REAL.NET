using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;

namespace UserNamespace
{
	class Program
	{
		static void Main(string[] args)
		{
			var start = new BroadcastBlock<int>(null);
			var transform = new TransformBlock<int, Random>(null);
			var end = new ActionBlock<Tuple<Random, int>>(null);

			var startBroadcastBlock = new BroadcastBlock<int>(null);
			var endJoinBlock = new JoinBlock<Random, int>();

			start.LinkTo(startBroadcastBlock);
			startBroadcastBlock.LinkTo(transform);
			startBroadcastBlock.LinkTo(endJoinBlock.Target2);
			transform.LinkTo(endJoinBlock.Target1);
			endJoinBlock.LinkTo(end);
		}
	}
}
