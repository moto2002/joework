/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ExcelToCSharp
 * 简    述： 
 * 创建时间：2015/8/10 15:16:23
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Text.RegularExpressions;


namespace AutoCSharp.Creator
{
    public class ExcelToCSharp : Base
    {
        private Dictionary<string, string> dicFunctions;
        private string m_keyType = "int";
        private string m_mainKey;
        private string m_linkTableName = "";
        private string m_linkMethodContent;
        private MethodItem m_linkMethod;

        public ExcelToCSharp(string inSpace, string inClassName, string inFolderName, Dictionary<string,string> dicFunc=null)
            : base(inSpace, inClassName, inFolderName)
        {
            dicFunctions = dicFunc;
            // 引用
            usingList.Add("ProtoBuf");
            usingList.Add("System.Collections.Generic");
            //ineritList.Add("IProtoBufable");
        }

        /// <summary>
        /// <para>0: 字段名</para>
        /// <para>1: a+i+0 指明类型及是否为Key值</para>
        /// <para>2: 注释</para>
        /// </summary>
        /// <param name="inList">0: 字段名</param>
        public void SetValue(string tableName, List<string[]> inList)
        {
            classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));

            List<KeyValuePair<string, string>> keyList = new List<KeyValuePair<string, string>>();// key 值

            string[] ss, arrContruct;
            string attrName, typeString, linkTable, cfgName, strListType, content, ks = "", bit = "";
            MethodItem mis;
            CodeSnippetStatement state;
            PropertyItem item, keyPropertyItem;
            Regex rgx = new Regex(@"[\[\]]{1}");
            bool isArray;

            for (int i = 0; i < inList.Count; i++)
            {
                arrContruct = inList[i];
                if (!arrContruct[1].Contains("+")) continue;
                ss = arrContruct[1].Split('+');
                attrName = Assist.FirstLetterLower(arrContruct[0]);
                typeString = ss[1];
                string strTmp = arrContruct[3].Trim();
                if (strTmp == "")
                {
                    strTmp = "0";
                }
                linkTable = Assist.FirstLetterUp(strTmp);
                cfgName = "Cfg" + linkTable;
                //link table 不指定id
                if (typeString == "t")
                {
                    mis = new MethodItem("GetList" + cfgName, MemberAttributes.Final | MemberAttributes.Public, new List<string>() { });
                    SetComment("获取List<" + cfgName + ">", mis.Method);
                    strListType = "List<" + cfgName + ">";
                    mis.Method.ReturnType = new CodeTypeReference(strListType);

                    content = dicFunctions["GetListCfgNameUnAttach"];
                    content = content.Replace("{TableName}", linkTable);
                    content = content.Replace("{CfgName}", cfgName);
                    content = content.Replace("{Type}", "long");
                    m_linkMethodContent = content;
                    m_linkTableName = linkTable;
                    m_linkMethod = mis;
                    //content = content.Replace("{MainKey}", **);

                    //state = new CodeSnippetStatement(content);
                    //mis.Method.Statements.Add(state);
                    //methodList.Add(mis);
                    continue;
                }

                //是否数组eg. [i]
                isArray = false;
                if (rgx.IsMatch(typeString))
                {
                    isArray = true;
                    typeString = rgx.Replace(typeString, "");
                }

                //link table 指定id
                if (Regex.IsMatch(linkTable, @"[a-zA-Z]+"))
                {
                    //string msg = className.Substring(3) + "的" + attrName + "第四列要把浮点数改成0";
                    //System.Windows.MessageBox.Show(msg);
                    if (isArray)
                    {
                        mis = new MethodItem("GetList" + cfgName, MemberAttributes.Final | MemberAttributes.Public, new List<string>() { });
                        SetComment("获取List<" + cfgName + ">", mis.Method);
                        strListType = "List<" + cfgName + ">";
                        mis.Method.ReturnType = new CodeTypeReference(strListType);
                        content = dicFunctions["GetListCfgNameInArray"];
                        content = content.Replace("{TableName}", linkTable);
                        content = content.Replace("{CfgName}", cfgName);
                        content = content.Replace("{Param}", attrName);
                        content = content.Replace("{Type}", "int");

                        state = new CodeSnippetStatement(content);
                        mis.Method.Statements.Add(state);
                        methodList.Add(mis);
                    }
                    else
                    {
                        mis = new MethodItem("Get" + cfgName, MemberAttributes.Final | MemberAttributes.Public, new List<string>() { });
                        SetComment("获取" + cfgName, mis.Method);
                        mis.Method.ReturnType = new CodeTypeReference(cfgName);
                        content = dicFunctions["GetCfgName"];
                        content = content.Replace("{TableName}", linkTable);
                        content = content.Replace("{Param}", attrName);

                        state = new CodeSnippetStatement(content);
                        mis.Method.Statements.Add(state);
                        methodList.Add(mis);
                    }
                }

                fieldList.Add(new FieldItem(arrContruct[0], arrContruct[1], MemberAttributes.Private));
                item = new PropertyItem(arrContruct[0]);
                item.SetGetName();
                item.SetSetName();

                item.SetComment(arrContruct[2]);
                item.SetValueType(arrContruct[1]);
                item.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                item.SetField("ProtoMember", (i + 1).ToString());
                propertyList.Add(item);

                if (ss[2] == "1")// 如果该属性是类的Key值，则加入列表
                {
                    if (m_mainKey == null)
                    {
                        m_mainKey = attrName;
                    }
                    string strType = item.Property.Type.BaseType;
                    if (strType == "System.Int32" || strType == "System.Int64")
                    {

                        string[] arrType = arrContruct[1].Split('+');
                        string keyBit = "";
                        if (arrType.Length == 4)
                        {
                            keyBit = arrType[3];
                        }
                        keyList.Add(new KeyValuePair<string, string>(attrName, keyBit));
                    }
                    else
                    {
                        throw new Exception(tableName + "的主键" + attrName + "必须是整数类型！");
                    }
                }
            }

