using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Admin.WebApi.ViewModel.ChatBot
{
    public class ChatBotPermissionDto
    {
        public bool CanReadChatBot { get; set; }
        public bool CanEditChatBot { get; set; }
    }
}
