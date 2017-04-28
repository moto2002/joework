/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ProtoToCSharp
 * 简    述： 
 * 创建时间：2015/8/20 10:25:20
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Reflection;

namespace AutoCSharp.Creator
{
    public class ProtoToCSharp : Base
    {
        /// <summary>
        /// .proto -> .cs
        /// </summary>
        /// <param name="inSpace">命名空间</param>
        /// <param name="inClassName">类名</param>
        /// <param name="inFolderName">文件保存的文件夹名</param>
        public ProtoToCSharp(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName) 
        {
            //usingList.Add("System");
            ineritList.Add("XNetProtocol");
        }

        private List<ProtoCSItem> GetItems(Dictionary<string, string> inDic)
        {
            //string lastValueName = "";// 上一属性的属性名
            List<ProtoCSItem> backList = new List<ProtoCSItem>();

            foreach (KeyValuePair<string, string> item in inDic)
            {
                bool isSelfDefine = false;                      // 是否为自定义类型
                bool isArray = item.Value.Contains("[]");       // 是否为数组

                Type t = GetType(item.Value, ref isSelfDefine); // 属性类型

                ProtoCSItem i = new ProtoCSItem(item.Key, t);
                i.IsSelfDefine = isSelfDefine;
                i.IsArray = isArray;
                //i.ArrayLengthName = isArray ? lastValueName : "";
                backList.Add(i);
                //backList.Add(new ProtoCSItem(item.Key, GetType(item.Value)));
                //lastValueName = item.Key;
            }

            return backList;
        }

        private Type GetType(string s, ref bool isSelfDefine)
        {
            Type t = Type.GetType("System.String");
            string ts = Assist.GetFieldType(s);
            if (ts.Contains("SMGame"))// 自定义类型
            {
                isSelfDefine = true;
                //t = Type.GetType("SMGame."+s);
                t = AutoCSharp.Do.Creator.TypeMapper.GetProtoTypeByName(s);
            }
            else
            {
                isSelfDefine = false;
                t = Type.GetType(ts);
            }
            return t;
        }

