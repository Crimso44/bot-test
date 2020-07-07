namespace ChatBot.Admin.DomainStorage.Contexts.Entities.ChatBot
{
    public class Word : EntityInt
    {
        public string WordName { get; set; }

        public int? WordTypeId { get; set; }
    }
}
