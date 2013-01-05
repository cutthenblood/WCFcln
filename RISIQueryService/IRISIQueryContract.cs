using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace RISIQueryService
{
    [ServiceContract]
    public interface IRISIQueryContract
    {
       // [OperationContract]
        //void Subscribe(string uri);
        [OperationContract]
        List<QueryResult> PostQuery(QueryData data);
    }
}
