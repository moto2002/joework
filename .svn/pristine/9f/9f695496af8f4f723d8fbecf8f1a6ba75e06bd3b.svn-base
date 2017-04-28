/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：GenManager
 * 简    述： 
 * 创建时间：2015/11/17 22:42:10
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;


public class GenManager
{
    private static GenManager m_instance;
    public static GenManager Instance
    {
        get {
            if (m_instance == null)
                m_instance = new GenManager();
            return m_instance;
        }
    }

    public void Gen_Excel2Xml()
    {
        Excel2Xml excel2xml = new Excel2Xml();
        excel2xml.Gen();

    }

    public void Gen_Excel2Json()
    {
        Excel2Json excel2json = new Excel2Json();
        excel2json.Gen();
    }

    public void Gen_Excel2CSV()
    {
        Excel2CSV excel2csv = new Excel2CSV();
        excel2csv.Gen();
    }

    public void Gen_Excel2CSharp()
    {
        Excel2CSharp excel2csharp = new Excel2CSharp();
        excel2csharp.Gen();
    }

    public void Gen_PBData()
    {
        GenPBDataBin genPBData = new GenPBDataBin();
        genPBData.Gen();
    }
}

