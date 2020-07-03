using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Code.Dto
{
    public class WeightDto
    {
        public int Id { get; set; }
        public double Weight { get; set; }
        public string Word { get; set; }
        public List<string> Words { get; set; }
    }
}
