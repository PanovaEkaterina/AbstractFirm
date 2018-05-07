using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class KlientRequestsModel
    {
        [DataMember]
        public string KlientName { get; set; }

        [DataMember]
        public string DateCreate { get; set; }

        [DataMember]
        public string PackageName { get; set; }

        [DataMember]
        public int Count { get; set; }

        [DataMember]
        public decimal Sum { get; set; }

        [DataMember]
        public string Status { get; set; }
    }
}
