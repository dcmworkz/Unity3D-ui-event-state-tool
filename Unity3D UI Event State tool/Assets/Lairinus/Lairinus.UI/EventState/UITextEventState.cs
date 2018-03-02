using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lairinus.UI
{
    [RequireComponent(typeof(RectTransform))]
    [RequireComponent(typeof(Text))]
    public class UITextEventState : UIEventState
    {
        [SerializeField] TextEventState _onBeginDragState = new TextEventState();
        [SerializeField] TextEventState _onDragState = new TextEventState();
        [SerializeField] TextEventState _onEndDragState = new TextEventState();
        [SerializeField] TextEventState _onHoverState = new TextEventState();
        [SerializeField] TextEventState _onInitializePotentialDragState = new TextEventState();
        [SerializeField] TextEventState _onNormalState = new TextEventState();
        [SerializeField] TextEventState _onPointerClickState = new TextEventState();
        [SerializeField] TextEventState _onPointerDownState = new TextEventState();
        [SerializeField] TextEventState _onPointerEnterState = new TextEventState();
        [SerializeField] TextEventState _onPointerExitState = new TextEventState();
        [SerializeField] TextEventState _onPointerUpState = new TextEventState();

        protected override void Awake()
        {
            base.Awake();
            graphicElement = GetComponent<Text>();

            onBeginDragState = _onBeginDragState;
            onDragState = _onDragState;
            onEndDragState = _onEndDragState;
            onHoverState = _onHoverState;
            onInitializePotentialDragState = _onInitializePotentialDragState;
            onNormalState = _onNormalState;
            onPointerClickState = _onPointerClickState;
            onPointerDownState = _onPointerDownState;
            onPointerEnterState = _onPointerEnterState;
            onPointerExitState = _onPointerExitState;
            onPointerUpState = _onPointerUpState;
        }
    }
}