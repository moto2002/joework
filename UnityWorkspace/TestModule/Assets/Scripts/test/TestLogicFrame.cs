using UnityEngine;
using System.Collections;

public class TestLogicFrame : MonoBehaviour {

	// Use this for initialization
	void Start () {
        LogicTimer m_logicTime = new LogicTimer(1f,5);
        m_logicTime.AddListener(() => {

            Debug.Log("logic frame");
        });
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
