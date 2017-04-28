using UnityEngine;
using System.Collections;
using System.IO;
using UnityEngine.UI;

public class GifTest : MonoBehaviour {

    public RawImage rawImage;
	void Start ()
    {
        string path = @"F:\JoeWorkspace\UnityWorkspace\learn_wangzherongyao_code\Assets\Gif_moudle\tuzi.gif";
        byte[] array  = File.ReadAllBytes(path);// CFileManager.ReadFile(path);
        Texture2D texture2D = null;
        using (MemoryStream memoryStream = new MemoryStream(array))
        {
            rawImage.texture = GifHelper.GifToTexture(memoryStream, 0);
        }
        
    }
	
	void Update () {
	
	}
}
