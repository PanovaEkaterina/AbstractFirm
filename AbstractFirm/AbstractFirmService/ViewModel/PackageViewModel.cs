using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class PackageViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PackageName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<PackageBlankViewModel> PackageBlanks { get; set; }
    }
}
