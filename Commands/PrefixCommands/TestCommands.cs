using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FIrstDiscordBotC_.Others;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Commands
{
    public class TestCommands:BaseCommandModule 
    {
        [Command("Test")]
        [Cooldown(1, 30, CooldownBucketType.Guild)]
        public async Task TestCommand(CommandContext context)
        {
            await context.Channel.SendMessageAsync($"Hello, {context.User.Username}. ");
        }

        [Command("Embed")]
        public async Task EmbedMessage(CommandContext context)
        {
            DiscordEmbedBuilder builder = new DiscordEmbedBuilder()
            {
                Color=DiscordColor.Green,
                Title="Embed message was called.",
                Description=$"Called by {context.User.Mention}"
            };

            await context.Channel.SendMessageAsync(embed: builder);
        }

        [Command("Card")]
        public async Task CardGame(CommandContext context)
        {
            CardSystem userCard = new CardSystem();

            DiscordEmbedBuilder userCardEmbed = new DiscordEmbedBuilder() 
            {
                Title = $"Your card is {userCard.SelectedCard}",
                Color=DiscordColor.Aquamarine
            };

            await context.Channel.SendMessageAsync(embed: userCardEmbed);

            CardSystem botCard = new CardSystem();

            DiscordEmbedBuilder botCardEmbed = new DiscordEmbedBuilder()
            {
                Title = $"Bot card is {botCard.SelectedCard}",
                Color = DiscordColor.Aquamarine
            };

            await context.Channel.SendMessageAsync(embed: botCardEmbed);

            if (userCard.SelectedNumber > botCard.SelectedNumber)
            {
                DiscordEmbedBuilder WinMessage = new DiscordEmbedBuilder()
                {
                    Title= "Congrats, you won!",
                    Color=DiscordColor.Green
                };

                await context.Channel.SendMessageAsync(embed: WinMessage);
            }
            else if(userCard.SelectedNumber < botCard.SelectedNumber)
            {
                DiscordEmbedBuilder WinMessage = new DiscordEmbedBuilder()
                {
                    Title = "Sadly, bot won!",
                    Color = DiscordColor.Red
                };

                await context.Channel.SendMessageAsync(embed: WinMessage);
            }
            else
            {
                DiscordEmbedBuilder WinMessage = new DiscordEmbedBuilder()
                {
                    Title = "Nobody won, it's draw.",
                    Color = DiscordColor.Orange
                };
            }
        }
    }
}
