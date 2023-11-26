using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using DSharpPlus.SlashCommands.EventArgs;
using FIrstDiscordBotC_.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Data.Odbc;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting.Contexts;
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
                             await CooldownError(args, cooldownCheck);
                        else if(check is RequirePermissionsAttribute requirePermissionsCheck)
                            await PermissionError(args, requirePermissionsCheck);
                    }
                }
            }
        }

        public static async Task HandleErrors(this SlashCommandErrorEventArgs args)
        {
            if (args != null)
            {
                if (args.Exception is SlashExecutionChecksFailedException exception)
                {
                    foreach(SlashCheckBaseAttribute check in exception.FailedChecks)
                    {
                        if(check is SlashRequirePermissionsAttribute requirePermissionsCheck)
                           await PermissionError(args, requirePermissionsCheck);
                    }
                }

                if (args.Exception is HierarchyException hierarchyException)
                   await HierarchyError(args);
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
        private static async Task<DiscordMessage> PermissionError(CommandErrorEventArgs args, RequirePermissionsAttribute permission)
        {
            DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder()
            {
                Content = "You don't have permission to do this.\n"+
                          "Excepted permission:" +permission.Permissions.ToString()
            };

            return await args.Context.Channel.SendMessageAsync(messageBuilder);
        }
        
        // Exceptions For Slash commands.

        private static async Task PermissionError(SlashCommandErrorEventArgs args, SlashRequirePermissionsAttribute permission)
        {
            DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            {
                IsEphemeral = true,
                Content = "Permission excepted: "+ permission.Permissions.ToString(),
            };

            await args.Context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
        }

        private static async Task HierarchyError(SlashCommandErrorEventArgs args)
        {
            DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            {
                IsEphemeral = true,
                Content = "Exception: Target have higher role."
            };

            await args.Context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);
        }
    }
}
