using RISIService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RISIQueryService
{
   public class RISIQueryClass:IRISIQueryContract
    {
       private List<IDataBase> GetAssemblies()
       {
           AssemblyFactory factory = new AssemblyFactory();
           factory.Load();
           return factory.Queryprocessors;
       }

        public void Subscribe(string uri)
        {
            throw new NotImplementedException();
        }

        public void PostQuery(QueryData data)
        {
            var mgr = GetAssemblies();
            List<byte[]> result = new List<byte[]>();
            mgr.ForEach(x => result.Add(x.ExecuteQuery(data)));
            var dt=Packer.Unpack(result[0]);
            DataTable tbl = new DataTable();
            tbl.ReadXml(dt);
        }

        List<QueryResult> IRISIQueryContract.PostQuery(QueryData data)
        {
            throw new NotImplementedException();
        }
    }
}
