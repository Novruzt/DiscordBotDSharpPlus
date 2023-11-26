using DSharpPlus;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.Attributes;
using FIrstDiscordBotC_.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Commands.SlashCommands
{
    public class ModerationSlashCommands:ApplicationCommandModule
    {
        [SlashCommand("Ban", "Bans user from server.")]
        [SlashRequirePermissions(Permissions.BanMembers)]
        public async Task BanSlashCommand(InteractionContext context, [Option("User", "User to ban")] DiscordUser user,
                                                                      [Option("Reason", "Reason for ban")] string reason = "default")
        {

            DiscordMember banMember = (DiscordMember)user;

            if (banMember.Hierarchy >= context.Member.Hierarchy)
                throw new HierarchyException();

                await context.DeferAsync();
            
                await context.Guild.BanMemberAsync(banMember, 0, reason);

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Banned user: " + banMember.Username +"\n" +"Banned Id: "+banMember.Id,
                    Description = "Ban Reason: " + reason,
                    Color = DiscordColor.Red
                };

            await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedBuilder));
        }
    }
}
