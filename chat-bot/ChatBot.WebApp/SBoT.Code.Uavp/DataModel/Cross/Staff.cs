using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace SBoT.Code.Uavp.DataModel.Cross
{
    [Table("Staff", Schema = "app")]
    public class Staff
    {
        public Guid Id { get; set; }
        public bool Active { get; set; }

        public string EmplNo { get; set; }
        public Guid DepartmentId { get; set; }
        public string SrcId { get; set; }
        public string SrcParentId { get; set; }
        public DateTime SrcCreatedDate { get; set; }
        public DateTime? VacOpenDate { get; set; }
        public string EmplName { get; set; }
        public string EmplPos { get; set; }
        public string EmplForm { get; set; }
        public string ContractType { get; set; }
        public decimal Rate { get; set; }
        public DateTime? EmplStartDate { get; set; }
        public DateTime? ApptStartDate { get; set; }
        public string City { get; set; }
        public string ExtLeave { get; set; }
        public string MaternityLeave { get; set; }
        public string SubEmlNo { get; set; }
        public string SubEmlName { get; set; }
        public Guid? VendorId { get; set; }
        public string Grade { get; set; }
        public string Specialization { get; set; }
    }
}
