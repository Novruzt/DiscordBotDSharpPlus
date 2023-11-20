using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using FIrstDiscordBotC_.BotExtensions;
using FIrstDiscordBotC_.Commands;
using FIrstDiscordBotC_.Config;
using FIrstDiscordBotC_.Configurations;
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

            //Event registrations for Client
            Client.Ready += Client_Ready;
            Client.MessageCreated += MessageCreateHandler;
            


            CommandsNextConfiguration commandConfig= new CommandsNextConfiguration()
            {
                StringPrefixes=new string[] {jsonReader.Prefix},
                EnableMentionPrefix = true,
                EnableDms=true,
                EnableDefaultHelp = false
            };

            //Event registrations for Commands
            Commands = Client.UseCommandsNext(commandConfig);
            Commands.CommandErrored += CommandErrorHandler;


            //Register commands classes to Commands.
            Commands.RegisterCommands<TestCommands>();
            Commands.RegisterCommands<InteractionCommands>();

            await Client.ConnectAsync();
            await Task.Delay(-1); //To keep but running forever if project is running.

        }

        private static async Task CommandErrorHandler(CommandsNextExtension sender, CommandErrorEventArgs args)
        {
            GlobalExceptionHandler handler = new GlobalExceptionHandler(args);
             await handler.HandleErrors(); //Handling all errors and exceptions for commands.
        }

        private static async Task MessageCreateHandler(DiscordClient sender, MessageCreateEventArgs args)
        {
                await args.WelcomeMessage(); //Extension method to welcome user.
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask;
        }
    }
}
