using System;
using System.Collections.Generic;
using System.Linq;
namespace AbstractFirmService.BindingModel
{
    public class RequestBindingModel
    {
        public int Id { get; set; }

        public int KlientId { get; set; }

        public int PackageId { get; set; }

        public int? LawyerId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }
    }
}
