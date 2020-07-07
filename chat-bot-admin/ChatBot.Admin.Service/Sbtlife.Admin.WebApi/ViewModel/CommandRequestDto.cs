using System;
using Newtonsoft.Json.Linq;

namespace ChatBot.Admin.WebApi.ViewModel
{
    public class CommandRequestDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public JObject Payload { get; set; }
    }
}
