using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using RISIService;

namespace RISIQueryService
{
    public enum QRColumnNames//later use reflection
    {
         Name,Producer,Dosage,Fas,Expires,Quantity,Price,Division,Address,Raiting
    }

    public class QRTemplate
    {
        private Dictionary<int,Action<QueryResult, string>> template;

        public Dictionary<int, Action<QueryResult, string>> Mapping
        {
            get { return template; }
        }
        
        public QRTemplate()
        {
            template = new Dictionary<int, Action<QueryResult, string>>();
        }

        public void MapColumns(DataColumnCollection columns)
        {

            QRColumnNames obj;

            foreach(var column in columns.Cast<DataColumn>()
                                           .Select((value, index) => new { index = index, value = value })
                                           .Where(x => QRColumnNames.TryParse(x.value.ColumnName, true, out obj))
                                           .Select(x => new {cnmae=x.value.ColumnName,idx=x.index}))
                template.Add(column.idx, CreateAct(column.cnmae));

        }

        private Action<QueryResult, string> CreateAct(string str)
        {
            switch (str)
            {
                case "Name":
                   return (queryres, dat) => queryres.Name = dat; break;
                case "Producer":
                    return(queryres, dat) => queryres.Producer = dat; break;
                case "Dosage":
                    return(queryres, dat) => queryres.Dosage = dat; break;
                case "Fas":
                    return(queryres, dat) => queryres.Fas = Convert.ToInt32(dat); break;
                case "Expires":
                   return(queryres, dat) => queryres.Expires = Convert.ToDateTime(dat); break;
                case "Quantity":
                    return(queryres, dat) => queryres.Quantity = Convert.ToDouble(dat); break;
                case "Price":
                    return(queryres, dat) => queryres.Price = Convert.ToDouble(dat); break;
                case "Division":
                    return(queryres, dat) => queryres.Division = dat; break;
                case "Address":
                    return(queryres, dat) => queryres.Address = dat; break;
                case "Raiting":
                    return(queryres, dat) => queryres.Raiting = Convert.ToDouble(dat); break;
            }
            return null;
        }
    }

  
    public static class QueryResultConverter
    {
        public static List<QueryResult> ToQueryResult(this ConcurrentBag<byte[]> data)
        {
            var output=new List<QueryResult>();

            foreach(byte[] chunk in data)
                {
                    var tbl=new DataTable();
                    tbl.ReadXml(Packer.Unpack(chunk));
                    if (tbl.Rows.Count == 0) continue;
                    var template=new QRTemplate();
                    template.MapColumns(tbl.Columns);
                    foreach (DataRow row in tbl.Rows)
                    {
                        var qresult=new QueryResult();
                        foreach (var m in template.Mapping)
                            m.Value(qresult, row[m.Key].ToString());
                        output.Add(qresult);
                    }

                }
            return output;
        }
    }
}
