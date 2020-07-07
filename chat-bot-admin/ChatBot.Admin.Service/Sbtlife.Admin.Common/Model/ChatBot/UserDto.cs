using System;
using System.Collections.Generic;
using System.Text;

namespace ChatBot.Admin.Common.Model.ChatBot
{
    public class UserDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SigmaLogin { get; set; }

        public string SigmaEmail { get; set; }
    }

    public class UserDtoSerializable
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public string SigmaLogin { get; set; }

        public string SigmaEmail { get; set; }
    }
}
