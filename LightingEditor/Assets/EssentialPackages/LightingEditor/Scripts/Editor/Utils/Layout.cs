using UnityEditor;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor.Utils
{
    public static class Layout
    {
        public static void HelpBox(string message, MessageType messageType, Color color)
        {
            var normalColor = GUI.color;
            GUI.color = color;
            EditorGUILayout.HelpBox(message, messageType);
            GUI.color = normalColor;
        }
    }
}