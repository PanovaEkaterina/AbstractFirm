using System;

namespace AbstractFirmModel
{
    public class Request
    {
        public int Id { get; set; }

        public int KlientId { get; set; }

        public int PackageId { get; set; }

        public int? LawyerId { get; set; }

        public int Count { get; set; }

        public decimal Sum { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime DateCreate { get; set; }

        public DateTime? DateImplement { get; set; }
    }
}
