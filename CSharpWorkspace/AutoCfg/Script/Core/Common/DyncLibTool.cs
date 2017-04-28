/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：DyncLibTool
 * 简    述： 
 * 创建时间：2015/12/3 15:02:29
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;

public class DyncLibTool
{


    /// <summary>
    ///  将一个目录下的所有cs文件动态编译成dll
    /// </summary>
    /// <param name="sourceFileList">cs文件路径列表</param>
    /// <param name="exeFile">要生成的dll文件名</param>
    /// <param name="usingList">引用的列表</param>
    /// <returns></returns>
    public static CompilerResults CompileCSharpCode(List<string> sourceFileList,string exeFile,List<string> usingList)
    {
        CSharpCodeProvider provider = new CSharpCodeProvider();

        // Build the parameters for source compilation.
        CompilerParameters cp = new CompilerParameters();

        // Add an assembly reference.
        //cp.ReferencedAssemblies.Add("System.dll");
        for (int i = 0; i < usingList.Count; i++)
        {
            cp.ReferencedAssemblies.Add(usingList[i]);
        }

        // Generate an executable instead of  a class library.
        //是否生成exe可执行文件
        //cp.GenerateExecutable = true;

        // Set the assembly file name to generate.
        cp.OutputAssembly = exeFile;

        // Save the assembly as a physical file.
        //是否只在内存中，flase：会保存在本地，PathToAssembly 可以查看保存的路径
        cp.GenerateInMemory = false;

        // Invoke compilation.
        CompilerResults cr = provider.CompileAssemblyFromFile(cp, sourceFileList.ToArray());

        if (cr.Errors.Count > 0)
        {
            // Display compilation errors.
            //Console.WriteLine("Errors building into {0}",cr.PathToAssembly);
            foreach (CompilerError ce in cr.Errors)
            {
                Console.WriteLine("  {0}", ce.ToString());
                Console.WriteLine();
            }
        }
        else
        {
            //Console.WriteLine("Source built into {1} successfully.",cr.PathToAssembly);
        }

        // Return the results of compilation.
        if (cr.Errors.Count > 0)
        {
            return null;
        }
        else
        {
            return cr;
        }
    }
}

