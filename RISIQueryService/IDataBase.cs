using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RISIQueryService
{
    public interface IDataBase
    {
        string PrepareQuery(QueryData data);
        byte[] ExecuteQuery(string query);
        byte[] ExecuteQuery();
        byte[] ExecuteQuery(QueryData data);
        void SetAssemblyPath(string path);
        void SetAssemblyName(string name);
        void SetSecurityMode(SecutriyMode mode);
    }
}
