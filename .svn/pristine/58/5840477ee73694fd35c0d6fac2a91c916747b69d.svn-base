using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.CodeDom;
using System.CodeDom.Compiler;
using Microsoft.CSharp;
using System.Reflection;

//create by wukun 2015/10/28
//PBData需要用到的类都是在运行时动态生成的 无需每次添加的时候添加进工程重新编译exe
//类的cs文件必须放在exe文件根目录的/ExcelItems/文件夹下

namespace AutoCSharp.Do.Creator
{
    class TypeMapper
    {

        //static Assembly assemblyDynamic;

        static AppDomain appDomain;
        static RemoteLoader remoteLoader;

        static int index=0;

        static void InitDomain()
        {
            UnLoadDomain();

            AppDomainSetup setup = new AppDomainSetup();
            setup.ApplicationName = "TypeMapperDomain";
            setup.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            setup.PrivateBinPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "private");
            setup.CachePath = setup.ApplicationBase;
            setup.ShadowCopyFiles = "true";
            setup.ShadowCopyDirectories = setup.ApplicationBase;

            appDomain = AppDomain.CreateDomain("TestDomain", null, setup);
            string name = Assembly.GetExecutingAssembly().GetName().FullName;
            remoteLoader = (RemoteLoader)appDomain.CreateInstanceAndUnwrap(
                name,
                typeof(RemoteLoader).FullName);

            remoteLoader.SetDomain(appDomain);
        }

        static void UnLoadDomain()
        {
            if (appDomain != null)
            {
                AppDomain.Unload(appDomain);
                appDomain = null;
            }
        }

        static void InitAssemblyFromFile(string typeName, bool loadAll, string dirName, List<string> lisName=null)
        {
            if (dirName == null)
            {
                dirName = "ExcelItems";
            }
            string csFileRootPath = Assist.RootPath + dirName;
            List<string> allCsFiles;

            if (loadAll)
            {
                allCsFiles = Assist.GetObjPaths(".cs", csFileRootPath);
                for (int i = 0; i < allCsFiles.Count; i++)
                {
                    string path = allCsFiles[i];
                    allCsFiles[i] = path.Replace("/", "\\");
                }

                //适配GInterface.m_DictConfig
                if (dirName == "ExcelItems")
                {
                    string classPath = Assist.RootPath + "Cfg\\GInterface.cs";
                    allCsFiles.Add(classPath);
                }
            }
            else
            {
                allCsFiles = new List<string>();
                if (lisName != null)
                {

                    lisName.ForEach(delegate(string name)
                    {
                        string path = csFileRootPath + "\\" + name + ".cs";
                        allCsFiles.Add(path);
                    });
                }
                else
                {
                    string path = csFileRootPath + "\\" + typeName + ".cs";
                    allCsFiles.Add(path);
                }
            }

            CSharpCodeProvider provider = new CSharpCodeProvider();

            CompilerParameters paras = new CompilerParameters();
            paras.GenerateExecutable = false;
            //paras.GenerateInMemory = true;
            paras.GenerateInMemory = false;
            
            string dllName = "TypeMapper1.dll";
            if (index > 0)
            {
                dllName = "TypeMapper2.dll";
                index = 0;
            }
            index++;
            paras.OutputAssembly = dllName;

            paras.ReferencedAssemblies.Add("protobuf-net.dll");
            paras.ReferencedAssemblies.Add("mscorlib.dll");
            paras.ReferencedAssemblies.Add("AutoCSharp.exe");
            //for test
            //string[] files = new string[2];
            //files[0] = "F:\\work\\game\\Client\\Demo3Tools\\ExceltoProtoBuf\\AutoCSharp\\bin\\Release\\ExcelItems\\CfgAccomplish.cs";
            //files[1] = "F:\\work\\game\\Client\\Demo3Tools\\ExceltoProtoBuf\\AutoCSharp\\bin\\Release\\ExcelItems\\PBData.cs";

            try
            {
                CompilerResults result = provider.CompileAssemblyFromFile(paras, allCsFiles.ToArray());
                //assemblyDynamic = result.CompiledAssembly;
               
                Assembly assem = result.CompiledAssembly;
                InitDomain();
                
                //remoteLoader.LoadAssembly(Assist.RootPath + dllName);
                remoteLoader.assembly = assem;
            }
            catch (Exception ex)
            {
                throw new Exception("动态加载cs文件出错,可能是表的字段重复或命名问题，请检查cs文件！！\n"+ex.ToString());
            }
        }

        public static Type GetTypeByName(string typeName,bool bNeedReInit=false,bool loadAll=true,string dirName=null) {
            if (remoteLoader == null || bNeedReInit == true)
            {
                InitAssemblyFromFile(typeName, loadAll, dirName);
            }

            //Type type = assemblyDynamic.GetType(typeName);
            Type type = remoteLoader.assembly.GetType(typeName);
             
            return type;
        }

        //proto自定义类型
        public static Type GetProtoTypeByName(string typeName)
        {
            //Type type = assemblyDynamic.GetType("SMGame." + typeName);
            Type type = remoteLoader.assembly.GetType("SMGame." + typeName);
            return type;
        }

        //proto协议自定义类型
        public static void InitProtoAssembly()
        {
            InitAssemblyFromFile("", true, "ProtoItem");
        }

        //proto协议自定义类型
        public static void CreateProtoAssembly(List<string> lisName)
        {
            InitAssemblyFromFile(null, false, "ProtoItem", lisName);
        }
    }
}
