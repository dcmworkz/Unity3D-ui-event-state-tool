using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lairinus.UI.Events;

/// <summary>
/// The editor is very laggy, EVEN WITHOUT THIS!
/// </summary>
[CustomEditor(typeof(UIEventState))]
public class UIEventStateInspector : Editor
{
    private UIEventState _thisEventStateObject = null;

    public override void OnInspectorGUI()
    {
        // Possible Event States label
        GUILayout.Space(InspectorUtility.columnSize);
        EditorGUILayout.LabelField("All Event States:", EditorStyles.boldLabel);
        GUILayout.Space(5);

        // InfoBox
        GUILayout.BeginHorizontal();
        GUILayout.Space(InspectorUtility.columnSize);
        EditorGUILayout.HelpBox("Select one of the following Events in order to modify its' transitions. Each state will be captured, but nothing will happen unless that Event State has enabled transitions", MessageType.Info);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Display Event Type buttons / selection grid
        GUILayout.BeginHorizontal();
        GUILayout.Space(InspectorUtility.columnSize);
        _thisEventStateObject.selectedEventTypeInternal = (UIEventState.EventType)GUILayout.SelectionGrid((int)_thisEventStateObject.selectedEventTypeInternal, InspectorUtility.GetEventsGUIContentArray(), 5);
        GUILayout.EndHorizontal();

        UIEventState.EventStateSingle foundEventState = null;
        if (_thisEventStateObject.eventStatesCollection == null)
            return;

        if (_thisEventStateObject.eventStatesCollection.ContainsKey(_thisEventStateObject.selectedEventTypeInternal))
            foundEventState = _thisEventStateObject.eventStatesCollection[_thisEventStateObject.selectedEventTypeInternal];

        if (foundEventState != null)
        {
            GUI_ShowUnityEvents(foundEventState);

            // Event Transition Time
            GUILayout.BeginHorizontal();
            GUILayout.Space(InspectorUtility.columnSize);
            foundEventState.allowedTransitionTime = EditorGUILayout.FloatField("Total " + _thisEventStateObject.selectedEventTypeInternal.ToString() + " Event Duration", foundEventState.allowedTransitionTime);
            GUILayout.EndHorizontal();
            ShowTransitionConfiguration(foundEventState);
            ShowVisualizationButtons(foundEventState);
        }
        else
            return;
    }

    private void GUI_ShowBasePropertyConfiguration(UIEventState.EventTransitionBase transition)
    {
        if (transition != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(InspectorUtility.columnSize);
            transition.enableTransition = EditorGUILayout.Toggle("Enable " + _thisEventStateObject.selectedEventTransitionInternal.ToString() + " Transition", transition.enableTransition);
            GUILayout.EndHorizontal();
        }
    }

