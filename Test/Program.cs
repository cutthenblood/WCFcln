using System.Collections.Concurrent;
using System.Data;
using System.IO;
using RISIQueryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RISIQueryService.ClientsInfo;
using RISIService;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("program start!!");
           // ClinetsInfoParser.Parse();
           var tbl=new DataTable();
            tbl.TableName = "custom";
            tbl.Columns.Add("Name");
            tbl.Columns.Add("Producer");
            tbl.Columns.Add("Trash");
            tbl.Columns.Add("Expires");
            tbl.Columns.Add("Quantity");
            tbl.Columns.Add("Trash2");
            tbl.Columns.Add("Division");

            for (int i = 0; i < 15; i++)
                tbl.Rows.Add("Name" + i.ToString(), "Producer" + i.ToString(), i.ToString(), DateTime.Now.ToShortDateString(),
                             i.ToString(),i.ToString(), "Division" + i.ToString());
            var stream=new MemoryStream();
            tbl.WriteXml(stream,XmlWriteMode.WriteSchema);
           var data= Packer.Pack(stream);
           var result = new ConcurrentBag<byte[]>();
            result.Add(data);
            result.ToQueryResult();

                Console.ReadKey();
        }
    }
}
