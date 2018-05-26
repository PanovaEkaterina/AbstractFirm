using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractFirmModel
{
    public class Blank
    {
        public int Id { get; set; }

        [Required]
        public string BlankName { get; set; }

        [ForeignKey("BlankId")]
        public virtual List<PackageBlank> PackageBlanks { get; set; }

        [ForeignKey("BlankId")]
        public virtual List<ArchiveBlank> SArchiveBlanks { get; set; }
    }
}
