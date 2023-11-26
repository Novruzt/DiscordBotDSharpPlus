using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Exceptions
{
    internal class HierarchyException : Exception
    {
        public HierarchyException()
        {
            
        }
        public HierarchyException(string message) : base(message) { }
    }
}
