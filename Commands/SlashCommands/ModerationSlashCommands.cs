using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using FIrstDiscordBotC_.Exceptions;
using FIrstDiscordBotC_.Extensions;
using Google.Apis.CustomSearchAPI.v1.Data;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FIrstDiscordBotC_.Config;

namespace FIrstDiscordBotC_.Commands.SlashCommands
{
    public class ModerationSlashCommands:ApplicationCommandModule
    {
        [SlashCommand("Ban", "Bans user from server.")]
        [SlashRequirePermissions(Permissions.BanMembers)]
        public async Task BanSlashCommand(InteractionContext context, [Option("User", "User to ban")] DiscordUser user,
                                                                      [Option("Reason", "Reason for ban")] string reason = "default")
        {
            await context.DeferAsync();
            DiscordMember banMember = (DiscordMember)user;

            if (banMember.Hierarchy >= context.Member.Hierarchy)
                throw new HierarchyException();
            try
            {
                DiscordChannel channel = context.Guild.GetChannel(784733575936737304);

                await context.Guild.BanMemberAsync(banMember, 0, reason);

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Banned user: " + banMember.Username + "\n" + "Banned Id: " + banMember.Id,
                    Description = "Ban Reason: " + reason,
                    Color = DiscordColor.Red
                };

                DiscordEmbedBuilder logBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Ban command used.",
                    Description = "User: " + context.User.Username + "\n" + "UserId: " + context.User.Id + "\n"
                    + "Banned user: " + banMember.Username + "\n" + "Banned userId: " + banMember.Id + "\n"
                    + $"Reason: {reason}",
                    Color = DiscordColor.Red
                };

                await channel.SendMessageAsync(embed: logBuilder);
                await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedBuilder));
            }
            catch
            {
                throw new DefaultException();
            }
               
        }

        [SlashCommand("Kick", "Kicks user from server.")]
        [SlashRequirePermissions(Permissions.KickMembers)]
        public async Task KickSlashCommand(InteractionContext context, [Option("User", "User to ban")] DiscordUser user,
                                                                       [Option("Reason", "Reason for ban")] string reason = "default")
        {
            await context.DeferAsync();
            DiscordMember kickMember = (DiscordMember)user;

            if (kickMember.Hierarchy >= context.Member.Hierarchy)
                throw new HierarchyException();
            try
            {
                await kickMember.RemoveAsync(reason);

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Kicked user: " + kickMember.Username + "\n" + "Kicked Id: " + kickMember.Id,
                    Description = "Kick Reason: " + reason,
                    Color = DiscordColor.Yellow
                };

                await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedBuilder));
            }
            catch
            {
                throw new DefaultException();
            }
        }
        [SlashCommand("Timeout", "Timeout user from server.")]
        [SlashRequirePermissions(Permissions.MuteMembers)]
        public async Task TimeoutSlashCommand(InteractionContext context, [Option("User", "User to ban")] DiscordUser user,
                                                                          [Option("Duration","Duration of timeout")] string duration = "1h",
                                                                          [Option("Reason", "Reason for ban")] string reason = "default")
        { 
            await context.DeferAsync();

            DiscordMember timeoutMember = (DiscordMember)user;

            if (timeoutMember.Hierarchy >= context.Member.Hierarchy)
                throw new HierarchyException();

            TimeSpan timeout;
            bool IsTime =TimeConverter.TryParse(duration, out timeout);
            if (!IsTime)
            {
                timeout = TimeSpan.FromHours(1);
            }

            DateTime timeoutDuration = DateTime.Now+timeout; //Method wants timeout until.

            try
            {
                await timeoutMember.TimeoutAsync(timeoutDuration, reason);

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Timeout user: " + timeoutMember.Username + "\n" + "Timeout Id: " + timeoutMember.Id,
                    Description = "Timeout Reason: " + reason + "\n" + "Timeout duration: " + timeout.ToString(),
                    Color = DiscordColor.Yellow
                };

                await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedBuilder));
            }
            catch
            {
                throw new DefaultException();
            }
        }

        [SlashCommand("Unmute", "Timeout user from server.")]
        [SlashRequirePermissions(Permissions.MuteMembers)]
        public async Task UnmuteSlashCommand(InteractionContext context, [Option("User", "User to ban")] DiscordUser user)
        {
            await context.DeferAsync();

            DiscordMember timeoutMember = (DiscordMember)user;

            if (timeoutMember.Hierarchy >= context.Member.Hierarchy)
                throw new HierarchyException();

            if(!timeoutMember.IsMuted)
                   throw new NotMutedException();
            try
            {
                await timeoutMember.TimeoutAsync(DateTime.Now);

                DiscordWebhookBuilder webhookBuilder = new DiscordWebhookBuilder()
                {
                    Content = "user is unmuted."
                };

                await context.EditResponseAsync(webhookBuilder);
            }
            catch
            {
                throw new DefaultException();
            }
        }
    }
}
