using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lairinus.UI.Events
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Graphic))]
    public partial class UIEventState : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        public enum EventType
        {
            OnNormal,
            OnHover,
            OnBeginDrag,
            OnEndDrag,
            OnDrag,
            OnPointerEnter,
            OnPointerExit,
            OnPointerUp,
            OnPointerDown,
            OnPointerClick
        }

        public enum EventTransitionType
        {
            ImageColor,
            ImageSpriteAndFill,
            Rotation,
            Scale,
            TextColor,
            TextFontSize
        }

        public EventType selectedEventTypeInternal { get; set; }
        public EventTransitionType selectedEventTransitionInternal { get; set; }
        public Dictionary<EventType, EventState> eventStatesCollection { get; protected set; }

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

        public RectTransform cachedRectTransform { get; private set; }
        public EventState onBeginDragState { get; set; }
        public EventState onDragState { get; set; }
        public EventState onEndDragState { get; set; }
        public EventState onHoverState { get; set; }
        public EventState onNormalState { get; set; }
        public EventState onPointerClickState { get; set; }
        public EventState onPointerDownState { get; set; }
        public EventState onPointerEnterState { get; set; }
        public EventState onPointerExitState { get; set; }
        public EventState onPointerUpState { get; set; }
        [SerializeField] private bool _enableDebugging = false;
        private bool isHovering = false;
        private EventState lastTriggeredState = null;

        #endregion Private Fields

        #region Public Methods

        public void OnBeginDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called the moment the user drags the mouse with left or right click held down
             */

            #endregion Remarks

            isHovering = true;
            HandleEventState(onBeginDragState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnBeginDrag.Replace("%%custom%%", name));
        }

        public void OnDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called while the user is pressing the left or the mouse button down while moving their mouse
             */

            #endregion Remarks

            isHovering = true;
            HandleEventState(onDragState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnDrag.Replace("%%custom%%", name));
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called the moment after the user lets up on their mouse after dragging
             */

            #endregion Remarks

            HandleEventState(onEndDragState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnEndDrag.Replace("%%custom%%", name));
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            #region Remarks

            /*   Internal use only
             *   ------------------
             *   Called after OnPointerUp. Very similar to OnPointerUp
             */

            #endregion Remarks

            isHovering = true;
            HandleEventState(onPointerClickState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerClick.Replace("%%custom%%", name));
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called when the user presses the left or right mouse button down
             */

            #endregion Remarks

            HandleEventState(onPointerDownState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerDown.Replace("%%custom%%", name));
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called when the mouse pointer enters the graphic element's region
             */

            #endregion Remarks

            isHovering = true;
            HandleEventState(onPointerEnterState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerEnter.Replace("%%custom%%", name));
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called when the mouse button leaves the graphic element's region.
             *   This is also the "normalized" state because this state is the only state that excludes active mouse input
             */

            #endregion Remarks

            isHovering = false;
            HandleEventState(onPointerExitState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerExit.Replace("%%custom%%", name));
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called when the user releases the left or right mouse button while hovered over the graphic element
             */

            #endregion Remarks

            HandleEventState(onPointerUpState);
            if (_enableDebugging)
                Debug.Log(Debugger.Debug_OnPointerUp.Replace("%%custom%%", name));
        }

        protected Graphic graphicElement { get; set; }

        protected virtual void Awake()
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   The same as a "new Class()" constructor in non-MonoBehaviours
             */

            #endregion Remarks

            cachedRectTransform = (RectTransform)transform;
            HandleEventState(onNormalState);
        }

        protected virtual void Start()
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Called directly after Awake()
             */

            #endregion Remarks

            if (graphicElement != null)
                graphicElement.raycastTarget = true;
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

            if (lastTriggeredState != currentEventState)
            {
                lastTriggeredState = currentEventState;
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

            float totalTransitionTime = eventState.allowedTransitionTime;
            float currentTransitionTime = 0;
            eventState.onStateEnterEvent.Invoke();
            while (currentTransitionTime < totalTransitionTime)
            {
                RunEventStateTransitions(eventState, currentTransitionTime, totalTransitionTime);

                yield return null;
                currentTransitionTime += Time.deltaTime;
            }

            RunEventStateTransitions(eventState, 1, 1);

            if (isHovering && onHoverState != lastTriggeredState)
            {
                HandleEventState(onHoverState);
            }
            else if (!isHovering && onNormalState != lastTriggeredState)
            {
                HandleEventState(onNormalState);
            }
            else if (isHovering)
                HandleEventState(onHoverState);
            else HandleEventState(onNormalState);
        }

        private void OnEnable()
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Resets the state of this Graphic object once it becomes active
             */

            #endregion Remarks

            isHovering = false;
            if (onNormalState != null)
                HandleEventState(onNormalState);
        }

        private void RunEventStateTransitions(EventState eventState, float currentTransitionTime, float totalTransitionTime)
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   Processes all transitions for the Graphic element attached to this UIEventState component
             */

            #endregion Remarks

            eventState.transitionScale.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
            eventState.transitionRotation.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
            if (eventState is ImageEventState)
            {
                ImageEventState ies = (ImageEventState)eventState;
                ies.imageColorTransition.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
                ies.imageSpriteAndFill.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
            }
            else if (eventState is TextEventState)
            {
                TextEventState tes = (TextEventState)eventState;
                tes.transitionTextColor.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
                tes.transitionFontSize.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
            }
        }

        #endregion Public Methods

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
            public static readonly string Debug_OnPointerUp = "UIEventState on GameObject %%custom%% => Registered POINTER UP";
            public static readonly string Debug_OnPotentialDragInitialized = "UIEventState on GameObject %%custom%% => Registered POTENTIAL DRAG INITIALIZED";

            #endregion Public Fields
        }

        #endregion Private Classes

        public void Initialize()
        {
            Awake();
        }
    }
}