/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：CreatePBData
 * 简    述： 
 * 创建时间：2015/11/30 23:15:16
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

public class CreatePBData : ToCSharpBase
{
    public CreatePBData(string inSpace, string inClassName, string inFolderName)
          : base(inSpace, inClassName, inFolderName)
    {

        usingList.Add("ProtoBuf");
        usingList.Add("System.Collections.Generic");

        classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));
    }

    public void SetValue(List<string> inNames)
    {
        for (int i = 0; i < inNames.Count; i++)
        {
            string classname = Stringer.FirstLetterUp(inNames[i]);
            
            ItemField field = new ItemField("Dictionary<string," + classname + ">", classname + "Dic", "new " + "Dictionary<string," + classname + ">()");
            field.SetAttributes(MemberAttributes.Final | MemberAttributes.Public);
            field.AddAttributes("ProtoMember", i + 1);
            fieldList.Add(field);
        }
       // Create();
    }
}

