using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace RISIQueryService
{

    


    public class AssemblyFactory
    {
        private List<IDataBase> _queryprocessors;

        public List<IDataBase> Queryprocessors
        {
            get { return _queryprocessors; }
           
        }
        private HashSet<Assembly> _loaded;
        public AssemblyFactory()
        {
            _queryprocessors = new List<IDataBase>();
            _loaded = new HashSet<Assembly>();
        }
        public void Load()
        {
            var path=AppDomain.CurrentDomain.BaseDirectory + "\\QueryProcessors";
            if (!Directory.Exists(path)) return;
            foreach (var dll in Directory.GetFiles(path,"*.dll"))
            {
                try
                {
                    var assemblyName = Path.GetFileNameWithoutExtension(dll);
                    Assembly assembly = Assembly.LoadFrom(dll);
                    if (_loaded.Where(x => x.FullName == assembly.FullName).Count() > 0)
                        continue;
                    _loaded.Add(assembly);
                    foreach (Type T in assembly.GetTypes())
                    {
                        try
                        {
                            IDataBase instance = (IDataBase)Activator.CreateInstance(T);
                            instance.SetAssemblyPath(Path.GetDirectoryName(dll));
                            instance.SetAssemblyName(assemblyName);
                            instance.SetSecurityMode(SecutriyMode.basic);
                            if (!File.Exists(Path.GetDirectoryName(dll) + "\\" + assemblyName + ".config"))
                                continue;

                            _queryprocessors.Add(instance);
                        }
                        catch { continue; }
                    }

                }
                catch { continue; }
            }

        }

    }
}
