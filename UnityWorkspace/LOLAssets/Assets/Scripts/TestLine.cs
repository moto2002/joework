using UnityEngine;
using System.Collections;

public class TestLine : MonoBehaviour
{


    public Transform startTrans;
    public Transform endTrans;

    LineRenderer m_lineRenderer;
    void Start()
    {
        this.m_lineRenderer = this.GetComponent<LineRenderer>();
    }

    void Update()
    {
        this.m_lineRenderer.SetPosition(0, startTrans.position + new Vector3(0,6f,0));
        this.m_lineRenderer.SetPosition(1,endTrans.position + new Vector3(0,1.5f,0));
    }
}
