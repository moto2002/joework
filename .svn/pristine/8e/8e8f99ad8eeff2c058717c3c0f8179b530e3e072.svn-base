using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TouchIdTest : MonoBehaviour {

    public Image img1, img2;
    public Text t1, t2;
	// Use this for initialization
	void Start () {
        EventTriggerListener.Get(img1.gameObject).OnDown += OnImg1Click;
        EventTriggerListener.Get(img2.gameObject).OnDown += OnImg2Click;
	}

    private void OnImg1Click(GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
    {
        t1.text = eventData.pointerId+"";
    }

    private void OnImg2Click(GameObject go, UnityEngine.EventSystems.PointerEventData eventData)
    {
        t2.text = eventData.pointerId + "";
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
