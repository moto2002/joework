using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Excel;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using System.Text;
using Xamasoft.JsonClassGenerator;
using System;
using Xamasoft.JsonClassGenerator.CodeWriters;

public class Generator
{
    /// <summary>
    /// 转换为Json
    /// </summary>
    public static bool Excel2Json(string _excelPath, string _jsonPath)
    {
        Encoding encoding = Encoding.UTF8;

        DirectoryInfo folder = new DirectoryInfo(_excelPath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;

        for (int index = 0; index < length; index++)
        {
            if (files[index].Name.EndsWith(".xlsx"))
            {
                string childPath = files[index].FullName;

                FileStream mStream = File.Open(childPath, FileMode.Open, FileAccess.Read);
                IExcelDataReader mExcelReader = ExcelReaderFactory.CreateOpenXmlReader(mStream);
                DataSet mResultSet = mExcelReader.AsDataSet();

				//重新构建一个DataSet
				DataSet m_newDataSet = new DataSet();
				DataTable m_newTable = new DataTable();
				m_newDataSet.Tables.Add(m_newTable);

                //判断Excel文件中是否存在数据表
                if (mResultSet.Tables.Count < 1)
                    return false;

                //默认读取第一个数据表
                DataTable mSheet = mResultSet.Tables[0];
                string jsonName = mSheet.TableName;

				//新构建的DataSet设置table名字
				m_newTable.TableName = jsonName;

                //判断数据表内是否存在数据
                if (mSheet.Rows.Count < 1)
                    return false;

                //读取数据表行数和列数
                int rowCount = mSheet.Rows.Count;
                int colCount = mSheet.Columns.Count;

				for (int k = 0; k < colCount; k++) 
				{
					//mSheet.Columns[k].ColumnName = mSheet.Rows[1][k].ToString();
					string temp =  mSheet.Rows[1][k].ToString();//属性名字_类型
					string[] tempArry = temp.Split('_');

					string pName = tempArry[0];//属性名字
					string typeName = tempArry[1];//类型

					//Debug.LogError(tempArry[0]+"|"+tempArry[1]);
					mSheet.Columns[k].ColumnName = pName;

					//需要什么类型自己扩展
					switch(typeName)
					{
					case "i":
						m_newTable.Columns.Add(new DataColumn(pName, typeof(int)));
						break;
					case "s":
						m_newTable.Columns.Add(new DataColumn(pName, typeof(string)));
						break;
					case "f":
						m_newTable.Columns.Add(new DataColumn(pName, typeof(float)));
						break;
					default:break;
					}
				}

				//思路来自：http://www.newtonsoft.com/json/help/html/SerializeDataSet.htm
				//读取数据
				for (int i = 2; i < rowCount; i++)
				{
					DataRow m_newRow = m_newTable.NewRow();
					for (int j = 0; j < colCount; j++)
					{
						m_newRow[mSheet.Columns[j].ColumnName] = mSheet.Rows[i][j];
					}
					m_newTable.Rows.Add(m_newRow);
				}

				m_newDataSet.AcceptChanges();

				//生成Json字符串
				string json = JsonConvert.SerializeObject(m_newDataSet, Newtonsoft.Json.Formatting.Indented);

                //写入文件
                using (FileStream fileStream = new FileStream(_jsonPath + "/" + jsonName + ".json", FileMode.Create, FileAccess.Write))
                {
                    using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                    {
                        textWriter.Write(json);
                    }
                }
            }
        }

        return true;
    }


    public static bool Json2CSharp(string _jsonPath, string _csharpPath)
    {
        Encoding encoding = Encoding.UTF8;

        DirectoryInfo folder = new DirectoryInfo(_jsonPath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;

        for (int index = 0; index < length; index++)
        {
            if (files[index].Name.EndsWith(".json"))
            {
                string csName = files[index].Name;
                csName = csName.Substring(0, csName.LastIndexOf("."));

                string csContent = "";
                using (StreamReader textReader = new StreamReader(files[index].FullName, Encoding.UTF8))
                {
                    string str = textReader.ReadToEnd();//读取文件
                    csContent = str;
                    textReader.Close();
                }

                JsonClassGenerator gen = Prepare(csContent, _csharpPath,csName);
                if (gen == null) return false;

                try
                {
                    gen.GenerateClasses();
                }
                catch (Exception ex)
                {
                    Debug.Log("Unable to generate the code: " + ex.Message);
                    return false;
                }


                //				//写入文件
                //				using (FileStream fileStream = new FileStream(_csharpPath + "/" + csName + ".cs", FileMode.Create, FileAccess.Write))
                //				{
                //					using (TextWriter textWriter = new StreamWriter(fileStream, encoding))
                //					{
                //						textWriter.Write(csContent);
                //					}
                //				}
            }
        }
        return true;
    }

    private static JsonClassGenerator Prepare(string _jsonData,string _csharpPath,string _csName)
    {
        if (_jsonData == string.Empty)
        {
            Debug.Log("Please insert some sample JSON.");
            return null;
        }

        JsonClassGenerator gen = new JsonClassGenerator();
        gen.Example = _jsonData;
        gen.InternalVisibility = false;
        gen.CodeWriter = new CSharpCodeWriter();
        gen.ExplicitDeserialization = false;
        gen.Namespace = "HuanJueGameData";
        gen.NoHelperClass = true;
        gen.SecondaryNamespace = null;
        gen.TargetFolder = _csharpPath;
        gen.UseProperties = true;
		gen.MainClass = "Cfg"+_csName;
        gen.UsePascalCase = false;
        gen.UseNestedClasses = false;
        gen.ApplyObfuscationAttributes = false;
        gen.SingleFile = true;
        gen.ExamplesInDocumentation = false;
        return gen;
    }
}

