using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractFirmModel
{
    public class Lawyer
    {
        public int Id { get; set; }

        [Required]
        public string LawyerFIO { get; set; }

        [ForeignKey("LawyerId")]
        public virtual List<Request> Requests { get; set; }
    }
}
