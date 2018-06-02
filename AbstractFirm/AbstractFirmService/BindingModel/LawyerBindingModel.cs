using System.Runtime.Serialization;

namespace AbstractFirmService.BindingModel
{
    [DataContract]
    public class LawyerBindingModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string LawyerFIO { get; set; }
    }
}
