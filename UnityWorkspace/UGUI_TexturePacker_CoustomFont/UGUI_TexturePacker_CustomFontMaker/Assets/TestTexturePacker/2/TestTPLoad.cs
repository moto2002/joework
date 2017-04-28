using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TestTPLoad : MonoBehaviour
{
    public Image img;
    private int index = 0;
    private int length;
    private Sprite[] niuniu_all;

    void Start()
    {
        niuniu_all = Resources.LoadAll<Sprite>("niuniu_new_copy");
        length = niuniu_all.Length;
     
        InvokeRepeating("Tick",1f,1f);
    }

    void Tick()
    {
        index = index%length;
        img.sprite = niuniu_all[index];
        index++;
    }
}
