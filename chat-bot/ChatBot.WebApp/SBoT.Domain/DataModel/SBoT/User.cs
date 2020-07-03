using System;
using System.Collections.Generic;
using System.Text;

namespace SBoT.Domain.DataModel.SBoT
{
    public class User
    {
        public Guid Id { get; set; }
        public string SigmaLogin { get; set; }
        public string Name { get; set; }
        public bool IsArchived { get; set; }
    }
}
