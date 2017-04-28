using UnityEngine;
using System.Collections;
using System;

public class NewBehaviourScript : MonoBehaviour
{
    public GameObject player;
    public float Le = 64f;
    public float Wi = 32f;

    public Texture[] mask;
    public float c = 0.1f;
    public float b = 0;
    int i = 0;
    void Cycle()
    {
        if (Time.time > b)
        {
            b = b + c;
            if (i < mask.Length - 1)
            {
                i++;

            }
            else
            {
                i = 0;
            }
            GetComponent<Renderer>().material.SetTexture("_Mask", mask[i]);


        }
    }
    void Start()
    {

    }

    void Update()
    {
        //Cycle();

        float offsetx = -(player.transform.position.x) / (Le * 2f);
        float offsety = -(player.transform.position.y) / (Wi * 2f);

        GetComponent<Renderer>().material.SetTextureOffset("_Mask", new Vector2(offsetx, offsety));

    }
}
