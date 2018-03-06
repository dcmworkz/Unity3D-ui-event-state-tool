using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lairinus.UI.Events
{
    #region Remarks

    /*
     *   External and internal use
     *   ------------------
     *   Contains pointer events that call events that can be used with a UnityEngine.UI.Image object.
     */

    #endregion Remarks

    [ExecuteInEditMode]
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Image))]
    public class UIImageEventState : UIEventState
    {
        #region Remarks

        /*
         *   External and internal use
         *   ------------------
         *   Contains pointer events that call events that can be used with an image object.
         */

        #endregion Remarks

        #region Protected Methods

        protected override void Awake()
        {
            #region Remarks

            /*
             *   Internal use only
             *   ------------------
             *   The same as a "new Class()" constructor in non-MonoBehaviours
             */

            #endregion Remarks

            base.Awake();
            graphicElement = GetComponent<Image>();
            onBeginDragState = _onBeginDragState;
            onDragState = _onDragState;
            onEndDragState = _onEndDragState;
            onHoverState = _onHoverState;
            onNormalState = _onNormalState;
            onPointerClickState = _onPointerClickState;
            onPointerDownState = _onPointerDownState;
            onPointerEnterState = _onPointerEnterState;
            onPointerExitState = _onPointerExitState;
            onPointerUpState = _onPointerUpState;

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
        }

        #endregion Protected Methods

        #region Private Fields

        [SerializeField] private ImageEventState _onBeginDragState = new ImageEventState(EventType.OnBeginDrag);
        [SerializeField] private ImageEventState _onDragState = new ImageEventState(EventType.OnDrag);
        [SerializeField] private ImageEventState _onEndDragState = new ImageEventState(EventType.OnEndDrag);
        [SerializeField] private ImageEventState _onHoverState = new ImageEventState(EventType.OnHover);
        [SerializeField] private ImageEventState _onNormalState = new ImageEventState(EventType.OnNormal);
        [SerializeField] private ImageEventState _onPointerClickState = new ImageEventState(EventType.OnPointerClick);
        [SerializeField] private ImageEventState _onPointerDownState = new ImageEventState(EventType.OnPointerDown);
        [SerializeField] private ImageEventState _onPointerEnterState = new ImageEventState(EventType.OnPointerEnter);
        [SerializeField] private ImageEventState _onPointerExitState = new ImageEventState(EventType.OnPointerExit);
        [SerializeField] private ImageEventState _onPointerUpState = new ImageEventState(EventType.OnPointerUp);

        #endregion Private Fields
    }
}