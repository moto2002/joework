using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Xml;
using System.IO;

/// <summary>
/// 解析texturepacker图集 , 转成Sprites
/// Author:bingheliefeng
/// </summary>
public class TexturePackerEditor : ScriptableWizard {

	public Texture2D altasTexture;
	public TextAsset altasTextAsset;
	[MenuItem("Tools/TexturePacker/图集生成Sprites(手动)")]
	static void CreateWizard () {
		ScriptableWizard.DisplayWizard<TexturePackerEditor>("Create Sprites", "Create");
	}

	public void OnWizardCreate(){
		if(altasTexture && altasTextAsset){
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(altasTextAsset.ToString());
			XmlNode root = xmlDoc.SelectSingleNode("TextureAtlas");
			Texture2D t = altasTexture;
			XmlNodeList nodeList =root.ChildNodes;
			List<SpriteMetaData> metaDatas=new List<SpriteMetaData>();
			//遍历所有子节点
			foreach (XmlNode xn in nodeList)
			{
				XmlElement xe = (XmlElement)xn;
				string name = xe.GetAttribute("name").Replace('/','_');
				float x = float.Parse( xe.GetAttribute("x"));
				float y = float.Parse( xe.GetAttribute("y"));
				float w = float.Parse( xe.GetAttribute("width"));
				float h = float.Parse( xe.GetAttribute("height"));
//				float fx = x;
//				float fy = y;
//				float fw = w;
//				float fh = h;
//				if(xe.HasAttribute("fx"))
//					fx = float.Parse( xe.GetAttribute("frameX"));
//				if(xe.HasAttribute("fy"))
//					fy = float.Parse( xe.GetAttribute("frameY"));
//				if(xe.HasAttribute("fw"))
//					fw = float.Parse( xe.GetAttribute("frameWidth"));
//				if(xe.HasAttribute("fh"))
//					fh = float.Parse( xe.GetAttribute("frameHeight"));

				SpriteMetaData metaData = new SpriteMetaData();
				metaData.name = name;
				metaData.rect = new Rect(x,t.height-h-y,w,h);
				metaDatas.Add(metaData);
			}

			if(metaDatas.Count>0){
				string textureAtlasPath = AssetDatabase.GetAssetPath(t);
				TextureImporter textureImporter = AssetImporter.GetAtPath(textureAtlasPath) as TextureImporter;
				textureImporter.maxTextureSize = 2048;
				textureImporter.spritesheet = metaDatas.ToArray();
				textureImporter.textureType = TextureImporterType.Sprite;
				textureImporter.spriteImportMode = SpriteImportMode.Multiple;
				textureImporter.spritePixelsPerUnit = 100;
				AssetDatabase.ImportAsset(textureAtlasPath, ImportAssetOptions.ForceUpdate);
			}
		}
	}

