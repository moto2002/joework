/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：XmlCreater
 * 简    述：
 * 创建时间：2015/7/13 15:06:29
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Xml;

namespace AutoCSharp.Creator
{
    public class XmlUnit : Base
    {
        public XmlUnit(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName)
        {
            // 引用
            usingList.Add("System");
            usingList.Add("System.Linq");
            usingList.Add("System.Text");
            usingList.Add("System.Collections.Generic");

            // 方法列表
            methodList.Add(new MethodItem("SetValue", MemberAttributes.Public | MemberAttributes.Final, new List<string>() { "Dictionary<string, string>" }));
        }

        public void SetNodeValue(XmlNode n)
        {
            for (int i = 0; i < n.Attributes.Count; i++)
            {
                string value = n.Attributes[i].Value;

                //AddField(n.Attributes[i].Name, value, MemberAttributes.Private);
                fieldList.Add(new FieldItem(n.Attributes[i].Name, value, MemberAttributes.Private));

                PropertyItem item = new PropertyItem(n.Attributes[i].Name);
                item.SetGetName();
                item.SetValueType(value);
                item.SetModifier(MemberAttributes.Public | MemberAttributes.Final);
                propertyList.Add(item);

                CodeConditionStatement condition = new CodeConditionStatement();
                condition.Condition = new CodeVariableReferenceExpression("inArg0.ContainsKey(\"" + n.Attributes[i].Name + "\")");

                string parseLeft = "";
                string parseRight = "";
                if (Assist.IsNumber(value))
                {
                    parseLeft = value.Contains(".") ? "float.Parse(" : "uint.Parse(";
                    parseRight = ")";
                }
                CodeVariableReferenceExpression right = new CodeVariableReferenceExpression(parseLeft + "inArg0[\"" + n.Attributes[i].Name + "\"]" + parseRight);
                CodePropertyReferenceExpression left = new CodePropertyReferenceExpression(new CodeThisReferenceExpression(), "_" + Assist.FirstLetterLower(n.Attributes[i].Name));

                if (Assist.IsNumber(value))
                {
                    CodeConditionStatement numCondition = new CodeConditionStatement();
                    numCondition.Condition = new CodeVariableReferenceExpression("inArg0[\"" + n.Attributes[i].Name + "\"] == \"\"");

                    numCondition.TrueStatements.Add(new CodeAssignStatement(left, new CodeVariableReferenceExpression("0")));
                    numCondition.FalseStatements.Add(new CodeAssignStatement(left, right));

                    condition.TrueStatements.Add(numCondition);
                }
                else
                {
                    condition.TrueStatements.Add(new CodeAssignStatement(left, right));
                }

                AddConditionStatement(condition);
            }
            Create();
        }
    }
}
