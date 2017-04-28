using AutoCSharp.Creator;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Windows;

namespace AutoCSharp
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// 显示 Xml 转换界面
        /// </summary>
        private void ShowAutoXml(object sender, RoutedEventArgs e)
        {
            StartGrid.Height = 0;
            StartGrid.Width = 0;
            AutoXml.Height = height;
            AutoXml.Width = width;
        }

        /// <summary>
        /// 显示 Excel 转换界面
        /// </summary>
        private void ShowAutoExcel(object sender, RoutedEventArgs e)
        {
            StartGrid.Height = 0;
            StartGrid.Width = 0;
            AutoExcel.Height = height;
            AutoExcel.Width = width;
        }

        /// <summary>
        /// Xml -> .cs
        /// </summary>
        private void XmlToCS(object sender, RoutedEventArgs e)
        {
            string path = (bool)RootXmlFolder.IsChecked ? Assist.RootPath + "Xml/" : OpenFolder();
            if (path != "")
            {
                if (Creater.Instance.Xml(path, XmlFolder.Text, XmlRootName.Text, NameSpaceText.Text, HeritNames.Text))
                {
                    MessageBox.Show("xml -> cs 完成！");
                }
            }
        }

        /// <summary>
        /// json -> bin
        /// </summary>
        private void Json2Bin(object sender, RoutedEventArgs e)
        {
            string path = Assist.RootPath + "Json/";
            if (Creater.Instance.Json2Bin()) 
            {
                MessageBox.Show("Json2Bin 完成！");
            }
        }

        /// <summary>
        /// Xml -> .cs (for Protocol Buffer)
        /// </summary>
        private void XmlToProtocolBufferCSharp(object sender, RoutedEventArgs e)
        {
            string path = (bool)RootExcelFolder.IsChecked ? Assist.RootPath + "Excel/" : OpenFolder();
            if (path != "")
            {
                if (Creater.Instance.ProtocolBufferXml(path, ExcelFolder.Text, "Config"))
                {
                    MessageBox.Show("xml -> cs (for Protocol Buffer) 完成！");
                }
            }
        }

        /// <summary>
        /// 将从属的Excel表转成PB解析类
        /// </summary>
        private void SubExcelTOPBCSharp(object sender, RoutedEventArgs e)
        {
            string path = (bool)RootExcelFolder.IsChecked ? Assist.RootPath + "Excel/SubExcel" : OpenFolder();
            if (path != "")
            {
                if (Creater.Instance.SubExcel(path, ExcelFolder.Text, ExcelNameSpace.Text, ExcelHerit.Text))
                {
                    MessageBox.Show("Sub Exel -> cs (for Protocol Buffer) 完成！");
                }
            }
        }

        /// <summary>
        /// Excel -> .cs
        /// </summary>
        private void ExcelToCS(object sender, RoutedEventArgs e)
        {
            string path = (bool)RootExcelFolder.IsChecked ? Assist.RootPath + "Excel/" : OpenFolder();
            if (path != "")
            {
                try
                {
                    if (Creater.Instance.Excel(path, ExcelFolder.Text, ".xlsx",ExcelNameSpace.Text, ExcelHerit.Text))
                    {
                        MessageBox.Show("Exel -> cs (for Protocol Buffer) 完成！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        /// <summary>
        /// 输出二进制文件 (for Protocol Buffer)
        /// </summary>
        private void CreatrBin(object sender, RoutedEventArgs e)
        {
            string path =Assist.RootPath + "Excel/";
            if (path != "")
            {
                try
                {
                    if (Creater.Instance.Bin(path, "Excel", ".xlsx"))
                    {
                        MessageBox.Show("Exel -> .bin (for Protocol Buffer) 完成！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        #region Assist

        private int width = 452;
        private int height = 290;
        public MainWindow()
        {
            InitializeComponent();
            AutoXml.Height = 0;
            AutoXml.Width = 0;
            AutoExcel.Height = 0;
            AutoExcel.Width = 0;
            StartGrid.Height = height;
            StartGrid.Width = width;
        }

        /// <summary>
        /// 返回到主界面
        /// </summary>
        private void BackRoot(object sender, RoutedEventArgs e)
        {
            AutoXml.Height = 0;
            AutoXml.Width = 0;
            AutoExcel.Height = 0;
            AutoExcel.Width = 0;
            StartGrid.Height = height;
            StartGrid.Width = width;
        }

        /// <summary>
        /// Log
        /// </summary>
        static public void Show(string s)
        {
            MessageBox.Show(s);
        }

        /// <summary>
        /// 返回打开的目录路径
        /// </summary>
        private string OpenFolder()
        {
            System.Windows.Forms.FolderBrowserDialog dlg = new System.Windows.Forms.FolderBrowserDialog();
            System.Windows.Interop.HwndSource source = PresentationSource.FromVisual(this) as System.Windows.Interop.HwndSource;
            System.Windows.Forms.IWin32Window win = new OpenFolderDialog(source.Handle);
            System.Windows.Forms.DialogResult result = dlg.ShowDialog(win);
            return dlg.SelectedPath;
        }

        #endregion

        /// <summary>
        /// 将 .proto 文件转成 .cs 文件
        /// <para>用于项目非ProtoBuf-net类型协议，以后用PB协议的话这里就没有用了</para>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CreateCSharpFromProto(object sender, RoutedEventArgs e)
        {
            string path = Assist.RootPath + "Protos/";
            if (path != "")
            {
                try
                {
                    if (Creater.Instance.Proto2CSharp(path, "Protos"))
                    {
                        MessageBox.Show(".proto -> .cs 完成！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void CreateTxtFromExcel(object sender, RoutedEventArgs e)
        {
            System.Data.DataSet ds;
            string inPath = Assist.RootPath + "Excel/";
            try
            {
                Assist.GetObjPaths(".xlsx", inPath).ForEach(delegate(string path)
                {
                    ds = Assist.ExcelToData(path);
                    Assist.ExcelToTxt(path, ds);
                });
                MessageBox.Show(".excel -> .txt 完成！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void TxtToCS(object sender, RoutedEventArgs e)
        {
            string path = (bool)RootExcelFolder.IsChecked ? Assist.RootPath + "Txt/" : OpenFolder();
            if (path != "")
            {
                try
                {
                    if (Creater.Instance.Excel(path, ExcelFolder.Text, ".txt", ExcelNameSpace.Text, ExcelHerit.Text))
                    {
                        MessageBox.Show("Txt -> cs (for Protocol Buffer) 完成！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void TxtBin(object sender, RoutedEventArgs e)
        {
            string path = Assist.RootPath + "Txt/";
            if (path != "")
            {
                try
                {
                    if (Creater.Instance.Bin(path, "Txt", ".txt"))
                    {
                        MessageBox.Show("Txt -> .bin (for Protocol Buffer) 完成！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void Txt2Xml(object sender, RoutedEventArgs e)
        {
            System.Data.DataTable dt;
            string inPath = Assist.RootPath + "Txt/";
            string filePath = "";
            try
            {
                Assist.GetObjPaths(".txt", inPath).ForEach(delegate(string path)
                {
                    filePath = path;
                    dt = Assist.TxtToData(path);
                    Assist.TxtToXML(path, dt);
                });
                MessageBox.Show("Txt -> Xml (for Server) 完成！");
            }
            catch (Exception ex)
            {
                MessageBox.Show(filePath + "," + ex.Message);
            }
        }
    }
}