using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace Lairinus.UI.Events
{
    public partial class UIGraphicEventState : MonoBehaviour
    {
        /*
         *    Internal and External use
         *    ---------------------
         *    Partial class created because the functionality would look cramped in the other file,
         *    and all of this functionality technically doesn't need to stand on its' own
         */

        [System.Serializable]
        public class ColorTransition : EventTransitionBase
        {
            /*   External and internal use
             *   -------------------------
             *   Transition information to modify an Image's color
             */

            public ColorTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public Color color { get { return _finalColor; } set { _finalColor = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to scale the Graphic object to its' desired target.
                 */

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

            [SerializeField] private Color _finalColor = Color.white;
        }

        [System.Serializable]
        public class EventTransitionBase
        {
            /*
             *    For internal use only
             *    ---------------------
             *    Contains some shared customization for successfully running transitions on Graphic objects
             */

            public EventTransitionBase(EventTransitionType _transitionType)
            {
                transitionType = _transitionType;
            }

            public bool enableTransition { get { return _enableTransition; } set { _enableTransition = value; } }

            public EventTransitionType transitionType { get; protected set; }

            public void RunTransition(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Runs the transition on the Graphic object
                 */

                if (!_enableTransition)
                    return;

                if (eventStateElement == null)
                    return;

                Image image = eventStateElement.imageElement;
                Text text = eventStateElement.textElement;
                if (image == null && text == null)
                    return;

                RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime, image, text);
            }

            // If true, this will ignore the transition time for the elements and snap directly to a certain scale.
            [SerializeField] protected bool _enableTransition = false;

            protected virtual void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                throw new System.NotImplementedException();
            }
        }

        [System.Serializable]
        public class MaterialTransition : EventTransitionBase
        {
            /*   External and internal use
             *   -------------------------
             *   Transition information to modify an Image's color
             */

            public MaterialTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public Material finalMaterial { get { return _finalMaterial; } set { _finalMaterial = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to scale the Graphic object to its' desired target.
                 */

                if (image != null)
                    image.material = _finalMaterial;
                else if (text != null)
                    text.material = _finalMaterial;
            }

            [SerializeField] private Material _finalMaterial = null;
        }

        [System.Serializable]
        public class RotationTransition : EventTransitionBase
        {
            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a Graphic object's Euler Angle rotation
             */

            public RotationTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public Vector3 rotation { get { return _rotation; } set { _rotation = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to rotate the Graphic object to its' desired target.
                 */

                eventStateElement.cachedRectTransform.eulerAngles = Vector3.Lerp(eventStateElement.cachedRectTransform.eulerAngles, _rotation, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    eventStateElement.cachedRectTransform.eulerAngles = _rotation;
            }

            [SerializeField] private Vector3 _rotation = new Vector3(0, 0, 0);
        }

        [System.Serializable]
        public class ScaleTransition : EventTransitionBase
        {
            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a Graphic object's scale
             */

            public ScaleTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public Vector3 scale { get { return _scale; } set { _scale = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to scale the target to its' desired target.
                 */

                eventStateElement.cachedRectTransform.localScale = Vector3.Lerp(eventStateElement.cachedRectTransform.localScale, _scale, currentTransitionTime / totalTransitionTime);
                if (currentTransitionTime >= totalTransitionTime)
                    eventStateElement.cachedRectTransform.localScale = _scale;
            }

            [SerializeField] private Vector3 _scale = new Vector3(1, 1, 1);
        }

        [System.Serializable]
        public class SpriteAndFillTransition : EventTransitionBase
        {
            /*   External and internal use
             *   -------------------------
             *   Transition information to modify a Graphic object's Euler Angle rotation
             */

            public SpriteAndFillTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public enum FillType
            {
                /*
                 * External and internal use
                 * ----------------------------
                 * Determines which direction the Sprite fills at.
                 * Incrementing fill starts at 0 and ends on 1.
                 * Decrementing fill starts on 1 and ends at 0
                 */

                IncrementFill,
                DecrementFill
            }

            public Image.FillMethod fillMethod { get { return _fillMethod; } set { _fillMethod = value; } }
            public FillType fillType { get { return _fillType; } set { _fillType = value; } }
            public Sprite finalSprite { get { return _finalSprite; } set { _finalSprite = value; } }
            public bool useImageFill { get { return _useImageFill; } set { _useImageFill = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Attempts to set the Image's sprite to the destination sprite
                 */

                if (image == null)
                    return;

                image.sprite = _finalSprite;
                if (_useImageFill)
                {
                    // We need to manually set the sprite to "filled" after we change from a potentially null sprite
                    if (image.sprite != null)
                    {
                        image.type = Image.Type.Filled;
                        image.fillMethod = _fillMethod;

                        if (_fillType == FillType.IncrementFill)
                            image.fillAmount = currentTransitionTime / totalTransitionTime;
                        else
                            image.fillAmount = 1 - (currentTransitionTime / totalTransitionTime);
                    }
                }

                if (currentTransitionTime >= totalTransitionTime)
                {
                    if (_fillType == FillType.IncrementFill)
                        image.fillAmount = 1;
                    else if (_fillType == FillType.DecrementFill)
                        image.fillAmount = 0;
                }
            }

            [SerializeField] private Image.FillMethod _fillMethod = Image.FillMethod.Vertical;
            [SerializeField] private FillType _fillType = FillType.IncrementFill;
            [SerializeField] private Sprite _finalSprite = null;
            [SerializeField] private bool _useImageFill = false;
        }

        [System.Serializable]
        public class TextFontSizeTransition : EventTransitionBase
        {
            public TextFontSizeTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public int finalFontSize { get { return _finalFontSize; } set { _finalFontSize = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Lerps the color of the Text object
                 */

                float elementFontSize = text.fontSize;
                elementFontSize = Mathf.Lerp(elementFontSize, _finalFontSize, currentTransitionTime / totalTransitionTime);
                text.fontSize = (int)Mathf.Ceil(elementFontSize);
                if (currentTransitionTime >= totalTransitionTime)
                    elementFontSize = _finalFontSize;
            }

            [SerializeField] private int _finalFontSize = 14;
        }

        [System.Serializable]
        public class TextWriteoutTransition : EventTransitionBase
        {
            public TextWriteoutTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            public string finalString { get { return _finalString; } set { _finalString = value; } }
            public bool writeoutOverDuration { get { return _writeoutOverDuration; } set { _writeoutOverDuration = value; } }

            protected override void RunTransition_Internal(UIGraphicEventState eventStateElement, float currentTransitionTime, float totalTransitionTime, Image image, Text text)
            {
                /*
                 *    For internal use only
                 *    ---------------------
                 *    Changes the actual Text of a text element
                 */

                if (text == null)
                    return;

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

            [SerializeField] private string _finalString = "";
            [SerializeField] private bool _writeoutOverDuration = false;
        }
    }
}