            //Key 属性
            int keyCnt = keyList.Count;
            if (keyCnt > 0)
            {
                keyPropertyItem = new PropertyItem("mkey");
                if (keyCnt > 1)
                {
                    this.m_keyType = "long";
                    List<Type> paramTypes = new List<Type>();
                    List<string> paramNames = new List<string>();
                    string strGenKey = "\t\treturn ";
                    for (int i = 0; i < keyCnt - 1; i++)
                    {
                        attrName = keyList[i].Key;
                        bit = keyList[i].Value;
                        //不配置的话默认16位累加
                        if (bit == "")
                        {
                            bit = "" + 16 * (keyCnt - 1 - i);
                        }
                        ks += "((long)this.m_" + attrName + " << " + bit + ") + ";
                        paramTypes.Add(typeof(System.Int32));
                        paramNames.Add(attrName);
                        strGenKey += "((long)" + attrName + " << " + bit + ") + ";
                    }

                    attrName = keyList[keyCnt - 1].Key;
                    paramTypes.Add(typeof(System.Int32));
                    paramNames.Add(attrName);
                    strGenKey += attrName + ";";

                    //GenKey方法
                    mis = new MethodItem("GenKey", MemberAttributes.Final | MemberAttributes.Public | MemberAttributes.Static, paramTypes, paramNames);
                    SetComment("GenKey方法", mis.Method);
                    mis.Method.ReturnType = new CodeTypeReference(typeof(System.Int64));
                    state = new CodeSnippetStatement(strGenKey);
                    mis.Method.Statements.Add(state);
                    methodList.Add(mis);
                }
                ks += "this.m_" + keyList[keyCnt - 1].Key;
                ks = "\t\t\treturn " + ks + ";";
                keyPropertyItem.SetGetName(ks, true);
                if (this.m_keyType == "int")
                {
                    keyPropertyItem.SetValueType(typeof(System.Int32));
                }
                else if (this.m_keyType == "long")
                {
                    keyPropertyItem.SetValueType(typeof(System.Int64));
                }

                keyPropertyItem.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                keyPropertyItem.SetComment("类的Key值");
                propertyList.Add(keyPropertyItem);
            }
        }


        public string GetKeyType()
        {
            return this.m_keyType;
        }

        public string GetMainKey()
        {
            return this.m_mainKey;
        }

        public void CreateCode(Dictionary<string,ExcelToCSharp> dicEc)
        {
            if(m_linkMethod != null)
            {
                string mainKey = dicEc[m_linkTableName].GetMainKey();
                string content = m_linkMethodContent.Replace("{MainKey}",mainKey);

                CodeSnippetStatement state = new CodeSnippetStatement(content);
                m_linkMethod.Method.Statements.Add(state);
                methodList.Add(m_linkMethod);
            }
            
            Create();
        }
    }
}
