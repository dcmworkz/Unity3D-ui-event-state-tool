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
public class UIImageEventStateEditor : Editor
{
    private UIEventState _imageEventState = null;

    private void OnEnable()
    {
        _imageEventState = (UIEventState)target;
        _imageEventState.Initialize();
    }

    public override void OnInspectorGUI()
    {
        GUI_CreateElementHeaderUI();

        EditorGUILayout.LabelField("Possible Event States:", EditorStyles.boldLabel);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.HelpBox("Select one of the following Events in order to modify its' transitions. Each state will be captured, but nothing will happen unless that Event State has enabled transitions", MessageType.Info);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        string[] eventTypeNames = Enum.GetNames(typeof(UIEventState.EventType));

        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        _imageEventState.selectedEventTypeInternal = (UIEventState.EventType)GUILayout.SelectionGrid((int)_imageEventState.selectedEventTypeInternal, eventTypeNames, 5);
        GUILayout.EndHorizontal();

        UIEventState.EventState foundEventState = null;
        if (_imageEventState.eventStatesCollection == null)
            return;

        if (_imageEventState.eventStatesCollection.ContainsKey(_imageEventState.selectedEventTypeInternal))
            foundEventState = _imageEventState.eventStatesCollection[_imageEventState.selectedEventTypeInternal];

        if (foundEventState != null)
        {
            GUI_ShowUnityEvents(foundEventState);
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            foundEventState.allowedTransitionTime = EditorGUILayout.FloatField("Total " + _imageEventState.selectedEventTypeInternal.ToString() + " event duration", foundEventState.allowedTransitionTime);
            GUILayout.EndHorizontal();
            ShowTransitionConfiguration(foundEventState);
            ShowActionableUI(foundEventState);
        }
        else
            return;
    }

    private void ShowTransitionConfiguration(UIEventState.EventState eventState)
    {
        if (eventState == null)
            return;

        GUI_ShowTransitionSelectionUI(eventState);

        // Show Base Properties
        UIEventState.EventTransitionType transitionType = _imageEventState.selectedEventTransitionInternal;

        switch (_imageEventState.selectedEventTransitionInternal)
        {
            // ----- Show Rotation in Custom Inspector ---- //
            case UIEventState.EventTransitionType.Rotation:
                {
                    UIEventState.RotationTransition _rotationTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
                        _rotationTransition = (UIEventState.RotationTransition)eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal];

                    ShowRotationPropertyConfiguration(_rotationTransition);
                }
                break;

            case UIEventState.EventTransitionType.Scale:
                {
                    // ----- Show Scale in Custom Inspector ---- //
                    UIEventState.ScaleTransition _scaleTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
                        _scaleTransition = (UIEventState.ScaleTransition)eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal];

                    ShowScalePropertyConfiguration(_scaleTransition);
                }
                break;

            case UIEventState.EventTransitionType.Color:
                {
                    // ----- Show Image Color in Custom Inspector ---- //
                    UIEventState.ColorTransition _imageColorTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
                        _imageColorTransition = (UIEventState.ColorTransition)eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal];

