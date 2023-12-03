using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Utils.LevelSystem
{
    public class DUser
    {
        public string Username { get; set; }
        public string AvatarUrl { get; set; }
        public ulong GuildID { get; set; }
        public int XP { get; set; }
        public int Level { get; set; }
    }
}
