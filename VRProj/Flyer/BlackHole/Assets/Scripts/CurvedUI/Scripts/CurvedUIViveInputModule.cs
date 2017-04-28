using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Collections.Generic;
using CurvedUI;

//Import SteamVR and add CURVEDUI_VIVE to your custom defines to use this class.

#if CURVEDUI_VIVE
using Valve.VR;

namespace CurvedUI {

    /// <summary>
    /// InputModule made for Vive controllers. Create PointerEvents that are later used by eventSystem to discover interactions.
    /// </summary>
    public class CurvedUIViveInputModule : BaseInputModule
    {

        #region SETTINGS
        [Tooltip("Which controller can cause Events to be fired and interact with game world. Default Right.")]
        public ActiveViveController EventController = ActiveViveController.Right;

        // name of button to use for click/submit
        public string submitButtonName = "Fire1";

        //// name of axis to use for scrolling/sliders
        //public string controlAxisName = "Horizontal";
        #endregion

        #region INTERNAL SETTINGS
        public enum ActiveViveController
        {
            Both = 0,
            Right = 1,
            Left = 2,
        }

        //not using these now, but may come handy later:

        //// smooth axis - default UI move handlers do things in steps, meaning you can smooth scroll a slider or scrollbar
        //// with axis control. This option allows setting value of scrollbar/slider directly as opposed to using move handler
        //// to avoid this
        //bool useSmoothAxis = true;

        //// multiplier controls how fast slider/scrollbar moves with respect to input axis value
        //float smoothAxisMultiplier = 0.01f;
        //// if useSmoothAxis is off, this next field controls how many steps per second are done when axis is on
        //float steppedAxisStepsPerSecond = 10f;
        #endregion

        #region INTERNAL VARIABLES
        private static CurvedUIViveInputModule instance;
        private static SteamVR_ControllerManager controllerManager;
        private static CurvedUIViveController rightCont;
        private static CurvedUIViveController leftCont;
        private CurvedUIPointerEventData rightControllerData;
        private CurvedUIPointerEventData leftControllerData;
        private GameObject currentDragging;
        private GameObject currentPointedAt;

        //not using these now, but may come handy later:
        //private Vector2 rightControllerTouchPos = Vector2.zero;
        //private Vector2 leftControllerTouchPos = Vector2.zero;
        //private bool rightDown = false;
        //private bool leftDown = false;
        #endregion


        #region LIFECYCLE
        protected override void Awake()
        {
            instance = this;
            base.Awake();
            SetupControllers();
        }
        #endregion


        #region EVENT PROCESSING
        /// <summary>
        /// Process is called by UI system to process events 
        /// </summary>
        public override void Process()
        {
            switch (EventController)
            {
                case ActiveViveController.Right:
                {
                    //in case only one controller is turned on, it will still be used to call events.
                    if (controllerManager.right.activeInHierarchy)
                        ProcessController(controllerManager.right);
                    else if (controllerManager.left.activeInHierarchy)
                        ProcessController(controllerManager.left);
                    break;
                }
                case ActiveViveController.Left:
                {
                    //in case only one controller is turned on, it will still be used to call events.
                    if (controllerManager.left.activeInHierarchy)
                        ProcessController(controllerManager.left);
                    else if (controllerManager.right.activeInHierarchy)
                        ProcessController(controllerManager.right);
                    break;
                }
                case ActiveViveController.Both:
                {
                        ProcessController(controllerManager.left);
                        ProcessController(controllerManager.right);
                    break;
                }
                default: goto case ActiveViveController.Right;
            }
        }

