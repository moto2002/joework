/*
 * Copyright (c) Killliu
 * All rights reserved.
 * 
 * 文 件 名：CreatePBData
 * 简    述：
 * 创建时间：2015/8/14 9:14:39
 * 创 建 人：刘沙
 * 修改描述：
 * 修改时间：
 * */
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace AutoCSharp.Creator
{
    public class CreatePBData : Base
    {
        public CreatePBData(string inSpace, string inClassName, string inFolderName)
            : base(inSpace, inClassName, inFolderName) { }

        public void SetValue(Dictionary<string, string[]> inDic)
        {
            usingList.Add("ProtoBuf");
            usingList.Add("System.Collections.Generic");

            classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));

            List<string> classnames = new List<string>();
            List<string> classexpla = new List<string>();
            List<string> classType = new List<string>();

            foreach (KeyValuePair<string, string[]> item in inDic)
            {
                classnames.Add(item.Key);
                classexpla.Add(item.Value[0]);
                classType.Add(item.Value[1]);
            }

            string strType;
            for (int i = 0; i < classnames.Count; i++)
            {
                strType = classType[i];
                string classname = Assist.FirstLetterUp(classnames[i]);
                FieldItem field = new FieldItem("Dictionary<" + strType + ",Cfg" + classname + ">", classname + "Dic", "new " + "Dictionary<" + strType + ",Cfg" + classname + ">()");
                field.SetAttributes(MemberAttributes.Final | MemberAttributes.Public);
                field.AddAttributes("ProtoMember", i + 1);
                fieldList.Add(field);

                SetComment(classexpla[i], field.Field);
            }
            Create();
        }

        //public void SetValue(List<string> inNames)
        //{
        //    usingList.Add("ProtoBuf");
        //    usingList.Add("System.Collections.Generic");

        //    classer.CustomAttributes.Add(new CodeAttributeDeclaration("ProtoContract"));

        //    for (int i = 0; i < inNames.Count; i++)
        //    {
        //        string classname = Assist.FirstLetterUp(inNames[i]);
        //        FieldItem field = new FieldItem("Dictionary<string," + classname + ">", classname + "Dic", "new " + "Dictionary<string," + classname + ">()");
        //        field.SetAttributes(MemberAttributes.Final | MemberAttributes.Public);
        //        field.AddAttributes("ProtoMember", i + 1);
        //        fieldList.Add(field);
        //    }
        //    Create();
        //}
    }
}
