using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.DATA.Models.Common
{
    public class Server:BaseEntity
    {
        public string GuildID { get; set; }
        public string Name { get; set; }
    }
}
