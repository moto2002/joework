using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using AutoCSharp.Creator;
using System.Runtime.InteropServices;
using System.Threading;

namespace AutoCSharp
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// 启动控制台
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool AllocConsole();

        /// <summary>
        /// 关闭控制台
        /// </summary>
        [DllImport("kernel32.dll")]
        public static extern bool FreeConsole();


        //[STAThread]
        //static int Main(string[] args)
        //{
        //    /*
        //    AutoCSharp.App app = new AutoCSharp.App();
            
        //    app.InitializeComponent();

        //    if (args.Length == 0)
        //    {
        //        app.Run();
        //        return 0;
        //    }
        //    else
        //    {
        //        int type = int.Parse(args[0]);
        //        switch (type)
        //        {
        //            case 1:
        //                try
        //                {
        //                    if (Creater.Instance.Excel(Assist.RootPath + "Excel/", "ExcelItems", ".xlsx"))
        //                    {
        //                        if (Creater.Instance.Bin(Assist.RootPath + "Excel/", "Excel", ".xlsx"))
        //                        {
        //                            return 0;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                    return 1;
        //                }
        //                break;
        //            case 2:
        //                try
        //                {
        //                    if (Creater.Instance.Excel(Assist.RootPath + "Txt/", "ExcelItems",".txt"))
        //                    {
        //                        if (Creater.Instance.Bin(Assist.RootPath + "Txt/", "Txt", ".txt"))
        //                        {
        //                            return 0;
        //                        }
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                    return 1;
        //                }
        //                break;
        //            case 3:
        //                System.Data.DataTable dt;
        //                string inPath = Assist.RootPath + "Txt/";
        //                string filePath = "";
        //                try
        //                {
        //                    Assist.GetObjPaths(".txt", inPath).ForEach(delegate(string path)
        //                    {
        //                        filePath = path;
        //                        dt = Assist.TxtToData(path);
        //                        Assist.TxtToXML(path, dt);
        //                    });
        //                    return 0;
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(filePath + "," + ex.Message);
        //                    return 1;
        //                }
        //                break;
        //            case 4:
        //                try
        //                {
        //                    if (Creater.Instance.Json2Bin())
        //                    {
        //                        return 0;
        //                    }
        //                }
        //                catch (Exception ex)
        //                {
        //                    MessageBox.Show(ex.Message);
        //                    return 1;
        //                }
        //                break;
        //            default:
        //                break;
        //        }
        //    }
        //    */
        //    return 0;
        //}
        
    }
}
