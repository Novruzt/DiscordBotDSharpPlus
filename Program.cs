using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using FIrstDiscordBotC_.Commands;
using FIrstDiscordBotC_.Config;
using System;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_
{
    internal class Program
    {
        public static DiscordClient Client;
        public static CommandsNextExtension Commands;
        static async Task Main(string[] args)
        {
            JsonReader jsonReader = new JsonReader();  
            await jsonReader.ReadJsonAsync();

            DiscordConfiguration discordConfiguration = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = jsonReader.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true
            };

            Client = new DiscordClient(discordConfiguration);

            Client.UseInteractivity(new InteractivityConfiguration
            {
                PollBehaviour = PollBehaviour.KeepEmojis,
                Timeout = TimeSpan.FromMinutes(1)  //Timeout for Commands Interactivity.
            });

            Client.Ready += Client_Ready;

            CommandsNextConfiguration commandConfig= new CommandsNextConfiguration()
            {
                StringPrefixes=new string[] {jsonReader.Prefix},
                EnableMentionPrefix = true,
                EnableDms=true,
                EnableDefaultHelp = false
            };

            Commands = Client.UseCommandsNext(commandConfig);

            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<InteractionCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1); //To keep but running forever if project is running.

        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
