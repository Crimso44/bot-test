namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class Pattern : EntityInt
    {
        public int CategoryId { get; set; }

        public string Phrase { get; set; }

        public int? WordCount { get; set; }

        public string Context { get; set; }
        public bool? OnlyContext { get; set; }
        public int? Mode { get; set; }
    }
}