        private void HanldItems(ProtoCSItem inItem)
        {
            string name = inItem.Name;
            string proName = "m_" + name;

            Console.WriteLine(name);

            if (!inItem.IsArray)
            {
                if (!inItem.IsSelfDefine)// 非数组的非自定义类型 eg. int, uint, btye etc.
                {
                    onAnalyze.Method.Statements.Add(Line("this." + proName, "bytes." + GetToStraamMathodName(inItem.MType)));// to stream

                    //if (inItem.MType == typeof(System.String))
                    //{
                    //    CodeExpression ieb = new CodeMethodInvokeExpression(
                    //                        new CodeTypeReferenceExpression("bytes"),
                    //                        "Write", new CodeVariableReferenceExpression("this." + proName + ".Length"));
                    //    toBytes.Method.Statements.Add(new CodeExpressionStatement(ieb));
                    //} 底层已写长度
                    CodeExpression invokeExpression = new CodeMethodInvokeExpression(
                                     new CodeTypeReferenceExpression("bytes"),
                                     "Write", new CodeVariableReferenceExpression("this." + proName + ""));
                    toBytes.Method.Statements.Add(new CodeExpressionStatement(invokeExpression));                               // to bytes
                }
                else // 非数组的自定义类型 eg. vector3unit, vector4unit etc.
                {
                    onAnalyze.Method.Statements.Add(Line("this." + proName, "new " + inItem.MType.Name + "()"));// onAnalyze

                    FieldInfo[] fi = inItem.MType.GetFields();
                    for (int i = 0; i < fi.Length; i++)
                    {
                        onAnalyze.Method.Statements.Add(Line("this." + proName + "." + fi[i].Name, "bytes." + GetReflectionString(fi[i].FieldType.ToString())));// to stream

                        CodeExpression ies = new CodeMethodInvokeExpression(
                              new CodeTypeReferenceExpression("bytes"),
                              "Write", new CodeVariableReferenceExpression("this." + proName + "." + fi[i].Name + ""));

                        toBytes.Method.Statements.Add(new CodeExpressionStatement(ies));                                                                            // onAnalyze
                    }
                }
            }
            else // --------------------- 数组 --------------------------------
            {
                //onAnalyze.Method.Statements.Add(Line("this." + proName, "new " + inItem.MType.Name.Replace("[]", "[" + inItem.ArrayLengthName + "]")));

                //CodeIterationStatement curBytesFor = AddFor(typeof(int), "i", 0, inItem.Name + "." + inItem.ArrayLengthName);
                //CodeIterationStatement curStreamFor = AddFor(typeof(int), "i", 0, "this." + inItem.ArrayLengthName);

                string strToBytes = "";
                string strOnAnalyze = "";
                string itemName = name + "Item";
                string typeName = inItem.MType.Name.Replace("[]","");
                string strCnt = "cnt_" + itemName;

                if (inItem.IsSelfDefine)// 数组自定义数据类型 eg. vector3unit[]、vector4unit[] etc.
                {
                    Type type = AutoCSharp.Do.Creator.TypeMapper.GetProtoTypeByName(typeName);
                    FieldInfo[] fi = type.GetFields();
                    strToBytes = "\t\t\tif(this." + proName + " != null) {"
                        + "\r\t\t\t\tbytes.Write(" + proName + ".Length);"
                        + "\r\t\t\t\t" + typeName + " " + itemName + ";"
                        + "\r\t\t\t\tfor(int i = 0; i < " + proName + ".Length; i++) {"
                        + "\r\t\t\t\t\t" + itemName + " = " + proName + "[i];";
                    
                    for (int i = 0; i < fi.Length; i++)
                    {
                        strToBytes += "\r\t\t\t\t\tbytes.Write(" + itemName + "." + fi[i].Name + ");";
                    }
                    strToBytes += "\r\t\t\t\t}\r\t\t\t}else {\r\t\t\t\tbytes.Write(0);\r\t\t\t}";

                    strOnAnalyze = "\t\t\tint " + strCnt + " = bytes.ReadInt();"
                        + "\r\t\t\tif(" + strCnt + " > 0) {\r\t\t\t\t"
                        + "this." + proName + " = new " + typeName + "[" + strCnt + "];"
                        + "\r\t\t\t\t" + typeName + " " + itemName + ";"
                        + "\r\t\t\t\tfor(int i = 0; i < " + strCnt + "; i++) {"
                        + "\r\t\t\t\t\t" + itemName + " = new " + typeName + "();";

                    for (int i = 0; i < fi.Length; i++)
                    {
                        strOnAnalyze += "\r\t\t\t\t\t" + itemName + "." + fi[i].Name + " = bytes." + GetReflectionString(fi[i].FieldType.ToString()) + ";";
                    }
                    strOnAnalyze += "\r\t\t\t\t\tthis." + proName + "[i] = " + itemName + ";\r\t\t\t\t}\r\t\t\t}";
                }
                else // 数组非自定义数据类型 eg. int[], byte[] etc. 
                {
                    strToBytes = "\t\t\tif(this." + proName + " != null) {"
                        + "\r\t\t\t\tbytes.Write(" + proName + ".Length);"
                        + "\r\t\t\t\tbytes.Write(" + proName + ");"
                        + "\r\t\t\t} else {\r\t\t\t\tbytes.Write(0);\r\t\t\t}";

                    strOnAnalyze = "\t\t\tint " + strCnt + " = bytes.ReadInt();"
                        + "\r\t\t\tthis." + proName + " = bytes.ReadBytes(" + strCnt + ");";
                }

                CodeSnippetStatement toBytesSts = new CodeSnippetStatement(strToBytes);
                CodeSnippetStatement onAnalyzeSts = new CodeSnippetStatement(strOnAnalyze);

                toBytes.Method.Statements.Add(toBytesSts);
                onAnalyze.Method.Statements.Add(onAnalyzeSts);
            }
        }

