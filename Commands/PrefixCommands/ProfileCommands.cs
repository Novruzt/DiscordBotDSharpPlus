using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using FIrstDiscordBotC_.DATA.Models.Common;
using FIrstDiscordBotC_.DATA.Models;
using FIrstDiscordBotC_.Utils.DatabaseUtils;
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

        #region JsonLevelSytem
        //[Command("Profile")]
        //public async Task ProfileCommand(CommandContext context)
        //{

        //    DUser user = new DUser()
        //    {
        //        Username = context.User.Username,
        //        Level = 1,
        //        XP = 0,
        //        GuildID=context.Guild.Id,
        //        AvatarUrl=context.User.AvatarUrl
        //    };

        //    if (!LevelHandler.CheckUser(context))
        //    {
        //        LevelHandler.StoreUserDetails(user);

        //        DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
        //        {
        //            Title = "Profile created",
        //            Description = "Call command again to view profile.",
        //            Color = DiscordColor.Green
        //        };

        //        await context.Channel.SendMessageAsync(embed: embedBuilder);
        //    }
        //    else
        //    {
        //        DUser dUser = LevelHandler.GetUser(context);

        //        DiscordMessageBuilder profileMessage = new DiscordMessageBuilder().AddEmbed(new DiscordEmbedBuilder()
        //                                                                                   .WithColor(DiscordColor.Azure)
        //                                                                                   .WithTitle(dUser.Username+"'s profile")
        //                                                                                   .AddField("Level", dUser.Level.ToString())
        //                                                                                   .AddField("XP", dUser.XP.ToString()+"/10")
        //                                                                                   .WithThumbnail(dUser.AvatarUrl));

        //        await context.Channel.SendMessageAsync(profileMessage);
        //    }
        //}

        #endregion

        [Command("Profile")]
        public async Task ProfileCommand(CommandContext context)
        {
            try
            {
                // Fetch the user details from the database
                User user = await DbLevelHandler.GetUserAsync(context.Guild.Id.ToString(), context.User.Username);

                if (user == null)
                {
                    // User not found, create a new profile entry
                    user = new User
                    {
                        Username = context.User.Username,
                        AvatarUrl = context.User.AvatarUrl,
                        XP = 0,
                        Level = 1,
                        GuildId = context.Guild.Id.ToString()
                    };

                    await DbLevelHandler.StoreUserDetailsAsync(user, new Server
                    {
                        GuildID = context.Guild.Id.ToString(),
                        Name = context.Guild.Name
                    });

                    DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
                    {
                        Title = "Profile created",
                        Description = "Call the command again to view your profile.",
                        Color = DiscordColor.Green
                    };

                    await context.Channel.SendMessageAsync(embed: embedBuilder);
                }
                else
                {
                    // User profile exists
                    DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder
                    {
                        Title = $"{user.Username}'s profile",
                        Color = DiscordColor.Azure
                    }
                    .AddField("Level", user.Level.ToString())
                    .AddField("XP", $"{user.XP}/10")
                    .WithThumbnail(user.AvatarUrl);

                    await context.Channel.SendMessageAsync(embed: embedBuilder);
                }
            }
            catch (Exception ex)
            {
                // Handle the exception (e.g., log it)
                await context.Channel.SendMessageAsync($"An error occurred: {ex.Message}");
            }
        }
    }
}
