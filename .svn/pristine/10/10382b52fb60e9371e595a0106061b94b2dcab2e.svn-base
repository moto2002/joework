/*
 * Copyright (c) Killliu 
 * All rights reserved.
 * 
 * 文 件 名：ProtoCSItem
 * 简    述：
 * 创建时间：2015/9/12 10:09:15
 * 创 建 人：刘 沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// .proto -> .cs 属性项
/// </summary>
public class ProtoCSItem
{
    private string name;
    /// <summary>
    /// 属性名
    /// </summary>
    public string Name { get { return name; } }

    private Type mType;
    /// <summary>
    /// 
    /// </summary>
    public Type MType { get { return mType; } }

    private bool isArray;
    /// <summary>
    /// 数组
    /// </summary>
    public bool IsArray 
    {
        set { isArray = value; }
        get { return isArray; } 
    }

    private bool isSelfDefine;
    /// <summary>
    /// 自定义类型
    /// </summary>
    public bool IsSelfDefine
    {
        set { isSelfDefine = value; }
        get { return isSelfDefine; }
    }

    private string arrayLengthName;
    /// <summary>
    /// 数组长度对应的属性名(eg. 用“count”描述数组长度时，其值为“count”)
    /// </summary>
    public string ArrayLengthName
    {
        set { arrayLengthName = value; }
        get { return arrayLengthName; }
    }

    public ProtoCSItem(string inName, Type inType)
    {
        name = inName;
        mType = inType;
    }
}
