using UnityEngine;
using System.Collections;
using UnityEditor;

public class Excel2Json2Cs : EditorWindow
{
	//默认路径;
     string excelPath = Application.dataPath + "/output/excel";
	string jsonPath = Application.dataPath + "/output/json";
	string csharpPath = Application.dataPath + "/output/csharp";

    [MenuItem("Tools/Excel2Json2C#")]
    public static void Execute()
    {
		GetWindow(typeof(Excel2Json2Cs));
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 100, 50), "请选择Excel路径"))
        {
			excelPath = EditorUtility.OpenFolderPanel("Choose Excel Path", "", "");
        }
		GUI.Label(new Rect(120,30,600,30),excelPath);

        if (GUI.Button(new Rect(10, 70, 100, 50), "请选择Json路径"))
        {
			jsonPath = EditorUtility.OpenFolderPanel("Choose Json Path", "", "");
        }
		GUI.Label(new Rect(120,90,600,30),jsonPath);

        if (GUI.Button(new Rect(10, 130, 100, 50), "请选择C#路径"))
        {
			csharpPath = EditorUtility.OpenFolderPanel("Choose CSharp Path", "", "");
        }
		GUI.Label(new Rect(120,150,600,30),csharpPath);


		if(GUI.Button(new Rect(150,250,100,50),"Excel2Json"))
		{
			if(Generator.Excel2Json(excelPath,jsonPath))
			{
				EditorUtility.DisplayDialog("","完成","确定");
			}
			else
			{
				EditorUtility.DisplayDialog("", "出错了", "确定");
			}

			AssetDatabase.Refresh();
		}

		if(GUI.Button(new Rect(300,250,100,50),"Json2CSharp"))
		{
			if(Generator.Json2CSharp(jsonPath,csharpPath))
			{
				EditorUtility.DisplayDialog("","完成","确定");
			}
			else
			{
				EditorUtility.DisplayDialog("", "出错了", "确定");
			}

			AssetDatabase.Refresh();
		}
    }
}
