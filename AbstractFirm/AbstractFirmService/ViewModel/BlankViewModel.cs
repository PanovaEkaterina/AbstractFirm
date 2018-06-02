using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class BlankViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string BlankName { get; set; }

    }
}
