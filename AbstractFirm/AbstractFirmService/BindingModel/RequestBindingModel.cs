using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class RequestBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int KlientId { get; set; }

        [DataMember]
        public int PackageId { get; set; }

        [DataMember]
        public int? LawyerId { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }
    }
}
