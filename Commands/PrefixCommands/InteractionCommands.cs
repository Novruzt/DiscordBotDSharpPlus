using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.EventHandling;
using DSharpPlus.Interactivity.Extensions;
using FIrstDiscordBotC_.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Commands
{
    public class InteractionCommands:BaseCommandModule
    {
        [Command("Interact")]
        public async Task Interact(CommandContext context)
        {
            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            InteractivityResult<DiscordMessage> messageToRetrieve = await interactivity.WaitForMessageAsync(message => message.Content=="Hello");

            if(messageToRetrieve.Result.Content=="Hello") 
            {
                await context.Channel.SendMessageAsync($"Welcome, {context.User.Mention}");
            }
        }

        [Command("React")]
        public async Task React(CommandContext context)
        {
            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            InteractivityResult<MessageReactionAddEventArgs> messageToReact = await interactivity.WaitForReactionAsync(message=> message.Message.Id== 1175448437169340476);

            if(messageToReact.Result.Message.Id== 1175448437169340476)
            {
                await context.Channel.SendMessageAsync($"{context.User.Mention} used emoji with the name of {messageToReact.Result.Emoji.Name}");
            }
        }

        [Command("Poll")]
        public async Task Poll(CommandContext context, string option1, string option2, string time, [RemainingText] string pollTitle)
        {
            InteractivityExtension interactivity = Program.Client.GetInteractivity();

            TimeSpan pollTime;
            bool IsSpan = TimeConverter.TryParse(time, out pollTime);
            if(!IsSpan)
            {
                pollTime = TimeSpan.FromSeconds(10);
            }

            DiscordEmoji[] emojiOptions = { DiscordEmoji.FromName(Program.Client, ":one:"),
                                            DiscordEmoji.FromName(Program.Client, ":zero:"),
                                            DiscordEmoji.FromName(Program.Client, ":two:")};
            
            string optionDescription = $"{emojiOptions[0]} | {option1} \n"+
                                       $"{emojiOptions[1]} | NotDecided\n"+
                                       $"{emojiOptions[2]} | {option2}";

            DiscordEmbedBuilder pollMessage = new DiscordEmbedBuilder
            {
                Color = DiscordColor.Azure,
                Title = pollTitle,
                Description = optionDescription
            };

            DiscordMessage sentPoll = await context.Channel.SendMessageAsync(embed:  pollMessage);

            foreach(DiscordEmoji emoji in emojiOptions)
            {
                await sentPoll.CreateReactionAsync(emoji);
            }

            ReadOnlyCollection<Reaction> totalReactions = await interactivity.CollectReactionsAsync(sentPoll, pollTime);

            int opt1 = 0;
            int opt0 = 0;
            int opt2 = 0;
           
            foreach(Reaction reaction in totalReactions) 
            {

                if (reaction.Emoji == emojiOptions[0]) opt1++;
                else if (reaction.Emoji == emojiOptions[1]) opt0++;
                else if (reaction.Emoji == emojiOptions[2]) opt2++;
                else continue;
            }

            int totalVotes=opt1 + opt0+opt2;

            string resultDescription=  $"{emojiOptions[0]}:  {opt1} votes \n" +
                                       $"{emojiOptions[1]}:  {opt0} votes  \n" +
                                       $"{emojiOptions[2]}:  {opt2} votes \n"+
                                       $"Total Votes:        {totalVotes}";

            DiscordEmbedBuilder resultEmbed = new DiscordEmbedBuilder()
            {
                Color= DiscordColor.Lilac,
                Title ="Results of the Poll",
                Description = resultDescription
            };

            await context.Channel.SendMessageAsync(embed: resultEmbed);
        }

    }
}
