using System;

namespace AbstractFirmModel
{
    public class MessageInfo
    {
        public int Id { get; set; }

        public string MessageId { get; set; }

        public string FromMailAddress { get; set; }

        public string Subject { get; set; }

        public string Body { get; set; }

        public DateTime DateDelivery { get; set; }

        public int? KlientId { get; set; }

        public virtual Klient Klient { get; set; }
    }
}
