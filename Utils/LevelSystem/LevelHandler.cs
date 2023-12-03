using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using FIrstDiscordBotC_.Exceptions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Utils.LevelSystem
{
    public static class LevelHandler
    {
        public static void StoreUserDetails(DUser user)
        {
            try
            {
                string path = "UserInfo.json";

                string json =File.ReadAllText(path);
                JObject jsonObject = JObject.Parse(json);

                List<DUser> members = jsonObject["members"].ToObject<List<DUser>>();
                members.Add(user);

                jsonObject["members"] = JArray.FromObject(members);
                File.WriteAllText(path, jsonObject.ToString());
            }
            catch (Exception ex)
            {
                throw new DefaultException(ex.Message);
            }

        }
        public static bool CheckUser(CommandContext context)
        {
            using (StreamReader reader = new StreamReader("UserInfo.json"))
            {
                string json = reader.ReadToEnd();
                LevelJsonFile userToSet = JsonConvert.DeserializeObject<LevelJsonFile>(json);

                foreach(DUser user in userToSet.members)
                {
                    if(user.Username == context.User.Username && user.GuildID == context.Guild.Id)
                    {
                        return true;
                    }
                }
            }
            return false;
        }
        public static DUser GetUser(CommandContext context) 
        {
            using (StreamReader reader = new StreamReader("UserInfo.json"))
            {
                string json = reader.ReadToEnd();
                LevelJsonFile userToSet = JsonConvert.DeserializeObject<LevelJsonFile>(json);

                foreach (DUser user in userToSet.members)
                {
                    if (user.Username == context.User.Username && user.GuildID == context.Guild.Id)
                    {
                        return new DUser()
                        {
                            Username = user.Username,
                            Level = user.Level,
                            XP = user.XP,
                            GuildID=user.GuildID,
                            AvatarUrl=context.User.AvatarUrl,
                        };
                    }
                }
            }
            return null;
        }
        public static async Task AddXp(this MessageCreateEventArgs args, ulong guildID)
        {
            bool IsLevelUp = false;
            try
            {
                string path = "UserInfo.json";

                var json =File.ReadAllText(path);
                JObject jsonObject = JObject.Parse(json);

                List<DUser> members = jsonObject["members"].ToObject<List<DUser>>();

                foreach (DUser user in members)
                {
                    if(user.Username == args.Author.Username && user.GuildID == guildID)
                    {
                        user.XP++;

                        if (user.XP >= 10)
                        {
                            user.Level++;
                            user.XP = 0;
                            IsLevelUp = true;
                        }

                        jsonObject["members"] = JArray.FromObject(members);
                        File.WriteAllText(path, jsonObject.ToString());

                        if (IsLevelUp)
                        {
                            DiscordMessageBuilder messageBuilder = new DiscordMessageBuilder()
                            {
                                Content = $"{args.Author.Mention}, you leveled up to {user.Level}."
                            };

                            await args.Channel.SendMessageAsync(messageBuilder);
                        }
                    }
                }
            }
            catch (Exception ex) 
            {
                throw new DefaultException(ex.Message);
            }
        }
    }
    internal class LevelJsonFile
    {
        public string userInfo { get; set; }
        public DUser[] members { get; set; }
    }
}
