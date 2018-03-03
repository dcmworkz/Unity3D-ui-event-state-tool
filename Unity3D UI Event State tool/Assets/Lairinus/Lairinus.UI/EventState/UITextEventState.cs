using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lairinus.UI.Events
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Text))]
    public class UITextEventState : UIEventState
    {
        #region Remarks

        /*
         *   External and internal use
         *   ------------------
         *   Contains pointer events that call events that can be used with a UnityEngine.UI.Text object.
         */

        #endregion Remarks

        [SerializeField] TextEventState _onBeginDragState = new TextEventState();
        [SerializeField] TextEventState _onDragState = new TextEventState();
        [SerializeField] TextEventState _onEndDragState = new TextEventState();
        [SerializeField] TextEventState _onHoverState = new TextEventState();
        [SerializeField] TextEventState _onNormalState = new TextEventState();
        [SerializeField] TextEventState _onPointerClickState = new TextEventState();
        [SerializeField] TextEventState _onPointerDownState = new TextEventState();
        [SerializeField] TextEventState _onPointerEnterState = new TextEventState();
        [SerializeField] TextEventState _onPointerExitState = new TextEventState();
        [SerializeField] TextEventState _onPointerUpState = new TextEventState();

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
            graphicElement = GetComponent<Text>();
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
        }
    }
}