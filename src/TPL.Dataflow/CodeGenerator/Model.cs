using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CodeGenerator
{
    /// <summary>
    /// Class representing abstract model for generation
    /// </summary>
    public class Model
    {
        public ICollection<Block> Blocks = new LinkedList<Block>();
    }
}
