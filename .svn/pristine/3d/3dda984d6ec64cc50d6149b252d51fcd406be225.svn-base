using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

[RequireComponent(typeof(RectTransform))]
public class Joystick : MonoBehaviour
{
    public float radius = 60.0f;
    public float percent = 0.7f;
    public Vector2 returnSpeed;
    public RectTransform canvas;

    /// <summary>
    /// 外部监听函数
    /// </summary>
    public static event Action<Vector2> OnJoystickStart;
    public static event Action<Vector2> OnJoystickMove;
    public static event Action OnJoystickEnd;

    private Vector2 startPos;
    private RectTransform handler;
    private RectTransform mTrans;
    private bool isDragging = false;
    private bool returnHandle = true;

    public Vector2 Coordinates
    {
        get
        {
            if (handler.anchoredPosition.magnitude < radius)
                return handler.anchoredPosition / radius;
            return handler.anchoredPosition.normalized;
        }
    }

    void Awake()
    {
        mTrans = GetComponent<RectTransform>();
        startPos = mTrans.anchoredPosition;
        handler = transform.FindChild("Handler").GetComponent<RectTransform>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.OnMouseButtonDown();
        }
        if (Input.GetMouseButton(0))
        {
            this.OnMouseButtonMove();
        }
        if (Input.GetMouseButtonUp(0))
        {
            this.OnMouseButtonUp();
        }
        if (returnHandle)
        {
            if (handler.anchoredPosition.magnitude > Mathf.Epsilon)
            {
                float x = handler.anchoredPosition.x * returnSpeed.x;
                float y = handler.anchoredPosition.y * returnSpeed.y;
                handler.anchoredPosition -= new Vector2(x, y) * Time.deltaTime;
            }
            else
            {
                returnHandle = false;
            }
        }
    }

    /// <summary>
    /// 获取两点之间的一个点
    /// </summary>
    private Vector3 BetweenPoint(Vector3 start, Vector3 end, float distance)
    {
        Vector3 normal = (end - start).normalized;
        return normal * distance + start;
    }

    /// <summary>
    /// 获取超出位置
    /// </summary>
    /// <returns></returns>
    Vector2 GetOverstepPos()
    {
        Vector3 inPoint = Input.mousePosition;
        if (Input.mousePosition.x > Screen.width * percent)
        {
            inPoint.x = Screen.width * percent;
        }
        Vector2 outPoint;
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, inPoint, Camera.main, out outPoint))
        {
            var distance = (outPoint - mTrans.anchoredPosition).magnitude;
            if (distance > radius)
            {
                return BetweenPoint(outPoint, mTrans.anchoredPosition, radius);
            }
            else
            {
                return Vector2.zero;
            }
        }
        return Vector2.zero;
    }

    /// <summary>
    /// 指尖单击
    /// </summary>
    void OnMouseButtonDown()
    {
        if (Input.mousePosition.x <= Screen.width * percent)
        {
            returnHandle = false;
            isDragging = true;
            Vector2 outPoint;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(canvas, Input.mousePosition, Camera.main, out outPoint))
            {
                var distance = (outPoint - mTrans.anchoredPosition).magnitude;
                if (distance > radius)
                {
                    mTrans.anchoredPosition = outPoint;
                }
                else
                {
                    handler.anchoredPosition = GetJoystickOffset();
                }
                if (OnJoystickStart != null)
                {
                    OnJoystickStart(Coordinates.normalized);
                }
            }
        }
    }

    /// <summary>
    /// 指尖移动
    /// </summary>
    void OnMouseButtonMove()
    {
        if (returnHandle == true && !isDragging) return;
        if (Input.mousePosition.x <= Screen.width * percent || isDragging)
        {
            Vector2 newPos = GetOverstepPos();
            if (newPos != Vector2.zero)
            {
                //半径之内
                mTrans.anchoredPosition = Vector2.Lerp(mTrans.anchoredPosition, newPos, 1.5f);
            }
            handler.anchoredPosition = GetJoystickOffset();
            if (OnJoystickMove != null)
            {
                OnJoystickMove(Coordinates.normalized);
            }
        }
    }

    /// <summary>
    /// 指尖抬起
    /// </summary>
    void OnMouseButtonUp()
    {
        isDragging = false;
        returnHandle = true;
        mTrans.anchoredPosition = startPos;
        if (OnJoystickEnd != null)
        {
            OnJoystickEnd();
        }
    }

    private Vector2 GetJoystickOffset()
    {
        Vector3 globalHandle;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(canvas, Input.mousePosition, Camera.main, out globalHandle))
        {
            handler.position = globalHandle;
        }
        var handleOffset = handler.anchoredPosition;

        if (handleOffset.magnitude > radius)
        {
            handleOffset = handleOffset.normalized * radius;
            handler.anchoredPosition = handleOffset;
        }
        return handleOffset;
    }

}