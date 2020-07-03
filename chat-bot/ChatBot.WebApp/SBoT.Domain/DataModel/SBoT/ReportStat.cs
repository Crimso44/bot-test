
namespace SBoT.Domain.DataModel.SBoT
{
    public class ReportStat
    {
        public int Id { get; set; }
        public string Question { get; set; }
        public double? IsAnswered { get; set; }
        public double? Like { get; set; }
        public int Count { get; set; }
    }
}
