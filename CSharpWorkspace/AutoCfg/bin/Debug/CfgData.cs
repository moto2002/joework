/*
 * Copyright (c) shihuanjue.com
 * All rights reserved.
 * 
 * 文 件 名：DataPool
 * 简    述： 
 * 创建时间：2015/12/3 16:47:26
 * 创 建 人：洪峤
 * 修改描述：
 * 修改时间：
 * */
using System;
using HuanJueData;


public class CfgData
{
    private static CfgData m_instance;
    public static CfgData Instance
    {
        get {
            if (m_instance == null)
                m_instance = new CfgData();
            return m_instance;
        }
    }

   public PBData m_PBData = new PBData();

}

