using UnityEngine;

public class TextureLoadTest : MonoBehaviour
{
	void Start ()
    {

        string path = Application.persistentDataPath + "/Example.jpg";
        transform.GetComponent<UnityEngine.UI.Image>().sprite = Assist.LoadSprite(path, 128, 128).toSprite();
    }
}
