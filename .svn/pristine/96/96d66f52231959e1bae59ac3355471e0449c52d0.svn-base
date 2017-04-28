using UnityEngine;
using VRTK;

public class VRTK_ControllerEvents_ListenerExample : MonoBehaviour
{
    private VRTK_ControllerEvents m_vrtkControllerEvents;
    void Awake()
    {
        m_vrtkControllerEvents = GetComponent<VRTK_ControllerEvents>();
        if (m_vrtkControllerEvents == null)
        {
            Debug.LogError("VRTK_ControllerEvents_ListenerExample is required to be attached to a SteamVR Controller that has the VRTK_ControllerEvents script attached to it");
            return;
        }
    }

    private void Start()
    {
        //Setup controller event listeners
        m_vrtkControllerEvents.TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        m_vrtkControllerEvents.TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
        m_vrtkControllerEvents.TriggerTouchStart += new ControllerInteractionEventHandler(DoTriggerTouchStart);
        m_vrtkControllerEvents.TriggerTouchEnd += new ControllerInteractionEventHandler(DoTriggerTouchEnd);
        m_vrtkControllerEvents.TriggerHairlineStart += new ControllerInteractionEventHandler(DoTriggerHairlineStart);
        m_vrtkControllerEvents.TriggerHairlineEnd += new ControllerInteractionEventHandler(DoTriggerHairlineEnd);
        m_vrtkControllerEvents.TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        m_vrtkControllerEvents.TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);
        m_vrtkControllerEvents.TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);
        m_vrtkControllerEvents.ApplicationMenuPressed += new ControllerInteractionEventHandler(DoApplicationMenuPressed);
        m_vrtkControllerEvents.ApplicationMenuReleased += new ControllerInteractionEventHandler(DoApplicationMenuReleased);
        m_vrtkControllerEvents.GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        m_vrtkControllerEvents.GripReleased += new ControllerInteractionEventHandler(DoGripReleased);
        m_vrtkControllerEvents.TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        m_vrtkControllerEvents.TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);
        m_vrtkControllerEvents.TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        m_vrtkControllerEvents.TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        m_vrtkControllerEvents.TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        Debug.Log("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        //还得去SteamVR_ControllerManager上获取SteamVR_TrackedObject对象的index ，比对左还是右的index,才知道是左手柄还是右手柄 ???
        //或者这样： string hand = this.transform.name == "Controller (left)"? "left":"right";

        DebugLogger(e.controllerIndex, "TRIGGER", "pressed", e);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "released", e);
    }

    private void DoTriggerTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "touched", e);
    }

    private void DoTriggerTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "untouched", e);
    }

    private void DoTriggerHairlineStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "hairline start", e);
    }

    private void DoTriggerHairlineEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "hairline end", e);
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "clicked", e);
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "unclicked", e);
    }

    private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TRIGGER", "axis changed", e);
    }

    private void DoApplicationMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "APPLICATION MENU", "pressed down", e);
    }

    private void DoApplicationMenuReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "APPLICATION MENU", "released", e);
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "GRIP", "pressed down", e);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "GRIP", "released", e);
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "pressed down", e);
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "released", e);
    }

    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "touched", e);
    }

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "untouched", e);
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(e.controllerIndex, "TOUCHPAD", "axis changed", e);
    }

    void OnDestroy()
    {
        //remove controller event listeners
        m_vrtkControllerEvents.TriggerPressed -= new ControllerInteractionEventHandler(DoTriggerPressed);
        m_vrtkControllerEvents.TriggerReleased -= new ControllerInteractionEventHandler(DoTriggerReleased);
        m_vrtkControllerEvents.TriggerTouchStart -= new ControllerInteractionEventHandler(DoTriggerTouchStart);
        m_vrtkControllerEvents.TriggerTouchEnd -= new ControllerInteractionEventHandler(DoTriggerTouchEnd);
        m_vrtkControllerEvents.TriggerHairlineStart -= new ControllerInteractionEventHandler(DoTriggerHairlineStart);
        m_vrtkControllerEvents.TriggerHairlineEnd -= new ControllerInteractionEventHandler(DoTriggerHairlineEnd);
        m_vrtkControllerEvents.TriggerClicked -= new ControllerInteractionEventHandler(DoTriggerClicked);
        m_vrtkControllerEvents.TriggerUnclicked -= new ControllerInteractionEventHandler(DoTriggerUnclicked);
        m_vrtkControllerEvents.TriggerAxisChanged -= new ControllerInteractionEventHandler(DoTriggerAxisChanged);
        m_vrtkControllerEvents.ApplicationMenuPressed -= new ControllerInteractionEventHandler(DoApplicationMenuPressed);
        m_vrtkControllerEvents.ApplicationMenuReleased -= new ControllerInteractionEventHandler(DoApplicationMenuReleased);
        m_vrtkControllerEvents.GripPressed -= new ControllerInteractionEventHandler(DoGripPressed);
        m_vrtkControllerEvents.GripReleased -= new ControllerInteractionEventHandler(DoGripReleased);
        m_vrtkControllerEvents.TouchpadPressed -= new ControllerInteractionEventHandler(DoTouchpadPressed);
        m_vrtkControllerEvents.TouchpadReleased -= new ControllerInteractionEventHandler(DoTouchpadReleased);
        m_vrtkControllerEvents.TouchpadTouchStart -= new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        m_vrtkControllerEvents.TouchpadTouchEnd -= new ControllerInteractionEventHandler(DoTouchpadTouchEnd);
        m_vrtkControllerEvents.TouchpadAxisChanged -= new ControllerInteractionEventHandler(DoTouchpadAxisChanged);
    }
}