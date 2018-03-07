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

        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Label("Events:");
        GUILayout.FlexibleSpace();
        _imageEventState.selectedEventTypeInternal = (UIEventState.EventType)GUILayout.SelectionGrid((int)_imageEventState.selectedEventTypeInternal, new string[] { UIEventState.EventType.OnNormal.ToString(), UIEventState.EventType.OnHover.ToString(), UIEventState.EventType.OnBeginDrag.ToString(), UIEventState.EventType.OnEndDrag.ToString(), UIEventState.EventType.OnDrag.ToString(), UIEventState.EventType.OnPointerEnter.ToString(), UIEventState.EventType.OnPointerExit.ToString(), UIEventState.EventType.OnPointerUp.ToString(), UIEventState.EventType.OnPointerDown.ToString(), UIEventState.EventType.OnPointerClick.ToString() }, 3);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        //_imageEventState.selectedEventTypeInternal = (UIEventState.EventType)EditorGUILayout.EnumPopup("Pointer Event", _imageEventState.selectedEventTypeInternal);

        UIEventState.EventState foundEventState = null;
        if (_imageEventState.eventStatesCollection == null)
            return;

        if (_imageEventState.eventStatesCollection.ContainsKey(_imageEventState.selectedEventTypeInternal))
            foundEventState = _imageEventState.eventStatesCollection[_imageEventState.selectedEventTypeInternal];

        if (foundEventState != null)
        {
            foundEventState.allowedTransitionTime = EditorGUILayout.Slider("Total Event Duration", foundEventState.allowedTransitionTime, 0, 5);
            ShowTransitionConfiguration(foundEventState);
        }
        else
            return;
    }

    private void ShowTransitionConfiguration(UIEventState.EventState eventState)
    {
        if (eventState == null)
            return;

        GUI_ShowUnityEvents(eventState);

        // Show Base Properties
        UIEventState.EventTransitionType transitionType = _imageEventState.selectedEventTransitionInternal;
        GUI_ShowTransitionSelectionUI(eventState);

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

        ShowActionableUI(eventState);
    }

    private void ShowInvalidElementTypeForTransition(string elementType)
    {
        EditorGUILayout.HelpBox("This transition is only available for " + elementType + " objects.", MessageType.Warning);
    }

    private void GUI_ShowUnityEvents(UIEventState.EventState eventState)
    {
        SerializedProperty sp = serializedObject.FindProperty(eventState.eventType.ToString() + "UnityEvent");
        EditorGUILayout.PropertyField(sp);
        serializedObject.ApplyModifiedProperties();
        GUILayout.Space(10);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
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
            string buttonText = "Enable Transition";
            if (transition.enableTransition)
                buttonText = "Disable Transition";

            if (GUILayout.Button(buttonText))
            {
                transition.enableTransition = !transition.enableTransition;
            }
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
        _imageEventState.selectedEventTransitionInternal = (UIEventState.EventTransitionType)EditorGUILayout.EnumPopup("Transition", _imageEventState.selectedEventTransitionInternal);
        if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
            GUI_ShowBasePropertyConfiguration(eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal]);
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
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
            imageColorTransition.finalColor = EditorGUILayout.ColorField("Final Color", imageColorTransition.finalColor);
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
        EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        GUILayout.Space(20);
        GUIContent labelContent = new GUIContent("This label is a good label");
        GUILayout.Label(labelContent);
        if (GUILayout.Button("Visualize Transition", GUILayout.Height(25)))
        {
            Debug.Log("Ok");
            _imageEventState.VisualizeTransitionInternal(eventState);
        }

        if (GUILayout.Button("Set Previous Transition", GUILayout.Height(25)))
        {
            Debug.Log("Ok");
            _imageEventState.VisualizeTransitionInternal(_imageEventState.previousEventState);
        }
    }
}