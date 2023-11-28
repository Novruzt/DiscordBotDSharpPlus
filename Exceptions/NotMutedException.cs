using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Exceptions
{
    internal class NotMutedException:Exception
    {
        public NotMutedException()
        {
            
        }
        public NotMutedException(string message) : base(message) { }
    }
}
