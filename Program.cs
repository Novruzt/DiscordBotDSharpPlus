﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.CommandsNext.Exceptions;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Enums;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using FIrstDiscordBotC_.BotExtensions;
using FIrstDiscordBotC_.Commands;
using FIrstDiscordBotC_.Commands.PrefixCommands;
using FIrstDiscordBotC_.Commands.SlashCommands;
using FIrstDiscordBotC_.Config;
using FIrstDiscordBotC_.Configurations;
using FIrstDiscordBotC_.DATA.Configs;
using FIrstDiscordBotC_.Utils.DatabaseUtils;
using FIrstDiscordBotC_.Utils.LevelSystem;
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
            await JsonReader.ReadJsonAsync();

            DiscordConfiguration discordConfiguration = new DiscordConfiguration()
            {
                Intents = DiscordIntents.All,
                Token = JsonReader.Token,
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
            Client.ComponentInteractionCreated += ButtonPressHandler;
           // Client.ClientErrored += ClientErrorHandler;

            CommandsNextConfiguration commandConfig= new CommandsNextConfiguration()
            {
                StringPrefixes=new string[] {JsonReader.Prefix},
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
            Commands.RegisterCommands<ProfileCommands>();
            Commands.RegisterCommands<ComponentCommands>();

            //Register slash commands
            SlashCommandsExtension slashCommandsConfig = Client.UseSlashCommands(); 
            slashCommandsConfig.RegisterCommands<TestSlashCommands>(784733575936737301); //GUILD(Server) Id.
            slashCommandsConfig.RegisterCommands<ModerationSlashCommands>(784733575936737301);  //Guild server id
            slashCommandsConfig.RegisterCommands<ApiSlashCommands>(784733575936737301);  //Guild server id  
            slashCommandsConfig.SlashCommandErrored += SlashCommandError;

            await Client.ConnectAsync();
            await Task.Delay(-1); //To keep bot running forever if project is running.

            //ContextManager.Dispose();
        }

        //private static async Task ClientErrorHandler(DiscordClient sender, ClientErrorEventArgs args)
        //{
        //    // throw new NotImplementedException();
        //}

        private static async Task SlashCommandError(SlashCommandsExtension sender, SlashCommandErrorEventArgs args)
        {
            await args.HandleErrors();  //Handling all error and exceptions for slash commands.
        }

        private static async Task ButtonPressHandler(DiscordClient sender, ComponentInteractionCreateEventArgs args)
        {
            await args.HandleButtons(Client); //Handling all buttons' interactions.
        }

        private static async Task CommandErrorHandler(CommandsNextExtension sender, CommandErrorEventArgs args)
        {
            await args.HandleErrors(); //Handling all errors and exceptions for commands.
        }

        private static async Task MessageCreateHandler(DiscordClient sender, MessageCreateEventArgs args)
        {
            await args.WelcomeMessage(); //Extension method to welcome user.
         //   await args.AddXp(args.Guild.Id); //Level System
            await DbLevelHandler.AddXpAsync(args);
        }

        private static Task Client_Ready(DiscordClient sender, ReadyEventArgs args)
        {
            return Task.CompletedTask; 
        }
    }
}
