/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：CreateCSItemData
 * 简    述： 
 * 创建时间：2015/11/30 23:37:37
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

public class CreateCSItemData : ToCSharpBase
{
    public CreateCSItemData(string inSpace, string inClassName, string inFolderName)
          : base(inSpace, inClassName, inFolderName)
    {
        usingList.Add("ProtoBuf");
        usingList.Add("System.Collections.Generic");
        classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));
    }

    //public void SetFieldsValue(List<string> inNames)
    //{

    //}

    public void AddFieldValue(ItemField itemField)
    {
        fieldList.Add(itemField);
    }

    public void AddPropertyValue(ItemProperty itemProperty)
    {
        propertyList.Add(itemProperty);
    }

    public void AddMethodValue(ItemMethod itemMethod)
    {
        methodList.Add(itemMethod);
    }
}

