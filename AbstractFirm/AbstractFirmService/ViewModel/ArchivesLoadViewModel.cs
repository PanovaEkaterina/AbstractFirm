using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class ArchivesLoadViewModel
    {
        [DataMember]
        public string ArchiveName { get; set; }

        [DataMember]
        public int TotalCount { get; set; }

        [DataMember]
        public IEnumerable<Tuple<string, int>> Blanks { get; set; }
    }
}
