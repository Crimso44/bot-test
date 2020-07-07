using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.DocumentStorage
{
    [Table("Files", Schema = "dbo")]
    public class Files
    {
        [Required]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Extension { get; set; }

        [Required]
        public Guid CatalogId { get; set; }

        [Required]
        public DateTime CreateDate { get; set; }

        [Required]
        public Guid CreateEmployeeId { get; set; }

        public bool IsArchive { get; set; }

        public DateTime? ArchiveDate { get; set; }

        [Required]
        public byte[] Data { get; set; }
    }
}
