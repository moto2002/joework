using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public static class LeanTweenExtend
{
    public static LTDescr Move(this RectTransform transform, Vector3 from, Vector3 to, float time)
    {
        return LeanTween.value(transform.gameObject, (p) => 
        {
            transform.anchoredPosition3D = p;
        }, from, to, time);
    }

    public static LTDescr Moveto(this RectTransform transform, Vector3 to, float time)
    {
        return LeanTween.value(transform.gameObject, (p)=> 
        {
            transform.anchoredPosition3D = p;
        }, transform.anchoredPosition3D, to, time);
    }

    public static LTDescr Rotatey(this RectTransform transform, float to, float time)
    {
        return LeanTween.rotateY(transform.gameObject, to, time);
    }

    public static LTDescr Alpha(this RectTransform transform, float to, float time)
    {
        return LeanTween.alpha(transform, to, time);
    }
}