        /// <summary>
        /// Processes Events from given controller.
        /// </summary>
        /// <param name="myController"></param>
        void ProcessController(GameObject myController)
        {
            //do not process events from this controller if it's off or not visible by base stations.
            if (!myController.gameObject.activeInHierarchy) return;

            CurvedUIViveController myControllerAssitant = myController.GetComponent<CurvedUIViveController>();
            if(myControllerAssitant == null)
            {
                myControllerAssitant = myController.AddComponent<CurvedUIViveController>();
            }

            // send update events if there is a selected object - this is important for InputField to receive keyboard events
            SendUpdateEventToSelectedObject();

            // see if there is a UI element that is currently being pointed at
            PointerEventData ControllerData;
            if (myControllerAssitant == rightCont) {
                ControllerData = GetControllerPointerData(myControllerAssitant, ref rightControllerData);
             
            }
            else {
                ControllerData = GetControllerPointerData(myControllerAssitant, ref leftControllerData);
            }

            currentPointedAt = ControllerData.pointerCurrentRaycast.gameObject;

            ProcessDownRelease(ControllerData, myControllerAssitant.IsTriggerDown, myControllerAssitant.IsTriggerUp);

            if (!myControllerAssitant.IsTriggerUp)
            {
                ProcessMove(ControllerData);
                ProcessDrag(ControllerData);
            }

            if (!Mathf.Approximately(ControllerData.scrollDelta.sqrMagnitude, 0.0f))
            {
                var scrollHandler = ExecuteEvents.GetEventHandler<IScrollHandler>(ControllerData.pointerCurrentRaycast.gameObject);
                ExecuteEvents.ExecuteHierarchy(scrollHandler, ControllerData, ExecuteEvents.scrollHandler);
               // Debug.Log("executing scroll handler");
            }

        }
        #endregion


        #region PRIVATE FUNCTIONS
        /// <summary>
        /// Create a pointerEventData that stores all the data associated with Vive controller.
        /// </summary>
        private CurvedUIPointerEventData GetControllerPointerData(CurvedUIViveController controller, ref CurvedUIPointerEventData ControllerData)
        {

            if (ControllerData == null)
                ControllerData = new CurvedUIPointerEventData(eventSystem);

            ControllerData.Reset();
            ControllerData.delta = Vector2.one; // to trick into moving
            ControllerData.position = Vector2.zero; // this will be overriden by raycaster
            ControllerData.Controller = controller.gameObject; // raycaster will use this object to override pointer position on screen. Keep it safe.
            ControllerData.scrollDelta = controller.TouchPadAxis - ControllerData.TouchPadAxis; // calcualte scroll delta
            ControllerData.TouchPadAxis = controller.TouchPadAxis; // assign finger position on touchpad

            eventSystem.RaycastAll(ControllerData, m_RaycastResultCache); //Raycast all the things!. Position will be overridden here by CurvedUIRaycaster

            //Get a current raycast to find if we're pointing at GUI object. 
            ControllerData.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
            _guiRaycastHit = (ControllerData.pointerCurrentRaycast.gameObject != null ? true : false);
            m_RaycastResultCache.Clear();

            return ControllerData;
        }

        /// <summary>
        /// Find if we're moving and pass appropriate events to gameobjects under the pointer. Shazam!
        /// </summary>
        /// <param name="pointerEvent"></param>
        protected virtual void ProcessMove(PointerEventData pointerEvent)
        {
            var targetGO = pointerEvent.pointerCurrentRaycast.gameObject;
            HandlePointerExitAndEnter(pointerEvent, targetGO);
        }

        /// <summary>
        /// Sends trigger down / trigger released events to gameobjects under the pointer.
        /// </summary>
        protected virtual void ProcessDownRelease(PointerEventData ControllerData, bool down, bool released)
        {
            var currentOverGo = ControllerData.pointerCurrentRaycast.gameObject;

            // PointerDown notification
            if (down)
            {
                ControllerData.eligibleForClick = true;
                ControllerData.delta = Vector2.zero;
                ControllerData.dragging = false;
                ControllerData.useDragThreshold = true;
                ControllerData.pressPosition = ControllerData.position;
                ControllerData.pointerPressRaycast = ControllerData.pointerCurrentRaycast;

                DeselectIfSelectionChanged(currentOverGo, ControllerData);

                if (ControllerData.pointerEnter != currentOverGo)
                {
                    // send a pointer enter to the touched element if it isn't the one to select...
                    HandlePointerExitAndEnter(ControllerData, currentOverGo);
                    ControllerData.pointerEnter = currentOverGo;
                }

                // search for the control that will receive the press
                // if we can't find a press handler set the press
                // handler to be what would receive a click.
                var newPressed = ExecuteEvents.ExecuteHierarchy(currentOverGo, ControllerData, ExecuteEvents.pointerDownHandler);

                // didnt find a press handler... search for a click handler
                if (newPressed == null)
                    newPressed = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);


                float time = Time.unscaledTime;

                if (newPressed == ControllerData.lastPress)
                {
                    var diffTime = time - ControllerData.clickTime;
                    if (diffTime < 0.3f)
                        ++ControllerData.clickCount;
                    else
                        ControllerData.clickCount = 1;

                    ControllerData.clickTime = time;
                }
                else
                {
                    ControllerData.clickCount = 1;
                }

                ControllerData.pointerPress = newPressed;
                ControllerData.rawPointerPress = currentOverGo;

                ControllerData.clickTime = time;

                // Save the drag handler as well
                ControllerData.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(currentOverGo);

                if (ControllerData.pointerDrag != null)
                    ExecuteEvents.Execute(ControllerData.pointerDrag, ControllerData, ExecuteEvents.initializePotentialDrag);
            }

