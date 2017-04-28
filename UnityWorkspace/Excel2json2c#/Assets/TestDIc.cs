using UnityEngine;
using System.Collections.Generic;
using Newtonsoft.Json;
using HuanJueGameData;

public class TestDIc : MonoBehaviour {

    public TextAsset txt;

	// Use this for initialization
	void Start () {

       CfguserLevelTab.userLevelTab = JsonConvert.DeserializeObject<Dictionary<string, UserLevelTab>>(txt.text);

        Debug.Log(CfguserLevelTab.userLevelTab["5"].totalExp);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
