using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Events;

namespace Lairinus.UI.Events
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Graphic))]
    public partial class UIEventState : MonoBehaviour, IPointerClickHandler, IPointerUpHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
    {
        #region Public Enums

        public enum EventTransitionType
        {
            SpriteAndFill,
            Rotation,
            Scale,
            Color,
            TextFontSize
        }

        public enum EventType
        {
            OnNormal = 0,
            OnHover = 1,
            OnBeginDrag = 2,
            OnEndDrag = 3,
            OnDrag = 4,
            OnPointerEnter = 5,
            OnPointerExit = 6,
            OnPointerUp = 7,
            OnPointerDown = 8,
            OnPointerClick = 9
        }

        #endregion Public Enums

        #region Public Properties

        public RectTransform cachedRectTransform { get; private set; }
        public EventState onBeginDragState { get { return _onBeginDragState; } }
        public EventState onDragState { get { return _onDragState; } }
        public EventState onEndDragState { get { return _onEndDragState; } }
        public EventState onHoverState { get { return _onHoverState; } }
        public EventState onNormalState { get { return _onNormalState; } }
        public EventState onPointerClickState { get { return _onPointerClickState; } }
        public EventState onPointerDownState { get { return _onPointerDownState; } }
        public EventState onPointerEnterState { get { return _onPointerEnterState; } }
        public EventState onPointerExitState { get { return _onPointerExitState; } }
        public EventState onPointerUpState { get { return _onPointerUpState; } }

        #endregion Public Properties

        #region Public Methods

        public void Initialize()
        {
            // Add all of the event states to a Dictionary for easy access
            eventStatesCollection = new Dictionary<EventType, EventState>();
            eventStatesCollection.Add(EventType.OnHover, onHoverState);
            eventStatesCollection.Add(EventType.OnNormal, onNormalState);
            eventStatesCollection.Add(EventType.OnPointerEnter, onPointerEnterState);
            eventStatesCollection.Add(EventType.OnPointerExit, onPointerExitState);
            eventStatesCollection.Add(EventType.OnPointerDown, onPointerDownState);
            eventStatesCollection.Add(EventType.OnPointerUp, onPointerUpState);
            eventStatesCollection.Add(EventType.OnPointerClick, onPointerClickState);
            eventStatesCollection.Add(EventType.OnBeginDrag, onBeginDragState);
            eventStatesCollection.Add(EventType.OnEndDrag, onEndDragState);
            eventStatesCollection.Add(EventType.OnDrag, onDragState);

            textElement = GetComponent<Text>();
            imageElement = GetComponent<Image>();
            cachedRectTransform = (RectTransform)transform;
            VisualizeTransitionInternal(onNormalState);
        }

        public void VisualizeTransitionInternal(EventState eventState)
        {
            if (currentEventState != eventState)
            {
                previousEventState = currentEventState;
                currentEventState = eventState;
            }

            RunEventStateTransitions(eventState, 1, 1);
        }

        #endregion Public Methods

        #region Public Classes

        [System.Serializable]
        public class EventState
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Contains the Graphic object transitions shared between the Graphic objects
             */

            #endregion Remarks

            #region Public Constructors

            public EventState(EventType thisEventType)
            {
                _eventType = thisEventType;
                eventTransitionCollection = new Dictionary<EventTransitionType, BaseTransition>();
                eventTransitionCollection.Add(EventTransitionType.Rotation, _transitionRotation);
                eventTransitionCollection.Add(EventTransitionType.Scale, _transitionScale);
                eventTransitionCollection.Add(EventTransitionType.Color, _colorTransition);
                eventTransitionCollection.Add(EventTransitionType.SpriteAndFill, _imageSpriteAndFill);
                eventTransitionCollection.Add(EventTransitionType.TextFontSize, _transitionFontSize);
            }

            #endregion Public Constructors

            #region Public Properties

            public Dictionary<EventTransitionType, BaseTransition> eventTransitionCollection { get; protected set; }
            public ColorTransition colorTransition { get { return _colorTransition; } }
            public SpriteAndFillTransition imageSpriteAndFill { get { return _imageSpriteAndFill; } }
            public TextFontSizeTransition textFontSizeTransition { get { return _transitionFontSize; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] private ColorTransition _colorTransition = new ColorTransition(EventTransitionType.Color);
            [SerializeField] private SpriteAndFillTransition _imageSpriteAndFill = new SpriteAndFillTransition(EventTransitionType.SpriteAndFill);
            [SerializeField] private TextFontSizeTransition _transitionFontSize = new TextFontSizeTransition(EventTransitionType.TextFontSize);

            #endregion Private Fields

            #region Public Properties

            public float allowedTransitionTime { get { return _allowedTransitionTime; } set { _allowedTransitionTime = value; } }
            public UnityEvent onStateEnterEvent { get { return _onStateEnterEvent; } set { _onStateEnterEvent = value; } }
            public RotationTransition transitionRotation { get { return _transitionRotation; } }
            public ScaleTransition transitionScale { get { return _transitionScale; } }

            #endregion Public Properties

            #region Private Fields

            public EventType eventType { get { return _eventType; } }
            [SerializeField] protected float _allowedTransitionTime = 0.25F;
            [SerializeField] protected UnityEvent _onStateEnterEvent = new UnityEvent();
            [SerializeField] protected RotationTransition _transitionRotation = new RotationTransition(EventTransitionType.Rotation);
            [SerializeField] protected ScaleTransition _transitionScale = new ScaleTransition(EventTransitionType.Scale);
            private EventType _eventType = EventType.OnBeginDrag;

            #endregion Private Fields
        }

        #endregion Public Classes

        #region Private Fields

        [SerializeField] private bool _enableDebugging = false;
        [SerializeField] private EventState _onBeginDragState = new EventState(EventType.OnBeginDrag);
        [SerializeField] private EventState _onDragState = new EventState(EventType.OnDrag);
        [SerializeField] private EventState _onEndDragState = new EventState(EventType.OnEndDrag);
        [SerializeField] private EventState _onHoverState = new EventState(EventType.OnHover);
        [SerializeField] private EventState _onNormalState = new EventState(EventType.OnNormal);
        [SerializeField] private EventState _onPointerClickState = new EventState(EventType.OnPointerClick);
        [SerializeField] private EventState _onPointerDownState = new EventState(EventType.OnPointerDown);
        [SerializeField] private EventState _onPointerEnterState = new EventState(EventType.OnPointerEnter);
        [SerializeField] private EventState _onPointerExitState = new EventState(EventType.OnPointerExit);
        [SerializeField] private EventState _onPointerUpState = new EventState(EventType.OnPointerUp);

        #endregion Private Fields

        #region Public Properties

        public EventState currentEventState { get; protected set; }
        public Dictionary<EventType, EventState> eventStatesCollection { get; protected set; }
        public EventState previousEventState { get; protected set; }
        public EventTransitionType selectedEventTransitionInternal { get; set; }
        public EventType selectedEventTypeInternal { get; set; }

        #endregion Public Properties

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

        #region Public Fields

        public UnityEvent OnBeginDragUnityEvent = new UnityEvent();
        public UnityEvent OnDragUnityEvent = new UnityEvent();
        public UnityEvent OnEndDragUnityEvent = new UnityEvent();
        public UnityEvent OnHoverUnityEvent = new UnityEvent();
        public UnityEvent OnNormalUnityEvent = new UnityEvent();
        public UnityEvent OnPointerClickUnityEvent = new UnityEvent();
        public UnityEvent OnPointerDownUnityEvent = new UnityEvent();
        public UnityEvent OnPointerEnterUnityEvent = new UnityEvent();
        public UnityEvent OnPointerExitUnityEvent = new UnityEvent();
        public UnityEvent OnPointerUpUnityEvent = new UnityEvent();

        #endregion Public Fields

        private bool isHovering = false;
        private EventState lastTriggeredState = null;

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

        public Image imageElement { get; private set; }
        public Text textElement { get; private set; }

        protected virtual void Awake()
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   The same as a "new Class()" constructor in non-MonoBehaviours
             */

            #endregion Remarks

            Initialize();
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

            if (textElement != null)
                textElement.raycastTarget = true;
            else if (imageElement != null)
                imageElement.raycastTarget = true;
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
            eventState.colorTransition.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
            eventState.imageSpriteAndFill.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
            eventState.textFontSizeTransition.RunTransition_Internal(this, currentTransitionTime, totalTransitionTime);
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
    }
}