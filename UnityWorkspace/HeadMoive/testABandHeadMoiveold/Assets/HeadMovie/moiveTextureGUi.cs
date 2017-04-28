using UnityEngine;
using System.Collections;

public class moiveTextureGUi : MonoBehaviour
{
    public MovieTexture mt;

    void Start()
    {
        mt.loop = true;
    }
    void OnGUI()
    {
        GUI.DrawTexture(new Rect(0,0,Screen.width,Screen.height),mt,ScaleMode.StretchToFill);

        if (GUILayout.Button("play"))
        {
            if (!mt.isPlaying)
            {
                mt.Play();
            }
        }
    }
}
