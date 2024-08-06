using DSharpPlus.EventArgs;
using FIrstDiscordBotC_.DATA.Models.Common;
using FIrstDiscordBotC_.DATA.Models;
using System;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using FIrstDiscordBotC_.DATA.Configs;
using FIrstDiscordBotC_.DATA;

namespace FIrstDiscordBotC_.Utils.DatabaseUtils
{
    internal class DbLevelHandler
    {
        // Store user details in the database
        public static async Task StoreUserDetailsAsync(User user, Server server = null)
        {
            using (var context = new DataContext())
            {
                try
                {
                    // Ensure the server exists
                    var dbServer = await context.Servers
                        .SingleOrDefaultAsync(s => s.GuildID == user.GuildId);

                    if (dbServer == null)
                    {
                        dbServer = new Server
                        {
                            GuildID = server.GuildID,
                            Name = server.Name // Use default value if Name is null
                        };
                        context.Servers.Add(dbServer);
                        await context.SaveChangesAsync();
                    }

                    // Add the user if not exists
                    if (!await context.Users.AnyAsync(u => u.Username == user.Username && u.GuildId == user.GuildId))
                    {
                        user.ServerId = dbServer.Id;
                        context.Users.Add(user);
                        await context.SaveChangesAsync();
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it)
                    throw new Exception("Failed to store user details.", ex);
                }
            }
        }

        // Retrieve a user by server ID and username
        public static async Task<User> GetUserAsync(string guildId, string username)
        {
            using (var context = new DataContext())
            {
                try
                {
                    return await context.Users
                        .Include(u => u.Server)
                        .SingleOrDefaultAsync(u => u.Username == username && u.GuildId == guildId);
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it)
                    throw new Exception("Failed to get user details.", ex);
                }
            }
        }

        // Add XP to a user and handle leveling up
        public static async Task AddXpAsync(MessageCreateEventArgs args)
        {

            if (args.Author.IsBot)
                return;

            using (var context = new DataContext())
            {
                try
                {
                    var guildId = args.Guild.Id.ToString();
                    User user = await context.Users
                        .Include(u => u.Server)
                        .SingleOrDefaultAsync(u => u.Username == args.Author.Username && u.GuildId == guildId);

                    if (user == null)
                    {
                        user = new User
                        {
                            Username = args.Author.Username,
                            AvatarUrl = args.Author.AvatarUrl,
                            XP = 0,
                            Level = 1,
                            GuildId = guildId
                        };
                        await StoreUserDetailsAsync(user, new Server()
                        {
                            GuildID = guildId,
                            Name = args.Guild.Name,
                        });
                    }
                    else
                    {
                        user.XP++;
                        if (user.XP >= 10)
                        {
                            user.Level++;
                            user.XP = 0;
                            await args.Channel.SendMessageAsync($"{args.Author.Mention}, you leveled up to {user.Level}!");
                        }

                        context.Entry(user).State = EntityState.Modified; // Mark the user entity as modified
                        await context.SaveChangesAsync(); // Save changes to the database
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception (e.g., log it)
                    throw new Exception("Failed to add XP.", ex);
                }
            }
        }

    }
}
