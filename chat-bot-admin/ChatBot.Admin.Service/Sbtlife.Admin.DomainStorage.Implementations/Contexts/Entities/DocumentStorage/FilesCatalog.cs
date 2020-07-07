using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ChatBot.Admin.DomainStorage.Contexts.Entities.DocumentStorage
{
    [Table("FilesCatalog", Schema = "dbo")]
    public class FilesCatalog
    {
        [Required]
        public Guid Id { get; set; }

        public Guid? ParentId { get; set; }

        [Required]
        public string Name { get; set; }

        public bool IsArchive { get; set; }

        public bool IsHidden { get; set; }

    }
}
