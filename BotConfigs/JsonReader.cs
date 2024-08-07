﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIrstDiscordBotC_.Config
{
    internal class JsonReader
    {
        public static string Token { get; set; }
        public static string Prefix { get; set; }
        public static string SearchApiKey { get; set; }
        public static string SearchCustomId { get; set; }
        public static string ChatGPTkey { get; set; }

        public static async Task ReadJsonAsync()
        {
            using (StreamReader reader = new StreamReader("config.json"))
            {
                string json = await reader.ReadToEndAsync();
                JsonStructure data = JsonConvert.DeserializeObject<JsonStructure>(json);    

                Prefix = data.Prefix;
                Token = data.Token;
                SearchApiKey = data.SearchApiKey;
                SearchCustomId = data.SearchCustomId;
                ChatGPTkey=data.ChatGPTkey;
            }
        }
    }

    internal sealed class JsonStructure
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
        public string SearchApiKey { get; set; }
        public string SearchCustomId { get; set; }
        public string ChatGPTkey { get; set; }
    }
}
