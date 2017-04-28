/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：CSarpCreater
 * 简    述： 
 * 创建时间：2015/7/14 20:10:02
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Xml;
using System.Text.RegularExpressions;
using System.Text;

namespace AutoCSharp.Creator
{
    public class Creater
    {
        #region 单例

        private volatile static Creater instance = null;
        private static readonly object locker = new object();
        public static Creater Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (locker)
                    {
                        if (instance == null)
                        {
                            instance = new Creater();
                        }
                    }
                }
                return instance;
            }
        }

        #endregion

        /// <summary>
        /// 将指定目录下的xml转成cs文件
        /// </summary>
        /// <param name="inPath">xml所在路径</param>
        /// <param name="inFolderName">根节点名</param>
        /// <param name="inNameSpace">命名空间</param>
        /// <param name="inHeritNames">继承</param>
        /// <returns></returns>
        public bool Xml(string inPath, string inFolderName, string inRootName, string inNameSpace, string inHeritNames)
        {
            Assist.CheckFolderExist(inFolderName);

            Dictionary<string, XmlDocument> doc = Assist.GetXml(inPath);

            foreach (KeyValuePair<string, XmlDocument> fieldStr in doc)
            {
                XmlUnit b = new XmlUnit(inNameSpace, fieldStr.Key, inFolderName);
                b.SetInherit(inHeritNames);
                b.SetNodeValue(fieldStr.Value.SelectSingleNode(inRootName).ChildNodes[0]);
            }
            return true;
        }

        /// <summary>
        /// 将指定目录下的xml转成ProtocolBuffer解析类
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <param name="inRootName"></param>
        /// <returns></returns>
        public bool ProtocolBufferXml(string inPath, string inFolderName, string inRootName)
        {
            Assist.CheckFolderExist(inFolderName);
            Dictionary<string, XmlDocument> doc = Assist.GetXml(inPath);

            foreach (KeyValuePair<string, XmlDocument> fieldStr in doc)
            {
                XmlNode rootNode = fieldStr.Value.SelectSingleNode(inRootName);

                for (int i = 0; i < rootNode.ChildNodes.Count; i++)
                {
                    XmlNode n = rootNode.ChildNodes[i];
                    XmlToCSharp pbxu = new XmlToCSharp("", n.Name, inFolderName);
                    pbxu.SetValue(n.Attributes);
                }
            }
            return true;
        }

        /// <summary>
        /// Sub Excel -> .cs
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <param name="inNameSpace"></param>
        /// <param name="inHeritNames"></param>
        /// <returns></returns>
        public bool SubExcel(string inPath, string inFolderName, string inNameSpace, string inHeritNames)
        {
            Assist.CheckFolderExist(inFolderName);
            Assist.GetObjPaths(".xlsx", inPath).ForEach(delegate(string path)
            {
                DataSet ds = Assist.ExcelToData(path);
                DataTable dt = ds.Tables[0];
                ExcelToCSharp e = new ExcelToCSharp(inHeritNames, "Cfg" + dt.TableName, inFolderName);
                e.SetInherit(inHeritNames);
                List<string[]> values = new List<string[]>();
                for (int x = 0; x < dt.Columns.Count; x++)
                {
                    string[] v = new string[3];
                    v[0] = dt.Rows[0][x].ToString();
                    v[1] = dt.Rows[2][x].ToString();
                    v[2] = dt.Rows[1][x].ToString();
                    values.Add(v);
                }
                e.SetValue(dt.TableName,values);
            });
            return true;
        }

        /// <summary>
        /// Excel -> .cs
        /// </summary>
        /// <param name="inFolderPath">输出文件夹名</param>
        /// <param name="suffixName">文件后缀名</param>
        /// <param name="inNameSpace">命名空间</param>
        /// <param name="inHeritNames">继承</param>
        /// <returns></returns>
        public bool Excel(string inPath, string inFolderName, string suffixName, string inNameSpace = "", string inHeritNames = "")
        {
            Assist.CheckFolderExist(inFolderName);
            Assist.DeleteFilesInFolder(inFolderName);

            Dictionary<string, string[]> finalclassname = new Dictionary<string, string[]>();

            //Load Cfg XML
            string cfgPath = Assist.RootPath + "Cfg"; ;
            Dictionary<string, XmlDocument> dicXML = Assist.GetXml(cfgPath);
            string xmlName;
            XmlNodeList nodeList;
            Dictionary<string, string> dicFunctions = new Dictionary<string, string>();
            foreach (XmlDocument xml in dicXML.Values)
            {
                xmlName = xml.DocumentElement.Name;
                if (xmlName == "Functions")
                {
                    //保证先Load Functions.xml
                    nodeList = xml.GetElementsByTagName("function");
                    foreach(XmlNode node in nodeList)
                    {
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            if (attr.Name == "name")
                            {
                                dicFunctions[attr.Value] = node.InnerText;
                                break;
                            }
                        }
                    }
                    break;
                }
            }

            Dictionary<string, ExcelToCSharp> dicEc = new Dictionary<string, ExcelToCSharp>();
            string tableName = "";
            string tableDes = "";

            string cfgName = "",desName = "",fieldName;
            ExcelToCSharp e;
            List<string[]> values;
            foreach (XmlDocument xml in dicXML.Values)
            {
                xmlName = xml.DocumentElement.Name;
                if (xmlName == "CfgItems")
                {
                    nodeList = xml.GetElementsByTagName("cfg");
                    foreach (XmlNode node in nodeList)
                    {
                        foreach (XmlAttribute attr in node.Attributes)
                        {
                            if (attr.Name == "name")
                            {
                                cfgName = attr.Value;
                            }
                            else if(attr.Name == "des")
                            {
                                desName = attr.Value;
                            }
                        }
                        //finalclassname.Add(cfgName, desName);
                        e = new ExcelToCSharp(inHeritNames, "Cfg"+Assist.FirstLetterUp(cfgName), inFolderName, dicFunctions);
                        e.SetInherit(inHeritNames);
                        values = new List<string[]>();
                        foreach(XmlNode paramNode in node.ChildNodes)
                        {
                            string[] v = new string[4];
                            foreach (XmlAttribute attr in paramNode.Attributes)
                            {
                                fieldName = attr.Name;
                                if (fieldName == "name")
                                {
                                    v[0] = attr.Value;
                                }
                                else if (fieldName == "type")
                                {
                                    v[1] = attr.Value;
                                }
                                else if (fieldName == "des")
                                {
                                    v[2] = attr.Value;
                                }
                                else if (fieldName == "link")
                                {
                                    v[3] = attr.Value;
                                }
                            }
                            values.Add(v);
                        }
                        e.SetValue(cfgName,values);
                        dicEc.Add(cfgName, e);
                    }
                }
            }

            try
            {
                Assist.GetObjPaths(suffixName, inPath).ForEach(delegate(string path)
                {
                    DataTable dt = null;
                    bool bExcel = true;
                    if (suffixName == ".xlsx")
                    {
                        DataSet ds = Assist.ExcelToData(path);
                        dt = ds.Tables[0];
                    }
                    else if (suffixName == ".txt")
                    {
                        tableDes = path;
                        dt = Assist.TxtToData(path);
                        bExcel = false;
                    }

                    if (dt != null)
                    {
                        
                        // ===================== 类名注释 ========================

                        string acn = path.Contains("(") ? path.Split('(')[1] : path.Split('（')[1];
                        string acnn = acn.Contains(")") ? acn.Split(')')[0] : acn.Split('）')[0];
                        tableDes = acnn;
                        // =======================================================
                        tableName = Assist.FirstLetterUp(dt.TableName);
                        e = new ExcelToCSharp(inHeritNames, "Cfg" + tableName, inFolderName, dicFunctions);
                        e.SetInherit(inHeritNames);

                        DataRowCollection rows = dt.Rows;
                        values = new List<string[]>();
                        string note = null;
                        string strFieldName = null;
                        for (int x = 0; x < dt.Columns.Count; x++)
                        {
                            if (rows[2][x].ToString() != "")
                            {
                                string[] v = new string[4];
                                v[0] = rows[0][x].ToString().Trim();
                                v[1] = rows[2][x].ToString().Trim();
                                if (bExcel)
                                {
                                    v[2] = rows[1][x].ToString().Trim();
                                    v[3] = rows[3][x].ToString().Trim();
                                }
                                else
                                {
                                    strFieldName = rows[1][x].ToString().Trim();
                                    note = rows[3][x].ToString().Trim();
                                    v[2] = note == "" ? strFieldName : string.Concat(strFieldName, ";", note);
                                    v[3] = rows[4][x].ToString().Trim();
                                }
                                
                                values.Add(v);
                            }
                        }
                        e.SetValue(acnn, values);
                        dicEc.Add(tableName, e);

                        finalclassname.Add(dt.TableName, new string[] { acnn, e.GetKeyType() });
                    }
                });
            }
            catch (Exception ex)
            {
                throw new Exception(tableDes + ex.Message + "可能是Excel打开表左下角的名字跟其它表重复。");
            }

            foreach (ExcelToCSharp ec in dicEc.Values)
            {
                ec.CreateCode(dicEc);
            }

            CreatePBData final = new CreatePBData(inHeritNames, "PBData", inFolderName);
            final.SetValue(finalclassname);
            return true;
        }

        /// <summary>
        /// Excel -> 二进制文件
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <param name="suffixName"></param>
        /// <returns></returns>
        public bool Bin(string inPath, string inFolderName, string suffixName)
        {
            
            Assist.CheckFolderExist(inFolderName);

            //PBData data = new PBData();

            //modify by wukun 2015/10/28
            //PBData需要用到的类都是动态生成的 无需每次添加的时候添加进工程重新编译exe
            //类的cs文件必须放在exe文件根目录的/ExcelfieldStrs/文件夹下
            Type PBDataType = AutoCSharp.Do.Creator.TypeMapper.GetTypeByName("PBData",true);
            
            object pbData = Activator.CreateInstance(PBDataType);

            //Test Cost Time
            System.Diagnostics.Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            DataSet ds;
            DataTable dt = null;
            string classname, subCfgName, subField, fieldTypeStr="",fieldName = "", fieldStr = "",fieldMaxStr="";
            Type classType, subClassType, listType;
            object classObj, subClassObj, listObj, key;

            FieldInfo fieldInfo;
            IDictionary fieldValue;
            int column = 0, subProIndex, arrSize;
            PropertyInfo[] arrPro,subArrPro;
            bool isBaseType;
            Regex reg = new Regex(@"\{.+?\}");
            MatchCollection matches;
            Type generic = typeof(List<>);
            string[] arrField;
            MethodInfo method;
            long maxVal;

            Dictionary<string,long> dicMaxValue;

            Assist.GetObjPaths(suffixName, inPath).ForEach(delegate(string path)
            {
                bool bExcel = true;
                int rowIndex = 4;
                if (suffixName == ".xlsx")
                {
                    ds = Assist.ExcelToData(path);
                    dt = ds.Tables[0];
                }
                else if (suffixName == ".txt")
                {
                    dt = Assist.TxtToData(path);
                    bExcel = false;
                    rowIndex = 5;
                }

                classname = Assist.FirstLetterUp(dt.TableName);

                //TODO Get Cfg Max Value
                dicMaxValue = new Dictionary<string, long>();
                for(column=0;column<dt.Columns.Count;column++)
                {
                    if (dt.Rows[2][column].ToString().Contains("+"))
                    {
                        fieldName = Assist.FirstLetterLower(dt.Rows[0][column].ToString().Trim());
                        fieldMaxStr = dt.Rows[rowIndex][column].ToString();
                        maxVal = 0;
                        if (!Regex.IsMatch(fieldMaxStr, @"\d.\d+"))
                        {
                            maxVal = long.Parse(fieldMaxStr);
                        }
                        dicMaxValue.Add(fieldName, maxVal);
                    }
                }
                

                //TODO 类型的获取应该动态获取
                classType = AutoCSharp.Do.Creator.TypeMapper.GetTypeByName("Cfg" + classname);
                fieldInfo = pbData.GetType().GetField(classname + "Dic");
                fieldValue = fieldInfo.GetValue(pbData) as IDictionary;

                int row = 5;
                if (!bExcel)
                    row = 6;
                bool bDeleted = false;
                try
                {
                    if (classType != null)
                    {
                        for (; row < dt.Rows.Count; row++)
                        {
                            classObj = Activator.CreateInstance(classType);
                            
                            column = 0;
                            if (dt.Rows[row][column].ToString() == "") break;
                            arrPro = classType.GetProperties();
                            foreach (PropertyInfo pro in arrPro)
                            {
                                fieldName = pro.Name;
                                if (fieldName == "mkey") continue;
                                fieldStr = dt.Rows[row][column].ToString();
                                isBaseType = ParseExcelField2ObjectPro(pro, classObj, fieldStr, dicMaxValue[fieldName]);
                                if (isBaseType)
                                {
                                    if (fieldName == "deleted")
                                    {
                                        bDeleted = (int)pro.GetValue(classObj) == 1;
                                    }
                                }
                                else
                                {
                                    if (!reg.IsMatch(fieldStr))
                                    {
                                        column++;
                                        continue;
                                        //throw new Exception("自定义结构配置错误");
                                    }
                                    if (pro.PropertyType.Name == "List`1")
                                    {
                                        matches = reg.Matches(fieldStr);
                                        
                                        subCfgName = pro.PropertyType.GenericTypeArguments[0].Name;
                                        subClassType = AutoCSharp.Do.Creator.TypeMapper.GetTypeByName(subCfgName);
                                        listType = generic.MakeGenericType(new System.Type[] { subClassType });
                                        listObj = Activator.CreateInstance(listType);
                                        foreach (object match in matches)
                                        {
                                            subField = match.ToString();
                                            subField = subField.Substring(1, subField.Length - 2);

                                            subClassObj = Activator.CreateInstance(subClassType);
                                            subArrPro = subClassType.GetProperties();
                                            arrField = subField.Split(',');
                                            arrSize = arrField.Length;
                                            subProIndex = 0;
                                            foreach (PropertyInfo subPro in subArrPro)
                                            {
                                                fieldName = subPro.Name;
                                                if (fieldName == "mkey") continue;
                                                if (subProIndex < arrSize)
                                                {
                                                    ParseExcelField2ObjectPro(subPro, subClassObj, arrField[subProIndex++]);
                                                }
                                            }
                                            method = listType.GetMethod("Add");
                                            method.Invoke(listObj,new object[]{subClassObj});
                                        }
                                        pro.SetValue(classObj, listObj);
                                    }
                                    else
                                    {
                                        fieldStr = fieldStr.Substring(1, fieldStr.Length - 2);
                                        subClassType = AutoCSharp.Do.Creator.TypeMapper.GetTypeByName(pro.PropertyType.Name);
                                        subClassObj = Activator.CreateInstance(subClassType);
                                        subArrPro = subClassType.GetProperties();
                                        arrField = fieldStr.Split(',');
                                        subProIndex = 0;
                                        foreach (PropertyInfo subPro in subArrPro)
                                        {
                                            fieldName = subPro.Name;
                                            if (fieldName == "mkey") continue;
                                            ParseExcelField2ObjectPro(subPro, subClassObj, arrField[subProIndex++]);
                                        }
                                        pro.SetValue(classObj, subClassObj);
                                    }
                                }
                                column++;
                            }

                            if (bDeleted == false)
                            {
                                PropertyInfo proInfo = classType.GetProperty("mkey");
                                key = proInfo.GetValue(classObj, null);
                                fieldValue.Add(key, classObj);
                            }
                        }

                        fieldInfo.SetValue(pbData, fieldValue);
                    }
                }
                catch (Exception ex)
                {

                    string errorMsg = classname + "表第" + (row+1) + "行\n" + fieldName + "字段\n" + fieldTypeStr + "\n" + fieldStr + "\n" + ex.Message;
                    throw new Exception(errorMsg);
                }
            });

            #region //SceneLayout 读取同级目录下的“Excel/SceneLayout/”内的所有 xml 文件，并将其数据写入 PBData

            //Dictionary<string, XmlDocument> doc = Assist.GetXml(Assist.RootPath + "Excel/SceneLayout/");
            //foreach (KeyValuePair<string, XmlDocument> fieldStr in doc)
            //{
            //    SceneLayout sl = new SceneLayout();
            //    XmlNodeList xcc = fieldStr.Value.SelectSingleNode("Config").ChildNodes;
            //    for (int i = 0; i < xcc.Count; i++)
            //    {
            //        SceneLayoutfieldStr sli = new SceneLayoutfieldStr();

            //        IProtoBufable xmlfieldStrValue = new SceneLayoutfieldStr() as IProtoBufable;// value
            //        List<string> xls = new List<string>();
            //        for (int x = 0; x < xcc[i].Attributes.Count; x++)
            //        {
            //            xls.Add(xcc[i].Attributes[x].Value);
            //        }
            //        xmlfieldStrValue.Set(xls);

            //        sl.fieldStr.Add(xmlfieldStrValue as SceneLayoutfieldStr);
            //    }
            //    data.SceneLayoutDic.Add(fieldStr.Key, sl);
            //}

            #endregion

            using (var file = System.IO.File.Create("PBData"))
            {
                try
                {
                    ProtoBuf.Serializer.Serialize(file, pbData);
                    stopWatch.Stop();
                    double msTime = stopWatch.Elapsed.TotalMilliseconds;
                    Console.WriteLine("耗时"+msTime);
                }
                catch (Exception e)
                {
                    MainWindow.Show(e.ToString());
                }
            }

            return true;
        }

        //check Max Value
        public bool CheckMaxValue(long val, long maxVal)
        {
            if (maxVal != 0 && val > maxVal) throw new Exception("超出最大值限制" + maxVal);
            return true;
        }


        /// <summary>
        /// 解析Excel单元格数据 到 对象属性
        /// <para></para>
        /// </summary>
        /// <param name="pro"></param>
        /// <param name="classObj"></param>
        /// <param name="fieldStr"></param>
        /// <param name="maxValue"></param>
        /// <returns>bool 是否基本结构</returns>
        public bool ParseExcelField2ObjectPro(PropertyInfo pro,object classObj,string fieldStr,long maxValue=0)
        {
            bool isBaseType = true;
            Type type = pro.PropertyType;
            if (type == typeof(System.String))
            {
                pro.SetValue(classObj, fieldStr);
            }
            else if (type == typeof(System.Int32))
            {
                int val = fieldStr == "" ? 0 : int.Parse(fieldStr);
                if (CheckMaxValue(val,maxValue)) pro.SetValue(classObj, val);
            }
            else if (type == typeof(System.UInt32))
            {
                uint val = fieldStr == "" ? 0 : uint.Parse(fieldStr);
                if (CheckMaxValue(val, maxValue)) pro.SetValue(classObj, val);
            }
            else if (type == typeof(System.Single))
            {
                float val = fieldStr == "" ? 0 : float.Parse(fieldStr);
                if (CheckMaxValue((long)val,maxValue)) pro.SetValue(classObj, val);
            }
            else if (type == typeof(System.Int64))
            {
                long val = fieldStr == "" ? 0 : long.Parse(fieldStr);
                if (CheckMaxValue(val, maxValue)) pro.SetValue(classObj, val);
            }
            else if (type == typeof(List<System.String>))
            {
                List<string> list = new List<string>();
                string[] arrString = fieldStr.Split('^');
                foreach (string str in arrString)
                {
                    list.Add(str);
                };
                pro.SetValue(classObj, list);
            }
            else if (type == typeof(List<System.Int32>))
            {
                List<int> list = new List<int>();
                if (!string.IsNullOrEmpty(fieldStr))
                {
                    string[] arrString = fieldStr.Split('^');
                    foreach (string str in arrString)
                    {
                        list.Add(int.Parse(str));
                    };
                }
                pro.SetValue(classObj, list);
            }
            else if (type == typeof(List<System.UInt32>))
            {
                List<uint> list = new List<uint>();
                if (!string.IsNullOrEmpty(fieldStr))
                {
                    string[] arrString = fieldStr.Split('^');
                    foreach (string str in arrString)
                    {
                        list.Add(uint.Parse(str));
                    };
                }
                pro.SetValue(classObj, list);
            }
            else if (type == typeof(List<System.Single>))
            {
                List<float> list = new List<float>();
                if (!string.IsNullOrEmpty(fieldStr))
                {
                    string[] arrString = fieldStr.Split('^');
                    foreach (string str in arrString)
                    {
                        list.Add(float.Parse(str));
                    };
                }
                pro.SetValue(classObj, list);
            }
            else if (type == typeof(List<System.Int64>))
            {
                List<long> list = new List<long>();
                if (!string.IsNullOrEmpty(fieldStr))
                {
                    string[] arrString = fieldStr.Split('^');
                    foreach (string str in arrString)
                    {
                        list.Add(long.Parse(str));
                    };
                }
                pro.SetValue(classObj, list);
            }
            else
            {
                isBaseType = false;
            }
            return isBaseType;
        }

        /// <summary>
        /// 将 .proto 文件转成 .cs 文件
        /// <para>用于项目非ProtoBuf-net类型协议，以后用PB协议的话这里就没有用了</para>
        /// </summary>
        /// <param name="inPath"></param>
        /// <param name="inFolderName"></param>
        /// <returns></returns>
        public bool Proto2CSharp(string inPath, string inFolderName)
        {
            Assist.CheckFolderExist(inFolderName);
            Assist.DeleteFilesInFolder("ProtoItem");

            Assist.GetObjPaths(".proto", inPath).ForEach(delegate(string path)
            {
                FileInfo targetFileInfo = new FileInfo(path);
                bool bCommonClass = path.EndsWith("_comm.proto");

                // 类名 -> 成员列表<成员名, 成员类型>
                Dictionary<string, Dictionary<string, string>> nClasses = null;
                if (targetFileInfo != null)
                {
                    Queue<string> sq = new Queue<string>();
                    StreamReader sr = new StreamReader(path, System.Text.Encoding.Default);
                    string line;
                    while (!sr.EndOfStream)
                    {
                        line = sr.ReadLine().Trim();
                        line = Regex.Replace(line, @"/{2}.+", "");// 去掉“//”行
                        line = Regex.Replace(line, @"{\s*", "");
                        sq.Enqueue(line);
                    }
                    sr.Close();
                    sr.Dispose();

                    string curNameSpace = sq.Dequeue().Remove(0, 8).Replace(";", "");

                    nClasses = HanldQueue(sq);

                    foreach (KeyValuePair<string, Dictionary<string, string>> fieldStr in nClasses)
                    {
                        if (fieldStr.Key != "")
                        {
                            ProtoToCSharp ptcs = new ProtoToCSharp(curNameSpace, fieldStr.Key, "ProtoItem");
                            try
                            {
                                ptcs.Create(fieldStr.Value, bCommonClass);
                            }
                            catch (Exception exp)
                            {
                                throw new Exception(exp.ToString() + ", Class" + fieldStr);
                            }
                        }
                    }
                }
                if (bCommonClass)
                {
                    AutoCSharp.Do.Creator.TypeMapper.InitProtoAssembly();
                }
            });
            return true;
        }

        public bool Json2Bin() 
        {
            string path =Assist.RootPath + "Json/";
            Assist.CheckFolderExist("Json");
            MemoryStream ms = new MemoryStream();
            List<string> allJson = Assist.GetObjPaths(".json", path);

            byte[] countBytes = BitConverter.GetBytes(allJson.Count);
            ms.Write(countBytes, 0, countBytes.Length);

            for (int i = 0; i < allJson.Count; i++) 
            {
                FileInfo targetFileInfo = new FileInfo(allJson[i]);
                string fileName = targetFileInfo.Name;
                fileName = fileName.Replace(".json", "");
                string txt = File.ReadAllText(allJson[i],Encoding.UTF8);
               
                byte[] nameBytes = Encoding.UTF8.GetBytes(fileName);
                byte[] nameLengthBytes = BitConverter.GetBytes(nameBytes.Length);
                ms.Write(nameLengthBytes, 0, nameLengthBytes.Length);
                ms.Write(nameBytes, 0, nameBytes.Length);

                byte[] textBytes = Encoding.UTF8.GetBytes(txt);
                byte[] textLengthBytes = BitConverter.GetBytes(textBytes.Length);
                ms.Write(textLengthBytes, 0, textLengthBytes.Length);
                ms.Write(textBytes, 0, textBytes.Length);
            }

            byte[] total = ms.ToArray();

            string filePath = Assist.RootPath+"JsonBin";

            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            FileStream fs = new FileStream(filePath, FileMode.Create);
            fs.Write(total, 0, total.Length);
            fs.Flush();
            fs.Close();

            return true;
        }

        private Dictionary<string, Dictionary<string, string>> HanldQueue(Queue<string> inQueue)
        {
            Dictionary<string, Dictionary<string, string>> back = new Dictionary<string, Dictionary<string, string>>();

            string curClassName = "";
            Dictionary<string, string> curMembers = new Dictionary<string, string>();

            while (inQueue.Count > 0)
            {
                string s = inQueue.Dequeue();
                if (string.IsNullOrWhiteSpace(s))
                {

                }
                else if (s.Contains("//"))
                {

                }
                else if (s.Contains("import"))
                {

                }
                else if (s.Contains("optional"))
                {

                }
                else if (s.Contains("message"))
                {
                    curClassName = s.Remove(0, 8).Replace("{", "");
                }
                else if (s.Contains("required"))
                {
                    string[] v = s.Split(' ');
                    curMembers.Add(v[2], v[1].Contains(".") ? v[1].Split('.')[1] : v[1]);
                }
                else
                {
                    Dictionary<string, string> ad = new Dictionary<string, string>();
                    foreach (KeyValuePair<string, string> fieldStr in curMembers)
                    {
                        ad.Add(fieldStr.Key, fieldStr.Value);
                    }
                    back.Add(curClassName, ad);
                    curClassName = "";
                    curMembers.Clear();
                }
            }


            return back;
        }



    }
}
