using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AutoCfg
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void btn_excel2xml_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("page1_excel2xml");
        }

        private void btn_excel2json_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("page2_excel2json");
        }

        private void btn_excel2csharp_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("page3_excel2csharp");
        }

        private void btn_xml2csharp_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("page4_xml2csharp");
        }

        private void btn_json2csharp_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("page5_json2csharp");
        }

        private void btn_excel2csv_Click(object sender, RoutedEventArgs e)
        {
            ShowPage("page6_excel2csv");
        }

        private string currentPageName = "page1_excel2xml";
        /// <summary>
        /// 关闭之前的页面，显示新页面
        /// </summary>
        /// <param name="pageName"></param>
        private void ShowPage(string pageName)
        {
            switch (currentPageName)
            {
                case "page1_excel2xml":
                    page1_excel2xml.Visibility = Visibility.Hidden;
                    break;
                case "page2_excel2json":
                    page2_excel2json.Visibility = Visibility.Hidden;
                    break;
                case "page3_excel2csharp":
                    page3_excel2csharp.Visibility = Visibility.Hidden;
                    break;
                case "page4_xml2csharp":
                    page4_xml2csharp.Visibility = Visibility.Hidden;
                    break;
                case "page5_json2csharp":
                    page5_json2csharp.Visibility = Visibility.Hidden;
                    break;
                case "page6_excel2csv":
                    page6_excel2csv.Visibility = Visibility.Hidden;
                    break;
                default:
                    break;
            }

            switch (pageName)
            {
                case "page1_excel2xml":
                    page1_excel2xml.Visibility = Visibility.Visible;
                    break;
                case "page2_excel2json":
                    page2_excel2json.Visibility = Visibility.Visible;
                    break;
                case "page3_excel2csharp":
                    page3_excel2csharp.Visibility = Visibility.Visible;
                    break;
                case "page4_xml2csharp":
                    page4_xml2csharp.Visibility = Visibility.Visible;
                    break;
                case "page5_json2csharp":
                    page5_json2csharp.Visibility = Visibility.Visible;
                    break;
                case "page6_excel2csv":
                    page6_excel2csv.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }

            currentPageName = pageName;
        }

        private void btn_gen_excel2xml_Click(object sender, RoutedEventArgs e)
        {
            GenManager.Instance.Gen_Excel2Xml();
        }

        private void btn_gen_excel2json_Click(object sender, RoutedEventArgs e)
        {
            GenManager.Instance.Gen_Excel2Json();
        }

        private void btn_gen_excel2csv_Click(object sender, RoutedEventArgs e)
        {
            GenManager.Instance.Gen_Excel2CSV();
        }

        private void btn_gen_excel2csharp_Click(object sender, RoutedEventArgs e)
        {
            GenManager.Instance.Gen_Excel2CSharp();
        }

        private void btn_gen_genPBdata_Click(object sender, RoutedEventArgs e)
        {
            GenManager.Instance.Gen_PBData();
        }

        
    }
}
