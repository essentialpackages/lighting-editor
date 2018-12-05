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
            BeginGroup(property.name);

            var lightMapper = property.FindPropertyRelative("_lightmapper");

            

            var ambientOcclusion = property.FindPropertyRelative("_ambientOcclusion");

            Action drawExtraFields = () =>
            {
                EditorGUI.indentLevel++;

                Inspector.DrawCheckbox(lightMapper.FindPropertyRelative("_prioritizeView"));
                Inspector.DrawIntField(lightMapper.FindPropertyRelative("_directSamples"));
                Inspector.DrawIntField(lightMapper.FindPropertyRelative("_indirectSamples"));
                Inspector.DrawPopupGroup(
                    lightMapper.FindPropertyRelative("_bounces"),
                    new[] {"None", "1", "2", "3", "4"}
                );

                var filtering = lightMapper.FindPropertyRelative("_filtering");
                Inspector.DrawPopupGroup(
                    filtering,
                    new[] {"None", "Auto", "Advanced"}
                );

                Action drawAdvancedFilter = () =>
                {
                    var directFilter = lightMapper.FindPropertyRelative("_directFilter");
                    Inspector.DrawPopupGroup(
                        directFilter,
                        new[] {"Gaussian", "A-Torus", "None"}
                    );

                    Action drawDirectRadius = () =>
                    {
                        Inspector.DrawIntSlider(
                            lightMapper.FindPropertyRelative("_directRadius"),
                            0,
                            5
                        );
                    };

                    Action drawDirectSigma = () =>
                    {
                        Inspector.DrawFloatSlider(
                            lightMapper.FindPropertyRelative("_directSigma"),
                            0.0f,
                            2.0f
                        );
                    };
                    
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
                    
                    Action drawIndirectRadius = () =>
                    {
                        Inspector.DrawIntSlider(
                            lightMapper.FindPropertyRelative("_indirectRadius"),
                            0,
                            5
                        );
                    };
                    
                    Action drawInirectSigma = () =>
                    {
                        Inspector.DrawFloatSlider(
                            lightMapper.FindPropertyRelative("_indirectSigma"),
                            0.0f,
                            2.0f
                        );
                    };

                    var indirectFilter = lightMapper.FindPropertyRelative("_indirectFilter");
                    Inspector.DrawPopupGroup(
                        indirectFilter,
                        new[] {"Gaussian", "A-Torus", "None"}
                    );
                    
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
                    
                    Action drawAmbientOcclusionRadius = () =>
                    {
                        Inspector.DrawIntSlider(
                            lightMapper.FindPropertyRelative("_ambientOcclusionRadius"),
                            0,
                            5
                        );
                    };
                    
                    Action drawAmbientOcclusionSigma = () =>
                    {
                        Inspector.DrawFloatSlider(
                            lightMapper.FindPropertyRelative("_ambientOcclusionSigma"),
                            0.0f,
                            2.0f
                        );
                    };
                    
                    EditorGUI.BeginDisabledGroup(!ambientOcclusion.boolValue);
                    var ambientOcclusionFilter = lightMapper.FindPropertyRelative("_ambientOcclusionFilter");
                    Inspector.DrawPopupGroup(
                        ambientOcclusionFilter,
                        new[] {"Gaussian", "A-Torus", "None"}
                    );

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
            
            var lightmapper = lightMapper.FindPropertyRelative("_lightMapper");
            Inspector.DrawPopupGroup(
                lightmapper,
                new [] {"Enlighten", "Progressive"}
            );
            
            switch (lightmapper.stringValue)
            {
                case "Enlighten":
                    break;
                case "Progressive":
                    drawExtraFields();
                    break;
                default:
                    drawExtraFields();
                    break;    
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!RealtimeEnabled);
            Inspector.DrawIntField(property.FindPropertyRelative("_indirectResolution"));
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!BakedEnabled);
            Inspector.DrawFloatField(property.FindPropertyRelative("_lightmapResolution"));
            Inspector.DrawIntField(property.FindPropertyRelative("_lightmapPadding"));
            Inspector.DrawPopupGroup(
                property.FindPropertyRelative("_lightmapSize"),
                new [] {"32", "64", "128", "256", "512", "1024", "2048", "4096"}
            );
            Inspector.DrawCheckbox(property.FindPropertyRelative("_compressLightmaps"));
            
            Inspector.DrawCheckbox(ambientOcclusion);

            if (ambientOcclusion.boolValue)
            {
                EditorGUI.indentLevel++;
                Inspector.DrawFloatField(property.FindPropertyRelative("_maxDistance"));
                Inspector.DrawFloatSlider(
                    property.FindPropertyRelative("_indirectContribution"),
                    0.0f,
                    10.0f
                );
                Inspector.DrawFloatSlider(
                    property.FindPropertyRelative("_directContribution"),
                    0.0f,
                    10.0f
                );
                EditorGUI.indentLevel--;
            }

            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!RealtimeEnabled && !BakedEnabled);
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
            EditorGUI.EndDisabledGroup();

            EndGroup();
        }

        private static void DrawOtherSettings(SerializedProperty property)
        {
            BeginGroup(property.name);

            var fog = property.FindPropertyRelative("_fog");
            Inspector.DrawCheckbox(fog);

            if (fog.boolValue)
            {
                EditorGUI.indentLevel++;
                Inspector.DrawPropertyField(property.FindPropertyRelative("_color"));

                var mode = property.FindPropertyRelative("_mode");
                Inspector.DrawPopupGroup(
                    mode,
                    new [] {"Linear", "Exponential", "Exponential Squared"}
                );

                Action addFieldsForLinearMode = () => {
                    Inspector.DrawFloatField(property.FindPropertyRelative("_start"));
                    Inspector.DrawFloatField(property.FindPropertyRelative("_end"));
                };

                Action addFieldsForExponentialMode = () =>
                {
                    Inspector.DrawFloatField(property.FindPropertyRelative("_density"));
                };

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
