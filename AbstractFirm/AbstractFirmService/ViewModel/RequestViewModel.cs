using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class RequestViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int KlientId { get; set; }

        [DataMember]
        public string KlientFIO { get; set; }

        [DataMember]
        public int PackageId { get; set; }

        [DataMember]
        public string PackageName { get; set; }

        [DataMember]
        public int? LawyerId { get; set; }

        [DataMember]
        public string LawyerName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string DateLawyer { get; set; }
    }
}
