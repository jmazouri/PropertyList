using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(PropertyList))]
public class PropertyPanelInspector : Editor
{
    private ReorderableList list;

    void OnEnable()
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty("TypeTemplates"),
                                   false, true, true, true);

        list.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, "Type Prefabs");
        };

        list.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var element = list.serializedProperty.GetArrayElementAtIndex(index);

            
            rect.y += 2;
            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.currentViewWidth * 0.4f, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("TypeName"), GUIContent.none);
            EditorGUI.PropertyField(
                new Rect(rect.x + EditorGUIUtility.currentViewWidth * 0.4f + 5, rect.y, EditorGUIUtility.currentViewWidth * 0.5f, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("Template"), GUIContent.none);

        };

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        ((PropertyList)target).DumpDebug = EditorGUILayout.ToggleLeft("Enable Debug", ((PropertyList) target).DumpDebug);

        list.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }

    // Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
