using UnityEngine;
using System.Collections;

public class CameraShake : MonoBehaviour 
{
	private Vector3 _currentPosition;		//记录抖动前的位置
	public float _shakeCD = 0.005f;		//抖动的频率
    private int _shakeCount = -1;         //设置抖动次数
	private float _shakeTime;
	
    void OnEnable()
    {
        _currentPosition = transform.position;	//记录抖动前的位置

        _shakeCount = 10;

    }
	void Update ()
	{
		if(_shakeTime + _shakeCD < Time.time && _shakeCount > 0)
		{
			_shakeCount --;
			float radio = Random.Range (-1f, 1f);
			
			if(_shakeCount == 1)	//抖动最后一次时设置为都动前记录的位置
				radio = 0;
			
			//transform.position = _currentPosition + Vector3.one * radio;
            transform.position = _currentPosition + new Vector3(1, 0, 1)*radio;

             if (transform.position.z == _currentPosition.z)
             {
                 this.enabled = false;
             }

             _shakeTime = Time.time;
		}
	}
}
