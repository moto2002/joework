using System;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.VR;
using UnityEngine.UI;

public class QuitView : MonoBehaviour
{
    private Text m_title, m_content, m_time, m_cost;
    private GameObject m_yes, m_no;

    private void Awake()
    {
        m_title = transform.FindChild("Title").GetComponent<Text>();
        m_content = transform.FindChild("Content").GetComponent<Text>();
        m_time = transform.FindChild("Time/Text").GetComponent<Text>();
        m_cost = transform.FindChild("Cost/Text").GetComponent<Text>();
        m_yes = transform.FindChild("Yes").gameObject;
        m_no = transform.FindChild("No").gameObject;

    }

    void Start()
    {
        EventTriggerAssist.Get(m_yes).LeftClick += (e) =>
        {
            if (GlobalData.m_isFirstEntered)
            {
                Application.Quit();
                return;
            }

            PlayTimeMgr.Inst.ExcuteRecord();
            GlobalData.m_isFirstEntered = true;
            Application.Quit();
        };

        EventTriggerAssist.Get(m_no).LeftClick += (e) =>
        {
            this.gameObject.SetActive(false);
        };

        this.gameObject.SetActive(false);
    }

    public void Show()
    {
        this.gameObject.SetActive(true);

        ResultData rd = PlayTimeMgr.Inst.StopRecord();
        if (rd != null)
        {
            m_time.text = rd.cfgPlayTime.m_duringTime;
            int _cost = GetCost(rd.ts);
            string str = _cost + "元";
            m_cost.text =str ;
            GlobalData.m_cost = str;
        }
        else
        {
            m_time.text = "0";
            m_cost.text = "0";
        }
    }

    private int GetCost(TimeSpan ts)
    {
        int timeStep = 10;//10min
        int Price = 10;//10元  10分钟10元，不足10分钟按10分钟算
        int cost = 0;//花费

        double d = ts.TotalMinutes;
        //Debug.Log(d);
        float f = (float)d / (float)timeStep;
        //Debug.Log(f);

        int _i = (int)f;

        if (f - _i > 0)
        {
            _i = _i + 1;
        }

        cost += _i * Price;

        return cost;
    }

    public void Hide()
    {
        this.gameObject.SetActive(false);
    }
}
