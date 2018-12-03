using System;
using EssentialPackages.LightingEditor.Editor.Utils;
using UnityEditor;
using UnityEngine;

namespace EssentialPackages.LightingEditor.Editor
{
    /// <inheritdoc />
    /// <summary>
    /// The inspector only improves the workflow by enabling/disabling property fields and providing
    /// enum popups for specific fields.
    /// </summary>
    [CustomEditor(typeof(SceneLightingSpecification))]
    public class SceneLightingSpecificationInspector : UnityEditor.Editor
    {
        private SceneLightingSpecification TargetScript { get; set; }
        
        private bool DisableAmbientMode { get; set; }
        private bool AmbientMode { get; set; }
        
        private void OnEnable()
        {
            TargetScript = (SceneLightingSpecification) target;
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();

            var environment = serializedObject.FindProperty("_environment");
            var realtimeLighting = serializedObject.FindProperty("_realtimeLighting");
            var mixedLighting = serializedObject.FindProperty("_mixedLighting");
            var lightmappingSettings = serializedObject.FindProperty("_lightmappingSettings");
            var otherSettings = serializedObject.FindProperty("_otherSettings");
            var debugSettings = serializedObject.FindProperty("_debugSettings");

            DisableAmbientMode = realtimeLighting.FindPropertyRelative("_realtimeGlobalIllumination").boolValue
                                 != mixedLighting.FindPropertyRelative("_bakedGlobalIllumination").boolValue;

            DrawEnvironmentGroup(environment);
            DrawRealtimeLightingGroup(realtimeLighting);
            DrawMixedLightingGroup(mixedLighting);
            DrawLightmappingSettings(lightmappingSettings);
            DrawOtherSettings(otherSettings);
            DrawDebugSettings(debugSettings);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEnvironmentGroup(SerializedProperty serializedProperty)
        {
            var environmentLighting = serializedProperty.FindPropertyRelative("_environmentLighting");
            var environmentReflections = serializedProperty.FindPropertyRelative("_environmentReflections");

            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(serializedProperty.name), EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            Inspector.DrawPropertyField(serializedProperty.FindPropertyRelative("_skyboxMaterial"));
            Inspector.DrawPropertyField(serializedProperty.FindPropertyRelative("_sunSource"));
            
            EditorGUILayout.Space();
            
            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(environmentLighting.name));
            EditorGUI.indentLevel++;

            var source = environmentLighting.FindPropertyRelative("_source");
            Inspector.DrawPopupGroup(
                source,
                new [] {"Skybox", "Gradient", "Color"}
            );
            
            Action addFieldsForSkybox = () => {
                Inspector.DrawFloatSlider(
                    environmentLighting.FindPropertyRelative("_intensityMultiplier"),
                    0.0f,
                    8.0f
                ); 
            };
            
            Action addFieldsForGradient = () => {
                Inspector.DrawPropertyField(environmentLighting.FindPropertyRelative("_skyColor"));
                Inspector.DrawPropertyField(environmentLighting.FindPropertyRelative("_equatorColor"));
                Inspector.DrawPropertyField(environmentLighting.FindPropertyRelative("_groundColor"));
            };
            
            Action addFieldsForColor = () => {
                Inspector.DrawPropertyField(environmentLighting.FindPropertyRelative("_ambientColor"));
            };

            switch (source.stringValue)
            {
                case "Skybox":
                    addFieldsForSkybox();
                    break;
                case "Gradient":
                    addFieldsForGradient();
                    break;
                case "Color":
                    addFieldsForColor();
                    break;
                default:
                    addFieldsForSkybox();
                    addFieldsForGradient();
                    addFieldsForColor();
                    break;
            }

            EditorGUI.BeginDisabledGroup(DisableAmbientMode);
            Inspector.DrawPopupGroup(
                environmentLighting.FindPropertyRelative("_ambientMode"),
                new [] {"Realtime", "Baked"}
            );
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(environmentReflections.name));
            EditorGUI.indentLevel++;

            source = environmentReflections.FindPropertyRelative("_source");
            Inspector.DrawPopupGroup(
                source,
                new [] {"Skybox", "Custom"}
            );
            
            Action addFieldsForSkyboxNo2 = () => {
                Inspector.DrawPopupGroup(
                    environmentReflections.FindPropertyRelative("_resolution"),
                    new [] {"16", "32", "64", "128", "256", "512", "1024", "2048"}
                );
            };
            
            Action addFieldsForCustom = () => {
                Inspector.DrawPropertyField(environmentReflections.FindPropertyRelative("_cubemap"));
            };
            
            switch (source.stringValue)
            {
                case "Skybox":
                    addFieldsForSkyboxNo2();
                    break;
                case "Custom":
                    addFieldsForCustom();
                    break;
                default:
                    addFieldsForSkyboxNo2();
                    addFieldsForCustom();
                    break;
            }

