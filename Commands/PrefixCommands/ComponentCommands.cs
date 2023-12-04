using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Commands.PrefixCommands
{
    internal class ComponentCommands:BaseCommandModule
    {
        [Command("Dropdown")]
        public async Task DropDownList(CommandContext context)
        {
            List<DiscordSelectComponentOption> options  = new List<DiscordSelectComponentOption>
            {  
                //<summary>
                //First argument is what user sees, second one is Id. It called <values> on Eventhandler.
                //ComponentInteractionCreateEventArgs for DiscordComponents.
                //e.Id = DiscordSelectComponent => First argument 
                //</summary>

                new DiscordSelectComponentOption("Option 1", "Option-1"),
                new DiscordSelectComponentOption("Option 2", "Option-2"),
                new DiscordSelectComponentOption("Option 3", "Option-3")
            };

            DiscordSelectComponent dropDown = new DiscordSelectComponent("dropDownList", "Select...", options);

            DiscordMessageBuilder dropDownMessage = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
                                                                                          .WithColor(DiscordColor.Gold)
                                                                                          .WithTitle("This embed has a DropDownList"))
                                                                                          .AddComponents(dropDown);

            await context.Channel.SendMessageAsync(dropDownMessage);
        }

        [Command("ChannelList")]
        public async Task ChannelList(CommandContext context)
        {
            List<ChannelType> channelTypes = new List<ChannelType>()
            {
                ChannelType.Text
            };

            DiscordChannelSelectComponent channelComponent = new DiscordChannelSelectComponent("ChannelList", "Select...", channelTypes);

            DiscordMessageBuilder dropDownMessage = new DiscordMessageBuilder()
                .WithContent("Channel List")
                .AddComponents(channelComponent);

            await context.Channel.SendMessageAsync(dropDownMessage);
        }

        [Command("RoleList")]
        public async Task RoleList(CommandContext context)
        {
            DiscordRoleSelectComponent roleList = new DiscordRoleSelectComponent("RoleList", "Select...");

            DiscordMessageBuilder dropDownMessage = new DiscordMessageBuilder()
                .WithContent("Role list")
                .AddComponents(roleList);

            await context.Channel.SendMessageAsync(dropDownMessage);
        }
    }
}
