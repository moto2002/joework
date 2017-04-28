using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

/*
1. IPointerEnterHandler：指针进入事件
2. IPointerExitHandler：指针退出事件
3. IPointerDownHandler：指针按下事件
4. IPointerUpHandler：指针抬起事件
5. IPointerClickHandler：指针点击事件
6. IInitializePotentialDragHandler：初始化潜在的拖动事件
7. IBeginDragHandler：拖动开始事件
8. IDragHandler：拖动中事件
9. IEndDragHandler：拖动结束事件
10. IDropHandler：接收拖动事件
11. IScrollHandler：滚动事件
12. ISelectHandler：选择事件
13. IDeselectHandler：取消选择事件
14. IUpdateSelectedHandler：选中物体每帧触发事件
15. IMoveHandler：移动事件(上下左右)
16. ISubmitHandler：提交事件
17. ICancelHandler：取消事件
*/

public class UGUIEventListener : UnityEngine.EventSystems.EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;

    public static UGUIEventListener Get(GameObject go)
    {
        UGUIEventListener listener = go.GetComponent<UGUIEventListener>();
        if (listener == null)
            listener = go.AddComponent<UGUIEventListener>();
        return listener;
    }
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (onClick != null)
            onClick(gameObject);
    }
    public override void OnPointerDown(PointerEventData eventData)
    {
        if (onDown != null)
            onDown(gameObject);
    }
    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (onEnter != null)
            onEnter(gameObject);
    }
    public override void OnPointerExit(PointerEventData eventData)
    {
        if (onExit != null)
            onExit(gameObject);
    }
    public override void OnPointerUp(PointerEventData eventData)
    {
        if (onUp != null)
            onUp(gameObject);
    }
    public override void OnSelect(BaseEventData eventData)
    {
        if (onSelect != null)
            onSelect(gameObject);
    }
    public override void OnUpdateSelected(BaseEventData eventData)
    {
        if (onUpdateSelect != null)
            onUpdateSelect(gameObject);
    }
}