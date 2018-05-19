using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class ArchiveBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ArchiveName { get; set; }
    }
}