            Inspector.DrawPopupGroup(
                environmentReflections.FindPropertyRelative("_compression"),
                new [] {"Uncompressed", "Compressed", "Auto"}
            );
            Inspector.DrawFloatSlider(
                environmentReflections.FindPropertyRelative("_intensityMultiplier"),
                0.0f,
                1.0f
            );
            Inspector.DrawIntSlider(
                environmentReflections.FindPropertyRelative("_bounces"),
                1,
                5
            );

            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void DrawRealtimeLightingGroup(SerializedProperty property)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(property.name),
                EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            Inspector.DrawCheckbox(property.FindPropertyRelative("_realtimeGlobalIllumination"));
            
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void DrawMixedLightingGroup(SerializedProperty property)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(property.name),
                EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            Inspector.DrawCheckbox(property.FindPropertyRelative("_bakedGlobalIllumination"));

            var lightingMode = property.FindPropertyRelative("_lightingMode");
            Inspector.DrawPopupGroup(
                lightingMode,
                new [] {"Baked Indirect", "Subtractive", "Shadowmask"}
            );

            if (lightingMode.stringValue == "Subtractive")
            {
                Inspector.DrawPropertyField(property.FindPropertyRelative("_realtimeShadowColor"));
            }
            
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void DrawLightmappingSettings(SerializedProperty property)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(property.name),
                EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            var lightMapper = property.FindPropertyRelative("_lightmapper");
            
            Inspector.DrawPopupGroup(
                lightMapper.FindPropertyRelative("_lightMapper"),
                new [] {"Enlighten", "Progressive"}
            );
            
            EditorGUI.indentLevel++;
            
            Inspector.DrawCheckbox(lightMapper.FindPropertyRelative("_prioritizeView"));
            Inspector.DrawIntField(lightMapper.FindPropertyRelative("_directSamples"));
            Inspector.DrawIntField(lightMapper.FindPropertyRelative("_indirectSamples"));
            Inspector.DrawPopupGroup(
                lightMapper.FindPropertyRelative("_bounces"),
                new [] {"None", "1", "2", "3", "4"}
            );
            Inspector.DrawPopupGroup(
                lightMapper.FindPropertyRelative("_filtering"),
                new [] {"None", "Auto", "Advanced"}
            );
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();
            
            Inspector.DrawIntField(property.FindPropertyRelative("_indirectResolution"));
            Inspector.DrawFloatField(property.FindPropertyRelative("_lightmapResolution"));
            Inspector.DrawIntField(property.FindPropertyRelative("_lightmapPadding"));
            Inspector.DrawPopupGroup(
                property.FindPropertyRelative("_lightmapSize"),
                new [] {"32", "64", "128", "256", "512", "1024", "2048", "4096"}
            );
            Inspector.DrawCheckbox(property.FindPropertyRelative("_compressLightmaps"));
            Inspector.DrawCheckbox(property.FindPropertyRelative("_ambientOcclusion"));
            Inspector.DrawPopupGroup(
                property.FindPropertyRelative("_directionalMode"),
                new [] {"Non-Directional", "Directional"}
            );
            Inspector.DrawFloatSlider(
                property.FindPropertyRelative("_indirectIntensity"),
                0.0f,
                5.0f
            );
            Inspector.DrawFloatSlider(
                property.FindPropertyRelative("_albedoBoost"),
                1.0f,
                10.0f
            );
            Inspector.DrawPopupGroup(
                property.FindPropertyRelative("_lightmapParameters"),
                new [] {"Default-Medium", "Default-HighResolution", "Default-LowResolution", "Default-VeryLowResolution", "Create New ..."}
            );
            
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void DrawOtherSettings(SerializedProperty property)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(property.name),
                EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            Inspector.DrawCheckbox(property.FindPropertyRelative("_fog"));
            Inspector.DrawPropertyField(property.FindPropertyRelative("_haloTexture"));
            Inspector.DrawFloatSlider(
                property.FindPropertyRelative("_haloStrength"),
                0.0f,
                1.0f
            );
            Inspector.DrawFloatField(property.FindPropertyRelative("_flareFadeSpeed"));
            Inspector.DrawFloatSlider(
                property.FindPropertyRelative("_flareFadeStrength"),
                0.0f,
                1.0f
            );
            Inspector.DrawPropertyField(property.FindPropertyRelative("_spotCookie"));
            
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void DrawDebugSettings(SerializedProperty property)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(property.name),
                EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
            
            var lightProbeVisualization = property.FindPropertyRelative("_lightProbeVisualization");
            
            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(lightProbeVisualization.name));
            EditorGUI.indentLevel++;

            Inspector.DrawPopupGroup(
                lightProbeVisualization.FindPropertyRelative("_dropdownMenu"),
                new []{"Only Probes Used By Selection", "All Probes No Cells", "All Probes With Cells", "None"},
                false
            );
            Inspector.DrawCheckbox(lightProbeVisualization.FindPropertyRelative("_displayWeights"));
            Inspector.DrawCheckbox(lightProbeVisualization.FindPropertyRelative("_displayOcclusion"));

            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            Inspector.DrawCheckbox(property.FindPropertyRelative("_autoGenerate"));
        }
    }
}