	[MenuItem("Tools/TexturePacker/选中图集配置生成Sprites(自动)")]
	static void ParseXML(){
		if(Selection.activeObject && Selection.activeObject is TextAsset)
		{
			string xmlPath = AssetDatabase.GetAssetPath(Selection.activeObject);
			string xml = AssetDatabase.LoadAssetAtPath<TextAsset>(xmlPath).text.ToString();
			XmlDocument xmlDoc = new XmlDocument();
			xmlDoc.LoadXml(xml);
			XmlNode root = xmlDoc.SelectSingleNode("TextureAtlas");
			XmlElement rootEle = (XmlElement)root;
			string imagePath = "";
			if(rootEle.HasAttribute("imagePath")){
				imagePath =rootEle.GetAttribute("imagePath");
			}
			else if(rootEle.HasAttribute("name")){
				imagePath =rootEle.GetAttribute("name")+".png";
			}

			imagePath = xmlPath.Substring( 0, xmlPath.LastIndexOf('/') ) +"/"+imagePath;
			Texture2D t = AssetDatabase.LoadAssetAtPath<Texture2D>(imagePath);
			if(t==null) return;

			XmlNodeList nodeList =root.ChildNodes;
			List<SpriteMetaData> metaDatas=new List<SpriteMetaData>();
			//遍历所有子节点
			foreach (XmlNode xn in nodeList)
			{
				XmlElement xe = (XmlElement)xn;
				string name = xe.GetAttribute("name").Replace('/','_');
				float x = float.Parse( xe.GetAttribute("x"));
				float y = float.Parse( xe.GetAttribute("y"));
				float w = float.Parse( xe.GetAttribute("width"));
				float h = float.Parse( xe.GetAttribute("height"));
//				float fx = x;
//				float fy = y;
//				float fw = w;
//				float fh = h;
//				if(xe.HasAttribute("fx"))
//					fx = float.Parse( xe.GetAttribute("frameX"));
//				if(xe.HasAttribute("fy"))
//					fy = float.Parse( xe.GetAttribute("frameY"));
//				if(xe.HasAttribute("fw"))
//					fw = float.Parse( xe.GetAttribute("frameWidth"));
//				if(xe.HasAttribute("fh"))
//					fh = float.Parse( xe.GetAttribute("frameHeight"));

				SpriteMetaData metaData = new SpriteMetaData();
				metaData.name = name;
				metaData.rect = new Rect(x,t.height-h-y,w,h);
				metaDatas.Add(metaData);
			}

			if(metaDatas.Count>0){
				string textureAtlasPath = AssetDatabase.GetAssetPath(t);
				TextureImporter textureImporter = AssetImporter.GetAtPath(textureAtlasPath) as TextureImporter;
				textureImporter.maxTextureSize = 2048;
				textureImporter.spritesheet = metaDatas.ToArray();
				textureImporter.textureType = TextureImporterType.Sprite;
				textureImporter.spriteImportMode = SpriteImportMode.Multiple;
				textureImporter.spritePixelsPerUnit = 100;
				AssetDatabase.ImportAsset(textureAtlasPath, ImportAssetOptions.ForceUpdate);
			}
		}
	}

	[MenuItem("Tools/TexturePacker/选中Sprites合并成图集",false,0)]
	static void PackSpriteToAtlas()
	{
		Object[] objs = Selection.GetFiltered(typeof(Texture2D),SelectionMode.DeepAssets);
		if(objs!=null&&objs.Length>0)
		{
			string atlasPath = AssetDatabase.GetAssetPath(objs[0]);
			atlasPath = atlasPath.Substring(0,atlasPath.LastIndexOf('/'));

			Texture2D atlas = new Texture2D(2048,2048,TextureFormat.ARGB32,false,true);
			List<Texture2D> textures = new List<Texture2D>();
			foreach(Object o in objs)
			{
				if(o is Texture2D)
				{
					Texture2D t = o as Texture2D;
					t = new Texture2D(1,1,TextureFormat.ARGB32,false,true);
					t.name = o.name;
					t.LoadImage(File.ReadAllBytes(Path.GetFullPath(AssetDatabase.GetAssetPath(o))));
					textures.Add(t);
				}
			}
			Rect[] rects = atlas.PackTextures(textures.ToArray(),2,2048);
			string atlasFilePath = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf('/'))+"/"+atlasPath+"/Atlas.png";
			File.WriteAllBytes( atlasFilePath , atlas.EncodeToPNG());
			AssetDatabase.Refresh();

			List<SpriteMetaData> Sprites = new List<SpriteMetaData>();
			for (int i = 0; i < rects.Length; i++) {
				SpriteMetaData smd = new SpriteMetaData();
				smd.name = textures[i].name;
				smd.rect = new Rect(rects[i].xMin * atlas.width, rects[i].yMin * atlas.height, rects[i].width * atlas.width, rects[i].height * atlas.height);
				smd.pivot = new Vector2(0.5f, 0.5f); 
				smd.alignment = (int)SpriteAlignment.Center;
				Sprites.Add(smd);
			}
			TextureImporter textureImporter = AssetImporter.GetAtPath(atlasPath+"/Atlas.png") as TextureImporter;
			textureImporter.maxTextureSize = 2048;
			textureImporter.spritesheet = Sprites.ToArray();
			textureImporter.textureType = TextureImporterType.Sprite;
			textureImporter.spriteImportMode = SpriteImportMode.Multiple;
			textureImporter.spritePivot = new Vector2(0.5f, 0.5f);
			textureImporter.spritePixelsPerUnit = 100;
			textureImporter.mipmapEnabled=false;
			AssetDatabase.ImportAsset(atlasPath+"/Atlas.png", ImportAssetOptions.ForceUpdate);

			AssetDatabase.Refresh(ImportAssetOptions.ForceUpdate);
		}
	}
}
