namespace EssentialPackages.LightingEditor.Editor.Utils
{
    public static class PopupOptions
    {
        // UnityEditor.LightmapEditorSettings.FilterMode
        public static readonly string[] FilterMode = {"None", "Auto", "Advanced"};
        // UnityEditor.LightmapEditorSettings.FilterType.
        public static readonly string[] FilterType = {"Gaussian", "A-Trous", "None"};
        // UnityEngine.MixedLightingMode
        public static readonly string[] MixedLightingMode = {"Baked Indirect", "Subtractive", "Shadowmask"};
        // UnityEngine.LightmapsMode
        public static readonly string[] LightmapsMode = {"Non-Directional", "Directional"};
    }
}