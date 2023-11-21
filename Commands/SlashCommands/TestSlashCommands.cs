using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity;
using DSharpPlus.SlashCommands;
using FIrstDiscordBotC_.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Remoting.Contexts;
using static DSharpPlus.Entities.DiscordEmbedBuilder;

namespace FIrstDiscordBotC_.Commands.SlashCommands
{
    public class TestSlashCommands :ApplicationCommandModule
    {
        [SlashCommand("Test", "test command to understanding slash commands")]
        public async Task TestSlashCommand(InteractionContext context, [Option("Name", "Type name to greeting.", true)] string name="Default")
        {
            DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            {
                IsEphemeral = true,
                Content="Calling slash command",
            };

            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);

            DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
            {
                Title = "Test",
                Description = name
            };

            await context.Channel.SendMessageAsync(embed: embedBuilder);
        }

        [SlashCommand("Poll", "Create your own poll. There will be 2 option. There is default sealed option as 3rd.")]
        public async Task PollSlashCommand(InteractionContext context, [Option("Question", "Main subject of poll.")] string question,
                                                                       [Option("Option-1", "Set Option 1 for poll.")] string option1,
                                                                       [Option("Option-2", "Set Option 2 for poll.")] string option2,
                                                                       [Option("Time-Limit", "Default time is 1h. Input type is 1h1m1s (You can miss what you want. Ex: 15m, 1h30s etc.)", true)] string time = "1h")
        {
            DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            {
                IsEphemeral = true,
                Content = "Calling slash command",
            };

            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);

            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            TimeSpan pollTime;
            bool IsSpan = time.TryParse(out pollTime);
            if (!IsSpan)
            {
                pollTime = TimeSpan.FromHours(1);
            }

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                            DiscordEmoji.FromName(Program.Client, ":zero:"),
                                            DiscordEmoji.FromName(Program.Client, ":two:")};

            string optionDescription = $"{emojiOptions[0]} | {option1} \n" +
                                       $"{emojiOptions[1]} | NotDecided\n" +
                                       $"{emojiOptions[2]} | {option2}";

            DiscordEmbedBuilder pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Azure,
                Title = question,
                Description = optionDescription
            };

            DiscordMessage sentPoll = await context.Channel.SendMessageAsync(embed: pollMessage);

            foreach (DiscordEmoji emoji in emojiOptions)
            {
                await sentPoll.CreateReactionAsync(emoji);
            }

            ReadOnlyCollection<Reaction> totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);

            int opt1 = 0;
            int opt0 = 0;
            int opt2 = 0;

            foreach (Reaction reaction in totalReactions)
            {

                if (reaction.Emoji == emojiOptions[0]) opt1++;
                else if (reaction.Emoji == emojiOptions[1]) opt0++;
                else if (reaction.Emoji == emojiOptions[2]) opt2++;
                else continue;
            }

            int totalVotes = opt1 + opt0 + opt2;

            string resultDescription = $"{emojiOptions[0]}:  {opt1} votes \n" +
                                       $"{emojiOptions[1]}:  {opt0} votes  \n" +
                                       $"{emojiOptions[2]}:  {opt2} votes \n" +
                                       $"Total Votes:        {totalVotes}";

            DiscordEmbedBuilder resultEmbed = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Lilac,
                Title = "Results of the Poll",
                Description = resultDescription
            };

            await context.Channel.SendMessageAsync(embed: resultEmbed);
        
        }

        [SlashCommand("Caption", "Give any image caption.")]
        public async Task CaptionSlashCommand(InteractionContext context, [Option("Caption", "Caption for image")] string caption,
                                                                          [Option("Image", "Image you want to upload.")] DiscordAttachment picture)
        {
            DiscordInteractionResponseBuilder responseBuilder = new DiscordInteractionResponseBuilder()
            {
                IsEphemeral = true,
                Content = "Calling slash command",
            };

            await context.CreateResponseAsync(InteractionResponseType.ChannelMessageWithSource, responseBuilder);

            DiscordEmbedBuilder captionMessage = new DiscordEmbedBuilder()
            {
                Color = DiscordColor.Lilac,
                ImageUrl = picture.Url, 
            };

            DiscordMessageBuilder builder = new DiscordMessageBuilder()
                .AddEmbed(captionMessage.WithFooter(caption));

            await context.Channel.SendMessageAsync(builder);
        }
    }
}
