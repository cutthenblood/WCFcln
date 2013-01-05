using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace RISIQueryService
{
    [DataContract]
   public class QueryData
    {
        [DataMember(IsRequired=true)]
        public string Guid_ES { get; set; }
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember]
        public string Barcode { get; set; }
        [DataMember(IsRequired = true)]
        public string Mnn_rus { get; set; }
        [DataMember]
        public string Mnn_lat { get; set; }
        [DataMember(IsRequired = true)]
        public string Tn_rus { get; set; }
        [DataMember]
        public string Tn_lat { get; set; }
        [DataMember]
        public string Dosage { get; set; }
        [DataMember(IsRequired = true)]
        public bool Jnvls { get; set; }
        [DataMember]
        public int Fas { get; set; }
        [DataMember]
        public string Producer { get; set; }
        [DataMember]
        public string Id_goods_global { get; set; }

            //[DataMember]
            //public string Name { get; set; }
            //[DataMember]
            //public string MNN { get; set; }
            //[DataMember]
            //public string Producer { get; set; }
            //[DataMember]
            //public string FarmGroup { get; set; }
            //[DataMember]
            //public string Lform { get; set; }
            //[DataMember]
            //public string Id_goods { get; set; }
            //[DataMember]
            //public Dictionary<string, string> values { get; set; }

    }

    [DataContract]
    public class QueryResult
    {
        [DataMember(IsRequired = true)]
        public string Name { get; set; }
        [DataMember]
        public string Producer { get; set; }
        [DataMember]
        public string Dosage { get; set; }
        [DataMember]
        public int Fas { get; set; }
        [DataMember]
        public DateTime Expires { get; set; }
        [DataMember(IsRequired = true)]
        public double Quantity { get; set; }
        [DataMember]
        public double Price { get; set; }
        [DataMember(IsRequired = true)]
        public string Division { get; set; }
        [DataMember(IsRequired = true)]
        public string Address { get; set; }
        [DataMember]
        public double Raiting { get; set; }
    }

}
