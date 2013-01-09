using System.Collections.Concurrent;
using System.ServiceModel;
using System.Threading.Tasks;
using NLog;
using RISIQueryService.QueryClasses;
using RISIService;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RISIQueryService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall)]
    public class RISIQueryClass : IRISIQueryContract
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();
       
     
        List<QueryResult> IRISIQueryContract.PostQuery(QueryData data)
        {
            var result = new ConcurrentBag<byte[]>();
            //  new List<byte[]>();
            var queryClassesManager = new QueryClassesManager();
            var factory = new QueryClassesFactory();
            try
            {
                queryClassesManager.LoadQueryClasses();
            }
            catch (Exception exp)
            {
                logger.Warn("queryClassesManager, failed \r\n"+exp.ToString());
                throw new FaultException("queryClassesManager, failed"); 
            }
           
            var queryClasses = queryClassesManager.Repo.QueryClasses;

            Parallel.ForEach(queryClasses, currentClass =>
                {
                    try
                    {
                      var  instance = factory.Create(currentClass);
                        result.Add(instance.ExecuteQuery(data));
                    }
                    catch (FaultException faultEx)
                    {
                        logger.Warn("failed on executing query \r\n" + faultEx.ToString());
                    }
                    catch (Exception exp)
                    {

                        logger.Warn("failed creating IDataBase instance \r\n" + exp.ToString());
                    }
                   
                    
                });
            List<QueryResult> re=null;
            try
            {
                re = result.ToQueryResult();
            }
            catch (Exception exp)
            {
                logger.Warn("failed creating results list \r\n" + exp.ToString());
                throw new FaultException("failed creating results list");
            }
           
            return re;

        }

        //public byte[] CheckRowUpdate(DataRow row)
        //{
            
        //}
    }
}
