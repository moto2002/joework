using UnityEngine;
using System.Collections;
using com.shihuanjue.game.Log;

public class Test : MonoBehaviour 
{
	void Start ()
	{
		Debug.Log (Application.persistentDataPath);
		LogHelper.CurrentLogLevels = LogLevel.DEBUG | LogLevel.ERROR;
		LogHelper.Debug ("debug");
		LogHelper.Critical ("critical");
		LogHelper.Error ("error");
		LogHelper.Debug ("debug");
		LogHelper.Critical ("critical");
		LogHelper.Error ("error");
	}
	

}
