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
        
        private bool RealtimeEnabled { get; set; }
        private bool BakedEnabled { get; set; }
        
        private void OnEnable()
        {
            TargetScript = (SceneLightingSpecification) target;
            
            var environment = serializedObject.FindProperty("_environment");
            var skyboxMaterial = environment.FindPropertyRelative("_skyboxMaterial");
            if (skyboxMaterial.objectReferenceValue == null)
            {
                skyboxMaterial.objectReferenceValue = RenderSettings.skybox;
            }
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

            RealtimeEnabled = realtimeLighting.FindPropertyRelative("_realtimeGlobalIllumination").boolValue;
            BakedEnabled = mixedLighting.FindPropertyRelative("_bakedGlobalIllumination").boolValue;
                                 

            DrawEnvironmentGroup(environment);
            DrawRealtimeLightingGroup(realtimeLighting);
            DrawMixedLightingGroup(mixedLighting);
            DrawLightmappingSettings(lightmappingSettings);
            DrawOtherSettings(otherSettings);
            DrawDebugSettings(debugSettings);

            serializedObject.ApplyModifiedProperties();
        }

        private void DrawEnvironmentGroup(SerializedProperty property)
        {
            var environmentLighting = property.FindPropertyRelative("_environmentLighting");
            var environmentReflections = property.FindPropertyRelative("_environmentReflections");
            var skyboxMaterial = property.FindPropertyRelative("_skyboxMaterial");
            var sunSource = property.FindPropertyRelative("_sunSource");
                
            BeginGroup(property.name);
            
            Inspector.DrawPropertyField(skyboxMaterial);
            Inspector.DrawPropertyField(sunSource);
            
            EditorGUILayout.Space();
            
            DrawSubHeader(environmentLighting.name);
            DrawEnvironmentLighting(environmentLighting);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            DrawSubHeader(environmentReflections.name);
            DrawEnvironmentReflections(environmentReflections);

            EndGroup();
        }

        private void DrawEnvironmentLighting(SerializedProperty property)
        {
            var source = property.FindPropertyRelative("_source");
            var intensityMultiplier = property.FindPropertyRelative("_intensityMultiplier");
            var skyColor = property.FindPropertyRelative("_skyColor");
            var equatorColor = property.FindPropertyRelative("_equatorColor");
            var groundColor = property.FindPropertyRelative("_groundColor");
            var ambientColor = property.FindPropertyRelative("_ambientColor");
            var ambientMode = property.FindPropertyRelative("_ambientMode");
            
            Action skyboxSelected = () => {
                Inspector.DrawFloatSlider(intensityMultiplier, 0.0f, 8.0f); 
            };
            
            Action gradientSelected = () => {
                Inspector.DrawPropertyField(skyColor);
                Inspector.DrawPropertyField(equatorColor);
                Inspector.DrawPropertyField(groundColor);
            };

            Action colorSelected = () => {
                Inspector.DrawPropertyField(ambientColor);
            };
            
            Inspector.DrawPopupGroup(source, new [] {"Skybox", "Gradient", "Color"});
            
            switch (source.stringValue)
            {
                case "Skybox":
                    skyboxSelected();
                    break;
                case "Gradient":
                    gradientSelected();
                    break;
                case "Color":
                    colorSelected();
                    break;
                default:
                    skyboxSelected();
                    gradientSelected();
                    colorSelected();
                    break;
            }
            
            if (RealtimeEnabled || BakedEnabled)
            {
                if (RealtimeEnabled && !BakedEnabled)
                {
                    ambientMode.stringValue = "Realtime";
                }
                else if (!RealtimeEnabled && BakedEnabled)
                {
                    ambientMode.stringValue = "Baked";
                }
            
                EditorGUI.BeginDisabledGroup(!RealtimeEnabled || !BakedEnabled);
                Inspector.DrawPopupGroup(ambientMode, new [] {"Realtime", "Baked"});
                EditorGUI.EndDisabledGroup();
            }
        }

        private static void DrawEnvironmentReflections(SerializedProperty property)
        {
            var source = property.FindPropertyRelative("_source");
            var resolution = property.FindPropertyRelative("_resolution");
            var cubemap = property.FindPropertyRelative("_cubemap");
            var compression = property.FindPropertyRelative("_compression");
            var intensityMultiplier = property.FindPropertyRelative("_intensityMultiplier");
            var bounces = property.FindPropertyRelative("_bounces");
            
            Action addFieldsForSkyboxNo2 = () => {
                Inspector.DrawPopupGroup(resolution, new [] {"16", "32", "64", "128", "256", "512", "1024", "2048"});
            };
            
            Action addFieldsForCustom = () => {
                Inspector.DrawPropertyField(cubemap);
            };
            
            Inspector.DrawPopupGroup(source, new [] {"Skybox", "Custom"});
            
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

            Inspector.DrawPopupGroup(compression, new [] {"Uncompressed", "Compressed", "Auto"});
            Inspector.DrawFloatSlider(intensityMultiplier, 0.0f, 1.0f);
            Inspector.DrawIntSlider(bounces, 1, 5);
        }

        private static void DrawRealtimeLightingGroup(SerializedProperty property)
        {
            BeginGroup(property.name);
            
            Inspector.DrawCheckbox(property.FindPropertyRelative("_realtimeGlobalIllumination"));
            
            EndGroup();
        }

        private static void DrawMixedLightingGroup(SerializedProperty property)
        {
            var bakedGlobalIllumination = property.FindPropertyRelative("_bakedGlobalIllumination");
            var lightingMode = property.FindPropertyRelative("_lightingMode");
            var realtimeShadowColor = property.FindPropertyRelative("_realtimeShadowColor");
            
            BeginGroup(property.name);

            Inspector.DrawCheckbox(bakedGlobalIllumination);

            EditorGUI.BeginDisabledGroup(!bakedGlobalIllumination.boolValue);
            Inspector.DrawPopupGroup(lightingMode, new [] {"Baked Indirect", "Subtractive", "Shadowmask"});
            if (lightingMode.stringValue == "Subtractive")
            {
                Inspector.DrawPropertyField(realtimeShadowColor);
            }
            EditorGUI.EndDisabledGroup();

            EndGroup();
        }

        private void DrawLightmappingSettings(SerializedProperty property)
        {
            var lightMapper = property.FindPropertyRelative("_lightmapper");
            var ambientOcclusion = property.FindPropertyRelative("_ambientOcclusion");
            var finalGather = property.FindPropertyRelative("_finalGather");
            var prioritizeView = lightMapper.FindPropertyRelative("_prioritizeView");
            var directSamples = lightMapper.FindPropertyRelative("_directSamples");
            var indirectSamples = lightMapper.FindPropertyRelative("_indirectSamples");
            var bounces = lightMapper.FindPropertyRelative("_bounces");
            var filtering = lightMapper.FindPropertyRelative("_filtering");
            var directFilter = lightMapper.FindPropertyRelative("_directFilter");
            var directRadius = lightMapper.FindPropertyRelative("_directRadius");
            var directSigma = lightMapper.FindPropertyRelative("_directSigma");
            var indirectRadius = lightMapper.FindPropertyRelative("_indirectRadius");
            var indirectSigma = lightMapper.FindPropertyRelative("_indirectSigma");
            var indirectFilter = lightMapper.FindPropertyRelative("_indirectFilter");
            var ambientOcclusionRadius = lightMapper.FindPropertyRelative("_ambientOcclusionRadius");
            var ambientOcclusionSigma = lightMapper.FindPropertyRelative("_ambientOcclusionSigma");
            var ambientOcclusionFilter = lightMapper.FindPropertyRelative("_ambientOcclusionFilter");
            var lightmapper = lightMapper.FindPropertyRelative("_lightMapper");
            var indirectResolution = property.FindPropertyRelative("_indirectResolution");
            var lightmapResolution = property.FindPropertyRelative("_lightmapResolution");
            var lightmapPadding = property.FindPropertyRelative("_lightmapPadding");
            var lightmapSize = property.FindPropertyRelative("_lightmapSize");
            var compressLightmaps = property.FindPropertyRelative("_compressLightmaps");
            var maxDistance = property.FindPropertyRelative("_maxDistance");
            var indirectContribution = property.FindPropertyRelative("_indirectContribution");
            var directContribution = property.FindPropertyRelative("_directContribution");
            var directionalMode = property.FindPropertyRelative("_directionalMode");
            var indirectIntensity = property.FindPropertyRelative("_indirectIntensity");
            var albedoBoost = property.FindPropertyRelative("_albedoBoost");
            var lightmapParameters = property.FindPropertyRelative("_lightmapParameters");
            
            BeginGroup(property.name);

            Action drawDirectRadius = () =>
            {
                Inspector.DrawIntSlider(directRadius, 0, 5);
            };

            Action drawDirectSigma = () =>
            {
                Inspector.DrawFloatSlider(directSigma, 0.0f, 2.0f);
            };
                
            Action drawIndirectRadius = () =>
            {
                Inspector.DrawIntSlider(indirectRadius, 0, 5);
            };
                    
            Action drawInirectSigma = () =>
            {
                Inspector.DrawFloatSlider(indirectSigma, 0.0f, 2.0f);
            };
            
            Action drawAmbientOcclusionRadius = () =>
            {
                Inspector.DrawIntSlider(ambientOcclusionRadius, 0, 5);
            };
                    
            Action drawAmbientOcclusionSigma = () =>
            {
                Inspector.DrawFloatSlider(ambientOcclusionSigma, 0.0f, 2.0f);
            };

            Action drawExtraFields = () =>
            {
                EditorGUI.indentLevel++;

                Inspector.DrawCheckbox(prioritizeView);
                Inspector.DrawIntField(directSamples);
                Inspector.DrawIntField(indirectSamples);
                Inspector.DrawPopupGroup(bounces, new[] {"None", "1", "2", "3", "4"});
                Inspector.DrawPopupGroup(filtering, new[] {"None", "Auto", "Advanced"});

                Action drawAdvancedFilter = () =>
                {
                    Inspector.DrawPopupGroup(directFilter, new[] {"Gaussian", "A-Torus", "None"});

                    switch (directFilter.stringValue)
                    {
                        case "Gaussian":
                            drawDirectRadius();
                            break;
                        case "A-Torus":
                            drawDirectSigma();
                            break;
                        case "None":
                            break;
                        default:
                            drawDirectRadius();
                            drawDirectSigma();
                            break;
                    }

                    Inspector.DrawPopupGroup(indirectFilter, new[] {"Gaussian", "A-Torus", "None"});
                    
                    switch (indirectFilter.stringValue)
                    {
                        case "Gaussian":
                            drawIndirectRadius();
                            break;
                        case "A-Torus":
                            drawInirectSigma();
                            break;
                        case "None":
                            break;
                        default:
                            drawIndirectRadius();
                            drawInirectSigma();
                            break;
                    }

                    EditorGUI.BeginDisabledGroup(!ambientOcclusion.boolValue);
                    
                    Inspector.DrawPopupGroup(ambientOcclusionFilter, new[] {"Gaussian", "A-Torus", "None"});

                    switch (ambientOcclusionFilter.stringValue)
                    {
                        case "Gaussian":
                            drawAmbientOcclusionRadius();
                            break;
                        case "A-Torus":
                            drawAmbientOcclusionSigma();
                            break;
                        case "None":
                            break;
                        default:
                            drawAmbientOcclusionRadius();
                            drawAmbientOcclusionSigma();
                            break;
                    }
                    EditorGUI.EndDisabledGroup();
                };

                switch (filtering.stringValue)
                {
                    case "None":
                        break;
                    case "Auto":
                        break;
                    case "Advanced":
                        drawAdvancedFilter();
                        break;
                    default:
                        drawAdvancedFilter();
                        break;
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            };

            EditorGUI.BeginDisabledGroup(!BakedEnabled);
            
            
            Inspector.DrawPopupGroup(lightmapper, new [] {"Enlighten", "Progressive"});
            
            if (lightmapper.stringValue != "Enlighten")
            {
                drawExtraFields();  
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!RealtimeEnabled);
            Inspector.DrawIntField(indirectResolution);
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!BakedEnabled);
            Inspector.DrawFloatField(lightmapResolution);
            Inspector.DrawIntField(lightmapPadding);
            Inspector.DrawPopupGroup(lightmapSize, new [] {"32", "64", "128", "256", "512", "1024", "2048", "4096"});
            Inspector.DrawCheckbox(compressLightmaps);
            
            Inspector.DrawCheckbox(ambientOcclusion);

            if (ambientOcclusion.boolValue)
            {
                EditorGUI.indentLevel++;
                Inspector.DrawFloatField(maxDistance);
                Inspector.DrawFloatSlider(indirectContribution, 0.0f, 10.0f);
                Inspector.DrawFloatSlider(directContribution, 0.0f, 10.0f);
                EditorGUI.indentLevel--;
            }

            if (lightmapper.stringValue != "Progressive")
            {
                Inspector.DrawCheckbox(finalGather);
            }

            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!RealtimeEnabled && !BakedEnabled);
            Inspector.DrawPopupGroup(directionalMode, new [] {"Non-Directional", "Directional"});
            Inspector.DrawFloatSlider(indirectIntensity, 0.0f, 5.0f);
            Inspector.DrawFloatSlider(albedoBoost, 1.0f, 10.0f);
            Inspector.DrawPopupGroup(
                lightmapParameters,
                new [] {"Default-Medium", "Default-HighResolution", "Default-LowResolution", "Default-VeryLowResolution", "Create New ..."}
            );
            EditorGUI.EndDisabledGroup();

            EndGroup();
        }

        private static void DrawOtherSettings(SerializedProperty property)
        {
            var fog = property.FindPropertyRelative("_fog");
            var color = property.FindPropertyRelative("_color");
            var mode = property.FindPropertyRelative("_mode");
            var start = property.FindPropertyRelative("_start");
            var end = property.FindPropertyRelative("_end");
            var density = property.FindPropertyRelative("_density");
            var haloTexture = property.FindPropertyRelative("_haloTexture");
            var haloStrength = property.FindPropertyRelative("_haloStrength");
            var flareFadeSpeed = property.FindPropertyRelative("_flareFadeSpeed");
            var flareFadeStrength = property.FindPropertyRelative("_flareFadeStrength");
            var spotCookie = property.FindPropertyRelative("_spotCookie");
            
            Action addFieldsForLinearMode = () => {
                Inspector.DrawFloatField(start);
                Inspector.DrawFloatField(end);
            };

            Action addFieldsForExponentialMode = () =>
            {
                Inspector.DrawFloatField(density);
            };
            
            BeginGroup(property.name);

            Inspector.DrawCheckbox(fog);

            if (fog.boolValue)
            {
                EditorGUI.indentLevel++;
                Inspector.DrawPropertyField(color);

                Inspector.DrawPopupGroup(
                    mode,
                    new [] {"Linear", "Exponential", "Exponential Squared"}
                );

                switch (mode.stringValue)
                {
                    case  "Linear":
                        addFieldsForLinearMode();
                        break;
                    case  "Exponential":
                        addFieldsForExponentialMode();
                        break;
                    case  "Exponential Squared":
                        addFieldsForExponentialMode();
                        break;
                    default:
                        addFieldsForLinearMode();
                        addFieldsForExponentialMode();
                        break;
                }
                
                EditorGUI.indentLevel--;
            }
            
            Inspector.DrawPropertyField(haloTexture);
            Inspector.DrawFloatSlider(haloStrength, 0.0f, 1.0f);
            Inspector.DrawFloatField(flareFadeSpeed);
            Inspector.DrawFloatSlider(flareFadeStrength, 0.0f, 1.0f);
            Inspector.DrawPropertyField(spotCookie);

            EndGroup();
        }

        private static void DrawDebugSettings(SerializedProperty property)
        {
            BeginGroup(property.name);
            
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

        private static void BeginGroup(string header)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(header),
                EditorStyles.boldLabel);
            EditorGUI.indentLevel++;
        }

        private static void EndGroup()
        {
            EditorGUI.indentLevel = 0;
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
        }

        private static void DrawSubHeader(string subheader)
        {
            EditorGUILayout.LabelField(ObjectNames.NicifyVariableName(subheader));
            EditorGUI.indentLevel++;
        }
    }
}
