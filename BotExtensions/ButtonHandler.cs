using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using FIrstDiscordBotC_.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.BotExtensions
{
    internal static class ButtonHandler
    {
        public static async Task HandleButtons(this ComponentInteractionCreateEventArgs args, DiscordClient client)
        {
            if (args.Interaction.Data.CustomId.StartsWith("HelpButton"))
                await HandleHelpButtons(args);
            if(args.Interaction.Data.CustomId.StartsWith("Test-"))
                await HandleTestButtons(args);
            if(args.Id== "dropDownList")
                await HandleDropDownList(args);
            if(args.Id== "ChannelList")
                await HandleChannelList(args, client);
            if (args.Id == "RoleList")
                await HandleRoleList(args, client);
        }
        private static async Task<DiscordMessage> HandleHelpButtons(ComponentInteractionCreateEventArgs args)
        {
            if(args.Interaction.Data.CustomId == "HelpButtonFun")
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                                 .WithContent("Fun Commands"));
            
            if(args.Interaction.Data.CustomId== "HelpButtonMod")            
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                                .WithContent("Moderation Commands"));
            
            if (args.Interaction.Data.CustomId == "HelpButtonVoice")            
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder()
                                                                                               .WithContent("Voice Channel Commands"));
            
            if (args.Interaction.Data.CustomId == "HelpButtonExit")
                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                                                                                                .WithContent("You exited from the help list."));
            return null;
        }

        private static async Task<DiscordMessage> HandleTestButtons(ComponentInteractionCreateEventArgs args)
        {
            
            if (args.Interaction.Data.CustomId == "Test-1")
                await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                                                                                                  .WithContent("You Pressed the 1st button"));

            else await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                                                                                                  .WithContent("You Pressed the one of the other buttons"));
            return null;
        }
        private static async Task<DiscordMessage> HandleDropDownList(ComponentInteractionCreateEventArgs args)
        {
            string[] options = args.Values;

            foreach(string option in options)
            {
                switch (option) 
                {
                    case "Option-1":
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                                                                                                  .WithContent("You Pressed the 1st option"));
                        break;

                    case "Option-2":
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                                                                                                  .WithContent("You Pressed the 2st option"));
                        break;

                    case "Option-3":
                        await args.Interaction.CreateResponseAsync(InteractionResponseType.UpdateMessage, new DiscordInteractionResponseBuilder()
                                                                                                  .WithContent("You Pressed the 3st option"));
                        break;
                }
            }   

            return null;
        }
        private static async Task<DiscordMessage> HandleChannelList(ComponentInteractionCreateEventArgs args, DiscordClient client)
        {
            string[] options = args.Values;
            foreach (string item in options)
            {
                DiscordChannel channel = await client.GetChannelAsync(ulong.Parse(item));
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{args.User.Mention} picked up {channel.Mention}"));
            }

            return null;
        }
        private static async Task<DiscordMessage> HandleRoleList(ComponentInteractionCreateEventArgs args, DiscordClient client)
        {
            string[] options = args.Values;
            foreach (string item in options)
            {
                DiscordRole role = args.Guild.GetRole(ulong.Parse(item));
                DiscordMember member = (DiscordMember)args.User;

                if (!member.Permissions.HasPermission(Permissions.ManageRoles))
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{args.User.Mention} doesn't have enough permission"));
                    return null;
                }    

                if (member.Roles.Any(c=>c.Id==role.Id))
                {
                    await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{args.User.Mention} already has this role"));
                    return null;
                }
                 
                await member.GrantRoleAsync(role);
                await args.Interaction.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, new DiscordInteractionResponseBuilder().WithContent($"{role.Name} is given to {args.User.Mention}"));
            }
            return null;
        }
    }
}
