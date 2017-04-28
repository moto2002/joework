using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

//shader 用unlit/transparent
[RequireComponent(typeof(LineRenderer))]
public class LineRenderHelper : MonoBehaviour
{
    LineRenderer lr;
    public float speed = 0.5f;
    Vector2 offset = new Vector2(0, 0);
    Vector2 scale = new Vector2(1f, 1f);
    Vector3 y_offset = new Vector3(0, 0.5f, 0);

    void Awake()
    {
        lr = this.GetComponent<LineRenderer>();
        if (lr == null)
            return;
        Debug.Log(lr.material.name);

        lr.material.SetFloat("_", 10f);
        scale.x = 10f;

        lr.material.SetTextureOffset("_MainTex", offset);
        lr.material.SetTextureScale("_MainTex", scale);

    }

    //public void MyStart(List<Node> vlist, float size)
    //{
    //    List<Vector3> _vlist = new List<Vector3>();
    //    for (int i = 0; i < vlist.Count; i++)
    //    {
    //        _vlist.Add(vlist[i].worldPos);
    //    }

    //    MyStart(_vlist,size);
    //}
    public void MyStart(List<Vector3> vlist, float size)
    {
        lr.SetWidth(size, size);//0.8 0.8
        lr.SetVertexCount(vlist.Count);

        for (int i = 0; i < vlist.Count; i++)
        {
           lr.SetPosition(i, vlist[i] + y_offset);
        }

        scale.x = vlist.Count;

        lr.material.SetTextureOffset("_MainTex", offset);

        lr.material.SetTextureScale("_MainTex", scale);

    }

    void Start()
    {

    }

    void Update()
    {
        offset.x -= speed * Time.deltaTime;
        if (offset.x == Int16.MinValue)
            offset.x = 0;

        lr.material.SetTextureOffset("_MainTex", offset);
    }
}
