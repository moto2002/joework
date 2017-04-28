/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ConstructItem
 * 简    述：构造函数
 * 创建时间：2015/8/16 17:35:43
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AutoCSharp.Creator
{
    /// <summary>
    /// 构造函数
    /// </summary>
    public class ConstructItem
    {
        private CodeConstructor construct;
        /// <summary>
        /// 构造函数
        /// </summary>
        public CodeConstructor Struct { get { return construct; } }

        /// <summary>
        /// 无参
        /// </summary>
        public ConstructItem()
        {
            construct = new CodeConstructor();
            construct.Attributes = MemberAttributes.Public;
        }

        /// <summary>
        /// 有参
        /// </summary>
        /// <param name="inPars"></param>
        public ConstructItem(List<string> inPars)
        {
            construct = new CodeConstructor();
            for (int i = 0; i < inPars.Count; i++)
            {
                construct.Parameters.Add(new CodeParameterDeclarationExpression(inPars[i], "inArg" + i));
            }
            construct.Attributes = MemberAttributes.Public;
        }    
    }
}
