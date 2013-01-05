using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RISIQueryService;
using RISIQueryService.RISIService;
using System.ServiceModel.Channels;
using System.Security.Cryptography.X509Certificates;

namespace EFQuery
{
   
    public class EFQueryClass:IDataBase
    {
        private string _assemblyPath;
        private string _assemblyName;
        private SecutriyMode _smode;
        public string PrepareQuery(QueryData data)
        {
            string query = "select top 10 * from goods where 1=1 ";
            if (data.Name != "")
                query += "and name like '%" + data.Name + "%'";

            return query;
           // throw new NotImplementedException();
        }

        public byte[] ExecuteQuery(string query)
        {
            throw new NotImplementedException();
        }

        public byte[] ExecuteQuery(QueryData data)
        {
           var query=PrepareQuery(data);
           var endpoints = AssemblyConfigParser.EndpointURI(_assemblyPath,_assemblyName,"EFQueryClass");
           if (endpoints.Count() == 0)
               throw new Exception("EFQuery.EFQueryClass:ExecuteQuery(QueryData data), no configuration provided for this type");
           RISISqlContractClient client;

           if (_smode == SecutriyMode.basic)
           {
               var bnd=endpoints.Where(x => x.Key.Contains("basic"));
               if (bnd.Count() > 0)
               {
                   client =
                         new RISISqlContractClient(bnd.First().Key,
                            new System.ServiceModel.EndpointAddress(bnd.First().Value));

                   client.ClientCredentials.ServiceCertificate.SetDefaultCertificate(StoreLocation.LocalMachine,
                                                               StoreName.My,
                                                               X509FindType.FindBySerialNumber,
                                                               "01");
                   client.ClientCredentials.ClientCertificate.SetCertificate(
                                                                       StoreLocation.LocalMachine,
                                                                       StoreName.My,
                                                                       X509FindType.FindBySerialNumber,
                                                                       "01");



                   var result = client.ExecuteSQLQuery(query);
                   return result;
               }
           }
           return null;

          
        }


        public byte[] ExecuteQuery()
        {
            throw new NotImplementedException();
        }

        public void SetAssemblyPath(string path)
        {
            _assemblyPath = path;
        }


        public void SetAssemblyName(string name)
        {
            _assemblyName = name;
        }


        public void SetSecurityMode(SecutriyMode mode)
        {
            _smode = mode;
        }
    }
}
