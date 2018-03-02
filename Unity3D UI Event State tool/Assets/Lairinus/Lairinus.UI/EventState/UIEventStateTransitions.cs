using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Lairinus.UI
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
             *    Contains some key values for successfully running transitions
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
                 *    Runs the transition on the element.
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
        public class ImageColorTransition : BaseTransition
        {
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
                 *    Attempts to scale the target to its' desired target.
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
        public class RotationTransition : BaseTransition
        {
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
                 *    Attempts to scale the target to its' desired target.
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

        #endregion Public Classes
    }
}