            // PointerUp notification
            if (released)
            {
                // Debug.Log("Executing pressup on: " + pointer.pointerPress);
                ExecuteEvents.Execute(ControllerData.pointerPress, ControllerData, ExecuteEvents.pointerUpHandler);

                // see if we mouse up on the same element that we clicked on...
                var pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentOverGo);

                // PointerClick and Drop events
                if (ControllerData.pointerPress == pointerUpHandler && ControllerData.eligibleForClick)
                {
                    ExecuteEvents.Execute(ControllerData.pointerPress, ControllerData, ExecuteEvents.pointerClickHandler);
                }
                else if (ControllerData.pointerDrag != null && ControllerData.dragging)
                {
                    ExecuteEvents.ExecuteHierarchy(currentOverGo, ControllerData, ExecuteEvents.dropHandler);
                }

                ControllerData.eligibleForClick = false;
                ControllerData.pointerPress = null;
                ControllerData.rawPointerPress = null;

                if (ControllerData.pointerDrag != null && ControllerData.dragging)
                    ExecuteEvents.Execute(ControllerData.pointerDrag, ControllerData, ExecuteEvents.endDragHandler);

                ControllerData.dragging = false;
                ControllerData.pointerDrag = null;

                if (ControllerData.pointerDrag != null)
                    ExecuteEvents.Execute(ControllerData.pointerDrag, ControllerData, ExecuteEvents.endDragHandler);

                ControllerData.pointerDrag = null;

