using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Order.Common.Models
{

    [DataContract]
    public class OrderRequest 
    {
        [DataMember(Name = "command")]
        public string Command { get; set; }

    }
}
