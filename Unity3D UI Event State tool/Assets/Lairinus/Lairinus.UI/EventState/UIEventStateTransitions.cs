using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

            // If true, this will ignore the transition time for the elements and snap directly to a certain scale.
            [SerializeField] protected bool _ignoreTransition = false;

            public bool ignoreTransiton { get { return _ignoreTransition; } set { _ignoreTransition = value; } }

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
        }

        [System.Serializable]
        public class ScaleTransition : BaseTransition
        {
            [SerializeField] private Vector2 _finalScale = new Vector3(1, 1, 1);
            public Vector2 finalScale { get { return _finalScale; } set { _finalScale = value; } }

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
        }
    }
}