using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lairinus.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Graphic))]
    public partial class UIEventState : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IInitializePotentialDragHandler
    {
        #region Remarks

        /*
         *   This component captures every pointer event and then:
         *     1. Changes the UI Graphic's color
         *     2. Changes the UI Graphic's sprite (if applicable)
         *     3. Calls methods attached to any GameObjects in the scene, or on other objects not in the scene
         *   Limitations:
         *      1. Can only be used on "UnityEngine.UI.Graphic" objects such as Images and Texts.
         *      2. The property "raycastTarget" MUST be set to true. If there are no raycasts, there are no events
         */

        #endregion Remarks

        #region Private Fields

        [SerializeField] private bool _enableDebugging = false;
<<<<<<< HEAD
        [SerializeField] private EventState _normalState = new EventState();
        [SerializeField] private EventState _onHoverState = new EventState();
        [SerializeField] private EventState _onPointerClickState = new EventState();
        [SerializeField] private EventState _onPointerDownState = new EventState();
        [SerializeField] private EventState _onPointerUpState = new EventState();
        [SerializeField] private EventState _onPointerEnterState = new EventState();
        [SerializeField] private EventState _onBeginDragState = new EventState();
        [SerializeField] private EventState _onEndDragState = new EventState();
        [SerializeField] private EventState _onDragState = new EventState();
        [SerializeField] private EventState _onInitializePotentialDragState = new EventState();
        private EventState _lastTriggeredState = null;

        public RectTransform cachedRectTransform { get; private set; }
=======
        [SerializeField] private EventState _onBeginDrag = new EventState();
        [SerializeField] private EventState _onDrag = new EventState();
        [SerializeField] private EventState _onEndDrag = new EventState();
        [SerializeField] private EventState _onIntiializePotentialDrag = new EventState();
        [SerializeField] private EventState _onPointerClick = new EventState();
        [SerializeField] private EventState _onPointerDown = new EventState();
        [SerializeField] private EventState _onPointerEnter = new EventState();
        [SerializeField] private EventState _onPointerExit = new EventState();
        [SerializeField] private EventState _onPointerUp = new EventState();

        #endregion Private Fields

        #region Public Methods
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de

        public void OnBeginDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Called the moment the user drags the mouse with left or right click held down
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_onBeginDragState);

=======
            _onBeginDrag.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnBeginDrag.Replace("%%custom%%", name));
        }

        public void OnDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Called while the user is pressing the left or the mouse button down while moving their mouse
             */

            #endregion Remarks

<<<<<<< HEAD
            Button i = null;

            HandleEventState(_onDragState);

=======
            _onDrag.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnDrag.Replace("%%custom%%", name));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Called the moment after the user lets up on their mouse after dragging
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_onEndDragState);

=======
            _onEndDrag.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnEndDrag.Replace("%%custom%%", name));
        }

        public void OnInitializePotentialDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Similar to OnPointerDown. Called when the left or right mouse button is pressed down. Called after the OnPointerDown event
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_onInitializePotentialDragState);

=======
            _onIntiializePotentialDrag.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPotentialDragInitialized.Replace("%%custom%%", name));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            #region Remarks

            /*   Internal use only
             *   ------------------
             *   Called after OnPointerUp. Very similar to OnPointerUp
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_onPointerClickState);

=======
            _onPointerClick.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerClick.Replace("%%custom%%", name));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            #region Remarks

            /*
<<<<<<< HEAD
             *   Internal use only
             *   ------------------
=======
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
             *   Called when the user presses the left or right mouse button down
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_onPointerDownState);

=======
            _onPointerDown.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerDown.Replace("%%custom%%", name));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            #region Remarks

            /*
<<<<<<< HEAD
             *   Internal use only
             *   ------------------
             *   Called when the mouse pointer enters the graphic element's region
=======
             *   Called when the user's pointer enters this element's region
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_onPointerEnterState);

=======
            _onPointerEnter.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerEnter.Replace("%%custom%%", name));
        }

        private void HandleEventState(EventState currentEventState)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Determines if this event state is new, or if it is current. If it is new, it is run, else it is ignored.
             *   If the state is new, we run all of the associated transitions.
             */

            #endregion Remarks

            if (_lastTriggeredState != currentEventState)
            {
                _lastTriggeredState = currentEventState;
                StopCoroutine("HandleEventStateRoutine");
                StartCoroutine("HandleEventStateRoutine", currentEventState);
            }
        }

        private IEnumerator HandleEventStateRoutine(EventState eventState)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Transitions all of the states over time, as opposed to all at once.
             */

            #endregion Remarks

            float totalTransitionTime = eventState.transitionTime;
            float currentTransitionTime = 0;
            while (currentTransitionTime < totalTransitionTime)
            {
                eventState.scaleTransition.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);

                yield return null;
                currentTransitionTime += Time.deltaTime;
            }
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            #region Remarks

            /*
<<<<<<< HEAD
             *   Internal use only
             *   ------------------
             *   Called when the mouse button leaves the graphic element's region.
             *   This is also the "normalized" state because this state is the only state that excludes active mouse input
=======
             *   Called when the user's pointer exits this element's region
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
             */

            #endregion Remarks

