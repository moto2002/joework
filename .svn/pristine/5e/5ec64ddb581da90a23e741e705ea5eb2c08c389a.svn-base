using UnityEngine;
using System.Collections.Generic;

public class XmlExample : MonoBehaviour
{
    void Start()
    {
        //----------------------------------------------------------------------------------<>
        //·添加根节点和子节点
        //		普通添加父节点子节点的方法。
        //		addRootNode("节点名, 值);
        //		addChildNode("父节点名, 节点名, 值);
        //----------------------------------------------------------------------------------<>
        XmlRW xmlRW = new XmlRW();
        xmlRW.addRootNode("player1", "");
        xmlRW.addChildNode("player1", "name", "Jonh");
        xmlRW.addRootNode("player2", "");
        xmlRW.addChildNode("player2", "name", "Jam");
        xmlRW.addRootNode("player3", "");
        xmlRW.addChildNode("player3", "name", "nax");


        //----------------------------------------------------------------------------------<>
        //·放入哈希表到某个节点下。
        //		也可以直接放入一个字典(哈希表)进去。
        //		这样写几个for循环就能保存所有数据。
        //			重载1:addDictToNode(根节点,字典名);
        //			重载2:addDictToNode(字典名);	默认为 在根节点下存放数据。
        //----------------------------------------------------------------------------------<>
        var dict = new Dictionary<string, string>();
        dict.Add("Lv", "1");
        dict.Add("hp", "7000");
        dict.Add("mp", "4000");
        dict.Add("ap", "4");
        dict.Add("dkp", "10000");

        //XmlRW.addDictToNode(dict);
        xmlRW.addRootNode("other", "");
        xmlRW.addDictToNode("other", dict);


        //----------------------------------------------------------------------------------<>
        //·创建一个临时的节点
        //		在一些情况下，需要创建一个临时节点，在合适的时候使用。	
        //----------------------------------------------------------------------------------<>
        //
        xmlRW.createTempNode("tempNode", "");
        xmlRW.addChildNode("tempNode", "hp", "50");
        xmlRW.addChildNode("tempNode", "mp", "60");
        xmlRW.addTempNode("other", "tempNode");


        //----------------------------------------------------------------------------------<>
        //写入数据到硬盘
        //----------------------------------------------------------------------------------<>
        //
        xmlRW.dataWrite("setting.xml");


        //----------------------------------------------------------------------------------<>
        //读入测试
        //----------------------------------------------------------------------------------<>
        xmlRW.dataRead("setting.xml");
        foreach (var go in xmlRW.getAllNode())
        {
            print("name: " + go.name + "    value:    " + go.text);
        }


        //----------------------------------------------------------------------------------<>
        //·读取指定节点
        //使用路径的方式指定读取xml数据。根节点则不用填写。
        //节点在该类中使用Node结构类来存放。
        //----------------------------------------------------------------------------------<>
        //
        print("------------------------------------------------\n");
        print("my dkp is :   " + xmlRW.findNode("other/dkp").text);
        print("node name :   " + xmlRW.findNode("other/dkp").name);


        //----------------------------------------------------------------------------------<>
        //·遍历所有子节点
        //----------------------------------------------------------------------------------<>
        //
        foreach (var go in xmlRW.getAllChildNode("other"))
        {
            print("other Inner Element :   " + go.name + "   Content: " + go.text);
        }
    }
}


//***********************************************************
//*	       XML_ReadWrite1.0           						*
//*                                   						*
//*	    xml元素是以名为Node结构类形式，存放的				*
//*		Node.name是元素的name信息，Node.text是元素的内容	*
//*		Node.path是元素的路径。								*
//*															*
//*		XMLRW的组成结构										*
//*			-元素添加创建模块								*
//*			-搜索查询模块									*
//*                                   						*
//***********************************************************
//***********************************************************
//*
//*	Functions:
//*-------------------------------------------------------------------<>
//*		-搜索查询类型函数:
//*	 		Node findNode(string p)						
//*														//搜索一个节点
//*														//示例在85-87行
//*			List<Node>	findSameNameNode(string n)		
//*														//搜索全局路径中同样名字的节点	如"player1/hp","player2/hp"。名为hp的节点就会被返回。
//*														//使用时，尽量使用迭代去遍历它的结果。
//*			List<Node> findSameNameNode(string p, string n)
//*														//搜索某路径下同样名字的节点	p是搜索路径，n是搜索名。
//*														
//*			List<Node>	getAllNode						
//*														//获得全局路径下所有节点
//*	
//*			List<Node>	getAllChildNode(string p)		
//*														//获得子目录下的所有节点。
//*														//示例在96 -99行。
//*
//*
//*-------------------------------------------------------------------<>
//*		-元素添加创建类型函数:
//*			void dataRead(string name)
//*														//读取当前目录下的xml数据文件。
//*														//例如：dataRead("setting.xml");
//*	
//*			void dataRead(string valPath, string name)
//*														//读取指定目录下的xml数据文件。
//*														//例如：dataRead(@"C:\", setting.xml);
//*
//*			void dataWrite(string name)
//*														//写入指定目录下的xml数据文件。
//*														//例如：dataWrite(setting.xml);
//*
//*
//*			void dataWrite(string valPath, string name)
//*														//写入指定目录下的xml数据文件。
//*														//例如：dataWrite(@"C:\", setting.xml);
//*
//*			void addDictToNode(string upPath, Dictionary<string, string> dict)
//*														//添加一个字典类数据到一个节点目录下。
//*														//示例在40行-50行。
//*
//*			void addRootNode(string n, string text)
//*														//添加根节点
//*														//string n是节点名，text是节点内容。
//*														//示例在23-29行。
//*
//*			void addChildNode(string upPath, string n, string text)
//*														//添加子节点
//*														//示例在23行-29行。
//*
//*			void createTempNode(string n, string text)
//*														//创建一个不在全局路径下的临时节点.
//*														//示例在56-59行。
//*														
//*			void addTempNode(string upPath, string NodePath)
//*														//把创建好的节点，赋给其他节点。
//*														//示例在56-69行。
//*