using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class KlientViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string KlientFIO { get; set; }
    }
}