<<<<<<< HEAD
            HandleEventState(_normalState);

=======
            _onPointerExit.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerExit.Replace("%%custom%%", name));
        }

        private void Awake()
        {
            cachedRectTransform = (RectTransform)transform;
            HandleEventState(_normalState);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            #region Remarks

            /*
<<<<<<< HEAD
             *   Internal use only
             *   ------------------
             *   Called when the user releases the left or right mouse button while hovered over the graphic element
=======
             *   Called when the user releases the left or right mouse button up
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
             */

            #endregion Remarks

<<<<<<< HEAD
=======
            _onPointerUp.InvokeCallbacks();
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerUp.Replace("%%custom%%", name));
        }

        #endregion Public Methods

        #region Public Classes

        [System.Serializable]
        public class EventState
        {
<<<<<<< HEAD
            [SerializeField] private float _transitionTime = 0.25F;
            public float transitionTime { get { return _transitionTime; } set { _transitionTime = value; } }

            [SerializeField] private bool _stateEnabled = true;
            public bool stateEnabled { get { return _stateEnabled; } set { _stateEnabled = value; } }

            // Flow Control - prevents an event handler being called multiple times (useful for persistent events such as OnDrag)
            public bool stateCalculationStarted { get; set; }

            [SerializeField] private ScaleTransition _scaleTransition = new ScaleTransition();
            public ScaleTransition scaleTransition { get { return _scaleTransition; } }
=======
            [SerializeField] private UnityEvent _callback = new UnityEvent();
            [SerializeField] private bool _stateEnabled = false;
            public bool stateEnabled { get { return _stateEnabled; } set { _stateEnabled = value; } }
            public UnityEvent callback { get { return _callback; } }

            public void InvokeCallbacks()
            {
                if (_stateEnabled)
                    _callback.Invoke();
            }
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
        }

        #endregion Public Classes

        #region Private Classes

        private class Debugger
        {
            #region Remarks

            /*
             *   Provides the user with useful information
             */

            #endregion Remarks

            #region Public Fields

            public static readonly string Debug_OnBeginDrag = "UIEventState on GameObject %%custom%% => Registered BEGIN DRAG";
            public static readonly string Debug_OnDrag = "UIEventState on GameObject %%custom%% => Registered DRAG";
            public static readonly string Debug_OnEndDrag = "UIEventState on GameObject %%custom%% => Registered END DRAG";
            public static readonly string Debug_OnPointerClick = "UIEventState on GameObject %%custom%% => Registered POINTER CLICK";
            public static readonly string Debug_OnPointerDown = "UIEventState on GameObject %%custom%% => Registered POINTER DOWN";
            public static readonly string Debug_OnPointerEnter = "UIEventState on GameObject %%custom%% => Registered POINTER ENTER";
            public static readonly string Debug_OnPointerExit = "UIEventState on GameObject %%custom%% => Registered POINTER EXIT";
<<<<<<< HEAD
            public static readonly string Debug_OnBeginDrag = "UIEventState on GameObject %%custom%% => Registered BEGIN DRAG";
            public static readonly string Debug_OnEndDrag = "UIEventState on GameObject %%custom%% => Registered END DRAG";
            public static readonly string Debug_OnDrag = "UIEventState on GameObject %%custom%% => Registered DRAG";
            public static readonly string Debug_OnPointerHover = "UIEventState on GameObject %%custom%% => Registered HOVER";
=======
            public static readonly string Debug_OnPointerUp = "UIEventState on GameObject %%custom%% => Registered POINTER UP";
>>>>>>> a07e4b68bb2d244226c548314a13c2cb3076f7de
            public static readonly string Debug_OnPotentialDragInitialized = "UIEventState on GameObject %%custom%% => Registered POTENTIAL DRAG INITIALIZED";

            #endregion Public Fields
        }

        #endregion Private Classes

        Graphic graphic = null;
        private void Awake()
        {
            graphic = GetComponent<Graphic>();        
        }
    }
}