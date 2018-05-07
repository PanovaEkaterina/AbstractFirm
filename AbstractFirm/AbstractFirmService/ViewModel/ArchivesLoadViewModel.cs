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
        public IEnumerable<ArchivesBlankLoadViewModel> Blanks { get; set; }

        [DataContract]
        public class ArchivesBlankLoadViewModel
        {
            [DataMember]
            public string BlankName { get; set; }

            [DataMember]
            public int Count { get; set; }
        }
    }
}
