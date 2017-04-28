using UnityEngine;
using System.Collections;

public class StarSkyView : MonoBehaviour
{

    private GameObject m_starParent;

    void Start()
    {
        GameObject star1, star2, star3, star4,xingyun1,xingyun2,xingyun3;
        star1 = Resources.Load("prefeb/diandian_UI 1") as GameObject;
        star2 = Resources.Load("prefeb/diandian_UI") as GameObject;
        // star3 = Resources.Load("prefeb/changtiao_UI") as GameObject;
        //star4 = Resources.Load("prefeb/changtiao2_UI") as GameObject;
        xingyun1 = Resources.Load("prefeb/xingyun1") as GameObject;
        xingyun2 = Resources.Load("prefeb/xingyun2") as GameObject;
        xingyun3 = Resources.Load("prefeb/xingyun3") as GameObject;

        m_starParent = new GameObject();
        m_starParent.name = "Stars";

        RandomCreateStars(star1, 60);
        RandomCreateStars(star2, 60);
        // RandomCreateStars(star3, 30);
        // RandomCreateStars(star4, 30);
        RandomCreateStars(xingyun1, 3);
        RandomCreateStars(xingyun2, 3);
        RandomCreateStars(xingyun3, 3);
    }

    void Update()
    {

    }


    private void RandomCreateStars(GameObject prefab, int count)
    {
        for (int i = 0; i < count; i++)
        {
            float x, y, z;
            x = Random.Range(0, 1f) > 0.5f ? Random.Range(-20f, -8f) : Random.Range(8f, 20f);
            y = Random.Range(0, 1f) > 0.5f ? Random.Range(-20f, -8f) : Random.Range(8f, 20f);
            z = Random.Range(0, 1f) > 0.5f ? Random.Range(-20f, -8f) : Random.Range(8f, 20f);

            Vector3 pos = new Vector3(x, y, z);

            GameObject inst = GameObject.Instantiate(prefab) as GameObject;
            inst.transform.SetParent(m_starParent.transform);
            inst.transform.localPosition = pos;
            inst.transform.localScale = Vector3.one;
        }
    }
}
