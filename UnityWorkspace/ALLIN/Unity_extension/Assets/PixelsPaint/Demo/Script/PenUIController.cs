using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PenUIController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler{

    private RectTransform _trans;
    private Vector2 _pos;

	public Painter paint;

    void Start()
    {
        _trans = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        _pos = _trans.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
            _pos += eventData.delta;
            _trans.position = _pos;
            paint.DrawGraphics(_trans.position);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        paint.DrawEnd();
		if (paint.paintType == Painter.PaintType.Scribble) {
			print (paint.IsScribbleCompleted ());
		}
    }
}