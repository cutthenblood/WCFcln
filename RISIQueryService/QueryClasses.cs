using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using NLog;

namespace RISIQueryService.QueryClasses
{
   public class QueryClassesRepo
    {
       private static readonly Lazy<QueryClassesRepo> lazy =
        new Lazy<QueryClassesRepo>(() => new QueryClassesRepo());

        private Dictionary<string, Assembly> _queryclasses;

        public Dictionary<string, Assembly> QueryClasses
        {
            get { return _queryclasses; }
            // set { _dbproxies = value; }
        }
        private QueryClassesRepo()
        {
            _queryclasses = new Dictionary<string, Assembly>();
        }

        public static QueryClassesRepo Instance
        {
            get
            {
                return lazy.Value;
            }
        }
    }

   public class QueryClassesManager
   {
       public QueryClassesRepo Repo;
       private static Logger logger = LogManager.GetCurrentClassLogger();
       public void LoadQueryClasses(bool reload = false)
       {


           Repo = QueryClassesRepo.Instance;
           if (reload)
               Repo.QueryClasses.Clear();
           var path = AppDomain.CurrentDomain.BaseDirectory + "\\QueryProcessors";
       
           if (!Directory.Exists(path))
               Directory.CreateDirectory(path);
           foreach (var dll in Directory.GetFiles(path, "*.dll"))
           {
               var name = Path.GetFileNameWithoutExtension(dll);
               if (!name.Contains("Query"))
               {
                 //  logger.Info("assembly," + name + " is not proxyclass!!");
                   continue;
               }
               if (Repo.QueryClasses.ContainsKey(name)) continue;
               try
               {
                   Assembly assembly = Assembly.LoadFrom(dll);
                   Repo.QueryClasses.Add(name, assembly);
               }
               catch (Exception exp)
               {
                    logger.Warn("assembly," + name + " is not loaded!!");
                   throw exp;
               }

           }
       }
    }

   public class QueryClassesFactory
   {
       private static Logger logger = LogManager.GetCurrentClassLogger();
       public IDataBase Create(KeyValuePair<string, Assembly> queryclass)
       {
           var T = queryclass.Value.GetTypes();
           try
           {
               var instance = (IDataBase)Activator.CreateInstance(T[0]);
               return instance;
           }
           catch (Exception exp)
           {
               logger.Warn("cannot create ", T[0].FullName);
               throw exp;
           }

       }
   }

}
