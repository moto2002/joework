using UnityEngine;
using System.Collections;

public class ScaleTexcoordTest : MonoBehaviour
{
    private float widthRate;
    private float heightRate;
    private float xOffsetRate;
    private float yOffsetRate;
    private UISprite sprite;

    float curr;

    void Awake()
    {
        sprite = GetComponent<UISprite>();
        widthRate = sprite.GetAtlasSprite().width * 1.0f / sprite.atlas.spriteMaterial.mainTexture.width;
		heightRate = sprite.GetAtlasSprite().height * 1.0f / sprite.atlas.spriteMaterial.mainTexture.height;
        xOffsetRate = sprite.GetAtlasSprite().x * 1.0f / sprite.atlas.spriteMaterial.mainTexture.width;
		yOffsetRate = (sprite.atlas.spriteMaterial.mainTexture.height-(sprite.GetAtlasSprite().y + sprite.GetAtlasSprite().height)) * 1.0f / sprite.atlas.spriteMaterial.mainTexture.height;
    }

    private void Start()
    {
        sprite.atlas.spriteMaterial.SetFloat("_WidthRate", widthRate);
        sprite.atlas.spriteMaterial.SetFloat("_HeightRate", heightRate);
        sprite.atlas.spriteMaterial.SetFloat("_XOffset", xOffsetRate);
        sprite.atlas.spriteMaterial.SetFloat("_YOffset", yOffsetRate);
    }

    
    void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 50), "加"))
        {
            curr += 0.1f;
            sprite.color = new Color(0,curr,1);
        }

        if (GUI.Button(new Rect(220, 100, 100, 50), "减"))
        {
            curr -= 0.1f;
            sprite.color = new Color(0, curr, 1);
        }
    }
}
