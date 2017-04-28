using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class LGButton : MonoBehaviour 
{

    public string Tag = "";
    public string Team = "";

    public bool selected = false;

    void OnClick()
    {
        //ScreenStateMgr.Instance().OnClick(this);
    }

}

