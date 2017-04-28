using UnityEngine;
using System.Collections;

public class GetRectTransformInfo : MonoBehaviour
{

    void Start()
    {
      
        

    }

    void Update()
    {

    }

    private void OnGUI()
    {
        if (GUI.Button(new Rect(100, 100, 100, 100), "Get RectTransform Info"))
        {
            RectTransform rectTrans = this.GetComponent<RectTransform>();
            //Debug.Log("position" + rectTrans.position);
            //Debug.Log("localPosition" + rectTrans.localPosition);
            //Debug.Log("offsetMin_offsetMax" + rectTrans.offsetMin + "_" + rectTrans.offsetMax);
            Debug.Log("rect" + rectTrans.rect);
            Debug.Log(Screen.width + "_" + Screen.height);
            //Debug.Log("pivot" + rectTrans.pivot);
            //Debug.Log("anchoredPosition" + rectTrans.anchoredPosition);
            //Debug.Log("anchoredPosition3D" + rectTrans.anchoredPosition3D);
            //Debug.Log("sizeDelta" + rectTrans.sizeDelta);
            //Debug.Log("anchorMin_anchorMax" + rectTrans.anchorMin + "_" + rectTrans.anchorMax);
        }
    }
}
