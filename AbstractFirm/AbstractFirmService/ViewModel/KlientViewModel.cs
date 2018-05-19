using System.Collections.Generic;
using System.Runtime.Serialization;

namespace AbstractFirmService.ViewModel
{
    [DataContract]
    public class KlientViewModel
    {
        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string KlientFIO { get; set; }

        [DataMember]
        public string Mail { get; set; }

        [DataMember]
        public List<MessageInfoViewModel> Messages { get; set; }
    }
}
