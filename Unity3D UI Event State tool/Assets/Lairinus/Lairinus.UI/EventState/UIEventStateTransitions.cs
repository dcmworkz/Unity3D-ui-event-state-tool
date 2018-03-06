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
        public class BaseTransition
        {
            #region Remarks

            /*
             *    For internal use only
             *    ---------------------
             *    Contains some shared customization for successfully running transitions on Graphic objects
             */

            #endregion Remarks

            public BaseTransition(EventTransitionType _transitionType)
            {
                transitionType = _transitionType;
            }

            public EventTransitionType transitionType { get; protected set; }

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

                if (_enableTransition)
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
        public class EventState
        {
            #region Remarks

            /*   External and internal use
             *   -------------------------
             *   Contains the Graphic object transitions shared between the Graphic objects
             */

            #endregion Remarks

            public EventState(EventType thisEventType)
            {
                _eventType = thisEventType;
            }

            #region Public Properties

            public float allowedTransitionTime { get { return _allowedTransitionTime; } set { _allowedTransitionTime = value; } }
            public UnityEvent onStateEnterEvent { get { return _onStateEnterEvent; } }
            public RotationTransition transitionRotation { get { return _transitionRotation; } }
            public ScaleTransition transitionScale { get { return _transitionScale; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] protected float _allowedTransitionTime = 0.25F;
            [SerializeField] protected UnityEvent _onStateEnterEvent = new UnityEvent();
            [SerializeField] protected RotationTransition _transitionRotation = new RotationTransition(EventTransitionType.Rotation);
            [SerializeField] protected ScaleTransition _transitionScale = new ScaleTransition(EventTransitionType.Scale);
            private EventType _eventType = EventType.OnBeginDrag;
            public EventType eventType { get { return _eventType; } }

            #endregion Private Fields

            public Dictionary<EventTransitionType, BaseTransition> eventTransitionCollection { get; protected set; }
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

                if (_enableTransition)
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

            public ImageColorTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

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
            public ImageSpriteTransition imageSpriteAndFill { get { return _imageSpriteAndFill; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] private ImageColorTransition _imageColorTransition = new ImageColorTransition(EventTransitionType.ImageColor);
            [SerializeField] private ImageSpriteTransition _imageSpriteAndFill = new ImageSpriteTransition(EventTransitionType.ImageSpriteAndFill);

            public ImageEventState(EventType thisEventType) : base(thisEventType)
            {
                eventTransitionCollection = new Dictionary<EventTransitionType, BaseTransition>();
                eventTransitionCollection.Add(EventTransitionType.Rotation, _transitionRotation);
                eventTransitionCollection.Add(EventTransitionType.Scale, _transitionScale);
                eventTransitionCollection.Add(EventTransitionType.ImageColor, _imageColorTransition);
                eventTransitionCollection.Add(EventTransitionType.ImageSpriteAndFill, _imageSpriteAndFill);
            }

            #endregion Private Fields
        }

        [System.Serializable]
        public class ImageSpriteTransition : BaseTransition
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

                if (_enableTransition)
                    return;

                base.RunTransition_Internal(eventStateElement, currentTransitionTime, totalTransitionTime);
                if (eventStateElement.graphicElement == null)
                    return;

                Image imageElement = (Image)eventStateElement.graphicElement;
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

            [SerializeField] private Image.FillMethod _fillMethod = Image.FillMethod.Vertical;
            [SerializeField] private FillType _fillType = FillType.IncrementFill;
            [SerializeField] private Sprite _finalSprite = null;
            [SerializeField] private bool _useImageFill = false;

            public ImageSpriteTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

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

                if (_enableTransition)
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

            public RotationTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

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

            [SerializeField] private Vector2 _finalScale = new Vector3(1, 1, 1);

            public ScaleTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

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

                if (_enableTransition)
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

            public TextColorTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

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

            public TextColorTransition transitionTextColor { get { return _transitionTextColor; } }
            public TextFontSizeTransition transitionFontSize { get { return _transitionFontSize; } }

            #endregion Public Properties

            #region Private Fields

            [SerializeField] private TextColorTransition _transitionTextColor = new TextColorTransition(EventTransitionType.TextColor);
            [SerializeField] private TextFontSizeTransition _transitionFontSize = new TextFontSizeTransition(EventTransitionType.TextFontSize);

            public TextEventState(EventType thisEventType) : base(thisEventType)
            {
                eventTransitionCollection = new Dictionary<EventTransitionType, BaseTransition>();
                eventTransitionCollection.Add(EventTransitionType.Rotation, _transitionRotation);
                eventTransitionCollection.Add(EventTransitionType.Scale, _transitionScale);
                eventTransitionCollection.Add(EventTransitionType.ImageColor, _transitionTextColor);
                eventTransitionCollection.Add(EventTransitionType.TextFontSize, _transitionFontSize);
            }

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

                if (_enableTransition)
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

            public TextFontSizeTransition(EventTransitionType _transitionType) : base(_transitionType)
            {
            }

            #endregion Private Fields
        }

        #endregion Public Classes
    }
}