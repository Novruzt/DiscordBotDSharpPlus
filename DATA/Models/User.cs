using FIrstDiscordBotC_.DATA.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.DATA.Models
{
    public class User:BaseEntity
    {
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }


        //server
        public string GuildId { get; set; }
        public int ServerId { get; set; }
        public Server Server { get; set; }
    }
}
