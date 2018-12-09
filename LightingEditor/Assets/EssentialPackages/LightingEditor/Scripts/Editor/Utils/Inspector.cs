using System;
using UnityEditor;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Utils
{
    public static class Inspector
    {
        public static void DrawTextField(SerializedProperty property, bool showLabel = true)
        {
            property.stringValue = (showLabel) ?
                EditorGUILayout.TextField(
                    ObjectNames.NicifyVariableName(property.name),
                    property.stringValue
                )
                : EditorGUILayout.TextField(property.stringValue);
        }

        public static void DrawFloatSlider(SerializedProperty property, float min, float max)
        {
            property.floatValue = EditorGUILayout.Slider(
                ObjectNames.NicifyVariableName(property.name),
                property.floatValue,
                min,
                max,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
            );
        }
         
        public static void DrawIntSlider(SerializedProperty property, int min, int max)
        {
            property.intValue = EditorGUILayout.IntSlider(
                ObjectNames.NicifyVariableName(property.name),
                property.intValue,
                min,
                max,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
            );
        }

        public static void DrawCheckbox(SerializedProperty property)
        {
            property.boolValue = EditorGUILayout.Toggle(
                ObjectNames.NicifyVariableName(property.name),
                property.boolValue
            );
        }

        public static void DrawIntField(SerializedProperty property)
        {
            property.intValue = EditorGUILayout.IntField(
                ObjectNames.NicifyVariableName(property.name),
                property.intValue,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
            );
        }
        
        public static void DrawFloatField(SerializedProperty property)
        {
            property.floatValue = EditorGUILayout.FloatField(
                ObjectNames.NicifyVariableName(property.name),
                property.floatValue,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 140)
            );
        }

        public static void DrawPropertyField(SerializedProperty property, bool allowSceneObjects = false)
        {
            EditorGUILayout.PropertyField(
                property,
                allowSceneObjects,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 120)
            );
        }
        
        /*public static UnityEngine.Object DrawObjectField(SerializedProperty property, Type type, bool allowSceneObjects = false)
        {
            return EditorGUILayout.ObjectField(
                ObjectNames.NicifyVariableName(property.name),
                (Light) property.exposedReferenceValue,
                type,
                allowSceneObjects,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 120)
            );
        }*/
        
        public static void DrawPopupGroup(SerializedProperty serializedProperty, string[] choice, bool showLabel = true)
        {
            EditorGUILayout.BeginHorizontal();

            Inspector.DrawTextField(serializedProperty, showLabel);
            var text = serializedProperty.stringValue;

            var index = Array.FindIndex(choice, element => element == text);
            
            var normalColor = GUI.backgroundColor;
            GUI.backgroundColor = (index == -1) ? Color.red : normalColor;
            var value = EditorGUILayout.Popup(index, choice, GUILayout.Width(100));
            GUI.backgroundColor = normalColor;

            if (value != -1)
            {
                text = choice[value];
            }
            
            serializedProperty.stringValue = text;

            EditorGUILayout.EndHorizontal();
        }
    }
}
