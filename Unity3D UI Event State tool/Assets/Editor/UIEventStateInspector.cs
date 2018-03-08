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
    private UIEventState _eventStateObject = null;

    private void OnEnable()
    {
        _eventStateObject = (UIEventState)target;
        _eventStateObject.Initialize();
    }

    public override void OnInspectorGUI()
    {
        // Possible Event States label
        EditorGUILayout.LabelField("Possible Event States:", EditorStyles.boldLabel);
        GUILayout.Space(5);

        // InfoBox
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.HelpBox("Select one of the following Events in order to modify its' transitions. Each state will be captured, but nothing will happen unless that Event State has enabled transitions", MessageType.Info);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        // Display Event Type buttons / selection grid
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        string[] eventTypeNames = Enum.GetNames(typeof(UIEventState.EventType));
        _eventStateObject.selectedEventTypeInternal = (UIEventState.EventType)GUILayout.SelectionGrid((int)_eventStateObject.selectedEventTypeInternal, eventTypeNames, 5);
        GUILayout.EndHorizontal();

        UIEventState.EventStateSingle foundEventState = null;
        if (_eventStateObject.eventStatesCollection == null)
            return;

        if (_eventStateObject.eventStatesCollection.ContainsKey(_eventStateObject.selectedEventTypeInternal))
            foundEventState = _eventStateObject.eventStatesCollection[_eventStateObject.selectedEventTypeInternal];

        if (foundEventState != null)
        {
            GUI_ShowUnityEvents(foundEventState);

            // Event Transition Time
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            foundEventState.allowedTransitionTime = EditorGUILayout.FloatField("Total " + _eventStateObject.selectedEventTypeInternal.ToString() + " event duration", foundEventState.allowedTransitionTime);
            GUILayout.EndHorizontal();
            ShowTransitionConfiguration(foundEventState);
            ShowVisualizationButtons(foundEventState);
        }
        else
            return;
    }

    private void ShowTransitionConfiguration(UIEventState.EventStateSingle eventState)
    {
        if (eventState == null)
            return;

        GUI_ShowTransitionSelectionUI(eventState);

        // Show Base Properties
        UIEventState.EventTransitionType transitionType = _eventStateObject.selectedEventTransitionInternal;

        switch (_eventStateObject.selectedEventTransitionInternal)
        {
            // ----- Show Rotation in Custom Inspector ---- //
            case UIEventState.EventTransitionType.Rotation:
                {
                    UIEventState.RotationTransition rotationTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
                        rotationTransition = (UIEventState.RotationTransition)eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal];

                    if (rotationTransition != null)
                    {
                        // Transition Rotation
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(30);
                        rotationTransition.finalRotation = EditorGUILayout.Vector3Field("Rotation", rotationTransition.finalRotation);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Rotation Transition property attached to the " + _eventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.Scale:
                {
                    // ----- Show Scale in Custom Inspector ---- //
                    UIEventState.ScaleTransition scaleTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
                        scaleTransition = (UIEventState.ScaleTransition)eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal];

                    if (scaleTransition != null)
                    {
                        // Transition Scale
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(30);
                        scaleTransition.finalScale = EditorGUILayout.Vector3Field("Scale", scaleTransition.finalScale);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Scale Transition property attached to the " + _eventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.Color:
                {
                    // ----- Show Image Color in Custom Inspector ---- //
                    UIEventState.ColorTransition imageColorTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
                        imageColorTransition = (UIEventState.ColorTransition)eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal];

                    if (imageColorTransition != null)
                    {
                        // The color we end on
                        GUILayout.BeginHorizontal();
                        GUILayout.Space(30);
                        imageColorTransition.color = EditorGUILayout.ColorField("Color", imageColorTransition.color);
                        GUILayout.EndHorizontal();
                    }
                    else
                        Debug.LogError("Error: The Image Color Transition property attached to the " + _eventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                }
                break;

            case UIEventState.EventTransitionType.SpriteAndFill:
                {
                    // ----- Show Image Sprite in Custom Inspector ---- //
                    if (_eventStateObject.imageElement != null)
                    {
                        UIEventState.SpriteAndFillTransition spriteAndFillTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
                            spriteAndFillTransition = (UIEventState.SpriteAndFillTransition)eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal];

                        if (spriteAndFillTransition != null)
                        {
                            // Use Image Fill
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            spriteAndFillTransition.useImageFill = EditorGUILayout.Toggle("Use Image Fill", spriteAndFillTransition.useImageFill);
                            GUILayout.EndHorizontal();

                            // Fill Type enum
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            spriteAndFillTransition.fillType = (UIEventState.SpriteAndFillTransition.FillType)EditorGUILayout.EnumPopup("Image Fill Type", spriteAndFillTransition.fillType);
                            GUILayout.EndHorizontal();

                            // Fill Method enum
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            spriteAndFillTransition.fillMethod = (UnityEngine.UI.Image.FillMethod)EditorGUILayout.EnumPopup("Image Fill Method", spriteAndFillTransition.fillMethod);
                            GUILayout.EndHorizontal();

                            // Transition Sprite
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            spriteAndFillTransition.finalSprite = (Sprite)EditorGUILayout.ObjectField("Sprite to show", spriteAndFillTransition.finalSprite, typeof(Sprite), true);
                            GUILayout.EndHorizontal();
                        }
                        else
                            Debug.LogError("Error: The Sprite and Fill Transition property attached to the " + _eventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
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
                    if (_eventStateObject.textElement != null)
                    {
                        UIEventState.TextFontSizeTransition fontSizeTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
                            fontSizeTransition = (UIEventState.TextFontSizeTransition)eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal];

                        if (fontSizeTransition != null)
                        {
                            // Transition Font Size
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            fontSizeTransition.finalFontSize = EditorGUILayout.IntField("Font Size", fontSizeTransition.finalFontSize);
                            GUILayout.EndHorizontal();
                        }
                        else
                            Debug.LogError("Error: The Font Size Transition property attached to the " + _eventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
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
                    if (_eventStateObject.textElement != null)
                    {
                        UIEventState.TextWriteoutTransition fontSizeTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
                            fontSizeTransition = (UIEventState.TextWriteoutTransition)eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal];

                        if (fontSizeTransition != null)
                        {
                            // Transition Text
                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            fontSizeTransition.finalString = EditorGUILayout.TextField("Display Text", fontSizeTransition.finalString);
                            GUILayout.EndHorizontal();

                            GUILayout.BeginHorizontal();
                            GUILayout.Space(30);
                            fontSizeTransition.writeoutOverDuration = EditorGUILayout.Toggle("Writeout Text", fontSizeTransition.writeoutOverDuration);
                            GUILayout.EndHorizontal();
                        }
                        else
                            Debug.LogError("Error: The Font Size Transition property attached to the " + _eventStateObject.name + " GameObject is NULL! Ensure you aren't setting this null!");
                    }
                    else
                    {
                        ShowInvalidElementTypeForTransition("Text");
                    }
                }
                break;
        }
    }

    private void ShowInvalidElementTypeForTransition(string elementType)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.HelpBox("This transition is only available for " + elementType + " objects.", MessageType.Warning);
        GUILayout.EndHorizontal();
    }

    private void GUI_ShowUnityEvents(UIEventState.EventStateSingle eventState)
    {
        // Shows Event State
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        SerializedProperty sp = serializedObject.FindProperty(eventState.eventType.ToString() + "UnityEvent");
        EditorGUILayout.PropertyField(sp);
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();
    }

    private void GUI_ShowBasePropertyConfiguration(UIEventState.EventTransition transition)
    {
        if (transition != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            transition.enableTransition = EditorGUILayout.Toggle("Enable " + _eventStateObject.selectedEventTransitionInternal.ToString() + " Transition", transition.enableTransition);
            GUILayout.EndHorizontal();
        }
    }

    private void GUI_ShowTransitionSelectionUI(UIEventState.EventStateSingle eventState)
    {
        GUILayout.Space(5);
        string[] eventTypeNames = Enum.GetNames(typeof(UIEventState.EventTransitionType));

        GUI.skin.button.margin = new RectOffset(0, 0, 0, 0);
        GUILayout.Space(25);
        EditorGUILayout.LabelField(_eventStateObject.selectedEventTypeInternal.ToString() + " Transitions:", EditorStyles.boldLabel);
        GUILayout.Space(5);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.HelpBox("The " + _eventStateObject.selectedEventTypeInternal.ToString() + " Event State contains all of the following transitions. Be sure the \"Enable Transition \" toggle is marked or else the Transition won't play!", MessageType.Info);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        _eventStateObject.selectedEventTransitionInternal = (UIEventState.EventTransitionType)GUILayout.SelectionGrid((int)_eventStateObject.selectedEventTransitionInternal, eventTypeNames, 4);
        GUILayout.EndHorizontal();

        if (eventState.eventTransitionCollection.ContainsKey(_eventStateObject.selectedEventTransitionInternal))
            GUI_ShowBasePropertyConfiguration(eventState.eventTransitionCollection[_eventStateObject.selectedEventTransitionInternal]);
    }

    private void ShowVisualizationButtons(UIEventState.EventStateSingle eventState)
    {
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Visualize Event States:", EditorStyles.boldLabel);
        GUILayout.Space(10);
        if (GUILayout.Button("Visualize current " + eventState.eventType.ToString() + " Event State", GUILayout.Height(25)))
        {
            Debug.Log("Ok");
            _eventStateObject.VisualizeTransitionInternal(eventState);
        }

        if (_eventStateObject.previousEventState != null)
        {
            if (GUILayout.Button("Set Previous " + _eventStateObject.previousEventState.eventType.ToString() + " Event State", GUILayout.Height(25)))
            {
                Debug.Log("Ok");
                _eventStateObject.VisualizeTransitionInternal(_eventStateObject.previousEventState);
            }
        }
        GUILayout.Space(10);
    }
}