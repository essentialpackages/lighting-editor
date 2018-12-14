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
        // RenderSettings.fogMode
        public static readonly string[] FogMode = {"Linear", "Exponential", "Exponential Squared"};

        public static readonly string[] LightProbesMode =
            {"Only Probes Used By Selection", "All Probes No Cells", "All Probes With Cells", "None"};
        // LightmapEditorSettings.maxAtlasSize
        public static readonly string[] LightmapAtlasSize = {"32", "64", "128", "256", "512", "1024", "2048", "4096"};
        // RenderSettings.defaultReflectionMode
        public static readonly string[] ReflectionsMode = {"Skybox", "Custom"};
        // RenderSettings.defaultReflectionResolution
        public static readonly string[] ReflectionsResolution =
            {"16", "32", "64", "128", "256", "512", "1024", "2048"};
        // LightmapEditorSettings.reflectionCubemapCompression
        public static readonly string[] ReflectionCubemapCompression = {"Uncompressed", "Compressed", "Auto"};
        // LightmapEditorSettings.bounces
        public static readonly string[] Bounces = {"None", "1", "2", "3", "4"};
        // RenderSettings.ambientMode
        public static readonly string[] AmbientMode = {"Skybox", "Gradient", "Color"}; // TODO name already in use for a different property?

    }
}