using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Exceptions
{
    internal class DefaultException:Exception
    {
        public DefaultException()
        {
            
        }
        public DefaultException(string message):base(message) { }
    }
}
