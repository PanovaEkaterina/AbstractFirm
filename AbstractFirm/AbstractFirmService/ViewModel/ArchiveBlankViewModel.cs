using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class ArchiveBlankViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ArchiveId { get; set; }

        [DataMember]
        public int BlankId { get; set; }

        [DataMember]
        public string BlankName { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
