using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public class ColorTransition : EventTransition
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Transition information to modify an Image's color
             */

            #endregion Remarks

            #region Public Properties

            public Color color { get { return _finalColor; } set { _finalColor = value; } }

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

                if (!_enableTransition)
                    return;

                Image image = eventStateElement.imageElement;
                Text text = eventStateElement.textElement;
                if (image == null && text == null)
                    return;

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                if (image != null)
                {
                    Color elementColor = image.color;
                    elementColor = Color.Lerp(elementColor, _finalColor, currentTransitionTime / totalTransitionTime);
                    if (currentTransitionTime >= totalTransitionTime)
                        elementColor = _finalColor;

                    image.color = elementColor;
                }
                else if (text != null)
                {
                    Color elementColor = text.color;
                    elementColor = Color.Lerp(elementColor, _finalColor, currentTransitionTime / totalTransitionTime);
                    if (currentTransitionTime >= totalTransitionTime)
                        elementColor = _finalColor;

                    text.color = elementColor;
                }
            }

            #endregion Public Methods

            #region Private Fields

            public ColorTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            [SerializeField] private Color _finalColor = Color.white;

            #endregion Private Fields
        }

        [System.Serializable]
        public class EventTransition
        {
            #region Remarks

            /*
             *    For internal use only
             *    ---------------------
             *    Contains some shared customization for successfully running transitions on Graphic objects
             */

            #endregion Remarks

            #region Public Constructors

            public EventTransition(EventTransitionType _transitionType)
            {
                transitionType = _transitionType;
            }

            #endregion Public Constructors

            #region Public Properties

            public EventTransitionType transitionType { get; protected set; }

            #endregion Public Properties



            #region Public Properties

            public bool enableTransition { get { return _enableTransition; } set { _enableTransition = value; } }

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

                if (!_enableTransition)
                    return;

                if (eventStateElement == null)
                    return;
            }

            #endregion Public Methods

            #region Protected Fields

            // If true, this will ignore the transition time for the elements and snap directly to a certain scale.
            [SerializeField] protected bool _enableTransition = false;

            #endregion Protected Fields
        }

        [System.Serializable]
        public class RotationTransition : EventTransition
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

                if (!_enableTransition)
                    return;

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                eventStateElement.cachedRectTransform.eulerAngles = Vector3.Lerp(eventStateElement.cachedRectTransform.eulerAngles, _finalRotation, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    eventStateElement.cachedRectTransform.eulerAngles = _finalRotation;
            }

            #endregion Public Methods

            #region Private Fields

            public RotationTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            [SerializeField] private Vector3 _finalRotation = new Vector3(0, 0, 0);

            #endregion Private Fields
        }

        [System.Serializable]
        public class ScaleTransition : EventTransition
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

                if (!_enableTransition)
                    return;

                if (eventStateElement != null)
                    base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);

                eventStateElement.cachedRectTransform.localScale = Vector3.Lerp(eventStateElement.cachedRectTransform.localScale, _finalScale, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    eventStateElement.cachedRectTransform.localScale = _finalScale;
            }

            #endregion Public Methods

            #region Private Fields

            public ScaleTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            [SerializeField] private Vector2 _finalScale = new Vector3(1, 1, 1);

            #endregion Private Fields
        }

        [System.Serializable]
        public class SpriteAndFillTransition : EventTransition
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a Graphic object's Euler Angle rotation
             */

            #endregion Remarks

            #region Public Properties

            public enum FillType
            {
                #region Remarks

                /*
                 * External and internal use
                 * ----------------------------
                 * Determines which direction the Sprite fills at.
                 * Incrementing fill starts at 0 and ends on 1.
                 * Decrementing fill starts on 1 and ends at 0
                 */

                #endregion Remarks

                IncrementFill,
                DecrementFill
            }

            public Image.FillMethod fillMethod { get { return _fillMethod; } set { _fillMethod = value; } }
            public FillType fillType { get { return _fillType; } set { _fillType = value; } }
            public Sprite finalSprite { get { return _finalSprite; } set { _finalSprite = value; } }
            public bool useImageFill { get { return _useImageFill; } set { _useImageFill = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to set the Image's sprite to the destination sprite
                 */

                #endregion Remarks

                if (!_enableTransition)
                    return;

                base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);
                if (eventStateElement.imageElement == null)
                    return;

                Image imageElement = eventStateElement.imageElement;
                if (imageElement == null)
                    return;

                imageElement.sprite = _finalSprite;
                if (_useImageFill)
                {
                    // We need to manually set the sprite to "filled" after we change from a potentially null sprite
                    if (imageElement.sprite != null)
                    {
                        imageElement.type = Image.Type.Filled;
                        imageElement.fillMethod = _fillMethod;

                        if (_fillType == FillType.IncrementFill)
                            imageElement.fillAmount = currentTransitionTime / totalTransitionTime;
                        else
                            imageElement.fillAmount = 1 - (currentTransitionTime / totalTransitionTime);
                    }
                }

                if (currentTransitionTime >= totalTransitionTime)
                {
                    if (_fillType == FillType.IncrementFill)
                        imageElement.fillAmount = 1;
                    else if (_fillType == FillType.DecrementFill)
                        imageElement.fillAmount = 0;
                }
            }

            #endregion Public Methods

            #region Private Fields

            public SpriteAndFillTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            [SerializeField] private Image.FillMethod _fillMethod = Image.FillMethod.Vertical;
            [SerializeField] private FillType _fillType = FillType.IncrementFill;
            [SerializeField] private Sprite _finalSprite = null;
            [SerializeField] private bool _useImageFill = false;

            #endregion Private Fields
        }

        [System.Serializable]
        public class TextFontSizeTransition : EventTransition
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

                if (!_enableTransition)
                    return;

                if (eventStateElement == null)
                    return;

                Text text = eventStateElement.textElement;
                if (text == null)
                    return;

                base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);
                float elementFontSize = text.fontSize;
                elementFontSize = Mathf.Lerp(elementFontSize, _finalFontSize, currentTransitionTime / totalTransitionTime);
                text.fontSize = (int)Mathf.Ceil(elementFontSize);
                if (currentTransitionTime >= totalTransitionTime)
                    elementFontSize = _finalFontSize;
            }

            #endregion Public Methods

            #region Private Fields

            public TextFontSizeTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            [SerializeField] private int _finalFontSize = 14;

            #endregion Private Fields
        }

        [System.Serializable]
        public class TextWriteoutTransition : EventTransition
        {
            #region Public Properties

            public string finalString { get { return _finalString; } set { _finalString = value; } }
            public bool writeoutOverDuration { get { return _writeoutOverDuration; } set { _writeoutOverDuration = value; } }

            #endregion Public Properties

            #region Public Methods

            public override void RunTransition_Internal(UIEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                #region Remarks

                /*
                 *    For internal use only
                 *    ---------------------
                 *    Changes the actual Text of a text element
                 */

                #endregion Remarks

                if (!_enableTransition)
                    return;

                if (eventStateElement == null)
                    return;

                Text text = eventStateElement.textElement;
                if (text == null)
                    return;

                base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);
                string endText = text.text;
                if (!_writeoutOverDuration || currentTransitionTime >= totalTransitionTime)
                    endText = _finalString;
                else
                {
                    int stringLength = _finalString.Length;
                    float roughStringPosition = stringLength * (currentTransitionTime / totalTransitionTime);
                    int finalStringLength = Mathf.CeilToInt(roughStringPosition);
                    if (finalStringLength > stringLength)
                        finalStringLength = stringLength;

                    if (finalStringLength < 1)
                        finalStringLength = 1;

                    endText = _finalString.Substring(0, finalStringLength - 1);
                }

                text.text = endText;
            }

            #endregion Public Methods

            #region Private Fields

            public TextWriteoutTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            [SerializeField] private string _finalString = "";
            [SerializeField] private bool _writeoutOverDuration = false;

            #endregion Private Fields
        }

        #endregion Public Classes
    }
}