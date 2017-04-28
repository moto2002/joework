using UnityEngine;
using System.Collections;

public class TestMoveMask : MonoBehaviour
{
    public int length = 50;
    public int width = 50;
    public Transform target;

    Renderer render;
    void Start()
    {
        this.render = this.gameObject.GetComponent<Renderer>();
    }

    void Update()
    {
        float offsetx = (this.target.position.x) / (this.length * 2f);
        float offsety = (this.target.position.z) / (this.width * 2f);

        this.render.material.SetTextureOffset("_Mask", new Vector2(offsetx, offsety));
    }
}
