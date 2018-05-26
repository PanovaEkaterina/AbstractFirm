using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractFirmModel
{
    public class Package
    {
        public int Id { get; set; }

        [Required]
        public string PackageName { get; set; }

        [Required]
        public decimal Price { get; set; }

        [ForeignKey("PackageId")]
        public virtual List<Request> Requests { get; set; }

        [ForeignKey("PackageId")]
        public virtual List<PackageBlank> PackageBlanks { get; set; }
    }
}
