using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Configurations
{
    internal static class GlobalExceptionHandler
    {
        /*
        private readonly CommandErrorEventArgs _args;
        public GlobalExceptionHandler(CommandErrorEventArgs args )
        {
            _args = args;
        }
        */
        public static async Task HandleErrors(this CommandErrorEventArgs args)
        {
            if(args != null ) 
            {
                if (args.Exception is ChecksFailedException exception)
                {
                    foreach (CheckBaseAttribute check in exception.FailedChecks)
                    {
                        if (check is CooldownAttribute cooldownCheck)
                             await CooldownError(args, (CooldownAttribute)check);
                    }
                }
            }
        }
        private static async  Task<DiscordMessage> CooldownError(CommandErrorEventArgs args, CooldownAttribute cooldown)
        {
            TimeSpan remainingCooldown = cooldown.GetRemainingCooldown(args.Context);
            string formattedTime = $"{(int)remainingCooldown.TotalHours}h{(int)remainingCooldown.Minutes}m{remainingCooldown.Seconds}s";

            DiscordEmbedBuilder cooldownMessage = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Red,
                Title= "Please, wait for the cooldown to end.",
                Description =formattedTime
            };

            return await args.Context.Channel.SendMessageAsync(embed: cooldownMessage);
        }
    }
}
