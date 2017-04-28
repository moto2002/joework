/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：Excel2CSharp
 * 简    述： 
 * 创建时间：2015/11/22 21:01:28
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using Excel;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Reflection;

public class Excel2CSharp
{
    private string m_inPath = "output/Excels";
    private string m_outPath = "output/ExcelsCS";

    int summary = 0;
    int titleIndex = 1;//第二行：字段名
    int contentBeginIndex = 3;//第四行：内容开始
    int typeIndex = 2;//第三行：是类型

    string m_nameSpace = "HuanJueData";
    string m_PBData = "PBData";

    string m_itemHeader = "";

    public Excel2CSharp()
    {

    }

    public void Gen()
    {
        string currDir = Environment.CurrentDirectory;
        this.m_inPath = Path.Combine(currDir, m_inPath);
        Assist.CreateDir(this.m_inPath);

        this.m_outPath = Path.Combine(currDir, m_outPath);
        Assist.CreateDir(this.m_outPath);

        ForGen(m_inPath, m_outPath);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="inPath"></param>
    /// <param name="outPath"></param>
    private void ForGen(string inPath, string outPath)
    {

        List<string> listClassNames = new List<string>();

        Assist.ClearDirFiles(outPath);

        DirectoryInfo folder = new DirectoryInfo(inPath);
        FileSystemInfo[] files = folder.GetFileSystemInfos();
        int length = files.Length;
        for (int index = 0; index < length; index++)
        {
            if (files[index] is DirectoryInfo)
            {
                //this.ForGen(files[index].FullName);
            }
            else
            {
                if (files[index].Name.EndsWith(".xlsx"))
                {
                    //DataSet mResultSet = Assist.ExcelToDataSet(files[index].FullName);
                    FileStream stream = File.Open(files[index].FullName, FileMode.Open, FileAccess.Read);
                    IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                    DataSet mResultSet = excelReader.AsDataSet();

                    //判断Excel文件中是否存在数据表
                    if (mResultSet.Tables.Count < 1)
                        return;

                    //默认读取第一个数据表
                    DataTable mSheet = mResultSet.Tables[0];

                    string tableName = mSheet.TableName;
                    string csName = m_itemHeader + Stringer.FirstLetterUp(tableName);
                    listClassNames.Add(csName);

                    CreateCSItemData csItemData = new CreateCSItemData(m_nameSpace, csName, m_outPath);

                    //读取数据表行数和列数
                    int rowCount = mSheet.Rows.Count;
                    int colCount = mSheet.Columns.Count;


                    List<string> mainKeyList = new List<string>();

                    for (int j = 0; j < colCount; j++)//列
                    {
                        string key = mSheet.Rows[titleIndex][j].ToString();
                        if (key == "")
                        {
                            Assist.Log("下标" + "(" + titleIndex + "," + j + ")" + "不能为空");
                            return;
                        }

                        string strData = mSheet.Rows[typeIndex][j].ToString();
                        string[] strs = strData.Split('+');
                        string keyType = strs[0];
                        int isMainkey = int.Parse(strs[1]);
                        string linkTableName = strs[2];

                        ItemField itemField = new ItemField(key, keyType, MemberAttributes.Private);
                        csItemData.AddFieldValue(itemField);

                        ItemProperty itemProperty = new ItemProperty(key);
                        itemProperty.SetGetName();
                        itemProperty.SetSetName();
                        itemProperty.SetComment(mSheet.Rows[summary][j].ToString());
                        itemProperty.SetValueType(keyType);
                        itemProperty.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                        itemProperty.SetField("ProtoMember", (j + 1).ToString());
                        csItemData.AddPropertyValue(itemProperty);

                        //是主键，或者是合成主键的一部分字段
                        if (isMainkey == 1)
                        {
                            mainKeyList.Add(key);
                        }

                        //有链接的表
                        if (linkTableName != "0")
                        {
                            linkTableName = m_itemHeader + Stringer.FirstLetterUp(linkTableName);
                            string subClassname = linkTableName;
                            ItemMethod mis = new ItemMethod("Get" + subClassname, MemberAttributes.Final | MemberAttributes.Public, new List<string>() { "System.String" });
                            csItemData.SetComment("获取" + subClassname, mis.Method);
                            mis.Method.ReturnType = new CodeTypeReference(subClassname);
                            mis.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("CfgData.Instance.m_PBData." + subClassname + "Dic[inArg0]")));
                            csItemData.AddMethodValue(mis);
                        }
                    }

                    //最多由3个字段组成主键
                    string MainKey = "";
                    if (mainKeyList.Count == 1)
                    {
                        MainKey = mainKeyList[0] + ".ToString()";
                    }
                    else if (mainKeyList.Count == 2)
                    {
                        MainKey = mainKeyList[0] + ".ToString() + \"_\" + this.m_" + mainKeyList[1] + ".ToString()";
                    }
                    else if (mainKeyList.Count == 3)
                    {
                        MainKey = mainKeyList[0] + ".ToString() + \"_\" + this.m_" + mainKeyList[1] + ".ToString() + \"_\" + this.m_" + mainKeyList[2] + ".ToString()";
                    }

                    ItemProperty itemProperty2 = new ItemProperty("MainKey");
                    itemProperty2.SetGetName(MainKey);
                    itemProperty2.SetComment("主键");
                    itemProperty2.SetValueType("string");
                    itemProperty2.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                    csItemData.AddPropertyValue(itemProperty2);

                    csItemData.Create();
                }
            }
        }

        CreatePBData final = new CreatePBData(m_nameSpace, m_PBData, m_outPath);
        final.SetValue(listClassNames);
        final.Create();

  
        List<string> dyncUsingList = new List<string>();
        dyncUsingList.Add("protobuf-net.dll");
        dyncUsingList.Add("mscorlib.dll");
  
        List<string> dyncCsFilesName = Assist.GetObjPaths(".cs", m_outPath);
        string csDataPoolPath = Path.Combine(Environment.CurrentDirectory, "CfgData.cs");
        dyncCsFilesName.Add(csDataPoolPath);
        for (int i = 0; i < dyncCsFilesName.Count; i++)
        {
            dyncCsFilesName[i] = dyncCsFilesName[i].Replace("/", "\\");
        }

        string dyncDllPath = Path.Combine(m_outPath, "CfgData.dll");
        CompilerResults cr = DyncLibTool.CompileCSharpCode(dyncCsFilesName, dyncDllPath, dyncUsingList);
        //Assembly assembly = cr.CompiledAssembly;
        Assist.Log("Gen OK");

      
    }
}

