using UnityEngine;
using System.Collections;

public class JoystickEvent : MonoBehaviour
{
    private Vector3 m_joystickDir;

    void Start()
    {
        Joystick.OnJoystickStart += On_JoystickMoveStart;
        Joystick.OnJoystickMove += On_JoystickMove;
        Joystick.OnJoystickEnd += On_JoystickMoveEnd;
    }

    void On_JoystickMove(Vector2 pos)
    {
        Debug.Log("On_JoystickMove-->>" + pos);
        m_joystickDir = new Vector3(pos.x,0,pos.y);

        CGameEvent _event = new CGameEvent("JoystickMove");
        _event.data = m_joystickDir;
        CGameEventMgr.Inst().Dispatch(_event);

    }

    void On_JoystickMoveStart(Vector2 pos)
    {
        Debug.Log("On_JoystickStart-->>" + pos);
        m_joystickDir = new Vector3(pos.x, 0, pos.y);

        CGameEvent _event = new CGameEvent("JoystickMoveStart");
        _event.data = m_joystickDir;
        CGameEventMgr.Inst().Dispatch(_event);

    }

    void On_JoystickMoveEnd()
    {
        Debug.Log("On_JoystickEnd-->>");

        CGameEvent _event = new CGameEvent("JoystickMoveEnd");
        CGameEventMgr.Inst().Dispatch(_event);
    }

    void Update()
    {

    }



}
