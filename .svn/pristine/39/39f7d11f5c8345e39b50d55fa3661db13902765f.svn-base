using UnityEngine;
using System.Collections;

public class EInputEvent
{
	public const string MouseEvent_LeftMouseDown = "MouseEvent_LeftMouseDown";
	public const string MouseEvent_LeftMouseDuring = "MouseEvent_LeftMouseDuring";
	public const string MouseEvent_LeftMouseUp = "MouseEvent_LeftMouseUp";
	
	public const string MouseEvent_RightMouseDown = "MouseEvent_RightMouseDown";
	public const string MouseEvent_RightMouseDuring = "MouseEvent_RightMouseDuring";
	public const string MouseEvent_RightMouseUp = "MouseEvent_RightMouseUp"; 
	
	public const string MouseEvent_MiddleMouseDown = "MouseEvent_MiddleMouseDown";
	public const string MouseEvent_MiddleMouseDuring = "MouseEvent_MiddleMouseDuring";
	public const string MouseEvent_MiddleMouseUp = "MouseEvent_MiddleMouseUp";
	
	public const string MouseEvent_MouseScrollWheel = "MouseEvent_MouseScrollWheel";//鼠标滚轮


	public const string TouchEvent_OneFingerTouch = "TouchEvent_OneFingerTouch";//单指点击
    public const string TouchEvent_OneFingerMove = "TouchEvent_OneFingerMove";//单指滑动
    public const string TouchEvent_TwoFingerMove = "TouchEvent_TwoFingerMove";//双指滑动

}

public class InputController : MonoBehaviour 
{
	public string Event_Current;

    private Vector2 oldPosition1;
    private Vector2 oldPosition2;

	void Start () 
	{
	
	}
	
	void Update () 
	{
		if (Application.isMobilePlatform) 
		{
			OnMobile();
		}
		else 
		{
			OnPC();
		}
	}

	void OnPC()
	{
		if (Input.GetMouseButtonDown (0)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_LeftMouseDown;
            OnPCEventComm();
		} 
		else if (Input.GetMouseButton (0)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_LeftMouseDuring;
            OnPCEventComm();
		} 
		else if (Input.GetMouseButtonUp (0)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_LeftMouseUp;
            OnPCEventComm();
		} 
		else if (Input.GetMouseButtonDown (1)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_RightMouseDown;
            OnPCEventComm();
		} 
		else if (Input.GetMouseButton (1)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_RightMouseDuring;
            OnPCEventComm();
		}
		else if (Input.GetMouseButtonUp (1)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_RightMouseUp;
            OnPCEventComm();
		} 
		else if (Input.GetMouseButtonDown (2))
		{
			this.Event_Current = EInputEvent.MouseEvent_MiddleMouseDown;
            OnPCEventComm();
		} 
		else if(Input.GetMouseButton(2))
		{
			this.Event_Current = EInputEvent.MouseEvent_MiddleMouseDuring;
            OnPCEventComm();
		}
		else if (Input.GetMouseButtonUp (2)) 
		{
			this.Event_Current = EInputEvent.MouseEvent_MiddleMouseUp;
            OnPCEventComm();
		}
		else if(Input.GetAxis("Mouse ScrollWheel")!=0)
		{
			this.Event_Current = EInputEvent.MouseEvent_MouseScrollWheel;
            OnPCMouseScrollWheel();
		}

	}

	void OnMobile()
	{
		if (Input.touchCount == 1 && Input.GetTouch (0).phase == TouchPhase.Began) 
		{
			this.Event_Current = EInputEvent.TouchEvent_OneFingerTouch;

            Vector3 pos = Input.GetTouch(0).position;

            CGameEvent _event = new CGameEvent(this.Event_Current);
            _event.sender = this.gameObject;
            _event.data = pos;
            this.CommDisPathEvent(_event);

		}
        else if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            this.Event_Current = EInputEvent.TouchEvent_OneFingerMove;

            float delt_x = Input.GetAxis("Mouse X") * Input.GetTouch(0).deltaTime * 50f;
            float delt_y = Input.GetAxis("Mouse Y") * Input.GetTouch(0).deltaTime * 50f;

            float[] delt_x_y = new float[2] { delt_x, delt_y };

            CGameEvent _event = new CGameEvent(this.Event_Current);
            _event.sender = this.gameObject;
            _event.data = delt_x_y;
            this.CommDisPathEvent(_event);

            //targetPos.x -= Input.GetAxis("Mouse X") * Input.GetTouch(0).deltaTime * 50f;
        }
        else if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
        {
            this.Event_Current = EInputEvent.TouchEvent_TwoFingerMove;

            float delt = 0;

            //放大
            if (isEnlarge(oldPosition1, oldPosition2, Input.GetTouch(0).position, Input.GetTouch(1).position))
            {
                // nowTargetSize -= Input.GetTouch(0).deltaTime * 30f;//一般控制相机
                delt = -0.5f;
            }
            else//缩小
            {
      
                //nowTargetSize += Input.GetTouch(0).deltaTime * 30f;
                delt = 0.5f;

            }

      
            CGameEvent _event = new CGameEvent(this.Event_Current);
            _event.sender = this.gameObject;
            _event.data = delt;
            this.CommDisPathEvent(_event);


            oldPosition1 = Input.GetTouch(0).position;
            oldPosition2 = Input.GetTouch(1).position;
        }

	}

    void OnPCEventComm()
    {
        Vector3 pos = Input.mousePosition;

        CGameEvent _event = new CGameEvent(this.Event_Current);
        _event.sender = this.gameObject;
        _event.data = pos;
        this.CommDisPathEvent(_event);
    }

    void OnPCMouseScrollWheel()
    {
        float ms = Input.GetAxis("Mouse ScrollWheel");

        CGameEvent _event = new CGameEvent(this.Event_Current);
        _event.sender = this.gameObject;
        _event.data = ms;
        this.CommDisPathEvent(_event);
    }

    void CommDisPathEvent(CGameEvent _event)
    {
        CGameEventMgr.Inst().Dispatch(_event);
    }

    private bool isEnlarge(Vector2 oP1, Vector2 oP2, Vector2 nP1, Vector2 nP2)
    {
        float leng1 = Mathf.Sqrt((oP1.x - oP2.x) * (oP1.x - oP2.x) + (oP1.y - oP2.y) * (oP1.y - oP2.y));
        float leng2 = Mathf.Sqrt((nP1.x - nP2.x) * (nP1.x - nP2.x) + (nP1.y - nP2.y) * (nP1.y - nP2.y));
        if (leng1 < leng2)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
