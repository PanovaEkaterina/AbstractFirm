using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class PackageBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string PackageName { get; set; }

        [DataMember]
        public decimal Price { get; set; }

        [DataMember]
        public List<PackageBlankBindingModel> PackageBlanks { get; set; }
    }
}
