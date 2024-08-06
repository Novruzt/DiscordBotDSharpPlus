using DSharpPlus.Entities;
using DSharpPlus.SlashCommands;
using FIrstDiscordBotC_.Config;
using Google.Apis.CustomSearchAPI.v1;
using Google.Apis.CustomSearchAPI.v1.Data;
using Google.Apis.Services;
using OpenAI_API;
using OpenAI_API.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Commands.SlashCommands
{
    internal class ApiSlashCommands: ApplicationCommandModule
    {
        [SlashCommand("Search", "Search image on google.")]
        public async Task SearchSlashCommand(InteractionContext context, [Option("Search", "image to search on google")] string search)
        {
            await context.DeferAsync();

            await JsonReader.ReadJsonAsync();

            CustomSearchAPIService searchService = new CustomSearchAPIService(new BaseClientService.Initializer()
            {
                ApiKey = JsonReader.SearchApiKey,
                ApplicationName = "FirstEngine"
            });

            CseResource.ListRequest listRequest = searchService.Cse.List();
            listRequest.Cx = JsonReader.SearchCustomId;
            listRequest.SearchType = CseResource.ListRequest.SearchTypeEnum.Image;
            listRequest.Q = search;

            Search searchRequest = await listRequest.ExecuteAsync();
            IList<Result> searchResults = searchRequest.Items;

            if (searchResults == null || searchResults.Count == 0)
                await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent("No result found"));
            else
            {
                Result result = searchResults.First();

                DiscordEmbedBuilder embedBuilder = new DiscordEmbedBuilder()
                {
                    Title = "Result for: " + search,
                    ImageUrl = result.Link,
                    Color=DiscordColor.Aquamarine
                };

                await context.EditResponseAsync(new DiscordWebhookBuilder().AddEmbed(embedBuilder));
            }
        }
        [SlashCommand("ChatGPT", "Ask a question to ChatGPT")]
        public async Task ChatGPTSlashCommand(InteractionContext context, [Option("Question", "Question for ChatGPT")] string question)
        {
            await context.DeferAsync();
            
            await JsonReader.ReadJsonAsync();

            OpenAIAPI api = new OpenAIAPI(JsonReader.ChatGPTkey);
            Conversation chat = api.Chat.CreateConversation();
            chat.AppendSystemMessage("Ask a question");

            chat.AppendUserInput(question);
            string response = await chat.GetResponseFromChatbotAsync();

            await context.EditResponseAsync(new DiscordWebhookBuilder().WithContent($"Response for: \n `{question}`: \n{response}"));
        }
    }
}
