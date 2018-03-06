using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Lairinus.UI.Events;

/// <summary>
/// The editor is very laggy, EVEN WITHOUT THIS!
/// </summary>
[CustomEditor(typeof(UIImageEventState))]
public class UIImageEventStateEditor : Editor
{
    private UIImageEventState _imageEventState = null;

    private void OnEnable()
    {
        _imageEventState = (UIImageEventState)target;
        _imageEventState.Initialize();
    }

    public override void OnInspectorGUI()
    {
        _imageEventState.selectedEventTypeInternal = (UIEventState.EventType)EditorGUILayout.EnumPopup("Pointer Event", _imageEventState.selectedEventTypeInternal);

        UIEventState.EventState foundEventState = null;
        if (_imageEventState.eventStatesCollection == null)
            return;

        if (_imageEventState.eventStatesCollection.ContainsKey(_imageEventState.selectedEventTypeInternal))
            foundEventState = _imageEventState.eventStatesCollection[_imageEventState.selectedEventTypeInternal];

        if (foundEventState != null)
        {
            foundEventState.allowedTransitionTime = EditorGUILayout.FloatField("Total Transition Time", foundEventState.allowedTransitionTime);
            _imageEventState.selectedEventTransitionInternal = (UIEventState.EventTransitionType)EditorGUILayout.EnumPopup("Transition", _imageEventState.selectedEventTransitionInternal);
            ShowPropertyConfiguration(foundEventState);
        }
        else
            return;
    }

    private void ShowPropertyConfiguration(UIEventState.EventState eventState)
    {
        if (eventState == null)
            return;

        // Show Base Properties
        UIEventState.EventTransitionType transitionType = _imageEventState.selectedEventTransitionInternal;
        if (eventState.eventTransitionCollection.ContainsKey(transitionType))
            ShowBasePropertyConfiguration(eventState.eventTransitionCollection[transitionType]);

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

            case UIEventState.EventTransitionType.ImageColor:
                {
                    // ----- Show Image Color in Custom Inspector ---- //
                    UIEventState.ImageColorTransition _imageColorTransition = null;
                    if (eventState.eventTransitionCollection.ContainsKey(_imageEventState.selectedEventTransitionInternal))
                        _imageColorTransition = (UIEventState.ImageColorTransition)eventState.eventTransitionCollection[_imageEventState.selectedEventTransitionInternal];

                    ShowImageColorPropertyConfiguration(_imageColorTransition);
                }
                break;
        }
    }

    private void ShowBasePropertyConfiguration(UIEventState.BaseTransition transition)
    {
        if (transition != null)
        {
            transition.enableTransition = EditorGUILayout.Toggle("Enable Transition", transition.enableTransition);
        }
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

    private void ShowImageColorPropertyConfiguration(UIEventState.ImageColorTransition imageColorTransition)
    {
        if (imageColorTransition != null)
        {
            imageColorTransition.finalColor = EditorGUILayout.ColorField("Final Color", imageColorTransition.finalColor);
        }
        else
            Debug.LogError("Error: The Image Color Transition property attached to the " + _imageEventState.name + " GameObject is NULL! Ensure you aren't setting this null!");
    }
}