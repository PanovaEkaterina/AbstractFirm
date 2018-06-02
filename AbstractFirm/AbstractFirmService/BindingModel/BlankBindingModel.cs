using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class BlankBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string BlankName { get; set; }
    }
}
