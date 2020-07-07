
namespace ChatBot.Admin.WebApi.Requests
{
    public class GetCollectionFilter
    {
        public string Search { get; set; }
        public int? Skip { get; set; }
        public int? Take { get; set; }
        public bool? IsArchived { get; set; }
    }
}
