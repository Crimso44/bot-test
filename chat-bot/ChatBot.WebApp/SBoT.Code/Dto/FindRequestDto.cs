
namespace SBoT.Code.Dto
{
    public class FindRequestDto
    {
        public string Query { get; set; }
        public int Skip { get; set; }
        public int Take { get; set; }
        public string Source { get; set; }
    }
}