    private void GUI_ShowTransitionSelectionUI(UIEventState.EventStateSingle eventState)
    {
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(5);
        string[] eventTypeNames = Enum.GetNames(typeof(UIEventState.EventTransitionType));

        GUI.skin.button.margin = new RectOffset(0, 0, 0, 0);
        GUILayout.Space(25);
        EditorGUILayout.LabelField(_thisEventStateObject.selectedEventTypeInternal.ToString() + " Transitions:", EditorStyles.boldLabel);
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Space(InspectorUtility.columnSize);
        EditorGUILayout.HelpBox("The " + _thisEventStateObject.selectedEventTypeInternal.ToString() + " Event State contains all of the following transitions. Be sure the \"Enable Transition \" toggle is marked or else the Transition won't play!", MessageType.Info);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Selection Grid for Event Transitions
        GUILayout.BeginHorizontal();
        GUILayout.Space(InspectorUtility.columnSize);
        _thisEventStateObject.selectedEventTransitionInternal = (UIEventState.EventTransitionType)GUILayout.SelectionGrid((int)_thisEventStateObject.selectedEventTransitionInternal, InspectorUtility.GetTransitionGUIContentArray(), 4);
        GUILayout.EndHorizontal();

        if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
            GUI_ShowBasePropertyConfiguration(eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal]);
    }

    private void GUI_ShowUnityEvents(UIEventState.EventStateSingle eventState)
    {
        // Shows Event State
        GUILayout.BeginHorizontal();
        GUILayout.Space(InspectorUtility.columnSize);
        SerializedProperty sp = serializedObject.FindProperty(eventState.eventType.ToString() + "UnityEvent");
        EditorGUILayout.PropertyField(sp);
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();
    }

    private void OnEnable()
    {
        _thisEventStateObject = (UIEventState)target;
        _thisEventStateObject.Initialize();
    }
    private void ShowInvalidElementTypeForTransition(string elementType)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(InspectorUtility.columnSize);
        EditorGUILayout.HelpBox("This transition is only available for " + elementType + " objects.", MessageType.Warning);
        GUILayout.EndHorizontal();
    }

    private void ShowTransitionConfiguration(UIEventState.EventStateSingle eventState)
    {
        if (eventState == null)
            return;

        GUI_ShowTransitionSelectionUI(eventState);

        // Show Base Properties
        UIEventState.EventTransitionType transitionType = _thisEventStateObject.selectedEventTransitionInternal;

        switch (_thisEventStateObject.selectedEventTransitionInternal)
        {
            // ----- Show Rotation in Custom Inspector ---- //
            case UIEventState.EventTransitionType.Rotation:
                {
                    UIEventState.RotationTransition rotationTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                        rotationTransition = (UIEventState.RotationTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                    if (rotationTransition != null)
                    {
                        // Transition Rotation
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(InspectorUtility.columnSize);
                        rotationTransition.rotation = EditorGUILayout.Vector3Field("Rotation", rotationTransition.rotation);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Rotation Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.Scale:
                {
                    // ----- Show Scale in Custom Inspector ---- //
                    UIEventState.ScaleTransition scaleTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                        scaleTransition = (UIEventState.ScaleTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                    if (scaleTransition != null)
                    {
                        // Transition Scale
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(InspectorUtility.columnSize);
                        scaleTransition.scale = EditorGUILayout.Vector3Field("Scale", scaleTransition.scale);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Scale Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.Color:
                {
                    // ----- Show Image Color in Custom Inspector ---- //
                    UIEventState.ColorTransition imageColorTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                        imageColorTransition = (UIEventState.ColorTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                    if (imageColorTransition != null)
                    {
                        // The color we end on
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(InspectorUtility.columnSize);
                        imageColorTransition.color = EditorGUILayout.ColorField("Color", imageColorTransition.color);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Image Color Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.Material:
                {
                    // ----- Show Material transition in Custom Inspector ---- //
                    UIEventState.MaterialTransition materialTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                        materialTransition = (UIEventState.MaterialTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                    if (materialTransition != null)
                    {
                        // Transition Material
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(InspectorUtility.columnSize);
                        materialTransition.finalMaterial = (Material)EditorGUILayout.ObjectField("Material to use", materialTransition.finalMaterial, typeof(Material), true);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Material Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.SpriteAndFill:
                {
                    // ----- Show Image Sprite in Custom Inspector ---- //
                    if (_thisEventStateObject.imageElement != null)
                    {
                        UIEventState.SpriteAndFillTransition spriteAndFillTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                            spriteAndFillTransition = (UIEventState.SpriteAndFillTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                        if (spriteAndFillTransition != null)
                        {
                            // Only show other properties if there is a sprite, otherwise it's useless
                            if (spriteAndFillTransition.finalSprite != null)
                            {
                                // Use Image Fill
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(InspectorUtility.columnSize);
                                spriteAndFillTransition.useImageFill = EditorGUILayout.Toggle("Use Image Fill", spriteAndFillTransition.useImageFill);
                                GUILayout.EndHorizontal();

                                // Fill Type enum
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(InspectorUtility.columnSize);
                                spriteAndFillTransition.fillType = (UIEventState.SpriteAndFillTransition.FillType)EditorGUILayout.EnumPopup("Image Fill Type", spriteAndFillTransition.fillType);
                                GUILayout.EndHorizontal();

                                // Fill Method enum
                                GUILayout.BeginHorizontal();
                                GUILayout.Space(InspectorUtility.columnSize);
                                spriteAndFillTransition.fillMethod = (UnityEngine.UI.Image.FillMethod)EditorGUILayout.EnumPopup("Image Fill Method", spriteAndFillTransition.fillMethod);
                                GUILayout.EndHorizontal();
                            }

                            // Transition Sprite
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(InspectorUtility.columnSize);
                            spriteAndFillTransition.finalSprite = (Sprite)EditorGUILayout.ObjectField("Sprite to Fill", spriteAndFillTransition.finalSprite, typeof(Sprite), true);
                            GUILayout.EndHorizontal();
                        }
                        else
                            Debug.LogError("Error: The Sprite and Fill Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                    }
                    else
                    {
                        ShowInvalidElementTypeForTransition("Image");
                    }
                }
                break;

            case UIEventState.EventTransitionType.TextFontSize:
                {
                    // ----- Show Font Size in Custom Inspector ---- //
                    if (_thisEventStateObject.textElement != null)
                    {
                        UIEventState.TextFontSizeTransition fontSizeTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                            fontSizeTransition = (UIEventState.TextFontSizeTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                        if (fontSizeTransition != null)
                        {
                            // Transition Font Size
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(InspectorUtility.columnSize);
                            fontSizeTransition.finalFontSize = EditorGUILayout.IntField("Font Size", fontSizeTransition.finalFontSize);
                            GUILayout.EndHorizontal();
                        }
                        else
                            Debug.LogError("Error: The Font Size Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                    }
                    else
                    {
                        ShowInvalidElementTypeForTransition("Text");
                    }
                }
                break;

            case UIEventState.EventTransitionType.TextWriteout:
                {
                    // ----- Show Font Size in Custom Inspector ---- //
                    if (_thisEventStateObject.textElement != null)
                    {
                        UIEventState.TextWriteoutTransition fontSizeTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_thisEventStateObject.selectedEventTransitionInternal))
                            fontSizeTransition = (UIEventState.TextWriteoutTransition)eventState.eventTransitionCollection[_thisEventStateObject.selectedEventTransitionInternal];

                        if (fontSizeTransition != null)
                        {
                            // Transition Text
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(InspectorUtility.columnSize);
                            fontSizeTransition.finalString = EditorGUILayout.TextField("Display Text", fontSizeTransition.finalString);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(InspectorUtility.columnSize);
                            fontSizeTransition.writeoutOverDuration = EditorGUILayout.Toggle("Writeout Text", fontSizeTransition.writeoutOverDuration);
                            GUILayout.EndHorizontal();
                        }
                        else
                            Debug.LogError("Error: The Font Size Transition property attached to the " + _thisEventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                    }
                    else
                    {
                        ShowInvalidElementTypeForTransition("Text");
                    }
                }
                break;
        }

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
    }
    private void ShowVisualizationButtons(UIEventState.EventStateSingle eventState)
    {
        GUILayout.Space(InspectorUtility.columnSize);
        EditorGUILayout.LabelField("Visualize Event States:", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (GUILayout.Button("Set Current " + eventState.eventType.ToString() + " Event State", GUILayout.Height(25)))
        {
            _thisEventStateObject.VisualizeTransitionInternal(eventState);
            Debug.Log("Event State " + eventState.eventType.ToString() + " visualized");
        }

        if (_thisEventStateObject.previousEventState != null)
        {
            if (GUILayout.Button("Set Previous " + _thisEventStateObject.previousEventState.eventType.ToString() + " Event State", GUILayout.Height(25)))
            {
                Debug.Log("Event State " + _thisEventStateObject.previousEventState.ToString() + " visualized");
                _thisEventStateObject.VisualizeTransitionInternal(_thisEventStateObject.previousEventState);
            }
        }
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(10);
    }

    private static class InspectorUtility
    {
        public static int columnSize { get { return 50; } }
        public static string GetTransitionTooltip(UIEventState.EventTransitionType transitionType)
        {
            switch (transitionType)
            {
                case UIEventState.EventTransitionType.Color:
                    return "ALL GRAPHIC TYPES: \n\nChanges this Graphic's Color. For Text objects, it changes the Text color. For Image objects, it changes the Image background color";

                case UIEventState.EventTransitionType.Material:
                    return "ALL GRAPHIC TYPES: \n\nAdjusts the Graphic's Material by immediately assigning it a new material";

                case UIEventState.EventTransitionType.Rotation:
                    return "ALL GRAPHIC TYPES: \n\nAdjusts the Graphic's Rotation. For 2D UI, use the positive and negative Z-axis to rotate the object right and left, respectively";

                case UIEventState.EventTransitionType.Scale:
                    return "ALL GRAPHIC TYPES: \n\nAdjusts the Graphic's Scale. in 2D UI, the Z-axis will have no influence on the object's scale";

                case UIEventState.EventTransitionType.SpriteAndFill:
                    return "IMAGE GRAPHICS ONLY! \n\nIf a sprite is chosen, you will have the option to make this transition fill in the sprite over the Event's duration";

                case UIEventState.EventTransitionType.TextFontSize:
                    return "TEXT GRAPHICS ONLY! \n\nAdjusts the font size of a text object. This has much more predictable results than scaling a Text object";

                case UIEventState.EventTransitionType.TextWriteout:
                    return "TEXT GRAPHICS ONLY! \n\nAdjusts a Text graphic's text all at once, or over the Event's duration";


                default: return "";
            }
        }

        public static string GetEventTooltip(UIEventState.EventType eventType)
        {
            switch (eventType)
            {
                case UIEventState.EventType.OnBeginDrag:
                    return "Called at the start of a Drag event. This is when you are hovering the UI element and moving the mouse while either the Left or Right mouse button is pressed down. \n\nMobile support will vary";
                case UIEventState.EventType.OnDrag:
                    return "Called at the start of a Drag event. This is when you are hovering the UI element and moving the mouse while either the Left or Right mouse button is pressed down. \n\nMobile support will vary";
                case UIEventState.EventType.OnEndDrag:
                    return "Called at the start of a Drag event. This is when you are hovering the UI element and moving the mouse while either the Left or Right mouse button is pressed down. \n\nMobile support will vary";
                case UIEventState.EventType.OnHover:
                    return "Called automatically while:\n1. The pointer is over the element\n2. The element is not in the middle of any other Event state";
                case UIEventState.EventType.OnNormal:
                    return "Called automatically on load at runtime. Also called while:\n1. The pointer is over the element\n2. The element is not in the middle of any other Event state";
                case UIEventState.EventType.OnPointerClick:
                    return "Called OnPointerUp at the end of a click.";
                case UIEventState.EventType.OnPointerDown:
                    return "Called once the pointer is pressed down";
                case UIEventState.EventType.OnPointerUp:
                    return "Almost identical to OnPointerClick, however this is less mobile-friendly";
                case UIEventState.EventType.OnPointerEnter:
                    return "Called when the mouse enters the Graphic element's region. \n\nMobile support will vary.";
                case UIEventState.EventType.OnPointerExit:
                    return "Called when the mouse leaves the Graphic element's region. \n\nMobile support will vary.";

                default:
                    return "";
            }
        }

        public static GUIContent [] GetTransitionGUIContentArray()
        {
            List<GUIContent> content = new List<GUIContent>();
            string [] names = Enum.GetNames(typeof(UIEventState.EventTransitionType));

            for (var a = 0; a < names.Length; a++)
            {
                try
                {
                    string transitionTooltip = GetTransitionTooltip((UIEventState.EventTransitionType)a);
                    content.Add(new GUIContent(names[a], transitionTooltip));
                }
                catch
                {
                    // do nothing
                }
            }

            return content.ToArray();
        }

        public static GUIContent[] GetEventsGUIContentArray()
        {
            List<GUIContent> content = new List<GUIContent>();
            string[] names = Enum.GetNames(typeof(UIEventState.EventType));

            for (var a = 0; a < names.Length; a++)
            {
                try
                {
                    string eventTooltip = GetEventTooltip((UIEventState.EventType)a);
                    content.Add(new GUIContent(names[a], eventTooltip));
                }
                catch
                {
                    // do nothing
                }
            }

            return content.ToArray();
        }

    }
}