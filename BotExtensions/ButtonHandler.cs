using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.BotExtensions
{
    internal static class ButtonHandler
    {
        public static async Task HandleButtons(this ComponentInteractionCreateEventArgs args)
        {
            if (args.Interaction.Data.CustomId.StartsWith("HelpButton"))
                await HandleHelpButtons(args);
            if(args.Interaction.Data.CustomId.StartsWith("Test-"))
                await HandleTestButtons(args);
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
    }
}
