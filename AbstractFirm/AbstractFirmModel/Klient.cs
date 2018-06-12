using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AbstractFirmModel
{
    public class Klient
    {
        public int Id { get; set; }

        [Required]
        public string KlientFIO { get; set; }

        public string Mail { get; set; }

        [ForeignKey("KlientId")]
        public virtual List<Request> Requests { get; set; }
    }
}