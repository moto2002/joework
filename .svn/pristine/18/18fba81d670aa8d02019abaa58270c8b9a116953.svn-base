/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：ConditionBase
 * 简    述： 
 * 创建时间：2015/7/14 14:07:21
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;

namespace AutoCSharp.Creator
{
    public class ConditionBase
    {
        CodeConditionStatement Condition;

        public ConditionBase(string inCondition)
        {
            Condition = new CodeConditionStatement();
            Condition.Condition = new CodeVariableReferenceExpression(inCondition);
        }
    }
}
