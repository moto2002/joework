using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class PenUISpriteController : MonoBehaviour,IBeginDragHandler,IDragHandler,IEndDragHandler{

    private RectTransform _trans;
	private Vector3 _pos;
	private Vector3 m_touchDownTargetOffset ;

	public Painter paint;

    void Start()
    {
        _trans = GetComponent<RectTransform>();
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
		_pos = _trans.position;
		Vector3 touchDownMousePos;
		RectTransformUtility.ScreenPointToWorldPointInRectangle(_trans,eventData.position,Camera.main,out touchDownMousePos);
		m_touchDownTargetOffset = _pos-touchDownMousePos;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (eventData.dragging)
        {
			RectTransformUtility.ScreenPointToWorldPointInRectangle(_trans,eventData.position,Camera.main,out _pos);
			_pos+=m_touchDownTargetOffset;
			_trans.position = _pos;
			paint.DrawSpriteGraphics(Camera.main.WorldToScreenPoint(_trans.position));
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