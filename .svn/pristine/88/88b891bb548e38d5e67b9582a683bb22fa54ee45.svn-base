/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：FiledItem
 * 简    述： 
 * 创建时间：2015/7/13 16:44:50
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
namespace AutoCSharp.Creator
{
    public class FieldItem
    {
        private CodeMemberField field;
        /// <summary>
        /// 字段
        /// </summary>
        public CodeMemberField Field { get { return field; } }

        /// <summary>
        /// 字段
        /// </summary>
        /// <param name="inName">字段名</param>
        /// <param name="inType">字段类型</param>
        /// <param name="inAtt">访问修饰符</param>
        public FieldItem(string inName, string inType, MemberAttributes inAtt)
        {
            object objType = Assist.stringToType(inType);
            if (objType is string)
            {
                field = new CodeMemberField((string)objType, "m_" + Assist.FirstLetterLower(inName));
            }
            else
            {
                field = new CodeMemberField((Type)objType, "m_" + Assist.FirstLetterLower(inName));
                if (Assist.isArray(inType))
                {
                    field.Type.BaseType = "List";
                }
            }
            
            field.Attributes = inAtt;
        }

        /// <summary>
        /// 字段
        /// <para>eg. public Dictionary<string,string> TestDic = new Dictionary<string,string>();</para>
        /// </summary>
        /// <param name="inLeft">字段类型</param>
        /// <param name="inFieldName"></param>
        /// <param name="inRight"></param>
        public FieldItem(string inLeft, string inFieldName, string inRight = "", MemberAttributes inAtt = MemberAttributes.Private)
        {
            inLeft = Assist.GetFieldType(inLeft);
            if(inLeft.Contains("SMGame."))
            {
                inLeft = inLeft.Replace("SMGame.","");
            }
            field = new CodeMemberField(inLeft, inFieldName);
            if (inRight != "")
            {
                CodeVariableReferenceExpression right = new CodeVariableReferenceExpression(inRight);
                field.InitExpression = right;
            }
            field.Attributes = inAtt;
        }

        /// <summary>
        /// 访问修饰符
        /// </summary>
        /// <param name="inAtt"></param>
        public void SetAttributes(MemberAttributes inAtt)
        {
            field.Attributes = inAtt;
        }

        /// <summary>
        /// 自定义特性
        /// <para>eg. [ProtoMember(1)]</para>
        /// </summary>
        /// <param name="inKey"></param>
        /// <param name="inValue"></param>
        public void AddAttributes(string inKey, object inValue)
        {
            field.CustomAttributes.Add(new CodeAttributeDeclaration(inKey, new CodeAttributeArgument(new CodePrimitiveExpression(inValue))));
        }
    }
}
