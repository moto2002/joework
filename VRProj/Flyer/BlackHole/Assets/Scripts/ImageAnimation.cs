using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageAnimation : MonoBehaviour
{

    public string m_source = "UI/Animation/";
    public bool m_loop = false;
    public float m_step = 0.1f;

    private Image m_img;
    private int index = 0;
    private int max;
    private Sprite[] sprites;

    void Start()
    {
        m_img = this.GetComponent<Image>();
        sprites = Resources.LoadAll<Sprite>(m_source);
        max = sprites.Length;
        InvokeRepeating("ChangeImage", 0.1f, m_step);
    }

    void ChangeImage()
    {
        if (index < max)
        {
            m_img.sprite = sprites[index];
            index++;
        }
        else
        {
            if (m_loop)
                index = 0;
        }
    }

    void Update()
    {

    }
}
