using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Lairinus.UI
{
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

        [SerializeField] private ImageEventState _onBeginDragState = new ImageEventState();
        [SerializeField] private ImageEventState _onDragState = new ImageEventState();
        [SerializeField] private ImageEventState _onEndDragState = new ImageEventState();
        [SerializeField] private ImageEventState _onHoverState = new ImageEventState();
        [SerializeField] private ImageEventState _onInitializePotentialDragState = new ImageEventState();
        [SerializeField] private ImageEventState _onNormalState = new ImageEventState();
        [SerializeField] private ImageEventState _onPointerClickState = new ImageEventState();
        [SerializeField] private ImageEventState _onPointerDownState = new ImageEventState();
        [SerializeField] private ImageEventState _onPointerEnterState = new ImageEventState();
        [SerializeField] private ImageEventState _onPointerExitState = new ImageEventState();
        [SerializeField] private ImageEventState _onPointerUpState = new ImageEventState();

        protected override void Awake()
        {
            base.Awake();
            graphicElement = GetComponent<Image>();
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