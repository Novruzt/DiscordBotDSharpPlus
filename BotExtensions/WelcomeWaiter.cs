using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.BotExtensions
{
    internal static class WelcomeWaiter
    {
        public static async  Task<DiscordMessage> WelcomeMessage(this MessageCreateEventArgs e)
        {
            if (!e.Author.IsBot && e.Message.Content.ToLower() == "salam")
                return await e.Channel.SendMessageAsync("Salam, xoş gəldin!");
            return null;
        }
    }
}