                // send exit events as we need to simulate this on touch up on touch device
                ExecuteEvents.ExecuteHierarchy(ControllerData.pointerEnter, ControllerData, ExecuteEvents.pointerExitHandler);
                ControllerData.pointerEnter = null;
            }
        }

        /// <summary>
        /// Find out if wer'e dragging something. If yes, tell interested gameobjects about it.
        /// </summary>
        /// <param name="pointerEvent"></param>
        protected virtual void ProcessDrag(PointerEventData pointerEvent)
        {
            bool moving = pointerEvent.IsPointerMoving();
            if (moving && pointerEvent.pointerDrag != null
                && !pointerEvent.dragging
                && ShouldStartDrag(pointerEvent.pressPosition, pointerEvent.position, eventSystem.pixelDragThreshold, pointerEvent.useDragThreshold))
            {

                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.beginDragHandler);
                pointerEvent.dragging = true;
            }

            // Drag notification
            if (pointerEvent.dragging && moving && pointerEvent.pointerDrag != null)
            {
                
                // Before doing drag we should cancel any pointer down state
                // And clear selection!
                if (pointerEvent.pointerPress != pointerEvent.pointerDrag)
                {
                    ExecuteEvents.Execute(pointerEvent.pointerPress, pointerEvent, ExecuteEvents.pointerUpHandler);

                    pointerEvent.eligibleForClick = false;
                    pointerEvent.pointerPress = null;
                    pointerEvent.rawPointerPress = null;
                }
                ExecuteEvents.Execute(pointerEvent.pointerDrag, pointerEvent, ExecuteEvents.dragHandler);
            }
        }

        protected void DeselectIfSelectionChanged(GameObject currentOverGo, BaseEventData pointerEvent)
        {
            // Selection tracking
            var selectHandlerGO = ExecuteEvents.GetEventHandler<ISelectHandler>(currentOverGo);
            // if we have clicked something new, deselect the old thing
            // leave 'selection handling' up to the press event though.
            if (selectHandlerGO != eventSystem.currentSelectedGameObject)
                eventSystem.SetSelectedGameObject(null, pointerEvent);
        }

        private bool ShouldStartDrag(Vector2 pressPos, Vector2 currentPos, float threshold, bool useDragThreshold)
        {
            if (!useDragThreshold)
                return true;

            return (pressPos - currentPos).sqrMagnitude >= threshold * threshold;
        }


        
        /// <summary>
        /// send update event to selected object
        /// needed for InputField to receive keyboard input. Would you even use that with vive?
        /// </summary>
        /// <returns></returns>
        private bool SendUpdateEventToSelectedObject()
        {
            if (eventSystem.currentSelectedGameObject == null)
                return false;
            BaseEventData data = GetBaseEventData();
            ExecuteEvents.Execute(eventSystem.currentSelectedGameObject, data, ExecuteEvents.updateSelectedHandler);
            return data.used;
        }

        /// <summary>
        /// Force selection of a gameobject.
        /// </summary>
        private void Select(GameObject go)
        {
            ClearSelection();
            if (ExecuteEvents.GetEventHandler<ISelectHandler>(go))
            {
                eventSystem.SetSelectedGameObject(go);
            }
        }

        /// <summary>
        /// Adds necessary components to Vive controller gameobjects. These will let us know what inputs are used on them.
        /// </summary>
        private void SetupControllers()
        {
            //Find Controller manager on the scene.
            if (controllerManager == null)
            {
                controllerManager = Object.FindObjectOfType<SteamVR_ControllerManager>();
                if (controllerManager == null)
                {
                    Debug.LogError("Can't find SteamVR_ControllerManager on scene. It is required to use VIVE control method. Make sure all SteamVR prefabs are present.");
                }
            }

            //attach assistant scripts to cui objects.
            if (rightCont == null)
            {
                CurvedUIViveController viveCont = controllerManager.right.GetComponent<CurvedUIViveController>();
                if (viveCont == null)
                {
                    viveCont = controllerManager.right.AddComponent<CurvedUIViveController>();
                }
                rightCont = viveCont;
            }

            if (leftCont == null)
            {
                CurvedUIViveController viveCont = controllerManager.left.GetComponent<CurvedUIViveController>();
                if (viveCont == null)
                {
                    viveCont = controllerManager.left.AddComponent<CurvedUIViveController>();
                }
                leftCont = viveCont;
            }
        }
        #endregion


        #region PUBLIC FUNCTIONS
        /// <summary>
        /// Clear the currently selected gameobject
        /// </summary>
        public void ClearSelection()
        {
            if (eventSystem.currentSelectedGameObject)
            {
                eventSystem.SetSelectedGameObject(null);
            }
        }
        #endregion

        #region SETTERS AND GETTERS
        /// <summary>
        /// guiRaycastHit is helpful if you have other places you want to use look input outside of UI system
        /// you can use this to tell if the UI raycaster hit a UI element
        /// </summary>
        public bool guiRaycastHit {
            get
            {
                return _guiRaycastHit;
            }
        }
        private bool _guiRaycastHit;

        //not needed now, but may come handy later.

        ///// <summary>
        ///// controlAxisUsed is helpful if you use same axis elsewhere
        ///// you can use this boolean to see if the UI used the axis control or not
        ///// if something is selected and takes move event, then this will be set
        ///// </summary>
        //public bool controlAxisUsed {
        //    get
        //    {
        //        return _controlAxisUsed;
        //    }
        //}
        //private bool _controlAxisUsed;

        ///// <summary>
        ///// buttonUsed is helpful if you use same button elsewhere
        ///// you can use this boolean to see if the UI used the button press or not
        ///// </summary>
        //public bool buttonUsed {
        //    get
        //    {
        //        return _buttonUsed;
        //    }
        //}
        //private bool _buttonUsed;

        
        /// <summary>
        /// Get or Set controller manager used by this input module.
        /// </summary>
        public SteamVR_ControllerManager ControllerManager {
            get { return controllerManager; }
            set
            {
                controllerManager = value;
                SetupControllers();
            }
        }
   
        /// <summary>
        /// Returns Right Vive Controller. Ask this component for any button states.;
        /// </summary>
        public static CurvedUIViveController Right {
            get { return rightCont ; }
        }

        /// <summary>
        /// Returns Left Vive Controller. Ask this component for any button states.;
        /// </summary>
        public static CurvedUIViveController Left {
            get { return leftCont; }
        }

        public static CurvedUIViveInputModule Instance {
            get
            {
                return instance;
            }
        }

        /// <summary>
        /// Gameobject we're currently pointing at.
        /// </summary>
        public GameObject CurrentPointedAt {
            get
            {
                return currentPointedAt;
            }
        }
        #endregion
    }
}

#else

namespace CurvedUI {

	/// <summary>
	/// InputModule made for Vive controllers. Create PointerEvents that are later used by eventSystem to discover interactions.
	/// </summary>
	public class CurvedUIViveInputModule : BaseInputModule
	{

		public override void Process(){}

	}
}


#endif






