using System;

namespace ChatBot.Admin.WebApi.ViewModel
{
    public class DictionaryItemDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public object Data { get; set; }
    }
}
