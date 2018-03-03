using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Lairinus.UI.Events
{
    public partial class UIEventState : MonoBehaviour
    {
        #region Remarks

        /*
         *    Internal and External use
         *    ---------------------
         *    Partial class created because the functionality would look cramped in the other file,
         *    and all of this functionality technically doesn't need to stand on its' own
         */

        #endregion Remarks

        #region Public Classes

        [System.Serializable]
        public class BaseTransition
        {
            #region Remarks

            /*
             *    For internal use only
             *    ---------------------
             *    Contains some shared customization for successfully running transitions on Graphic objects
             */

            #endregion Remarks

            #region Public Properties

            public bool ignoreTransiton { get { return _ignoreTransition; } set { _ignoreTransition = value; } }

            #endregion Public Properties

            #region Public Methods

            public virtual void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Runs the transition on the Graphic object
                 */

                #endregion Remarks

                if (_ignoreTransition)
                    return;
            }

            #endregion Public Methods

            #region Protected Fields

            // If true, this will ignore the transition time for the elements and snap directly to a certain scale.
            [SerializeField] protected bool _ignoreTransition = false;

            #endregion Protected Fields
        }

        [System.Serializable]
        public class EventState
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Contains the Graphic object transitions shared between the Graphic objects
             */

            #endregion Remarks

            #region Public Properties

            public UnityEvent onStateEnterEvent { get { return _onStateEnterEvent; } }
            public RotationTransition rotationTransition { get { return _rotationTransition; } }
            public ScaleTransition scaleTransition { get { return _scaleTransition; } }
            public bool stateEnabled { get { return _stateEnabled; } set { _stateEnabled = value; } }
            public float transitionTime { get { return _transitionTime; } set { _transitionTime = value; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] protected UnityEvent _onStateEnterEvent = new UnityEvent();
            [SerializeField] protected RotationTransition _rotationTransition = new RotationTransition();
            [SerializeField] protected ScaleTransition _scaleTransition = new ScaleTransition();
            [SerializeField] protected bool _stateEnabled = true;
            [SerializeField] protected float _transitionTime = 0.25F;

            #endregion Private Fields
        }

        [System.Serializable]
        public class ImageColorTransition : BaseTransition
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Transition information to modify an Image's color
             */

            #endregion Remarks

            #region Public Properties

            public Color finalColor { get { return _finalColor; } set { _finalColor = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to scale the Graphic object to its' desired target.
                 */

                #endregion Remarks

                if (_ignoreTransition)
                    return;

                Image image = null;
                if (eventStateElement is UIImageEventState)
                {
                    UIImageEventState imageEventState = (UIImageEventState)eventStateElement;
                    image = (Image)imageEventState.graphicElement;
                }

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                Color elementColor = image.color;
                elementColor = Color.Lerp(elementColor, _finalColor, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    elementColor = _finalColor;

                image.color = elementColor;
            }

            #endregion Public Methods

            #region Private Fields

            [SerializeField] private Color _finalColor = Color.white;

            #endregion Private Fields
        }

        [System.Serializable]
        public class ImageEventState : EventState
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Contains the transition information for a specific EventState for a UnityEngine.UI.Image object
             */

            #endregion Remarks

            #region Public Properties

            public ImageColorTransition imageColorTransition { get { return _imageColorTransition; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] private ImageColorTransition _imageColorTransition = new ImageColorTransition();

            #endregion Private Fields
        }

        [System.Serializable]
        public class RotationTransition : BaseTransition
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a Graphic object's Euler Angle rotation
             */

            #endregion Remarks

            #region Public Properties

            public Vector3 finalRotation { get { return _finalRotation; } set { _finalRotation = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to rotate the Graphic object to its' desired target.
                 */

                #endregion Remarks

                if (_ignoreTransition)
                    return;

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                eventStateElement.cachedRectTransform.eulerAngles = Vector3.Lerp(eventStateElement.cachedRectTransform.eulerAngles, _finalRotation, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    eventStateElement.cachedRectTransform.eulerAngles = _finalRotation;
            }

            #endregion Public Methods

            #region Private Fields

            [SerializeField] private Vector3 _finalRotation = new Vector3(0, 0, 0);

            #endregion Private Fields
        }

        [System.Serializable]
        public class ScaleTransition : BaseTransition
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a Graphic object's scale
             */

            #endregion Remarks

            #region Public Properties

            public Vector2 finalScale { get { return _finalScale; } set { _finalScale = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to scale the target to its' desired target.
                 */

                #endregion Remarks

                if (_ignoreTransition)
                    return;

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                eventStateElement.cachedRectTransform.localScale = Vector3.Lerp(eventStateElement.cachedRectTransform.localScale, _finalScale, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    eventStateElement.cachedRectTransform.localScale = _finalScale;
            }

            #endregion Public Methods

            #region Private Fields

            [SerializeField] private Vector2 _finalScale = new Vector3(1, 1, 1);

            #endregion Private Fields
        }

        [System.Serializable]
        public class TextColorTransition : BaseTransition
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a UnityEngine.UI.Text object's color
             */

            #endregion Remarks

            #region Public Properties

            public Color finalColor { get { return _finalColor; } set { _finalColor = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Lerps the color of the Text object
                 */

                #endregion Remarks

                if (_ignoreTransition)
                    return;

                Text text = null;
                if (eventStateElement is UITextEventState)
                {
                    UITextEventState textEventState = (UITextEventState)eventStateElement;
                    text = (Text)textEventState.graphicElement;
                }

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                Color elementColor = text.color;
                elementColor = Color.Lerp(elementColor, _finalColor, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    elementColor = _finalColor;

                text.color = elementColor;
            }

            #endregion Public Methods

            #region Private Fields

            [SerializeField] private Color _finalColor = Color.white;

            #endregion Private Fields
        }

        [System.Serializable]
        public class TextEventState : EventState
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Contains the transition information for a specific EventState for a UnityEngine.UI.Text object
             */

            #endregion Remarks

            #region Public Properties

            public TextColorTransition colorTransition { get { return _colorTransition; } }
            public TextFontSizeTransition fontSizeTransition { get { return _fontSizeTransition; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] private TextColorTransition _colorTransition = new TextColorTransition();
            [SerializeField] private TextFontSizeTransition _fontSizeTransition = new TextFontSizeTransition();

            #endregion Private Fields
        }

        [System.Serializable]
        public class TextFontSizeTransition : BaseTransition
        {
            #region Public Properties

            public int finalFontSize { get { return _finalFontSize; } set { _finalFontSize = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Lerps the color of the Text object
                 */

                #endregion Remarks

                if (_ignoreTransition)
                    return;

                Text text = null;
                if (eventStateElement is UITextEventState)
                {
                    UITextEventState textEventState = (UITextEventState)eventStateElement;
                    text = (Text)textEventState.graphicElement;
                }

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                float elementFontSize = text.fontSize;
                elementFontSize = Mathf.Lerp(elementFontSize, _finalFontSize, currentTransitionTime / totalTransitionTime);
                text.fontSize = (int)Mathf.Ceil(elementFontSize);
                if (currentTransitionTime >= totalTransitionTime)
                    elementFontSize = _finalFontSize;
            }

            #endregion Public Methods

            #region Private Fields

            [SerializeField] private int _finalFontSize = 14;

            #endregion Private Fields
        }

        #endregion Public Classes
    }
}