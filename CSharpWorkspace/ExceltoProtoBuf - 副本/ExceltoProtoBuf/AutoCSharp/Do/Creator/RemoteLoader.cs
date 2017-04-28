using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace AutoCSharp.Do.Creator
{
    class RemoteLoader:MarshalByRefObject
    {
        public RemoteLoader()
        {
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }
        public void SetDomain(AppDomain arg_domain)
        {
            arg_domain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            Console.WriteLine(args.Name);
            return null;
        } 

        public Assembly assembly = null;

        public string FullName
        {
            get { return assembly.FullName; }
        }

        public void LoadAssembly(string arg_strAssemblyFullName)
        {
            assembly = Assembly.LoadFile(arg_strAssemblyFullName);
        } 
    }
}
