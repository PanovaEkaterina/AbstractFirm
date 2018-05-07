using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractFirmModel
{
    public class Archive
    {
        public int Id { get; set; }

        [Required]
        public string ArchiveName { get; set; }

        [ForeignKey("ArchiveId")]
        public virtual List<ArchiveBlank> ArchiveBlanks { get; set; }
    }
}
