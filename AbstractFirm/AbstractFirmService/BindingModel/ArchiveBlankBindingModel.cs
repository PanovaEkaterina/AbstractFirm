using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class ArchiveBlankBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int ArchiveId { get; set; }

        [DataMember]
        public int BlankId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
