using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class KlientBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string KlientFIO { get; set; }
    }
}
