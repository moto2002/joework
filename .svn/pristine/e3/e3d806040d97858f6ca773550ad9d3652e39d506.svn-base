using UnityEngine;
using System.Collections;

public class test : MonoBehaviour
{
	public UISprite sprite1;
	public UISprite sprite2;
	public UISprite sprite3;

    public Material default_mat;
	public Material mask1_mat;
	public Material mask2_mat;

	void OnGUI()
	{
		if(GUI.Button( new Rect(0,100,100,50),"过滤图1"))
		{
			sprite1.atlas.spriteMaterial = mask1_mat;
		}

		if(GUI.Button( new Rect(110,100,100,50),"过滤图2"))
		{
			sprite1.atlas.spriteMaterial = mask2_mat;
		}

		if(GUI.Button( new Rect(0,160,100,50),"变灰"))
		{
			sprite2.color = new Color(0,1,1);
		}

		if(GUI.Button( new Rect(110,160,100,50),"流光"))
		{
			sprite3.color = new Color(1,0,1);
		}
	}

    void OnDestroy()
    {
        sprite1.atlas.spriteMaterial = default_mat;
    }
}