        private MethodItem toBytes;
        private MethodItem onAnalyze;
        private MethodItem clone;
        private MethodItem OnHandle;


        /// <summary>
        /// string: 属性名 string: 属性类型
        /// </summary>
        /// <param name="inValues"></param>
        public void Create(Dictionary<string, string> inValues, bool bCommonClass)
        {
            foreach (KeyValuePair<string, string> i in inValues)
            {
                fieldList.Add(new FieldItem(i.Value, "m_"+i.Key, "", MemberAttributes.Public));
            }
            clone = new MethodItem("Clone", MemberAttributes.Family | MemberAttributes.Override, new List<string>() { });
            clone.SetReturn("XNetProtocol");
            clone.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("new " + className + "()")));

            toBytes = new MethodItem("ToBytes", MemberAttributes.Public | MemberAttributes.Override, new List<string>() { });
            toBytes.SetReturn("System.byte[]");
            toBytes.Method.Statements.Add(Line("ByteArray bytes", "new ByteArray()"));
            
            onAnalyze = new MethodItem("OnAnalyze", MemberAttributes.Family | MemberAttributes.Override, new List<string>() { "System.byte[]" });
            onAnalyze.SetReturn("System.void");
            onAnalyze.Method.Statements.Add(Line("ByteArray bytes", "new ByteArray(inArg0)"));

            GetItems(inValues).ForEach(i => HanldItems(i));

            toBytes.Method.Statements.Add(new CodeMethodReturnStatement(new CodeArgumentReferenceExpression("bytes.ToBytes()")));

            methodList.Add(clone);
            methodList.Add(toBytes);
            methodList.Add(onAnalyze);

            if (bCommonClass == false)
            {
                OnHandle = new MethodItem("OnHandle", MemberAttributes.Family | MemberAttributes.Override, new List<string>() { });
                OnHandle.SetReturn("System.void");
                methodList.Add(OnHandle);
            }
            
            Create();
        }

        private string GetReflectionString(string s)
        {
            string b = s == "System.Single" ? "ReadFloat()" :
                       s == "System.Int32" ? "ReadInt()" :
                       s == "System.Int64" ? "ReadInt64()" :
                       s == "System.String" ? "ReadString()" :
                       s == "System.UInt32" ? "ReadUnsignedInt32()" :
                       s == "System.UInt64" ? "ReadUnsignedInt64()" :
                       s == "System.Byte" ? "ReadByte()" :
                       s == "System.Int16" ? "ReadShort()" : 
                       s == "System.UInt16" ? "ReadUnsignedShort()" : 
                       s == "System.SByte" ? "ReadByte()" : "";
            return b;
        }

        private string GetReadString(string s)
        {
            string b = s == "float" ? "ReadFloat()" :
                       s == "int32" ? "ReadInt()" :
                       s == "int64" ? "ReadInt64()" :
                       s == "string" ? "ReadString()" :
                       s == "uint32" ? "ReadUnsignedInt32()" :
                       s == "uint64" ? "ReadUnsignedInt64()" :
                       s == "byte"  ? "ReadByte()" :
                       s == "int8"  ? "ReadByte()" :
                       s == "uint8"  ? "ReadByte()" :
                       s == "short" ? "ReadShort()" :
                       s == "ushort" ? "ReadUnsignedShort()" : "";
            return b;
        }

        private string GetToStraamMathodName(Type t)
        {
            string b = t == typeof(System.Single) ? "ReadFloat()" :
                       t == typeof(System.Int32) ? "ReadInt()" :
                       t == typeof(System.Int64) ? "ReadInt64()" :
                       t == typeof(System.String) ? "ReadString()" :
                       t == typeof(System.UInt32) ? "ReadUnsignedInt32()" :
                       t == typeof(System.UInt64) ? "ReadUnsignedInt64()" :
                       t == typeof(System.Byte) ? "ReadByte()" :
                       t == typeof(System.Int16) ? "ReadShort()" :
                       t == typeof(System.UInt16) ? "ReadUnsignedShort()" : "";
            return b;
        }

    }
}
