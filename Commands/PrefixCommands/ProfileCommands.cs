using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FIrstDiscordBotC_.Utils.LevelSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Commands.PrefixCommands
{
    internal class ProfileCommands: BaseCommandModule
    {
        [Command("Profile")]
        public async Task ProfileCommand(CommandContext context)
        {

            DUser user = new DUser()
            {
                Username = context.User.Username,
                Level = 1,
                XP = 0,
                GuildID=context.Guild.Id,
                AvatarUrl=context.User.AvatarUrl
            };

            if (!LevelHandler.CheckUser(context))
            {
                LevelHandler.StoreUserDetails(user);

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Profile created",
                    Description = "Call command again to view profile.",
                    Color = DiscordColor.Green
                };

                await context.Channel.SendMessageAsync(embed: embedBuilder);
            }
            else
            {
                DUser dUser = LevelHandler.GetUser(context);

                DiscordMessageBuilder profileMessage = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
                                                                                           .WithColor(DiscordColor.Azure)
                                                                                           .WithTitle(dUser.Username+"'s profile")
                                                                                           .AddField("Level", dUser.Level.ToString())
                                                                                           .AddField("XP", dUser.XP.ToString()+"/10")
                                                                                           .WithThumbnail(dUser.AvatarUrl));

                await context.Channel.SendMessageAsync(profileMessage);
            }
        }
    }
}
