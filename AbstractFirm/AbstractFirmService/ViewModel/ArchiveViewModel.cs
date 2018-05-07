using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class ArchiveViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string ArchiveName { get; set; }

        [DataMember]
        public List<ArchiveBlankViewModel> ArchiveBlanks { get; set; }
    }
}
