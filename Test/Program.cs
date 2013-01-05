using RISIQueryService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RISIQueryService.ClientsInfo;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("program start!!");
           // ClinetsInfoParser.Parse();
           ClientsInfoWatcher ciw=new ClientsInfoWatcher();
            ciw.BeginWatching();
            Console.ReadKey();
        }
    }
}
