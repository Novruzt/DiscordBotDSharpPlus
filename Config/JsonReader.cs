using Newtonsoft.Json;
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
        public string Token { get; set; }
        public string Prefix { get; set; }

        public async Task ReadJsonAsync()
        {
            using (StreamReader reader = new StreamReader("config.json"))
            {
                string json = await reader.ReadToEndAsync();
                JsonStructure data = JsonConvert.DeserializeObject<JsonStructure>(json);    

                this.Prefix = data.Prefix;
                this.Token = data.Token;
            }
        }
    }

    internal sealed class JsonStructure
    {
        public string Token { get; set; }
        public string Prefix { get; set; }
    }
}
