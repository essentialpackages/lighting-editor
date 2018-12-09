﻿using System;
using System.Linq;
using System.Reflection;
using EssentialPackages.LightingEditor.Editor.Classes;
using EssentialPackages.LightingEditor.Editor.Utils;
using UnityEditor;
using UnityEditor.Compilation;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Rendering;
using Assembly = System.Reflection.Assembly;

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
        private bool showAdditionalInformation = false;
        
        private void OnEnable()
        {   
            var environment = serializedObject.FindProperty("_environment");
            var skyboxMaterial = environment.FindPropertyRelative("_skyboxMaterial");
            if (skyboxMaterial.objectReferenceValue == null)
            {
                skyboxMaterial.objectReferenceValue = RenderSettings.skybox;
            }

            TargetScript = (SceneLightingSpecification) target;
        }

        public override void OnInspectorGUI()
        {
            //DrawDefaultInspector();
            EditorGUILayout.BeginVertical();
            
            showAdditionalInformation = EditorGUILayout.Foldout(showAdditionalInformation, "Tips");
            if (showAdditionalInformation)
            {
                EditorGUILayout.HelpBox(HelpMessage.Summary, MessageType.Info);
                EditorGUILayout.HelpBox(HelpMessage.Advice, MessageType.Info);
                EditorGUILayout.HelpBox(HelpMessage.Footnote, MessageType.Info);
            }

            SaveRenderSettings();
            LoadRenderSettings();
            EditorGUILayout.EndVertical();
            EndGroup();

            var environment = serializedObject.FindProperty("_environment");
            var realtimeLighting = serializedObject.FindProperty("_realtimeLighting");
            var mixedLighting = serializedObject.FindProperty("_mixedLighting");
            var lightmappingSettings = serializedObject.FindProperty("_lightmappingSettings");
            var otherSettings = serializedObject.FindProperty("_otherSettings");
            var debugSettings = serializedObject.FindProperty("_debugSettings");

            var realtimeEnabled = realtimeLighting.FindPropertyRelative("_realtimeGlobalIllumination").boolValue;
            var bakedEnabled = mixedLighting.FindPropertyRelative("_bakedGlobalIllumination").boolValue;
                                 
            DrawEnvironmentGroup(environment, realtimeEnabled, bakedEnabled);
            DrawRealtimeLightingGroup(realtimeLighting);
            DrawMixedLightingGroup(mixedLighting);
            DrawLightmappingSettings(lightmappingSettings, realtimeEnabled, bakedEnabled);
            DrawOtherSettings(otherSettings);
            DrawDebugSettings(debugSettings);

            serializedObject.ApplyModifiedProperties();
        }

        private void SaveRenderSettings()
        {
            if (GUILayout.Button("Scriptable Object < Scene Lighting Settings"))
            {
                var environment = serializedObject.FindProperty("_environment");
                var environmentLighting = environment.FindPropertyRelative("_environmentLighting");
                var environmentReflections = environment.FindPropertyRelative("_environmentReflections");
                var realtimeLighting = serializedObject.FindProperty("_realtimeLighting");
                var mixedLighting = serializedObject.FindProperty("_mixedLighting");
                var lightmappingSettings = serializedObject.FindProperty("_lightmappingSettings");
                var debugSettings = serializedObject.FindProperty("_debugSettings");
                
                var realtimeGlobalIllumination = realtimeLighting.FindPropertyRelative("_realtimeGlobalIllumination");
                
                var bakedGlobalIllumination = mixedLighting.FindPropertyRelative("_bakedGlobalIllumination");
                var lightingMode = mixedLighting.FindPropertyRelative("_lightingMode");
                var realtimeShadowColor = mixedLighting.FindPropertyRelative("_realtimeShadowColor");
                
                // TODO UnityCsReference/Editor/Mono/SceneModeWindows/LightingWindowBakeSettings.cs
                var lightMapper = lightmappingSettings.FindPropertyRelative("_lightmapper");
                var ambientOcclusion = lightmappingSettings.FindPropertyRelative("_ambientOcclusion");
                var finalGather = lightmappingSettings.FindPropertyRelative("_finalGather");
                var rayCount = lightmappingSettings.FindPropertyRelative("_rayCount");
                var denoising = lightmappingSettings.FindPropertyRelative("_denoising");
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
                var indirectResolution = lightmappingSettings.FindPropertyRelative("_indirectResolution");
                var lightmapResolution = lightmappingSettings.FindPropertyRelative("_lightmapResolution");
                var lightmapPadding = lightmappingSettings.FindPropertyRelative("_lightmapPadding");
                var lightmapSize = lightmappingSettings.FindPropertyRelative("_lightmapSize");
                var compressLightmaps = lightmappingSettings.FindPropertyRelative("_compressLightmaps");
                var maxDistance = lightmappingSettings.FindPropertyRelative("_maxDistance");
                var indirectContribution = lightmappingSettings.FindPropertyRelative("_indirectContribution");
                var directContribution = lightmappingSettings.FindPropertyRelative("_directContribution");
                var directionalMode = lightmappingSettings.FindPropertyRelative("_directionalMode");
                var indirectIntensity = lightmappingSettings.FindPropertyRelative("_indirectIntensity");
                var albedoBoost = lightmappingSettings.FindPropertyRelative("_albedoBoost");
                var lightmapParameters = lightmappingSettings.FindPropertyRelative("_lightmapParameters");
                
                // TODO UnityCsReference/Editor/Mono/SettingsWindow/OtherRenderingEditor.cs
                var otherSettings = serializedObject.FindProperty("_otherSettings");
                var fog = otherSettings.FindPropertyRelative("_fog");
                var color = otherSettings.FindPropertyRelative("_color");
                var mode = otherSettings.FindPropertyRelative("_mode");
                var start = otherSettings.FindPropertyRelative("_start");
                var end = otherSettings.FindPropertyRelative("_end");
                var density = otherSettings.FindPropertyRelative("_density");
                var haloTexture = otherSettings.FindPropertyRelative("_haloTexture");
                var haloStrength = otherSettings.FindPropertyRelative("_haloStrength");
                var flareFadeSpeed = otherSettings.FindPropertyRelative("_flareFadeSpeed");
                var flareFadeStrength = otherSettings.FindPropertyRelative("_flareFadeStrength");
                var spotCookie = otherSettings.FindPropertyRelative("_spotCookie");
                
                var lightProbeVisualization = debugSettings.FindPropertyRelative("_lightProbeVisualization");
                var dropdownMenu = lightProbeVisualization.FindPropertyRelative("_dropdownMenu");
                var displayWeights = lightProbeVisualization.FindPropertyRelative("_displayWeights");
                var displayOcclusion = lightProbeVisualization.FindPropertyRelative("_displayOcclusion");
                var autoGenerate = debugSettings.FindPropertyRelative("_autoGenerate");
                
                environment.FindPropertyRelative("_skyboxMaterial").objectReferenceValue = RenderSettings.skybox;

                var source = environmentLighting.FindPropertyRelative("_source");
                switch (RenderSettings.ambientMode)
                {
                        case AmbientMode.Skybox:
                            source.stringValue = "Skybox";
                        break;
                        case AmbientMode.Trilight:
                            source.stringValue = "Gradient";
                        break;
                        case AmbientMode.Flat:
                            source.stringValue = "Color";
                        break;
                        case AmbientMode.Custom:
                            break;
                        default:
                            break;
                }

                // Important note No. 1: Because of the serialization of Light components, one cannot set the
                // 'Sun Source' via property.FindPropertyRelative("_sunSource").objectReferenceValue
                // As a workaround, set the value of your target script and call Update() afterwards to save changes.
                // See also: https://docs.unity3d.com/ScriptReference/EditorUtility.SetDirty.html
                // Important note No. 2: If you have removed the default light source in your lighting window,
                // you will see 'Missing (Light)' inside the corresponding field. When saving this change,
                // you would suspect, that 'Sun Source' inside the scriptable object will be set to Null. But instead
                // RenderSettings.sun can still return a valid light component. Some experiments have shown, that the
                // first light component which was created after deleting the reference inside the lighting window,
                // will be used. The order of multiple light components inside the scene hierarchy does not play any
                // role.
                TargetScript.Environment.SunSource = RenderSettings.sun;
                serializedObject.Update();

                environmentLighting.FindPropertyRelative("_intensityMultiplier").floatValue =
                    RenderSettings.ambientIntensity;
                environmentLighting.FindPropertyRelative("_skyColor").colorValue = RenderSettings.ambientSkyColor;
                environmentLighting.FindPropertyRelative("_equatorColor").colorValue = RenderSettings.ambientEquatorColor;
                environmentLighting.FindPropertyRelative("_groundColor").colorValue = RenderSettings.ambientGroundColor;
                environmentLighting.FindPropertyRelative("_ambientColor").colorValue = RenderSettings.ambientLight;

                environmentReflections.FindPropertyRelative("_source").stringValue =
                    RenderSettings.defaultReflectionMode.ToString();
                environmentReflections.FindPropertyRelative("_resolution").stringValue =
                    RenderSettings.defaultReflectionResolution.ToString();
                environmentReflections.FindPropertyRelative("_cubemap").objectReferenceValue =
                    RenderSettings.customReflection;
                environmentReflections.FindPropertyRelative("_compression").stringValue =
                    LightmapEditorSettings.reflectionCubemapCompression.ToString();
                environmentReflections.FindPropertyRelative("_intensityMultiplier").floatValue = RenderSettings.reflectionIntensity;
                environmentReflections.FindPropertyRelative("_bounces").intValue = RenderSettings.reflectionBounces;

                // Realtime Lighting
                realtimeGlobalIllumination.boolValue = Lightmapping.realtimeGI;
                
                // Mixed Lighting
                bakedGlobalIllumination.boolValue = Lightmapping.bakedGI;
                switch (LightmapEditorSettings.mixedBakeMode)
                {
                    case MixedLightingMode.IndirectOnly:
                        lightingMode.stringValue = "Baked Indirect";
                        break;
                    case MixedLightingMode.Subtractive:
                        lightingMode.stringValue = "Subtractive";
                        break;
                    case MixedLightingMode.Shadowmask:
                        lightingMode.stringValue = "Shadowmask";
                        break;
                    default:
                        lightingMode.stringValue = LightmapEditorSettings.mixedBakeMode.ToString();
                        break;
                }
                
                realtimeShadowColor.colorValue = RenderSettings.subtractiveShadowColor;
                
                // Lightmapping Settings
                // Important note: Some fields which can be seen in the Lighting Window cannot be accessed via
                // LightmapEditorSettings or RenderSettings. They are covered in the internal class
                // LightingWindowBakeSettings. To get access, use Reflections to invoke the private method
                // GetLightmapSettings. Make sure you receive a UnityEngine.Object. Finally create a temporary
                // SerializedObject and work with FindProperty to get access to the missing fields.
                var t = typeof(LightmapEditorSettings);
                var m = t.GetMethod("GetLightmapSettings", BindingFlags.Static | BindingFlags.NonPublic);
                var o = m.Invoke(null, null) as UnityEngine.Object;
                var so = new SerializedObject(o);
                var t2 = typeof(RenderSettings);
                var m2 = t2.GetMethod("GetRenderSettings", BindingFlags.Static | BindingFlags.NonPublic);
                var o2 = m2.Invoke(null, null) as UnityEngine.Object;
                var so2 = new SerializedObject(o2);
                
                ambientOcclusion.boolValue =  LightmapEditorSettings.enableAmbientOcclusion;
                prioritizeView.boolValue = LightmapEditorSettings.prioritizeView;;
                directSamples.intValue = LightmapEditorSettings.directSampleCount;
                indirectSamples.intValue = LightmapEditorSettings.indirectSampleCount;
                var bou = new[] {"None", "1", "2", "3", "4"};
                var modes = new[] {"Gaussian", "A-Trous", "None"};
                bounces.stringValue = bou[LightmapEditorSettings.bounces];
                filtering.stringValue = LightmapEditorSettings.filteringMode.ToString();
                directFilter.stringValue = modes[(int)LightmapEditorSettings.filterTypeDirect];
                directRadius.intValue = LightmapEditorSettings.filteringGaussRadiusDirect;
                directSigma.floatValue = LightmapEditorSettings.filteringAtrousPositionSigmaDirect;
                indirectRadius.intValue = LightmapEditorSettings.filteringGaussRadiusIndirect;
                indirectSigma.floatValue = LightmapEditorSettings.filteringAtrousPositionSigmaIndirect;
                indirectFilter.stringValue = modes[(int)LightmapEditorSettings.filterTypeIndirect];
                ambientOcclusionRadius.intValue = LightmapEditorSettings.filteringGaussRadiusAO;
                ambientOcclusionSigma.floatValue = LightmapEditorSettings.filteringAtrousPositionSigmaAO;
                ambientOcclusionFilter.stringValue = modes[(int)LightmapEditorSettings.filterTypeAO];
                switch (LightmapEditorSettings.lightmapper)
                {
                    case LightmapEditorSettings.Lightmapper.Enlighten:
                        lightmapper.stringValue = "Enlighten";
                        break;
                    case LightmapEditorSettings.Lightmapper.ProgressiveCPU:
                        lightmapper.stringValue = "Progressive";
                        break;
                    default:
                        lightmapper.stringValue = LightmapEditorSettings.lightmapper.ToString();
                        break;
                }
                 
                indirectResolution.floatValue = LightmapEditorSettings.realtimeResolution;
                lightmapResolution.floatValue = LightmapEditorSettings.bakeResolution;
                lightmapPadding.intValue = LightmapEditorSettings.padding;
                lightmapSize.stringValue = LightmapEditorSettings.maxAtlasSize.ToString();
                compressLightmaps.boolValue = LightmapEditorSettings.textureCompression;
                maxDistance.floatValue = LightmapEditorSettings.aoMaxDistance;
                indirectContribution.floatValue = LightmapEditorSettings.aoExponentIndirect;
                directContribution.floatValue = LightmapEditorSettings.aoExponentDirect;
                finalGather.boolValue = so.FindProperty("m_LightmapEditorSettings.m_FinalGather").boolValue;
                rayCount.intValue = so.FindProperty("m_LightmapEditorSettings.m_FinalGatherRayCount").intValue;
                denoising.boolValue = so.FindProperty("m_LightmapEditorSettings.m_FinalGatherFiltering").boolValue;
                
                switch (LightmapEditorSettings.lightmapsMode)
                {
                    case LightmapsMode.NonDirectional:
                        directionalMode.stringValue = "Non-Directional";
                        break;
                    case LightmapsMode.CombinedDirectional:
                        directionalMode.stringValue = "Directional";
                        break;
                    default:
                        directionalMode.stringValue = LightmapEditorSettings.lightmapsMode.ToString();
                        break;
                }
                indirectIntensity.floatValue = Lightmapping.indirectOutputScale;
                albedoBoost.floatValue = Lightmapping.bounceBoost;
                
                // TODO Debug.Log(so.FindProperty("m_LightmapEditorSettings.m_LightmapParameters").objectReferenceValue as LightmapParameters);
                lightmapParameters.stringValue = so.FindProperty("m_LightmapEditorSettings.m_LightmapParameters").objectReferenceValue.ToString().Split()[0];

                // Other Settings
                fog.boolValue = RenderSettings.fog;
                color.colorValue = RenderSettings.fogColor;
                mode.stringValue = RenderSettings.fogMode.ToString();
                start.floatValue = RenderSettings.fogStartDistance;
                end.floatValue = RenderSettings.fogEndDistance;
                density.floatValue = RenderSettings.fogDensity;
                haloTexture.objectReferenceValue = so2.FindProperty("m_HaloTexture").objectReferenceValue;
                haloStrength.floatValue = RenderSettings.haloStrength;
                flareFadeSpeed.floatValue = RenderSettings.flareFadeSpeed;
                flareFadeStrength.floatValue = RenderSettings.flareStrength;
                spotCookie.objectReferenceValue = so2.FindProperty("m_SpotCookie").objectReferenceValue;

                // Debug Settings
                /*dropdownMenu.stringValue = (...) UnityEditor.LightProbeVisualization.lightProbeVisualizationMode;
                displayWeights.boolValue = UnityEditor.LightProbeVisualization.showInterpolationWeights;
                displayOcclusion.boolValue = UnityEditor.LightProbeVisualization.showOcclusions;*/
                
                if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.Iterative)
                {
                    autoGenerate.boolValue = true;
                }
                else if (Lightmapping.giWorkflowMode == Lightmapping.GIWorkflowMode.OnDemand)
                {
                    autoGenerate.boolValue = false;
                }
            }
        }

        private void LoadRenderSettings()
        {
            EditorGUI.BeginDisabledGroup(true);
            if (GUILayout.Button("Scriptable Object > Scene Lighting Settings"))
            {
                
            }
            EditorGUI.EndDisabledGroup();
        }

        private void DrawEnvironmentGroup(SerializedProperty property, bool realtimeEnabled, bool bakedEnabled)
        {
            var environmentLighting = property.FindPropertyRelative("_environmentLighting");
            var environmentReflections = property.FindPropertyRelative("_environmentReflections");
            var skyboxMaterial = property.FindPropertyRelative("_skyboxMaterial");
            var sunSource = property.FindPropertyRelative("_sunSource");
 
            BeginGroup(property.name, EditorStyles.boldLabel);
            
            Inspector.DrawPropertyField(skyboxMaterial);
            
            // Important note: Because of the serialization of Light components, one cannot set
            // property.FindPropertyRelative("_sunSource").objectReferenceValue
            TargetScript.Environment.SunSource = EditorGUILayout.ObjectField(
                ObjectNames.NicifyVariableName(sunSource.name),
                TargetScript.Environment.SunSource,
                typeof (Light),
                true,
                GUILayout.Width(EditorGUIUtility.currentViewWidth - 120)
            ) as Light;

            EditorGUILayout.Space();
            
            BeginGroup(environmentLighting.name, EditorStyles.label);
            DrawEnvironmentLighting(environmentLighting, realtimeEnabled, bakedEnabled);
            
            EditorGUI.indentLevel--;
            EditorGUILayout.Space();

            BeginGroup(environmentReflections.name, EditorStyles.label);
            DrawEnvironmentReflections(environmentReflections);

            EndGroup();
        }

        private static void DrawEnvironmentLighting(SerializedProperty property, bool realtimeEnabled, bool bakedEnabled)
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

            if (!realtimeEnabled && !bakedEnabled)
            {
                return;
            }
            if (realtimeEnabled && !bakedEnabled)
            {
                ambientMode.stringValue = "Realtime";
            }
            else if (!realtimeEnabled)
            {
                ambientMode.stringValue = "Baked";
            }
            
            EditorGUI.BeginDisabledGroup(!realtimeEnabled || !bakedEnabled);
            Inspector.DrawPopupGroup(ambientMode, new [] {"Realtime", "Baked"});
            EditorGUI.EndDisabledGroup();
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
            BeginGroup(property.name, EditorStyles.boldLabel);
            
            Inspector.DrawCheckbox(property.FindPropertyRelative("_realtimeGlobalIllumination"));
            
            EndGroup();
        }

        private static void DrawMixedLightingGroup(SerializedProperty property)
        {
            var bakedGlobalIllumination = property.FindPropertyRelative("_bakedGlobalIllumination");
            var lightingMode = property.FindPropertyRelative("_lightingMode");
            var realtimeShadowColor = property.FindPropertyRelative("_realtimeShadowColor");
            
            BeginGroup(property.name, EditorStyles.boldLabel);

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

        private static void DrawLightmappingSettings(SerializedProperty property, bool realtimeEnabled, bool bakedEnabled)
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
            var rayCount = property.FindPropertyRelative("_rayCount");
            var denoising = property.FindPropertyRelative("_denoising");
            var directionalMode = property.FindPropertyRelative("_directionalMode");
            var indirectIntensity = property.FindPropertyRelative("_indirectIntensity");
            var albedoBoost = property.FindPropertyRelative("_albedoBoost");
            var lightmapParameters = property.FindPropertyRelative("_lightmapParameters");
            
            BeginGroup(property.name, EditorStyles.boldLabel);

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
                    Inspector.DrawPopupGroup(directFilter, new[] {"Gaussian", "A-Trous", "None"});

                    switch (directFilter.stringValue)
                    {
                        case "Gaussian":
                            drawDirectRadius();
                            break;
                        case "A-Trous":
                            drawDirectSigma();
                            break;
                        case "None":
                            break;
                        default:
                            drawDirectRadius();
                            drawDirectSigma();
                            break;
                    }

                    Inspector.DrawPopupGroup(indirectFilter, new[] {"Gaussian", "A-Trous", "None"});
                    
                    switch (indirectFilter.stringValue)
                    {
                        case "Gaussian":
                            drawIndirectRadius();
                            break;
                        case "A-Trous":
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
                    
                    Inspector.DrawPopupGroup(ambientOcclusionFilter, new[] {"Gaussian", "A-Trous", "None"});

                    switch (ambientOcclusionFilter.stringValue)
                    {
                        case "Gaussian":
                            drawAmbientOcclusionRadius();
                            break;
                        case "A-Trous":
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

                if (!filtering.stringValue.Equals("None") & !filtering.stringValue.Equals("Auto"))
                {
                    drawAdvancedFilter();
                }

                EditorGUI.indentLevel--;
                EditorGUILayout.Space();
            };

            EditorGUI.BeginDisabledGroup(!bakedEnabled);
            
            Inspector.DrawPopupGroup(lightmapper, new [] {"Enlighten", "Progressive"});
            
            if (lightmapper.stringValue != "Enlighten")
            {
                drawExtraFields();  
            }
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!realtimeEnabled);
            Inspector.DrawFloatField(indirectResolution);
            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!bakedEnabled);
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
                if (finalGather.boolValue)
                {
                    Inspector.DrawIntField(rayCount);
                    Inspector.DrawCheckbox(denoising);
                }
            }

            EditorGUI.EndDisabledGroup();
            
            EditorGUI.BeginDisabledGroup(!realtimeEnabled && !bakedEnabled);
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
            
            BeginGroup(property.name, EditorStyles.boldLabel);

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
            var lightProbeVisualization = property.FindPropertyRelative("_lightProbeVisualization");
            var dropdownMenu = lightProbeVisualization.FindPropertyRelative("_dropdownMenu");
            var displayWeights = lightProbeVisualization.FindPropertyRelative("_displayWeights");
            var displayOcclusion = lightProbeVisualization.FindPropertyRelative("_displayOcclusion");
            var autoGenerate = property.FindPropertyRelative("_autoGenerate");
            
            BeginGroup(property.name, EditorStyles.boldLabel);

            var normalColor = GUI.color;
            GUI.color = Color.yellow;
            EditorGUILayout.HelpBox("At the moment debug settings can neither be saved nor loaded", MessageType.Info);
            GUI.color = normalColor;
            
            BeginGroup(lightProbeVisualization.name, EditorStyles.label);

            Inspector.DrawPopupGroup(
                dropdownMenu,
                new []{"Only Probes Used By Selection", "All Probes No Cells", "All Probes With Cells", "None"},
                false
            );
            Inspector.DrawCheckbox(displayWeights);
            Inspector.DrawCheckbox(displayOcclusion);

            EditorGUI.indentLevel--;
            
            EditorGUILayout.Space();
            EditorGUILayout.Space();
            
            Inspector.DrawHorizontalLine();
            
            EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            Inspector.DrawCheckboxLeft(autoGenerate);
            EditorGUILayout.EndHorizontal();
        }

        private static void BeginGroup(string header, GUIStyle guiStyle)
        {
            EditorGUILayout.LabelField(
                ObjectNames.NicifyVariableName(header),
                guiStyle);
            EditorGUI.indentLevel++;
        }

        private static void EndGroup()
        {
            EditorGUI.indentLevel = 0;
            Inspector.DrawHorizontalLine();
        }
    }
}
