using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class PackageBlankBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public int PackageId { get; set; }

        [DataMember]
        public int BlankId { get; set; }

        [DataMember]
        public int Count { get; set; }
    }
}
