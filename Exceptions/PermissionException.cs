using DSharpPlus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Exceptions
{
    internal class PermissionException:Exception
    {
        public Permissions Permissions;
        public PermissionException() 
        {

        }
        public PermissionException(string message):base(message) { }
        public PermissionException(Permissions permissions)
        {
            this.Permissions = permissions;
        }
    }
}
