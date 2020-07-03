using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace SBoT.Domain.DataModel.SBoT
{
    [Table("BirthdayException")]
    public class BirthdayException
    {
        public Guid EmployeeId { get; set; }
    }
}
