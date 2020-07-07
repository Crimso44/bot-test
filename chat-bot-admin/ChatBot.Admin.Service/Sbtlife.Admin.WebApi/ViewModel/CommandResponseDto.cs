
namespace ChatBot.Admin.WebApi.ViewModel
{
    public class CommandResponseDto
    {
        public bool Error { get; set; }
        public string Text { get; set; }
        public object Payload { get; set; }
    }
}