                    ShowImageColorPropertyConfiguration(_imageColorTransition);
                }
                break;

            case UIEventState.EventTransitionType.SpriteAndFill:
                {
                    // ----- Show Image Sprite in Custom Inspector ---- //
                    if (_imageEventState.imageElement != null)
                    {
                        UIEventState.SpriteAndFillTransition _imageSpriteAndFillTransition = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
                            _imageSpriteAndFillTransition = (UIEventState.SpriteAndFillTransition)eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal];

                        ShowImageSpriteAndFillPropertyConfiguration(_imageSpriteAndFillTransition);
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
                    if (_imageEventState.textElement != null)
                    {
                        UIEventState.TextFontSizeTransition _textFontSize = null;
                        if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
                            _textFontSize = (UIEventState.TextFontSizeTransition)eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal];

                        ShowFontSizeTransitionConfiguration(_textFontSize);
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

    private void GUI_ShowUnityEvents(UIEventState.EventState eventState)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        SerializedProperty sp = serializedObject.FindProperty(eventState.eventType.ToString() + "UnityEvent");
        EditorGUILayout.PropertyField(sp);
        serializedObject.ApplyModifiedProperties();
        GUILayout.EndHorizontal();
    }

    private void GUI_CreateElementHeaderUI()
    {
        GUILayout.Space(25);
        EditorGUILayout.HelpBox("Lairinus Image Event State\n    1. Select a Pointer Event\n    2. Select a Transition to modify\n    3. Click the \"Visualize Transition\" button to see it in action ", MessageType.Info);
        GUILayout.Space(25);
    }

    private void GUI_ShowBasePropertyConfiguration(UIEventState.BaseTransition transition)
    {
        if (transition != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            transition.enableTransition = EditorGUILayout.Toggle("Enable " + _imageEventState.selectedEventTransitionInternal.ToString() + " Transition", transition.enableTransition);
            GUILayout.EndHorizontal();
        }
    }

    private void ShowFontSizeTransitionConfiguration(UIEventState.TextFontSizeTransition fontSizeTransition)
    {
        if (fontSizeTransition != null)
        {
            fontSizeTransition.finalFontSize = EditorGUILayout.IntField("Final Font Size", fontSizeTransition.finalFontSize);
        }
        else
            Debug.LogError("Error: The Font Size Transition property attached to the " + _imageEventState.name + " GameObject is NULL! Ensure you aren't setting this null!");
    }

    private void GUI_ShowTransitionSelectionUI(UIEventState.EventState eventState)
    {
        GUILayout.Space(5);
        string[] eventTypeNames = Enum.GetNames(typeof(UIEventState.EventTransitionType));

        GUI.skin.button.margin = new RectOffset(0, 0, 0, 0);
        GUILayout.Space(25);
        GUI.skin.label.onNormal.textColor = Color.red;
        GUI.skin.textField.onNormal.textColor = Color.red;
        EditorGUILayout.LabelField(_imageEventState.selectedEventTypeInternal.ToString() + " Transitions:", EditorStyles.boldLabel);
        GUILayout.Space(5);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        EditorGUILayout.HelpBox("The " + _imageEventState.selectedEventTypeInternal.ToString() + " Event State contains all of the following transitions. Be sure the \"Enable Transition \" toggle is marked or else the Transition won't play!", MessageType.Info);
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        GUILayout.Space(30);
        _imageEventState.selectedEventTransitionInternal = (UIEventState.EventTransitionType)GUILayout.SelectionGrid((int)_imageEventState.selectedEventTransitionInternal, eventTypeNames, 4);
        GUILayout.EndHorizontal();

        if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
            GUI_ShowBasePropertyConfiguration(eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal]);
    }

    private void ShowRotationPropertyConfiguration(UIEventState.RotationTransition rotationTransition)
    {
        if (rotationTransition != null)
        {
            rotationTransition.finalRotation = EditorGUILayout.Vector3Field("Final Rotation", rotationTransition.finalRotation);
        }
        else
            Debug.LogError("Error: The Rotation Transition property attached to the " + _imageEventState.name + " GameObject is NULL! Ensure you aren't setting this null!");
    }

    private void ShowScalePropertyConfiguration(UIEventState.ScaleTransition scaleTransition)
    {
        if (scaleTransition != null)
        {
            scaleTransition.finalScale = EditorGUILayout.Vector3Field("Final Scale", scaleTransition.finalScale);
        }
        else
            Debug.LogError("Error: The Scale Transition property attached to the " + _imageEventState.name + " GameObject is NULL! Ensure you aren't setting this null!");
    }

    private void ShowImageColorPropertyConfiguration(UIEventState.ColorTransition imageColorTransition)
    {
        if (imageColorTransition != null)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Space(30);
            imageColorTransition.finalColor = EditorGUILayout.ColorField("Final Color", imageColorTransition.finalColor);
            GUILayout.EndHorizontal();
        }
        else
            Debug.LogError("Error: The Image Color Transition property attached to the " + _imageEventState.name + " GameObject is NULL! Ensure you aren't setting this null!");
    }

    private void ShowImageSpriteAndFillPropertyConfiguration(UIEventState.SpriteAndFillTransition spriteAndFillTransition)
    {
        if (spriteAndFillTransition != null)
        {
            spriteAndFillTransition.finalSprite = (Sprite)EditorGUILayout.ObjectField("Final Sprite", spriteAndFillTransition.finalSprite, typeof(Sprite), true);
            spriteAndFillTransition.useImageFill = EditorGUILayout.Toggle("Use Image Fill", spriteAndFillTransition.useImageFill);
            spriteAndFillTransition.fillType = (UIEventState.SpriteAndFillTransition.FillType)EditorGUILayout.EnumPopup("Image Fill Type", spriteAndFillTransition.fillType);
            spriteAndFillTransition.fillMethod = (UnityEngine.UI.Image.FillMethod)EditorGUILayout.EnumPopup("Image Fill Method", spriteAndFillTransition.fillMethod);
        }
        else
            Debug.LogError("Error: The Image Color Transition property attached to the " + _imageEventState.name + " GameObject is NULL! Ensure you aren't setting this null!");
    }

    private void ShowActionableUI(UIEventState.EventState eventState)
    {
        GUILayout.Space(30);
        EditorGUILayout.LabelField("Visualize Event States:", EditorStyles.boldLabel);
        GUILayout.Space(10);
        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Visualize Transition", GUILayout.Height(25), GUILayout.Width(200)))
        {
            Debug.Log("Ok");
            _imageEventState.VisualizeTransitionInternal(eventState);
        }

        GUILayout.Space(30);

        if (GUILayout.Button("Set Previous Transition", GUILayout.Height(25), GUILayout.Width(200)))
        {
            Debug.Log("Ok");
            _imageEventState.VisualizeTransitionInternal(_imageEventState.previousEventState);
        }
        GUILayout.EndHorizontal();
        GUILayout.Space(10);
    }

    private void ShowEventsConfiguration(UIEventState.EventState foundEventState)
    {
    }
}