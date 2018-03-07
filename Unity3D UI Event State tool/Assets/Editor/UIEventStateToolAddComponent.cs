using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Lairinus.UI.Events
{
    public class UIEventStateToolAddComponent : MonoBehaviour
    {
        [MenuItem("Lairinus/UI/Components/Add UI Event State")]
        private static void AddUIEventStateComponent()
        {
            #region Remarks

            /*
             * Applies the "UIEventState" component to specific type of Graphic elements that are selected.
             */

            #endregion Remarks

            foreach (GameObject go in Selection.gameObjects)
            {
                Graphic existingGraphicComponent = go.GetComponent<Graphic>();
                if (existingGraphicComponent == null)
                {
                    Debug.LogError("Error: You cannot add the UIEventState component to the GameObject " + go.name + " because this component does not have the Image or Text component attached!");
                    continue;
                }

                go.AddComponent(typeof(UIEventState));
            }
        }
    }
}