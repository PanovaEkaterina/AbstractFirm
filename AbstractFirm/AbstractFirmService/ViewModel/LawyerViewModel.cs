using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class LawyerViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string LawyerFIO { get; set; }
    }
}
