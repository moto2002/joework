/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：MethodItemBase
 * 简    述： 
 * 创建时间：2015/7/13 21:03:18
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AutoCSharp.Creator
{
    public class MethodItem
    {
        private CodeMemberMethod method;
        public CodeMemberMethod Method { get { return method; } }

        protected internal CodeConditionStatement statement;

        public MethodItem(string inName, MemberAttributes inAtt, List<string> inParameters,List<string> paramNames=null)
        {
            method = new CodeMemberMethod();
            method.Name = inName;
            method.Attributes = inAtt;
            string paramName;
            for (int i = 0; i < inParameters.Count; i++)
            {
                if (paramNames == null)
                {
                    paramName = "inArg" + i;
                }
                else
                {
                    paramName = paramNames[i];
                }
                method.Parameters.Add(new CodeParameterDeclarationExpression(inParameters[i], paramName));
            }
            statement = new CodeConditionStatement();
        }
        public MethodItem(string inName, MemberAttributes inAtt, List<Type> inParameters, List<string> paramNames = null)
        {
            method = new CodeMemberMethod();
            method.Name = inName;
            method.Attributes = inAtt;
            string paramName;
            for (int i = 0; i < inParameters.Count; i++)
            {
                if (paramNames == null)
                {
                    paramName = "inArg" + i;
                }
                else
                {
                    paramName = paramNames[i];
                }
                method.Parameters.Add(new CodeParameterDeclarationExpression(inParameters[i], paramName));
            }
            statement = new CodeConditionStatement();
        }

        public void SetReturn(string s)
        {
            method.ReturnType = new CodeTypeReference(s);
        }
    }
}
