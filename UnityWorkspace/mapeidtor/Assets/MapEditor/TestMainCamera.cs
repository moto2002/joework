using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class TestMainCamera : MonoBehaviour
{
    //限定camera在一定范围内
    public Transform minPos;
    public Transform maxPos;

    public Vector3 targetPos;

    private Vector2 oldPosition1;
    private Vector2 oldPosition2;

    //cameraSize
    public const float minTargetSize = 5f;
    public const float midTargetSize = 15f;
    public const float maxTargetSize = 30f;
    public float nowTargetSize = 0;

    private Vector3 MousePos;

    public bool bCanMoveScene = true;

    bool tweened = false;
    float tweenTime;//缓动时间
    const float MaxTweenTime = 1f;
    float tweenSpeed = 20f;

    //float lastX;
    //float lastZ;

    void Start()
    {
        targetPos = transform.position;
        nowTargetSize = Camera.main.orthographicSize;
        Input.simulateMouseWithTouches = true;
    }

    void Update()
    {
        if (nowTargetSize > maxTargetSize)
        {
            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, maxTargetSize, Time.deltaTime * 2f);
            if (Camera.main.orthographicSize - maxTargetSize <= 0.5f)
            {
                Camera.main.orthographicSize = maxTargetSize;
                nowTargetSize = Camera.main.orthographicSize;

      
            }
        }
        else
        {
            nowTargetSize = Mathf.Clamp(nowTargetSize, minTargetSize, maxTargetSize);

            Camera.main.orthographicSize = Mathf.Lerp(Camera.main.orthographicSize, nowTargetSize, Time.deltaTime * 10f);

            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * tweenSpeed);

            if (bCanMoveScene)
            {
                if (Application.isEditor)
                {
                    float Ms = Input.GetAxis("Mouse ScrollWheel");

                    nowTargetSize -= Ms * 15;
                    nowTargetSize = Mathf.Clamp(nowTargetSize, minTargetSize, maxTargetSize);

                    if (!Input.GetMouseButton(1))
                    {
                        MousePos = Input.mousePosition;
                    }
                    else if (Input.GetMouseButton(1))
                    {
                        Vector3 _Temp = MousePos - Input.mousePosition;
                        Vector3 _t = new Vector3(_Temp.x, 0f, _Temp.y);
                        targetPos += _t / 20;
                        MousePos = Input.mousePosition;
                    }

					if(Input.GetMouseButton(0))
					{

						Brush.Inst().Brushing(Input.mousePosition);

					}
					if(Input.GetMouseButton(2))
					{
						Brush.Inst().UnBrushing(Input.mousePosition);
					}

                }

                if (Input.touchCount > 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved)
                    {
                        if (isEnlarge(oldPosition1, oldPosition2, Input.GetTouch(0).position, Input.GetTouch(1).position))
                        {
                            //nowTargetSize -= 0.5f;
                            nowTargetSize -= Input.GetTouch(0).deltaTime * 30f;
                        }
                        else
                        {
                            //nowTargetSize += 0.5f;
                            nowTargetSize += Input.GetTouch(0).deltaTime * 30f;
                        }
                        nowTargetSize = Mathf.Clamp(nowTargetSize, minTargetSize, maxTargetSize);
                    }
                    oldPosition1 = Input.GetTouch(0).position;
                    oldPosition2 = Input.GetTouch(1).position;
                }
                else if (Input.touchCount == 1)
                {
                    if (Input.GetTouch(0).phase == TouchPhase.Began)
                    {
                        tweened = false;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Moved)
                    {
                        //targetPos.x -= Input.GetAxis("Mouse X") * 0.4f;
                        //targetPos.z -= Input.GetAxis("Mouse Y") * 0.4f;

                        targetPos.x -= Input.GetAxis("Mouse X") * Input.GetTouch(0).deltaTime * 50f;
                        targetPos.z -= Input.GetAxis("Mouse Y") * Input.GetTouch(0).deltaTime * 50f;

                        //lastX = Input.GetAxis("Mouse X") * Input.GetTouch(0).deltaTime;
                        //lastZ = Input.GetAxis("Mouse Y") * Input.GetTouch(0).deltaTime;
                    }
                    else if (Input.GetTouch(0).phase == TouchPhase.Ended)
                    {
                        tweened = true;
                        tweenTime = 0f;
                    }
                    oldPosition1 = Input.GetTouch(0).position;
                }
            }
            if (tweened)
            {
                tweenTime += Time.deltaTime;
                if (tweenTime > MaxTweenTime)
                {
                    tweened = false;
                }
                else
                {
                   // tweenSpeed = Mathf.Lerp(tweenSpeed,2f,1f);
            
                }
            }

           //CheckPos();
        }
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
    public void LockCameraSize()
    {
        nowTargetSize = minTargetSize;
    }
    public bool IsLockedCameraSize()
    {
        return Mathf.Abs(minTargetSize - nowTargetSize) < 0.5f;
    }
    private void CheckPos()
    {
        Vector3 old = transform.position;

        if (transform.position.x > maxPos.transform.position.x)
            old.x = maxPos.transform.position.x;
        if (transform.position.z > maxPos.transform.position.z)
            old.z = maxPos.transform.position.z;
        if (transform.position.x < minPos.transform.position.x)
            old.x = minPos.transform.position.x;
        if (transform.position.z < minPos.transform.position.z)
            old.z = minPos.transform.position.z;

        transform.position = old;
    }


}
