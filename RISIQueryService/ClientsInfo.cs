using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Xml.Linq;

namespace RISIQueryService.ClientsInfo
{
    public class ClientsInfo
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Id { get; set; }
        public class AssemblyInfo
        {
            public string TypeName { get; set; }
            public string AssemblyName { get; set; }
            public string AssemblyPath { get; set; }

        }
        public List<AssemblyInfo> ClientAssemblies { get; set; }

        
        public Dictionary<string, string> EndpoinURIs { get; set; }
        public ClientsInfo()
        {
            EndpoinURIs = new Dictionary<string, string>();
            ClientAssemblies = new List<AssemblyInfo>();
        }
    }
    
    public sealed class ClientsInfoRepo
    {
        private static readonly Lazy<ClientsInfoRepo> lazy =
            new Lazy<ClientsInfoRepo>(() => new ClientsInfoRepo());
        public ClientsInfoRepo() { _clients = new List<ClientsInfo>(); }
        List<ClientsInfo> _clients;

        public List<ClientsInfo> Clients
        {
            get { return _clients; }
        }

        public static ClientsInfoRepo Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }

    public class ClinetsInfoParser
    {
        public static void Parse()
        {
            var path = AppDomain.CurrentDomain.BaseDirectory + "ClientsInfo.xml";
            XDocument doc = XDocument.Load(path);
            ClientsInfoRepo repo=ClientsInfoRepo.Instance;
            foreach (XElement el in doc.Root.Elements())
            {
                if (el.Name == "Clients")
                    foreach (var clients in el.Elements())
                    if (clients.Name == "Client")
                        {
                            var client=new ClientsInfo();
                            foreach(var clientattr in clients.Attributes())
                            {
                                if(clientattr.Name=="name")
                                   client.Name=clientattr.Value;
                                if(clientattr.Name=="description")
                                    client.Description=clientattr.Value;
                                if (clientattr.Name == "id")
                                    client.Id = clientattr.Value;
                            }
                            foreach(var cli in clients.Elements())
                            {
                                if(cli.Name=="Assemblies")
                                    foreach (var clientassembly in cli.Elements())
                                    {
                                        var assemb = new ClientsInfo.AssemblyInfo();
                                        if (clientassembly.Name == "Assembly")
                                            foreach (var assemblyattr in clientassembly.Attributes())
                                            {
                                                if (assemblyattr.Name == "name")
                                                    assemb.AssemblyName = assemblyattr.Value;
                                                if (assemblyattr.Name == "type")
                                                    assemb.TypeName = assemblyattr.Value;
                                                if (assemblyattr.Name == "path")
                                                    assemb.AssemblyPath = assemblyattr.Value;
                                            }
                                        client.ClientAssemblies.Add(assemb);
                                    }
                                 if(cli.Name=="EndPointURIs")
                                     foreach(var clientendpoints in cli.Elements())
                                         if(clientendpoints.Name=="EndPoint")
                                             foreach(var edpattr in clientendpoints.Attributes())
                                                if(edpattr.Name=="name")
                                                    client.EndpoinURIs.Add(edpattr.Value,clientendpoints.Value);
                            }
                            repo.Clients.Add(client);
                        }
            }
        
        }
    }

    public class ClientsInfoWatcher
    {
        internal string path;
        private Timer tm;
        private DateTime dt;
        public void BeginWatching()
        {
            path = AppDomain.CurrentDomain.BaseDirectory + "ClientsInfo.xml";
            var fi = new FileInfo(path);
            dt = fi.LastWriteTime;
            tm = new Timer(TimerCallback, null, 0, 5000);
            
        }

        private void TimerCallback(object state)
        {

            //var lwt = (DateTime)state;
            var fi = new FileInfo(path);
            Console.WriteLine("Timer started!  " + dt.ToString("dd/MM/yyyy hh:mm:ss"));
            if (dt < fi.LastWriteTime)
            {
                Console.WriteLine("****  info changed   ************");
                Console.WriteLine(fi.LastWriteTime.ToString("dd/MM/yyyy hh:mm:ss"));
                ClinetsInfoParser.Parse();
                dt = fi.LastWriteTime;
            }
        }
    }

    //public class AssemblyConfigParser
    //{
    //    public static Dictionary<string,string> EndpointURI(string path,string assemblyname,string typename)
    //    {
    //        if (!File.Exists(path+"\\"+assemblyname + ".config"))
    //            return null;
    //        Dictionary<string, string> endpointURIs =new Dictionary<string, string>();
    //        List<ClientsInfo> clientInfo=new List<ClientsInfo>();

    //        XDocument doc = XDocument.Load(path + "\\" + assemblyname + ".config");
    //        foreach (XElement el in doc.Root.Elements())
    //        {
    //            if (el.Name == assemblyname)
    //                foreach(var assemblySection in el.Elements())
    //                    if(assemblySection.Name==typename)
    //                        foreach(XElement clientSection in assemblySection.Elements())
    //                            if (clientSection.Name == "Clinet")
    //                            {
    //                              //  foreach(XAttribute attr in clientSection.Attributes())
    //                               // if(attr.Name=="name"
    //                            }
                            
    //                    //    foreach (XElement typeSection in assemblySection.Elements())
    //                    //        {
    //                    //if(typeSection.Name=="endpointURI")
    //                    //    foreach (XAttribute attr in typeSection.Attributes())
    //                    //    {
    //                    //        if(attr.Name=="bindingName")
    //                    //            endpointURIs.Add(attr.Value,typeSection.Value);
    //                    //    }
    //                    //        }

    //        }
    //        return endpointURIs;


    //    }
    //    //public System.ServiceModel.Security.UserNamePasswordClientCredential Credentials
    //}
}
