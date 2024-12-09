// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Hidden/BOXOPHOBIC/The Visual Engine/Helpers/Impostors Baker"
{
	Properties
	{
		[StyledCategory(Emissive Settings, true, 0, 10)]_EmissiveCategory("[ Emissive Category ]", Float) = 1
		_EmissiveIntensityValue("Emissive Intensity", Range( 0 , 1)) = 0
		[Enum(None,0,Any,1,Baked,2,Realtime,3)]_EmissiveFlagMode("Emissive GI Mode", Float) = 0
		[Enum(Constant,0,Multiply With Base Albedo,1)]_EmissiveColorMode("Emissive Color", Float) = 0
		[HDR]_EmissiveColor("Emissive Color", Color) = (1,1,1,1)
		[Enum(Nits,0,EV100,1)]_EmissivePowerMode("Emissive Power", Float) = 0
		_EmissivePowerValue("Emissive Power", Float) = 1
		[Space(10)][StyledTextureSingleLine]_EmissiveMaskTex("Emissive Mask", 2D) = "white" {}
		[Enum(Main UV,0,Extra UV,1)][Space(10)]_EmissiveSampleMode("Mask Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_EmissiveCoordMode("Mask UV Mode", Float) = 0
		[StyledVector(9)]_EmissiveCoordValue("Mask UV Value", Vector) = (1,1,0,0)
		_EmissiveMaskValue("Emissive TexR Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_EmissiveMaskRemap("Emissive TexR Mask", Vector) = (0,1,0,0)
		_EmissiveMeshValue("Emissive Mesh Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_EmissiveMeshMode("Emissive Mesh Mask", Float) = 0
		[StyledRemapSlider]_EmissiveMeshRemap("Emissive Mesh Mask", Vector) = (0,1,0,0)
		[Space(10)][StyledToggle]_EmissiveElementMode("Use Glow Globals / Elements", Float) = 0
		[StyledSpace(10)]_EmissiveEnd("[ Emissive End ]", Float) = 1
		[HideInInspector]_emissive_power_value("_emissive_power_value", Float) = 1
		[HideInInspector]_emissive_vert_mode("_emissive_vert_mode", Vector) = (0,0,0,0)
		[HideInInspector]_emissive_coord_value("_emissive_coord_value", Vector) = (1,1,0,0)
		[NoScaleOffset][StyledTextureSingleLine]_NoiseTex3D("Noise Mask 3D", 3D) = "white" {}
		[HideInInspector]_IsVersion("_IsVersion", Float) = 2040
		[HideInInspector]_render_normal("_render_normal", Vector) = (1,1,1,0)
		[StyledCategory(Object Settings, true, 0, 10)]_ObjectCategory("[ Object Category ]", Float) = 1
		[StyledMessage(Info, Use the Object Height and Radius to remap the procedural height and spherical masks when used for motion., 0, 10)]_ObjectBoundsInfo("# ObjectBoundsInfo", Float) = 0
		[Enum(Legacy,0,Standard,1)]_ObjectModelMode("Object Model Mode", Float) = 1
		[Enum(Off,0,Baked,1,Procedural,2)]_ObjectPivotMode("Object Pivots Mode", Float) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_ObjectPhaseMode("Object Phase Mask", Float) = 0
		_ObjectHeightValue("Object Height Value", Range( 0 , 40)) = 1
		_ObjectRadiusValue("Object Radius Value", Range( 0 , 40)) = 1
		[HideInInspector]_object_phase_mode("_object_phase_mode", Vector) = (0,0,0,0)
		[StyledSpace(10)]_ObjectEnd("[ Object End ]", Float) = 1
		[StyledCategory(Main Settings, true, 0, 10)]_MainCategory("[Main Category ]", Float) = 1
		[StyledMessage(Info, Use the Multi Mask remap sliders to mask out the branches from the leaves when using Dual Colors or for Global Effects. The mask is stored in the Shader texture blue channel. , 0, 10)]_MainMultiMaskInfo("# MainMultiMaskInfo", Float) = 0
		[StyledTextureSingleLine]_MainAlbedoTex("Main Albedo", 2D) = "white" {}
		[StyledTextureSingleLine]_MainNormalTex("Main Normal", 2D) = "bump" {}
		[StyledTextureSingleLine]_MainShaderTex("Main Shader", 2D) = "white" {}
		[Enum(Main UV,0,Extra UV,1,Planar,2,Triplanar,3,Stochastic,4,Stochastic Triplanar,5)][Space(10)]_MainSampleMode("Main Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_MainCoordMode("Main UV Mode", Float) = 0
		[StyledVector(9)]_MainCoordValue("Main UV Value", Vector) = (1,1,0,0)
		[HideInInspector]_main_coord_value("_main_coord_value", Vector) = (1,1,0,0)
		[Enum(Constant,0,Dual Colors,1)]_MainColorMode("Main Color", Float) = 0
		[HDR]_MainColor("Main Color", Color) = (1,1,1,1)
		[HDR]_MainColorTwo("Main ColorB", Color) = (1,1,1,1)
		_MainAlphaClipValue("Main Alpha", Range( 0 , 1)) = 0.5
		_MainAlbedoValue("Main Albedo", Range( 0 , 1)) = 1
		_MainNormalValue("Main Normal", Range( -8 , 8)) = 1
		_MainMetallicValue("Main Metallic", Range( 0 , 1)) = 0
		_MainOcclusionValue("Main Occlusion", Range( 0 , 1)) = 0
		[StyledRemapSlider]_MainMultiRemap("Main Multi Mask", Vector) = (0,0,0,0)
		_MainSmoothnessValue("Main Smoothness", Range( 0 , 1)) = 0
		[StyledSpace(10)]_MainEnd("[Main End ]", Float) = 1
		[StyledCategory(Layer Settings, true, 0, 10)]_LayerCategory("[ Layer Category ]", Float) = 1
		_SecondIntensityValue("Layer Intensity", Range( 0 , 1)) = 0
		[Enum(Off,0,Bake Settings To Impostors,1)]_SecondBakeMode("Layer Baking", Float) = 1
		[Space(10)][StyledTextureSingleLine]_SecondAlbedoTex("Layer Albedo", 2D) = "white" {}
		[StyledTextureSingleLine]_SecondNormalTex("Layer Normal", 2D) = "bump" {}
		[StyledTextureSingleLine]_SecondShaderTex("Layer Shader", 2D) = "white" {}
		[Enum(Main UV,0,Extra UV,1,Planar,2,Triplanar,3,Stochastic,4,Stochastic Triplanar,5)][Space(10)]_SecondSampleMode("Layer Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_SecondCoordMode("Layer UV Mode", Float) = 0
		[StyledVector(9)]_SecondCoordValue("Layer UV Value", Vector) = (1,1,0,0)
		[Enum(Constant,0,Dual Colors,1)]_SecondColorMode("Layer Color", Float) = 0
		[HDR]_SecondColor("Layer Color", Color) = (1,1,1,1)
		[HDR]_SecondColorTwo("Layer ColorB", Color) = (1,1,1,1)
		_SecondAlphaClipValue("Layer Alpha", Range( 0 , 1)) = 0.5
		_SecondAlbedoValue("Layer Albedo", Range( 0 , 1)) = 1
		_SecondNormalValue("Layer Normal", Range( -8 , 8)) = 1
		_SecondMetallicValue("Layer Metallic", Range( 0 , 1)) = 0
		_SecondOcclusionValue("Layer Occlusion", Range( 0 , 1)) = 0
		[StyledRemapSlider]_SecondMultiRemap("Layer Multi Mask", Vector) = (0,0,0,0)
		_SecondSmoothnessValue("Layer Smoothness", Range( 0 , 1)) = 0
		[Space(10)]_SecondBlendIntensityValue("Layer Blend Intensity", Range( 0 , 1)) = 1
		_SecondBlendAlbedoValue("Layer Blend Albedos", Range( 0 , 1)) = 0
		_SecondBlendNormalValue("Layer Blend Normals", Range( 0 , 1)) = 0
		_SecondBlendShaderValue("Layer Blend Shaders", Range( 0 , 1)) = 0
		[Space(10)][StyledTextureSingleLine]_SecondMaskTex("Layer Mask", 2D) = "white" {}
		[Enum(Main UV,0,Extra UV,1,Planar,2,Triplanar,3)][Space(10)]_SecondMaskSampleMode("Mask Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_SecondMaskCoordMode("Mask UV Mode", Float) = 0
		[StyledVector(9)]_SecondMaskCoordValue("Mask UV Value", Vector) = (1,1,0,0)
		_SecondMaskValue("Layer TexB Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_SecondMaskRemap("Layer TexB Mask", Vector) = (0,1,0,0)
		_SecondProjValue("Layer ProjY Mask", Range( 0 , 1)) = 0
		[StyledRemapSlider]_SecondProjRemap("Layer ProjY Mask", Vector) = (0,1,0,0)
		_SecondMeshValue("Layer Mesh Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_SecondMeshMode("Layer Mesh Mask", Float) = 2
		[StyledRemapSlider]_SecondMeshRemap("Layer Mesh Mask", Vector) = (0,1,0,0)
		[StyledRemapSlider]_SecondBlendRemap("Layer Blend Mask", Vector) = (0,1,0,0)
		[Space(10)][StyledToggle]_SecondElementMode("Use Coat Globals / Elements", Float) = 0
		[StyledSpace(10)]_LayerEnd("[ Layer End ]", Float) = 1
		[HideInInspector]_second_coord_value("_second_coord_value", Vector) = (1,1,0,0)
		[HideInInspector]_second_vert_mode("_second_vert_mode", Vector) = (0,0,0,0)
		[HideInInspector]_second_mask_coord_value("_second_mask_coord_value", Vector) = (1,1,0,0)
		[StyledCategory(Detail Settings, true, 0, 10)]_DetailCategory("[ Detail Category ]", Float) = 1
		_ThirdIntensityValue("Detail Intensity", Range( 0 , 1)) = 0
		[Enum(Off,0,Bake Settings To Impostors,1)]_ThirdBakeMode("Detail Baking", Float) = 1
		[Space(10)][StyledTextureSingleLine]_ThirdAlbedoTex("Detail Albedo", 2D) = "white" {}
		[StyledTextureSingleLine]_ThirdNormalTex("Detail Normal", 2D) = "bump" {}
		[StyledTextureSingleLine]_ThirdShaderTex("Detail Shader", 2D) = "white" {}
		[Enum(Main UV,0,Extra UV,1,Planar,2,Triplanar,3,Stochastic,4,Stochastic Triplanar,5)][Space(10)]_ThirdSampleMode("Detail Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_ThirdCoordMode("Detail UV Mode", Float) = 0
		[StyledVector(9)]_ThirdCoordValue("Detail UV Value", Vector) = (1,1,0,0)
		[Enum(Constant,0,Dual Colors,1)]_ThirdColorMode("Detail Color", Float) = 0
		[HDR]_ThirdColor("Detail Color", Color) = (1,1,1,1)
		[HDR]_ThirdColorTwo("Detail ColorB", Color) = (1,1,1,1)
		_ThirdAlphaClipValue("Detail Alpha", Range( 0 , 1)) = 0.5
		_ThirdAlbedoValue("Detail Albedo", Range( 0 , 1)) = 1
		_ThirdNormalValue("Detail Normal", Range( -8 , 8)) = 1
		_ThirdMetallicValue("Detail Metallic", Range( 0 , 1)) = 0
		_ThirdOcclusionValue("Detail Occlusion", Range( 0 , 1)) = 0
		[StyledRemapSlider]_ThirdMultiRemap("Detail Multi Mask", Vector) = (0,0,0,0)
		_ThirdSmoothnessValue("Detail Smoothness", Range( 0 , 1)) = 0
		[Space(10)]_ThirdBlendIntensityValue("Detail Blend Intensity", Range( 0 , 1)) = 1
		_ThirdBlendAlbedoValue("Detail Blend Albedos", Range( 0 , 1)) = 0
		_ThirdBlendNormalValue("Detail Blend Normals", Range( 0 , 1)) = 0
		_ThirdBlendShaderValue("Detail Blend Shaders", Range( 0 , 1)) = 0
		[Space(10)][StyledTextureSingleLine]_ThirdMaskTex("Detail Mask", 2D) = "white" {}
		[Enum(Main UV,0,Extra UV,1,Planar,2,Triplanar,3)][Space(10)]_ThirdMaskSampleMode("Mask Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_ThirdMaskCoordMode("Mask UV Mode", Float) = 0
		[StyledVector(9)]_ThirdMaskCoordValue("Mask UV Value", Vector) = (1,1,0,0)
		_ThirdMaskValue("Detail TexG Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_ThirdMaskRemap("Detail TexG Mask", Vector) = (0,1,0,0)
		_ThirdProjValue("Detail ProjY Mask", Range( 0 , 1)) = 0
		[StyledRemapSlider]_ThirdProjRemap("Detail ProjY Mask", Vector) = (0,1,0,0)
		_ThirdMeshValue("Detail Mesh Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_ThirdMeshMode("Detail Mesh Mask", Float) = 1
		[StyledRemapSlider]_ThirdMeshRemap("Detail Mesh Mask", Vector) = (0,1,0,0)
		[StyledRemapSlider]_ThirdBlendRemap("Detail Blend Mask", Vector) = (0,1,0,0)
		[Space(10)][StyledToggle]_ThirdElementMode("Use Coat Globals / Elements", Float) = 0
		[HideInInspector]_third_coord_value("_third_coord_value", Vector) = (1,1,0,0)
		[HideInInspector]_third_vert_mode("_third_vert_mode", Vector) = (0,0,0,0)
		[HideInInspector]_third_mask_coord_value("_third_mask_coord_value", Vector) = (1,1,0,0)
		[StyledSpace(10)]_DetailEnd("[ Detail End ]", Float) = 1
		[StyledCategory(Occlusion Settings, true, 0, 10)]_OcclusionCategory("[ Occlusion Category ]", Float) = 1
		_OcclusionIntensityValue("Occlusion Intensity", Range( 0 , 1)) = 0
		[Enum(Off,0,Bake Settings To Impostors,1)]_OcclusionBakeMode("Occlusion Baking", Float) = 1
		[HDR]_OcclusionColorOne("Occlusion ColorA", Color) = (1,1,1,1)
		[HDR]_OcclusionColorTwo("Occlusion ColorB", Color) = (0.25,0.25,0.25,1)
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)][Space(10)]_OcclusionMeshMode("Occlusion Mesh Mask", Float) = 1
		[StyledRemapSlider]_OcclusionMeshRemap("Occlusion Mesh Mask", Vector) = (0,1,0,0)
		[StyledSpace(10)]_OcclusionEnd("[ Occlusion End ]", Float) = 1
		[HideInInspector]_occlusion_vert_mode("_occlusion_vert_mode", Vector) = (0,0,0,0)
		[StyledCategory(Gradient Settings, true, 0, 10)]_GradientCategory("[ Gradient Category ]", Float) = 1
		_GradientIntensityValue("Gradient Intensity", Range( 0 , 1)) = 0
		[Enum(Off,0,Bake Settings To Impostors,1)]_GradientBakeMode("Gradient Baking", Float) = 1
		[HDR]_GradientColorOne("Gradient ColorA", Color) = (1,0.6135602,0,1)
		[HDR]_GradientColorTwo("Gradient ColorB", Color) = (0.754717,0.0389044,0.03203986,1)
		[Space(10)]_GradientMultiValue("Gradient Multi Mask", Range( 0 , 1)) = 1
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_GradientMeshMode("Gradient Mesh Mask", Float) = 3
		[StyledRemapSlider]_GradientMeshRemap("Gradient Mesh Mask", Vector) = (0,1,0,0)
		[StyledSpace(10)]_GradientEnd("[ Gradient End ]", Float) = 1
		[HideInInspector]_gradient_vert_mode("_gradient_vert_mode", Vector) = (0,0,0,0)
		[StyledCategory(Tinting Settings, true, 0, 10)]_TintingCategory("[ Tinting Category ]", Float) = 1
		_TintingIntensityValue("Tinting Intensity", Range( 0 , 1)) = 0
		[Enum(Keep Dynamic On Impostors,0,Bake Settings To Impostors,1)]_TintingBakeMode("Tinting Baking", Float) = 0
		_TintingGrayValue("Tinting Gray", Range( 0 , 1)) = 1
		[HDR][Gamma]_TintingColor("Tinting Color", Color) = (1,1,1,1)
		[StyledSpace(10)]_TintingSpace("[ Tinting Space ]", Float) = 1
		_TintingMultiValue("Tinting Multi Mask", Range( 0 , 1)) = 1
		_TintingLumaValue("Tinting Luma Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_TintingLumaRemap("Tinting Luma Mask", Vector) = (0,1,0,0)
		_TintingMeshValue("Tinting Mesh Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_TintingMeshMode("Tinting Mesh Mask", Float) = 3
		[StyledRemapSlider]_TintingMeshRemap("Tinting Mesh Mask", Vector) = (0,1,0,0)
		[StyledRemapSlider]_TintingBlendRemap("Tinting Blend Mask", Vector) = (0.1,0.2,0,0)
		[Space(10)][StyledToggle]_TintingElementMode("Use Paint Globals / Elements", Float) = 1
		[StyledSpace(10)]_TintingEnd("[ Tinting End]", Float) = 1
		[HideInInspector]_tinting_vert_mode("_tinting_vert_mode", Vector) = (0,0,0,0)
		[StyledCategory(Dryness Settings, true, 0, 10)]_DrynessCategory("[ Dryness Category ]", Float) = 1
		_DrynessIntensityValue("Dryness Intensity", Range( 0 , 1)) = 0
		[Enum(Keep Dynamic On Impostors,0,Bake Settings To Impostors,1)]_DrynessBakeMode("Dryness Baking", Float) = 0
		_DrynessGrayValue("Dryness Gray", Range( 0 , 1)) = 1
		_DrynessShiftValue("Dryness Shift", Range( 0 , 1)) = 0
		[HDR][Gamma]_DrynessColor("Dryness Color", Color) = (1,0.7083712,0.495283,1)
		_DrynessSubsurfaceValue("Dryness Subsurface", Range( 0 , 1)) = 0.5
		_DrynessSmoothnessValue("Dryness Smoothness", Range( 0 , 1)) = 0.5
		[StyledSpace(10)]_DrynessSpace("[ Dryness Space ]", Float) = 1
		_DrynessMultiValue("Dryness Multi Mask", Range( 0 , 1)) = 1
		_DrynessLumaValue("Dryness Luma Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_DrynessLumaRemap("Dryness Luma Mask", Vector) = (0,1,0,0)
		_DrynessMeshValue("Dryness Mesh Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_DrynessMeshMode("Dryness Mesh Mask", Float) = 3
		[StyledRemapSlider]_DrynessMeshRemap("Dryness Mesh Mask", Vector) = (0,1,0,0)
		[StyledRemapSlider]_DrynessBlendRemap("Dryness Blend Mask", Vector) = (0.1,0.2,0,0)
		[Space(10)][StyledToggle]_DrynessElementMode("Use Atmo Globals / Elements", Float) = 1
		[StyledSpace(10)]_DrynessEnd("[ Dryness End ]", Float) = 1
		[HideInInspector]_dryness_vert_mode("_dryness_vert_mode", Vector) = (0,0,0,0)
		[HideInInspector]_RenderNormal("_RenderNormal", Float) = 0
		[StyledCategory(Overlay Settings, true, 0, 10)]_OverlayCategory("[ Overlay Category ]", Float) = 1
		_OverlayIntensityValue("Overlay Intensity", Range( 0 , 1)) = 0
		[Enum(Keep Dynamic On Impostors,0,Bake Settings To Impostors,1)]_OverlayBakeMode("Overlay Baking", Float) = 0
		[Enum(Off,0,On,1)]_OverlayTextureMode("Overlay Maps", Float) = 0
		[Space(10)][StyledTextureSingleLine]_OverlayAlbedoTex("Overlay Albedo", 2D) = "white" {}
		[StyledTextureSingleLine]_OverlayNormalTex("Overlay Normal", 2D) = "bump" {}
		[Enum(Planar,0,Triplanar,1,Stochastic,2,Stochastic Triplanar,3)][Space(10)]_OverlaySampleMode("Overlay Sampling", Float) = 0
		[Enum(Tilling And Offset,0,Scale And Offset,1)]_OverlayCoordMode("Overlay UV Mode", Float) = 0
		[StyledVector(9)]_OverlayCoordValue("Overlay UV Value", Vector) = (1,1,0,0)
		[HDR]_OverlayColor("Overlay Color", Color) = (0.2815503,0.4009458,0.5377358,1)
		_OverlayNormalValue("Overlay Normal", Range( -8 , 8)) = 1
		_OverlaySubsurfaceValue("Overlay Subsurface", Range( 0 , 1)) = 0.5
		_OverlaySmoothnessValue("Overlay Smoothness", Range( 0 , 1)) = 0.5
		[Space(10)][StyledTextureSingleLine]_OverlayGlitterTexRT("Overlay Glitter RT", 2D) = "black" {}
		[Space(10)]_OverlayGlitterIntensityValue("Overlay Glitter Intensity", Range( 0 , 1)) = 0
		[HDR]_OverlayGlitterColor("Overlay Glitter Color", Color) = (0.7215686,1.913725,2.996078,1)
		_OverlayGlitterTillingValue("Overlay Glitter Tilling", Range( 0 , 8)) = 4
		_OverlayGlitterDistValue("Overlay Glitter Limit", Range( 0 , 200)) = 100
		[StyledSpace(10)]_OverlaySpace("[ Overlay Space ]", Float) = 1
		_OverlayLumaValue("Overlay Luma Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_OverlayLumaRemap("Overlay Luma Mask", Vector) = (0,1,0,0)
		_OverlayProjValue("Overlay ProjY Mask", Range( 0 , 1)) = 0.5
		[StyledRemapSlider]_OverlayProjRemap("Overlay ProjY Mask", Vector) = (0,1,0,0)
		_OverlayMeshValue("Overlay Mesh Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_OverlayMeshMode("Overlay Mesh Mask", Float) = 1
		[StyledRemapSlider]_OverlayMeshRemap("Overlay Mesh Mask", Vector) = (0,1,0,0)
		[StyledRemapSlider]_OverlayBlendRemap1("Overlay Blend Mask", Vector) = (0.1,0.2,0,0)
		[Space(10)][StyledToggle]_OverlayElementMode("Use Atmo Globals / Elements", Float) = 1
		[HideInInspector]_overlay_vert_mode("_overlay_vert_mode", Vector) = (0,0,0,0)
		[HideInInspector]_overlay_coord_value("_overlay_coord_value", Vector) = (1,1,0,0)
		[StyledSpace(10)]_OverlayEnd("[ Overlay End ]", Float) = 1
		[StyledCategory(Wetness Settings, true, 0, 10)]_WetnessCategory("[ Wetness Category ]", Float) = 1
		_WetnessIntensityValue("Wetness Intensity", Range( 0 , 1)) = 0
		[Enum(Keep Dynamic On Impostors,0,Bake Settings To Impostors,1)]_WetnessBakeMode("Wetness Baking", Float) = 0
		_WetnessContrastValue("Wetness Contrast", Range( 0 , 1)) = 0.2
		_WetnessSmoothnessValue("Wetness Smoothness", Range( 0 , 1)) = 0.8
		[Space(10)]_WetnessWaterIntensityValue("Wetness Water Intensity", Range( 0 , 1)) = 0
		[HDR]_WetnessWaterColor("Wetness Water Color", Color) = (0.5420078,0.7924528,0.6068289,1)
		_WetnessWaterBaseValue("Wetness Water Base Mask", Range( 0 , 1)) = 1
		[StyledRemapSlider]_WetnessWaterBlendRemap("Wetness Water Blend Mask", Vector) = (0.1,0.2,0,0)
		[Space(10)][StyledToggle]_WetnessElementMode("Use Atmo Globals / Elements", Float) = 1
		[StyledSpace(10)]_WetnessEnd("[ Wetness End ]", Float) = 1
		[StyledCategory(Cutout Settings, true, 0, 10)]_CutoutCategory("[ Cutout Category ]", Float) = 1
		_CutoutIntensityValue("Cutout Intensity", Range( 0 , 1)) = 0
		[Enum(Keep Dynamic On Impostors,0,Bake Settings To Impostors,1)]_CutoutBakeMode("Cutout Baking", Float) = 0
		[Space(10)]_CutoutMultiValue("Cutout Multi Mask", Range( 0 , 1)) = 1
		_CutoutAlphaValue("Cutout Alpha Mask", Range( 0 , 1)) = 0
		[Enum(Vertex R,0,Vertex G,1,Vertex B,2,Vertex A,3)]_CutoutMeshMode("Cutout Mesh Mask", Float) = 0
		_CutoutMeshValue("Cutout Mesh Mask", Range( 0 , 1)) = 0
		[StyledRemapSlider]_CutoutMeshRemap("Cutout Mesh Mask", Vector) = (0,1,0,0)
		_CutoutNoiseValue("Cutout Noise Mask", Range( 0 , 1)) = 1
		_CutoutNoiseTillingValue("Cutout Noise Tilling", Range( 0 , 100)) = 10
		[Space(10)][StyledToggle]_CutoutElementMode("Use Fade Globals / Elements", Float) = 1
		[HideInInspector]_cutout_vert_mode("_cutout_vert_mode", Vector) = (0,0,0,0)
		[StyledSpace(10)]_CutoutEnd("[ Cutout End ]", Float) = 1
		_SecondBakeMode("_SecondBakeMode", Float) = 0
		_ThirdBakeMode("_ThirdBakeMode", Float) = 0
		_OcclusionBakeMode("_OcclusionBakeMode", Float) = 0
		_GradientBakeMode("_GradientBakeMode", Float) = 0
		_CutoutBakeMode("_CutoutBakeMode", Float) = 0
		_TintingBakeMode("_TintingBakeMode", Float) = 0
		_DrynessBakeMode("_DrynessBakeMode", Float) = 0
		_OverlayBakeMode("_OverlayBakeMode", Float) = 0
		_WetnessBakeMode("_WetnessBakeMode", Float) = 0
		[HideInInspector]_RenderCull("_RenderCull", Float) = 0

	}

	SubShader
	{
		Tags { "RenderType"="Opaque" }
	LOD 100
		CGINCLUDE
		#pragma target 4.5
		ENDCG
		Cull [_RenderCull]
		

		Pass
		{
			Name "Unlit"
			CGPROGRAM
			#define ASE_USING_SAMPLING_MACROS 1

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"
			#pragma multi_compile_fwdbase
			#include "UnityShaderVariables.cginc"
			#define ASE_NEEDS_VERT_POSITION
			#define ASE_NEEDS_FRAG_NORMAL
			#define ASE_NEEDS_FRAG_COLOR
			#pragma shader_feature_local_fragment TVE_MAIN_SAMPLE_MAIN_UV TVE_MAIN_SAMPLE_EXTRA_UV TVE_MAIN_SAMPLE_PLANAR_2D TVE_MAIN_SAMPLE_PLANAR_3D TVE_MAIN_SAMPLE_STOCHASTIC_2D TVE_MAIN_SAMPLE_STOCHASTIC_3D
			#pragma shader_feature_local_vertex TVE_PIVOT_OFF TVE_PIVOT_BAKED TVE_PIVOT_PROC
			#pragma shader_feature_local_fragment TVE_ALPHA_CLIP
			#pragma shader_feature_local_fragment TVE_SECOND
			#pragma shader_feature_local_fragment TVE_SECOND_SAMPLE_MAIN_UV TVE_SECOND_SAMPLE_EXTRA_UV TVE_SECOND_SAMPLE_PLANAR_2D TVE_SECOND_SAMPLE_PLANAR_3D TVE_SECOND_SAMPLE_STOCHASTIC_2D TVE_SECOND_SAMPLE_STOCHASTIC_3D
			#pragma shader_feature_local_fragment TVE_SECOND_MASK_SAMPLE_MAIN_UV TVE_SECOND_MASK_SAMPLE_EXTRA_UV TVE_SECOND_MASK_SAMPLE_PLANAR_2D TVE_SECOND_MASK_SAMPLE_PLANAR_3D
			#pragma shader_feature_local_fragment TVE_SECOND_ELEMENT
			#pragma shader_feature_local_fragment TVE_THIRD
			#pragma shader_feature_local_fragment TVE_THIRD_SAMPLE_MAIN_UV TVE_THIRD_SAMPLE_EXTRA_UV TVE_THIRD_SAMPLE_PLANAR_2D TVE_THIRD_SAMPLE_PLANAR_3D TVE_THIRD_SAMPLE_STOCHASTIC_2D TVE_THIRD_SAMPLE_STOCHASTIC_3D
			#pragma shader_feature_local_fragment TVE_THIRD_MASK_SAMPLE_MAIN_UV TVE_THIRD_MASK_SAMPLE_EXTRA_UV TVE_THIRD_MASK_SAMPLE_PLANAR_2D TVE_THIRD_MASK_SAMPLE_PLANAR_3D
			#pragma shader_feature_local_fragment TVE_THIRD_ELEMENT
			#pragma shader_feature_local_fragment TVE_OCCLUSION
			#pragma shader_feature_local_fragment TVE_GRADIENT
			#pragma shader_feature_local_fragment TVE_TINTING
			#pragma shader_feature_local_fragment TVE_TINTING_ELEMENT
			#pragma shader_feature_local_fragment TVE_DRYNESS
			#pragma shader_feature_local_fragment TVE_DRYNESS_SHIFT
			#pragma shader_feature_local_fragment TVE_DRYNESS_ELEMENT
			#pragma shader_feature_local_fragment TVE_OVERLAY
			#pragma shader_feature_local_fragment TVE_OVERLAY_TEX
			#pragma shader_feature_local_fragment TVE_OVERLAY_SAMPLE_PLANAR_2D TVE_OVERLAY_SAMPLE_PLANAR_3D TVE_OVERLAY_SAMPLE_STOCHASTIC_2D TVE_OVERLAY_SAMPLE_STOCHASTIC_3D
			#pragma shader_feature_local_fragment TVE_OVERLAY_GLITTER
			#pragma shader_feature_local_fragment TVE_OVERLAY_ELEMENT
			#pragma shader_feature_local_fragment TVE_WETNESS
			#pragma shader_feature_local_fragment TVE_WETNESS_WATER
			#pragma shader_feature_local_fragment TVE_WETNESS_ELEMENT
			#pragma shader_feature_local_fragment TVE_CUTOUT
			#pragma shader_feature_local_fragment TVE_CUTOUT_ELEMENT
			#pragma shader_feature_local_fragment TVE_EMISSIVE
			#pragma shader_feature_local_fragment TVE_EMISSIVE_SAMPLE_MAIN_UV TVE_EMISSIVE_SAMPLE_EXTRA_UV
			#pragma shader_feature_local_fragment TVE_EMISSIVE_ELEMENT
			  
			struct TVEVisualData
			{  
				half Dummy;  
				half3 Albedo;
				half3 AlbedoRaw;
				half2 NormalTS;
				half3 NormalWS; 
				half4 Shader;
				half4 Emissive;
				half AlphaClip;
				half AlphaFade;
				half MultiMask;
				half Grayscale;
				half Luminosity;
				half3 Translucency;
				half Transmission;
				half Thickness;
				float Diffusion;
			};  
			    
			struct TVEModelData
			{    
				half Dummy;    
				half3 PositionOS;
				half3 PositionWS;
				half3 PositionWO;
				half3 PositionRawOS;
				half3 PositionAddOS;
				half3 PivotOS;
				half3 PivotWS;
				half3 PivotWO;
				half3 NormalOS;
				half3 NormalWS;
				half3 NormalRawOS;
				half3 NormalRawWS;
				half4 TangentOS;
				half3 ViewDirWS;
				half4 VertexData;
				half4 MotionData;
				half4 BoundsData;
				half4 RotationData;
			};    
			      
			struct TVEGlobalData
			{      
				half Dummy;      
				half4 CoatParams;
				half4 PaintParams;
				half4 GlowParams;
				half4 AtmoParams;
				half4 FadeParams;
				half4 FormParams;
				half4 LandParams;
				half4 WindParams;
				half4 PushParams;
			};      
			        
			#if defined(SHADER_API_D3D11) || defined(SHADER_API_XBOXONE) || defined(UNITY_COMPILER_HLSLCC) || defined(SHADER_API_PSSL) || (defined(SHADER_TARGET_SURFACE_ANALYSIS) && !defined(SHADER_TARGET_SURFACE_ANALYSIS_MOJOSHADER))//ASE Sampler Macros
			#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
			#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex.SampleLevel(samplerTex,coord, lod)
			#define SAMPLE_TEXTURE2D_BIAS(tex,samplerTex,coord,bias) tex.SampleBias(samplerTex,coord,bias)
			#define SAMPLE_TEXTURE2D_GRAD(tex,samplerTex,coord,ddx,ddy) tex.SampleGrad(samplerTex,coord,ddx,ddy)
			#define SAMPLE_TEXTURE3D(tex,samplerTex,coord) tex.Sample(samplerTex,coord)
			#else//ASE Sampling Macros
			#define SAMPLE_TEXTURE2D(tex,samplerTex,coord) tex2D(tex,coord)
			#define SAMPLE_TEXTURE2D_LOD(tex,samplerTex,coord,lod) tex2Dlod(tex,float4(coord,0,lod))
			#define SAMPLE_TEXTURE2D_BIAS(tex,samplerTex,coord,bias) tex2Dbias(tex,float4(coord,0,bias))
			#define SAMPLE_TEXTURE2D_GRAD(tex,samplerTex,coord,ddx,ddy) tex2Dgrad(tex,coord,ddx,ddy)
			#define SAMPLE_TEXTURE3D(tex,samplerTex,coord) tex3D(tex,coord)
			#endif//ASE Sampling Macros
			


			struct appdata
			{
				float4 vertex : POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 ase_color : COLOR;
			};

			struct v2f
			{
				UNITY_POSITION(pos);
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
				float4 ase_texcoord : TEXCOORD0;
				float4 ase_texcoord1 : TEXCOORD1;
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				float4 ase_texcoord5 : TEXCOORD5;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord6 : TEXCOORD6;
				float4 ase_tangent : TANGENT;
				float4 ase_color : COLOR;
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
			};

			uniform half _IsVersion;
			UNITY_DECLARE_TEX3D_NOSAMPLER(_NoiseTex3D);
			SamplerState sampler_NoiseTex3D;
			uniform half _RenderCull;
			uniform half _RenderNormal;
			uniform half _EmissiveCategory;
			uniform half _EmissiveEnd;
			uniform half _EmissivePowerMode;
			uniform half _EmissivePowerValue;
			uniform half _EmissiveElementMode;
			uniform half _EmissiveFlagMode;
			uniform half _MainCategory;
			uniform half _MainEnd;
			uniform half _MainSampleMode;
			uniform half _MainCoordMode;
			uniform half4 _MainCoordValue;
			uniform half _MainMultiMaskInfo;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_MainAlbedoTex);
			SamplerState sampler_Linear_Repeat_Aniso8;
			SamplerState sampler_Point_Repeat;
			SamplerState sampler_Linear_Repeat;
			uniform half4 _main_coord_value;
			uniform half _ObjectCategory;
			uniform half _ObjectEnd;
			uniform half _ObjectModelMode;
			uniform half _ObjectPivotMode;
			uniform half _ObjectPhaseMode;
			uniform half _ObjectBoundsInfo;
			uniform float3 TVE_WorldOrigin;
			uniform half4 _object_phase_mode;
			uniform half _ObjectHeightValue;
			uniform half _ObjectRadiusValue;
			uniform half _MainAlbedoValue;
			uniform half4 _MainColorTwo;
			uniform half4 _MainColor;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_MainShaderTex);
			uniform half _MainMetallicValue;
			uniform half _MainOcclusionValue;
			uniform half _MainSmoothnessValue;
			uniform half4 _MainMultiRemap;
			uniform half _MainColorMode;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_MainNormalTex);
			uniform half _MainNormalValue;
			uniform half _MainAlphaClipValue;
			uniform half _LayerCategory;
			uniform half _LayerEnd;
			uniform half _SecondSampleMode;
			uniform half _SecondCoordMode;
			uniform half4 _SecondCoordValue;
			uniform half _SecondMaskSampleMode;
			uniform half _SecondMaskCoordMode;
			uniform half4 _SecondMaskCoordValue;
			uniform half _SecondElementMode;
			uniform half _SecondBakeMode;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_SecondAlbedoTex);
			uniform half4 _second_coord_value;
			uniform half _SecondAlbedoValue;
			uniform half4 _SecondColorTwo;
			uniform half4 _SecondColor;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_SecondShaderTex);
			uniform half _SecondMetallicValue;
			uniform half _SecondOcclusionValue;
			uniform half _SecondSmoothnessValue;
			uniform half4 _SecondMultiRemap;
			uniform half _SecondColorMode;
			uniform half _SecondBlendAlbedoValue;
			uniform half _SecondIntensityValue;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_SecondMaskTex);
			uniform half4 _second_mask_coord_value;
			uniform half4 _SecondMaskRemap;
			uniform half _SecondMaskValue;
			uniform half4 _SecondProjRemap;
			uniform half _SecondProjValue;
			uniform half4 _second_vert_mode;
			uniform half4 _SecondMeshRemap;
			uniform half _SecondMeshValue;
			uniform half _SecondMeshMode;
			uniform half TVE_IsEnabled;
			uniform half4 _SecondBlendRemap;
			uniform half _SecondBlendIntensityValue;
			uniform half _SecondBlendNormalValue;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_SecondNormalTex);
			uniform half _SecondNormalValue;
			uniform half _SecondBlendShaderValue;
			uniform half _SecondAlphaClipValue;
			uniform half _DetailCategory;
			uniform half _DetailEnd;
			uniform half _ThirdSampleMode;
			uniform half _ThirdCoordMode;
			uniform half4 _ThirdCoordValue;
			uniform half _ThirdMaskSampleMode;
			uniform half _ThirdMaskCoordMode;
			uniform half4 _ThirdMaskCoordValue;
			uniform half _ThirdElementMode;
			uniform half _ThirdBakeMode;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_ThirdAlbedoTex);
			uniform half4 _third_coord_value;
			uniform half _ThirdAlbedoValue;
			uniform half4 _ThirdColorTwo;
			uniform half4 _ThirdColor;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_ThirdShaderTex);
			uniform half _ThirdMetallicValue;
			uniform half _ThirdOcclusionValue;
			uniform half _ThirdSmoothnessValue;
			uniform half4 _ThirdMultiRemap;
			uniform half _ThirdColorMode;
			uniform half _ThirdBlendAlbedoValue;
			uniform half _ThirdIntensityValue;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_ThirdMaskTex);
			uniform half4 _third_mask_coord_value;
			uniform half4 _ThirdMaskRemap;
			uniform half _ThirdMaskValue;
			uniform half4 _third_vert_mode;
			uniform half4 _ThirdMeshRemap;
			uniform half _ThirdMeshValue;
			uniform half _ThirdMeshMode;
			uniform half4 _ThirdProjRemap;
			uniform half _ThirdProjValue;
			uniform half4 _ThirdBlendRemap;
			uniform half _ThirdBlendIntensityValue;
			uniform half _ThirdBlendNormalValue;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_ThirdNormalTex);
			uniform half _ThirdNormalValue;
			uniform half _ThirdBlendShaderValue;
			uniform half _ThirdAlphaClipValue;
			uniform half _OcclusionCategory;
			uniform half _OcclusionEnd;
			uniform half _OcclusionBakeMode;
			uniform half4 _OcclusionColorTwo;
			uniform half4 _OcclusionColorOne;
			uniform half4 _occlusion_vert_mode;
			uniform half4 _OcclusionMeshRemap;
			uniform half _OcclusionMeshMode;
			uniform half _OcclusionIntensityValue;
			uniform half _GradientCategory;
			uniform half _GradientEnd;
			uniform half _GradientBakeMode;
			uniform half4 _GradientColorTwo;
			uniform half4 _GradientColorOne;
			uniform half4 _gradient_vert_mode;
			uniform half4 _GradientMeshRemap;
			uniform half _GradientMeshMode;
			uniform half _GradientIntensityValue;
			uniform half _GradientMultiValue;
			uniform half _TintingCategory;
			uniform half _TintingEnd;
			uniform half _TintingSpace;
			uniform half _TintingBakeMode;
			uniform half _TintingElementMode;
			uniform half _TintingGrayValue;
			uniform float4 _TintingColor;
			uniform half _TintingIntensityValue;
			uniform half _TintingMultiValue;
			uniform half4 _TintingLumaRemap;
			uniform half _TintingLumaValue;
			uniform half4 _tinting_vert_mode;
			uniform half4 _TintingMeshRemap;
			uniform half _TintingMeshValue;
			uniform half _TintingMeshMode;
			uniform half4 _TintingBlendRemap;
			uniform half _DrynessCategory;
			uniform half _DrynessEnd;
			uniform half _DrynessSpace;
			uniform half _DrynessElementMode;
			uniform half _DrynessBakeMode;
			uniform half _DrynessGrayValue;
			uniform half _DrynessShiftValue;
			uniform float4 _DrynessColor;
			uniform half _DrynessIntensityValue;
			uniform half _DrynessMultiValue;
			uniform half4 _DrynessLumaRemap;
			uniform half _DrynessLumaValue;
			uniform half4 _dryness_vert_mode;
			uniform half4 _DrynessMeshRemap;
			uniform half _DrynessMeshValue;
			uniform half _DrynessMeshMode;
			uniform half4 _DrynessBlendRemap;
			uniform half _DrynessSmoothnessValue;
			uniform half _DrynessSubsurfaceValue;
			uniform half _OverlayCategory;
			uniform half _OverlayEnd;
			uniform half _OverlaySpace;
			uniform half _OverlayElementMode;
			uniform half _OverlayBakeMode;
			uniform half4 _OverlayColor;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_OverlayAlbedoTex);
			uniform half4 _overlay_coord_value;
			uniform half _OverlaySampleMode;
			uniform half _OverlayCoordMode;
			uniform half4 _OverlayCoordValue;
			uniform half _OverlayTextureMode;
			uniform half _OverlayGlitterIntensityValue;
			uniform half4 _OverlayGlitterColor;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_OverlayGlitterTexRT);
			uniform half _OverlayGlitterTillingValue;
			uniform half _OverlayGlitterDistValue;
			uniform half _OverlayIntensityValue;
			uniform half4 _OverlayProjRemap;
			uniform half _OverlayProjValue;
			uniform half4 _OverlayLumaRemap;
			uniform half _OverlayLumaValue;
			uniform half4 _overlay_vert_mode;
			uniform half4 _OverlayMeshRemap;
			uniform half _OverlayMeshValue;
			uniform half _OverlayMeshMode;
			uniform half4 _OverlayBlendRemap1;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_OverlayNormalTex);
			uniform half _OverlayNormalValue;
			uniform half _OverlaySmoothnessValue;
			uniform half _OverlaySubsurfaceValue;
			uniform half _WetnessCategory;
			uniform half _WetnessEnd;
			uniform half _WetnessElementMode;
			uniform half _WetnessBakeMode;
			uniform half4 _WetnessWaterColor;
			uniform half _WetnessWaterIntensityValue;
			uniform half _WetnessIntensityValue;
			uniform half _WetnessWaterBaseValue;
			uniform half4 _WetnessWaterBlendRemap;
			uniform half _WetnessContrastValue;
			uniform half _WetnessSmoothnessValue;
			uniform half _CutoutCategory;
			uniform half _CutoutEnd;
			uniform half _CutoutElementMode;
			uniform half _CutoutBakeMode;
			uniform half _CutoutIntensityValue;
			uniform half _CutoutAlphaValue;
			uniform half _CutoutNoiseTillingValue;
			uniform half _CutoutNoiseValue;
			uniform half4 _cutout_vert_mode;
			uniform half4 _CutoutMeshRemap;
			uniform half _CutoutMeshValue;
			uniform half _CutoutMeshMode;
			uniform half _CutoutMultiValue;
			uniform half4 _emissive_vert_mode;
			uniform half4 _EmissiveMeshRemap;
			uniform half _EmissiveMeshValue;
			uniform half _EmissiveMeshMode;
			UNITY_DECLARE_TEX2D_NOSAMPLER(_EmissiveMaskTex);
			uniform half4 _emissive_coord_value;
			uniform half _EmissiveSampleMode;
			uniform half _EmissiveCoordMode;
			uniform half4 _EmissiveCoordValue;
			uniform half4 _EmissiveMaskRemap;
			uniform half _EmissiveMaskValue;
			uniform half _EmissiveIntensityValue;
			uniform half4 _EmissiveColor;
			uniform half _EmissiveColorMode;
			uniform float _emissive_power_value;
			uniform half3 _render_normal;
			void ComputeWorldCoords( half4 Coords, half3 WorldPosition, out half2 ZX, out half2 ZY, out half2 XY )
			{
				ZX = WorldPosition.xz * Coords.xx - Coords.zz;
				ZY = WorldPosition.yz * Coords.yx - Coords.wz; 
				XY = WorldPosition.xy * Coords.xy - Coords.zw;
			}
			
			half4 SampleMain( UNITY_DECLARE_TEX2D_NOSAMPLER(Texture), SamplerState Sampler, half4 Coords, half2 TexCoord )
			{
				half2 UV = TexCoord * Coords.xy + Coords.zw;
				half4 tex = SAMPLE_TEXTURE2D( Texture, Sampler, UV);
				return tex;
			}
			
			half4 SampleExtra( UNITY_DECLARE_TEX2D_NOSAMPLER(Texture), SamplerState Sampler, half4 Coords, half2 TexCoord )
			{
				half2 UV = TexCoord * Coords.xy + Coords.zw;
				half4 tex = SAMPLE_TEXTURE2D( Texture, Sampler, UV);
				return tex;
			}
			
			half4 SamplePlanar2D( UNITY_DECLARE_TEX2D_NOSAMPLER(Texture), SamplerState Sampler, half4 Coords, float3 WorldPosition )
			{
				half2 UV = WorldPosition.xz * Coords.xy + Coords.zw;
				half4 tex = SAMPLE_TEXTURE2D( Texture, Sampler, UV);
				return tex;
			}
			
			void ComputeTriplanarWeights( half3 WorldNormal, out half T1, out half T2, out half T3 )
			{
				half3 powNormal = abs( WorldNormal.xyz );
				half3 weights = max( powNormal * powNormal * powNormal * powNormal * powNormal * powNormal * powNormal * powNormal, 0.000001 );
				weights /= ( weights.x + weights.y + weights.z ).xxx;
				T1 = weights.y;
				T2 = weights.x;
				T3 = weights.z;
			}
			
			half4 SamplePlanar3D( UNITY_DECLARE_TEX2D_NOSAMPLER(Texture), SamplerState Sampler, half4 Coords, float3 WorldPosition, half3 WorldNormal )
			{
				half2 ZX, ZY, XY;
				ComputeWorldCoords(Coords, WorldPosition, ZX, ZY, XY);
				half T1, T2, T3;
				ComputeTriplanarWeights(WorldNormal, T1, T2, T3);
				half4 tex1 = SAMPLE_TEXTURE2D( Texture, Sampler, ZX);
				half4 tex2 = SAMPLE_TEXTURE2D( Texture, Sampler, ZY);
				half4 tex3 = SAMPLE_TEXTURE2D( Texture, Sampler, XY);
				return tex1 * T1 + tex2 * T2 + tex3 * T3;
			}
			
			void ComputeStochasticCoords( float2 UV, out float2 UV1, out float2 UV2, out float2 UV3, out float W1, out float W2, out float W3 )
			{
				half2 vertex1, vertex2, vertex3;
				// Scaling of the input
				half2 uv = UV * 3.464; // 2 * sqrt (3)
				// Skew input space into simplex triangle grid
				const float2x2 gridToSkewedGrid = float2x2( 1.0, 0.0, -0.57735027, 1.15470054 );
				half2 skewedCoord = mul( gridToSkewedGrid, uv );
				// Compute local triangle vertex IDs and local barycentric coordinates
				int2 baseId = int2( floor( skewedCoord ) );
				half3 temp = half3( frac( skewedCoord ), 0 );
				temp.z = 1.0 - temp.x - temp.y;
				if ( temp.z > 0.0 )
				{
					W1 = temp.z;
					W2 = temp.y;
					W3 = temp.x;
					vertex1 = baseId;
					vertex2 = baseId + int2( 0, 1 );
					vertex3 = baseId + int2( 1, 0 );
				}
				else
				{
					W1 = -temp.z;
					W2 = 1.0 - temp.y;
					W3 = 1.0 - temp.x;
					vertex1 = baseId + int2( 1, 1 );
					vertex2 = baseId + int2( 1, 0 );
					vertex3 = baseId + int2( 0, 1 );
				}
				UV1 = UV + frac( sin( mul( float2x2( 127.1, 311.7, 269.5, 183.3 ), vertex1 ) ) * 43758.5453 );
				UV2 = UV + frac( sin( mul( float2x2( 127.1, 311.7, 269.5, 183.3 ), vertex2 ) ) * 43758.5453 );
				UV3 = UV + frac( sin( mul( float2x2( 127.1, 311.7, 269.5, 183.3 ), vertex3 ) ) * 43758.5453 );
				return;
			}
			
			float4 SampleStochastic2D( UNITY_DECLARE_TEX2D_NOSAMPLER(Texture), SamplerState Sampler, half4 Coords, float3 WorldPosition )
			{
				half2 UV = WorldPosition.xz * Coords.xy + Coords.zw;
				half2 UV1, UV2, UV3;
				half W1, W2, W3;
				ComputeStochasticCoords(UV, UV1, UV2, UV3, W1, W2, W3 );
				half4 tex1 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV1, ddx(UV), ddy(UV));
				half4 tex2 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV2, ddx(UV), ddy(UV));
				half4 tex3 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV3, ddx(UV), ddy(UV));
				return tex1 * W1 + tex2 * W2 + tex3 * W3;
			}
			
			half4 SampleStochastic3D( UNITY_DECLARE_TEX2D_NOSAMPLER(Texture), SamplerState Sampler, half4 Coords, float3 WorldPosition, half3 WorldNormal )
			{
				half2 ZX, ZY, XY;
				ComputeWorldCoords(Coords, WorldPosition, ZX, ZY, XY);
				half2 UV1, UV2, UV3;
				half W1, W2, W3;
				half4 tex1, tex2, tex3;
				ComputeStochasticCoords(ZX, UV1, UV2, UV3, W1, W2, W3 );
				tex1 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV1, ddx(ZX), ddy(ZX));
				tex2 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV2, ddx(ZX), ddy(ZX));
				tex3 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV3, ddx(ZX), ddy(ZX));
				half4 texZX = tex1 * W1 + tex2 * W2 + tex3 * W3;
				ComputeStochasticCoords(ZY, UV1, UV2, UV3, W1, W2, W3 );
				tex1 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV1, ddx(ZY), ddy(ZY));
				tex2 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV2, ddx(ZY), ddy(ZY));
				tex3 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV3, ddx(ZY), ddy(ZY));
				half4 texZY = tex1 * W1 + tex2 * W2 + tex3 * W3;
				ComputeStochasticCoords(XY, UV1, UV2, UV3, W1, W2, W3 );
				tex1 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV1, ddx(XY), ddy(XY));
				tex2 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV2, ddx(XY), ddy(XY));
				tex3 = SAMPLE_TEXTURE2D_GRAD( Texture, Sampler, UV3, ddx(XY), ddy(XY));
				half4 texXY = tex1 * W1 + tex2 * W2 + tex3 * W3;
				half T1, T2, T3;
				ComputeTriplanarWeights(WorldNormal, T1, T2, T3);
				return texZX * T1 + texZY * T2 + texXY * T3;
			}
			
			half3 HSVToRGB( half3 c )
			{
				half4 K = half4( 1.0, 2.0 / 3.0, 1.0 / 3.0, 3.0 );
				half3 p = abs( frac( c.xxx + K.xyz ) * 6.0 - K.www );
				return c.z * lerp( K.xxx, saturate( p - K.xxx ), c.y );
			}
			
			half3 RGBToHSV(half3 c)
			{
				half4 K = half4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
				half4 p = lerp( half4( c.bg, K.wz ), half4( c.gb, K.xy ), step( c.b, c.g ) );
				half4 q = lerp( half4( p.xyw, c.r ), half4( c.r, p.yzx ), step( p.x, c.r ) );
				half d = q.x - min( q.w, q.y );
				half e = 1.0e-10;
				return half3( abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
			}


			v2f vert(appdata v )
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (v.vertex).xyz, 1 )).xyz;
				float3 vertexToFrag73_g76826 = ase_worldPos;
				o.ase_texcoord3.xyz = vertexToFrag73_g76826;
				float4x4 break19_g76828 = unity_ObjectToWorld;
				float3 appendResult20_g76828 = (float3(break19_g76828[ 0 ][ 3 ] , break19_g76828[ 1 ][ 3 ] , break19_g76828[ 2 ][ 3 ]));
				half3 ObjectData20_g76830 = appendResult20_g76828;
				half3 WorldData19_g76830 = ase_worldPos;
				#ifdef TVE_OBJECT_BATCHED
				float3 staticSwitch14_g76830 = WorldData19_g76830;
				#else
				float3 staticSwitch14_g76830 = ObjectData20_g76830;
				#endif
				float3 temp_output_124_0_g76828 = staticSwitch14_g76830;
				float3 temp_output_239_7_g76826 = temp_output_124_0_g76828;
				float4x4 break19_g76832 = unity_ObjectToWorld;
				float3 appendResult20_g76832 = (float3(break19_g76832[ 0 ][ 3 ] , break19_g76832[ 1 ][ 3 ] , break19_g76832[ 2 ][ 3 ]));
				float3 _Vector0 = float3(0,0,0);
				float3 appendResult60_g76836 = (float3(v.ase_texcoord3.x , 0.0 , v.ase_texcoord3.y));
				half3 PositionOS131_g76826 = v.vertex.xyz;
				float3 break233_g76826 = PositionOS131_g76826;
				float3 appendResult234_g76826 = (float3(break233_g76826.x , 0.0 , break233_g76826.z));
				#if defined( TVE_PIVOT_OFF )
				float3 staticSwitch229_g76826 = _Vector0;
				#elif defined( TVE_PIVOT_BAKED )
				float3 staticSwitch229_g76826 = appendResult60_g76836;
				#elif defined( TVE_PIVOT_PROC )
				float3 staticSwitch229_g76826 = appendResult234_g76826;
				#else
				float3 staticSwitch229_g76826 = _Vector0;
				#endif
				half3 PivotOS149_g76826 = staticSwitch229_g76826;
				float3 temp_output_122_0_g76832 = PivotOS149_g76826;
				float3 PivotsOnly105_g76832 = (mul( unity_ObjectToWorld, float4( temp_output_122_0_g76832 , 0.0 ) ).xyz).xyz;
				half3 ObjectData20_g76834 = ( appendResult20_g76832 + PivotsOnly105_g76832 );
				half3 WorldData19_g76834 = ase_worldPos;
				#ifdef TVE_OBJECT_BATCHED
				float3 staticSwitch14_g76834 = WorldData19_g76834;
				#else
				float3 staticSwitch14_g76834 = ObjectData20_g76834;
				#endif
				float3 temp_output_124_0_g76832 = staticSwitch14_g76834;
				float3 temp_output_237_7_g76826 = temp_output_124_0_g76832;
				#if defined( TVE_PIVOT_OFF )
				float3 staticSwitch236_g76826 = temp_output_239_7_g76826;
				#elif defined( TVE_PIVOT_BAKED )
				float3 staticSwitch236_g76826 = temp_output_237_7_g76826;
				#elif defined( TVE_PIVOT_PROC )
				float3 staticSwitch236_g76826 = temp_output_237_7_g76826;
				#else
				float3 staticSwitch236_g76826 = temp_output_239_7_g76826;
				#endif
				float3 vertexToFrag76_g76826 = staticSwitch236_g76826;
				o.ase_texcoord4.xyz = vertexToFrag76_g76826;
				float3 ase_worldNormal = UnityObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord6.xyz = ase_worldNormal;
				float3 ase_worldTangent = UnityObjectToWorldDir(v.ase_tangent);
				o.ase_texcoord7.xyz = ase_worldTangent;
				float ase_vertexTangentSign = v.ase_tangent.w * ( unity_WorldTransformParams.w >= 0.0 ? 1.0 : -1.0 );
				float3 ase_worldBitangent = cross( ase_worldNormal, ase_worldTangent ) * ase_vertexTangentSign;
				o.ase_texcoord8.xyz = ase_worldBitangent;
				
				float3 objectToViewPos = UnityObjectToViewPos(v.vertex.xyz);
				float eyeDepth = -objectToViewPos.z;
				o.ase_texcoord.w = eyeDepth;
				
				o.ase_texcoord.xyz = v.ase_texcoord.xyz;
				o.ase_texcoord1.xy = v.ase_texcoord2.xy;
				o.ase_texcoord2 = v.vertex;
				o.ase_texcoord5 = v.ase_texcoord3;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				o.ase_texcoord3.w = 0;
				o.ase_texcoord4.w = 0;
				o.ase_texcoord6.w = 0;
				o.ase_texcoord7.w = 0;
				o.ase_texcoord8.w = 0;

				v.vertex.xyz +=  float3(0,0,0) ;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}


			void frag(v2f i , bool ase_vface : SV_IsFrontFace,
				out half4 outGBuffer0 : SV_Target0,
				out half4 outGBuffer1 : SV_Target1,
				out half4 outGBuffer2 : SV_Target2,
				out half4 outGBuffer3 : SV_Target3,
				out half4 outGBuffer4 : SV_Target4,
				out half4 outGBuffer5 : SV_Target5,
				out half4 outGBuffer6 : SV_Target6,
				out half4 outGBuffer7 : SV_Target7,
				out float outDepth : SV_Depth
			)
			{
				UNITY_SETUP_INSTANCE_ID( i );
				float localBreakData4_g139296 = ( 0.0 );
				float localCompData3_g139288 = ( 0.0 );
				TVEVisualData Data3_g139288 = (TVEVisualData)0;
				half Dummy145_g139278 = ( _EmissiveCategory + _EmissiveEnd + ( _EmissivePowerMode + _EmissivePowerValue ) + _EmissiveElementMode + _EmissiveFlagMode );
				float In_Dummy3_g139288 = Dummy145_g139278;
				float localBreakData4_g139287 = ( 0.0 );
				float localIfVisualData25_g139277 = ( 0.0 );
				TVEVisualData Data25_g139277 = (TVEVisualData)0;
				float localIfVisualData25_g139263 = ( 0.0 );
				TVEVisualData Data25_g139263 = (TVEVisualData)0;
				float localIfVisualData25_g139233 = ( 0.0 );
				TVEVisualData Data25_g139233 = (TVEVisualData)0;
				float localIfVisualData25_g139196 = ( 0.0 );
				TVEVisualData Data25_g139196 = (TVEVisualData)0;
				float localIfVisualData25_g139172 = ( 0.0 );
				TVEVisualData Data25_g139172 = (TVEVisualData)0;
				float localIfVisualData25_g139147 = ( 0.0 );
				TVEVisualData Data25_g139147 = (TVEVisualData)0;
				float localIfVisualData25_g139136 = ( 0.0 );
				TVEVisualData Data25_g139136 = (TVEVisualData)0;
				float localIfVisualData25_g139125 = ( 0.0 );
				TVEVisualData Data25_g139125 = (TVEVisualData)0;
				float localIfVisualData25_g139091 = ( 0.0 );
				TVEVisualData Data25_g139091 = (TVEVisualData)0;
				float localCompData3_g139055 = ( 0.0 );
				TVEVisualData Data3_g139055 = (TVEVisualData)0;
				half4 Dummy130_g139039 = ( _MainCategory + _MainEnd + ( _MainSampleMode + _MainCoordMode + _MainCoordValue ) + _MainMultiMaskInfo );
				float In_Dummy3_g139055 = Dummy130_g139039.x;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139048) = _MainAlbedoTex;
				float localFilterTexture19_g139051 = ( 0.0 );
				SamplerState SamplerDefault19_g139051 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerPoint19_g139051 = sampler_Point_Repeat;
				SamplerState SamplerLow19_g139051 = sampler_Linear_Repeat;
				SamplerState SamplerMedium19_g139051 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh19_g139051 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS19_g139051 = SamplerDefault19_g139051;
				#if defined (TVE_FILTER_DEFAULT)
				    SS19_g139051 = SamplerDefault19_g139051;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS19_g139051 = SamplerPoint19_g139051;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS19_g139051 = SamplerLow19_g139051;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS19_g139051 = SamplerMedium19_g139051;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS19_g139051 = SamplerHigh19_g139051;
				#endif
				SamplerState Sampler276_g139048 = SS19_g139051;
				half4 Local_Coords180_g139039 = _main_coord_value;
				float4 temp_output_37_0_g139048 = Local_Coords180_g139039;
				half4 Coords276_g139048 = temp_output_37_0_g139048;
				half2 TexCoord276_g139048 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139048 = SampleMain( Texture276_g139048 , Sampler276_g139048 , Coords276_g139048 , TexCoord276_g139048 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139048) = _MainAlbedoTex;
				SamplerState Sampler275_g139048 = SS19_g139051;
				half4 Coords275_g139048 = temp_output_37_0_g139048;
				half2 TexCoord275_g139048 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139048 = SampleExtra( Texture275_g139048 , Sampler275_g139048 , Coords275_g139048 , TexCoord275_g139048 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139048) = _MainAlbedoTex;
				SamplerState Sampler238_g139048 = SS19_g139051;
				half4 Coords238_g139048 = temp_output_37_0_g139048;
				TVEModelData Data16_g76840 = (TVEModelData)0;
				half Dummy207_g76826 = ( _ObjectCategory + _ObjectEnd + _ObjectModelMode + _ObjectPivotMode + _ObjectPhaseMode + _ObjectBoundsInfo );
				float In_Dummy16_g76840 = Dummy207_g76826;
				half3 PositionOS131_g76826 = i.ase_texcoord2.xyz;
				float3 In_PositionOS16_g76840 = PositionOS131_g76826;
				float3 vertexToFrag73_g76826 = i.ase_texcoord3.xyz;
				half3 PositionWS122_g76826 = vertexToFrag73_g76826;
				float3 In_PositionWS16_g76840 = PositionWS122_g76826;
				float3 vertexToFrag76_g76826 = i.ase_texcoord4.xyz;
				half3 PivotWS121_g76826 = vertexToFrag76_g76826;
				#ifdef TVE_SCOPE_DYNAMIC
				float3 staticSwitch204_g76826 = ( PositionWS122_g76826 - PivotWS121_g76826 );
				#else
				float3 staticSwitch204_g76826 = PositionWS122_g76826;
				#endif
				half3 PositionWO132_g76826 = ( staticSwitch204_g76826 - TVE_WorldOrigin );
				float3 In_PositionWO16_g76840 = PositionWO132_g76826;
				float3 In_PositionRawOS16_g76840 = PositionOS131_g76826;
				float3 In_PositionAddOS16_g76840 = float3( 0,0,0 );
				float3 _Vector0 = float3(0,0,0);
				float3 appendResult60_g76836 = (float3(i.ase_texcoord5.x , 0.0 , i.ase_texcoord5.y));
				float3 break233_g76826 = PositionOS131_g76826;
				float3 appendResult234_g76826 = (float3(break233_g76826.x , 0.0 , break233_g76826.z));
				#if defined( TVE_PIVOT_OFF )
				float3 staticSwitch229_g76826 = _Vector0;
				#elif defined( TVE_PIVOT_BAKED )
				float3 staticSwitch229_g76826 = appendResult60_g76836;
				#elif defined( TVE_PIVOT_PROC )
				float3 staticSwitch229_g76826 = appendResult234_g76826;
				#else
				float3 staticSwitch229_g76826 = _Vector0;
				#endif
				half3 PivotOS149_g76826 = staticSwitch229_g76826;
				float3 In_PivotOS16_g76840 = PivotOS149_g76826;
				float3 In_PivotWS16_g76840 = PivotWS121_g76826;
				half3 PivotWO133_g76826 = ( PivotWS121_g76826 - TVE_WorldOrigin );
				float3 In_PivotWO16_g76840 = PivotWO133_g76826;
				half3 NormalOS134_g76826 = i.ase_normal;
				float3 In_NormalOS16_g76840 = NormalOS134_g76826;
				float3 ase_worldNormal = i.ase_texcoord6.xyz;
				float3 normalizedWorldNormal = normalize( ase_worldNormal );
				half3 Normal_WS95_g76826 = normalizedWorldNormal;
				float3 In_NormalWS16_g76840 = Normal_WS95_g76826;
				float3 In_NormalRawOS16_g76840 = NormalOS134_g76826;
				float3 objToWorldDir298_g76826 = normalize( mul( unity_ObjectToWorld, float4( i.ase_normal, 0 ) ).xyz );
				half3 Normal_RawWS136_g76826 = objToWorldDir298_g76826;
				float3 In_NormalRawWS16_g76840 = Normal_RawWS136_g76826;
				half4 TangentlOS153_g76826 = i.ase_tangent;
				float4 In_TangentOS16_g76840 = TangentlOS153_g76826;
				float3 normalizeResult296_g76826 = normalize( ( _WorldSpaceCameraPos - PositionWS122_g76826 ) );
				half3 ViewDirWS169_g76826 = normalizeResult296_g76826;
				float3 In_ViewDirWS16_g76840 = ViewDirWS169_g76826;
				half4 VertexMasks171_g76826 = i.ase_color;
				float4 In_VertexData16_g76840 = VertexMasks171_g76826;
				float4 break33_g76839 = _object_phase_mode;
				float temp_output_30_0_g76839 = ( i.ase_color.r * break33_g76839.x );
				float temp_output_29_0_g76839 = ( i.ase_color.g * break33_g76839.y );
				float temp_output_31_0_g76839 = ( i.ase_color.b * break33_g76839.z );
				float temp_output_28_0_g76839 = ( temp_output_30_0_g76839 + temp_output_29_0_g76839 + temp_output_31_0_g76839 + ( i.ase_color.a * break33_g76839.w ) );
				float3 break243_g76826 = PivotWO133_g76826;
				float temp_output_315_0_g76826 = (frac( ( temp_output_28_0_g76839 + ( break243_g76826.x + break243_g76826.z ) ) )*2.0 + -1.0);
				float4 appendResult177_g76826 = (float4(1.0 , 1.0 , temp_output_315_0_g76826 , 1.0));
				half4 MotionMasks176_g76826 = appendResult177_g76826;
				float4 In_MotionData16_g76840 = MotionMasks176_g76826;
				half Object_HeightValue267_g76826 = _ObjectHeightValue;
				half Object_RadiusValue268_g76826 = _ObjectRadiusValue;
				half Bounds_HeightMask274_g76826 = saturate( ( (PositionOS131_g76826).y / Object_HeightValue267_g76826 ) );
				half Bounds_SphereMask282_g76826 = saturate( ( length( PositionOS131_g76826 ) / max( Object_HeightValue267_g76826 , Object_RadiusValue268_g76826 ) ) );
				float4 appendResult253_g76826 = (float4(Object_HeightValue267_g76826 , Object_RadiusValue268_g76826 , Bounds_HeightMask274_g76826 , Bounds_SphereMask282_g76826));
				half4 BoundsData254_g76826 = appendResult253_g76826;
				float4 In_BoundsData16_g76840 = BoundsData254_g76826;
				float4 In_RotationData16_g76840 = float4( 0,0,0,0 );
				Data16_g76840.Dummy = In_Dummy16_g76840;
				Data16_g76840.PositionOS = In_PositionOS16_g76840;
				Data16_g76840.PositionWS = In_PositionWS16_g76840;
				Data16_g76840.PositionWO = In_PositionWO16_g76840;
				Data16_g76840.PositionRawOS = In_PositionRawOS16_g76840;
				Data16_g76840.PositionAddOS = In_PositionAddOS16_g76840;
				Data16_g76840.PivotOS = In_PivotOS16_g76840;
				Data16_g76840.PivotWS = In_PivotWS16_g76840;
				Data16_g76840.PivotWO = In_PivotWO16_g76840;
				Data16_g76840.NormalOS = In_NormalOS16_g76840;
				Data16_g76840.NormalWS = In_NormalWS16_g76840;
				Data16_g76840.NormalRawOS = In_NormalRawOS16_g76840;
				Data16_g76840.NormalRawWS = In_NormalRawWS16_g76840;
				Data16_g76840.TangentOS = In_TangentOS16_g76840;
				Data16_g76840.ViewDirWS = In_ViewDirWS16_g76840;
				Data16_g76840.VertexData = In_VertexData16_g76840;
				Data16_g76840.MotionData = In_MotionData16_g76840;
				Data16_g76840.BoundsData = In_BoundsData16_g76840;
				Data16_g76840.RotationData = In_RotationData16_g76840;
				TVEModelData Data15_g139054 = Data16_g76840;
				float Out_Dummy15_g139054 = 0;
				float3 Out_PositionWS15_g139054 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139054 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139054 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139054 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139054 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139054 = float3( 0,0,0 );
				float4 Out_VertexData15_g139054 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139054 = float4( 0,0,0,0 );
				Out_Dummy15_g139054 = Data15_g139054.Dummy;
				Out_PositionWS15_g139054 = Data15_g139054.PositionWS;
				Out_PositionWO15_g139054 = Data15_g139054.PositionWO;
				Out_PivotWS15_g139054 = Data15_g139054.PivotWS;
				Out_PivotWO15_g139054 = Data15_g139054.PivotWO;
				Out_NormalWS15_g139054 = Data15_g139054.NormalWS;
				Out_ViewDirWS15_g139054 = Data15_g139054.ViewDirWS;
				Out_VertexData15_g139054 = Data15_g139054.VertexData;
				Out_BoundsData15_g139054 = Data15_g139054.BoundsData;
				half3 Model_PositionWO222_g139039 = Out_PositionWO15_g139054;
				float3 temp_output_279_0_g139048 = Model_PositionWO222_g139039;
				half3 WorldPosition238_g139048 = temp_output_279_0_g139048;
				half4 localSamplePlanar2D238_g139048 = SamplePlanar2D( Texture238_g139048 , Sampler238_g139048 , Coords238_g139048 , WorldPosition238_g139048 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139048) = _MainAlbedoTex;
				SamplerState Sampler246_g139048 = SS19_g139051;
				half4 Coords246_g139048 = temp_output_37_0_g139048;
				half3 WorldPosition246_g139048 = temp_output_279_0_g139048;
				half3 Model_NormalWS226_g139039 = Out_NormalWS15_g139054;
				float3 temp_output_280_0_g139048 = Model_NormalWS226_g139039;
				half3 WorldNormal246_g139048 = temp_output_280_0_g139048;
				half4 localSamplePlanar3D246_g139048 = SamplePlanar3D( Texture246_g139048 , Sampler246_g139048 , Coords246_g139048 , WorldPosition246_g139048 , WorldNormal246_g139048 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139048) = _MainAlbedoTex;
				SamplerState Sampler234_g139048 = SS19_g139051;
				float4 Coords234_g139048 = temp_output_37_0_g139048;
				float3 WorldPosition234_g139048 = temp_output_279_0_g139048;
				float4 localSampleStochastic2D234_g139048 = SampleStochastic2D( Texture234_g139048 , Sampler234_g139048 , Coords234_g139048 , WorldPosition234_g139048 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139048) = _MainAlbedoTex;
				SamplerState Sampler263_g139048 = SS19_g139051;
				half4 Coords263_g139048 = temp_output_37_0_g139048;
				half3 WorldPosition263_g139048 = temp_output_279_0_g139048;
				half3 WorldNormal263_g139048 = temp_output_280_0_g139048;
				half4 localSampleStochastic3D263_g139048 = SampleStochastic3D( Texture263_g139048 , Sampler263_g139048 , Coords263_g139048 , WorldPosition263_g139048 , WorldNormal263_g139048 );
				#if defined( TVE_MAIN_SAMPLE_MAIN_UV )
				float4 staticSwitch184_g139039 = localSampleMain276_g139048;
				#elif defined( TVE_MAIN_SAMPLE_EXTRA_UV )
				float4 staticSwitch184_g139039 = localSampleExtra275_g139048;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_2D )
				float4 staticSwitch184_g139039 = localSamplePlanar2D238_g139048;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_3D )
				float4 staticSwitch184_g139039 = localSamplePlanar3D246_g139048;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch184_g139039 = localSampleStochastic2D234_g139048;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch184_g139039 = localSampleStochastic3D263_g139048;
				#else
				float4 staticSwitch184_g139039 = localSampleMain276_g139048;
				#endif
				half4 Local_AlbedoTex185_g139039 = staticSwitch184_g139039;
				float3 lerpResult53_g139039 = lerp( float3( 1,1,1 ) , (Local_AlbedoTex185_g139039).xyz , _MainAlbedoValue);
				half3 Local_AlbedoRGB107_g139039 = lerpResult53_g139039;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139050) = _MainShaderTex;
				float localFilterTexture30_g139053 = ( 0.0 );
				SamplerState SamplerDefault30_g139053 = sampler_Linear_Repeat;
				SamplerState SamplerPoint30_g139053 = sampler_Point_Repeat;
				SamplerState SamplerLow30_g139053 = sampler_Linear_Repeat;
				SamplerState SamplerMedium30_g139053 = sampler_Linear_Repeat;
				SamplerState SamplerHigh30_g139053 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS30_g139053 = SamplerDefault30_g139053;
				#if defined (TVE_FILTER_DEFAULT)
				    SS30_g139053 = SamplerDefault30_g139053;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS30_g139053 = SamplerPoint30_g139053;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS30_g139053 = SamplerLow30_g139053;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS30_g139053 = SamplerMedium30_g139053;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS30_g139053 = SamplerHigh30_g139053;
				#endif
				SamplerState Sampler276_g139050 = SS30_g139053;
				float4 temp_output_37_0_g139050 = Local_Coords180_g139039;
				half4 Coords276_g139050 = temp_output_37_0_g139050;
				half2 TexCoord276_g139050 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139050 = SampleMain( Texture276_g139050 , Sampler276_g139050 , Coords276_g139050 , TexCoord276_g139050 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139050) = _MainShaderTex;
				SamplerState Sampler275_g139050 = SS30_g139053;
				half4 Coords275_g139050 = temp_output_37_0_g139050;
				half2 TexCoord275_g139050 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139050 = SampleExtra( Texture275_g139050 , Sampler275_g139050 , Coords275_g139050 , TexCoord275_g139050 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139050) = _MainShaderTex;
				SamplerState Sampler238_g139050 = SS30_g139053;
				half4 Coords238_g139050 = temp_output_37_0_g139050;
				float3 temp_output_279_0_g139050 = Model_PositionWO222_g139039;
				half3 WorldPosition238_g139050 = temp_output_279_0_g139050;
				half4 localSamplePlanar2D238_g139050 = SamplePlanar2D( Texture238_g139050 , Sampler238_g139050 , Coords238_g139050 , WorldPosition238_g139050 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139050) = _MainShaderTex;
				SamplerState Sampler246_g139050 = SS30_g139053;
				half4 Coords246_g139050 = temp_output_37_0_g139050;
				half3 WorldPosition246_g139050 = temp_output_279_0_g139050;
				float3 temp_output_280_0_g139050 = Model_NormalWS226_g139039;
				half3 WorldNormal246_g139050 = temp_output_280_0_g139050;
				half4 localSamplePlanar3D246_g139050 = SamplePlanar3D( Texture246_g139050 , Sampler246_g139050 , Coords246_g139050 , WorldPosition246_g139050 , WorldNormal246_g139050 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139050) = _MainShaderTex;
				SamplerState Sampler234_g139050 = SS30_g139053;
				float4 Coords234_g139050 = temp_output_37_0_g139050;
				float3 WorldPosition234_g139050 = temp_output_279_0_g139050;
				float4 localSampleStochastic2D234_g139050 = SampleStochastic2D( Texture234_g139050 , Sampler234_g139050 , Coords234_g139050 , WorldPosition234_g139050 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139050) = _MainShaderTex;
				SamplerState Sampler263_g139050 = SS30_g139053;
				half4 Coords263_g139050 = temp_output_37_0_g139050;
				half3 WorldPosition263_g139050 = temp_output_279_0_g139050;
				half3 WorldNormal263_g139050 = temp_output_280_0_g139050;
				half4 localSampleStochastic3D263_g139050 = SampleStochastic3D( Texture263_g139050 , Sampler263_g139050 , Coords263_g139050 , WorldPosition263_g139050 , WorldNormal263_g139050 );
				#if defined( TVE_MAIN_SAMPLE_MAIN_UV )
				float4 staticSwitch198_g139039 = localSampleMain276_g139050;
				#elif defined( TVE_MAIN_SAMPLE_EXTRA_UV )
				float4 staticSwitch198_g139039 = localSampleExtra275_g139050;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_2D )
				float4 staticSwitch198_g139039 = localSamplePlanar2D238_g139050;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_3D )
				float4 staticSwitch198_g139039 = localSamplePlanar3D246_g139050;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch198_g139039 = localSampleStochastic2D234_g139050;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch198_g139039 = localSampleStochastic3D263_g139050;
				#else
				float4 staticSwitch198_g139039 = localSampleMain276_g139050;
				#endif
				half4 Local_MasksTex199_g139039 = staticSwitch198_g139039;
				float lerpResult23_g139039 = lerp( 1.0 , (Local_MasksTex199_g139039).y , _MainOcclusionValue);
				float4 appendResult73_g139039 = (float4(( (Local_MasksTex199_g139039).x * _MainMetallicValue ) , lerpResult23_g139039 , (Local_MasksTex199_g139039).z , ( (Local_MasksTex199_g139039).w * _MainSmoothnessValue )));
				half4 Local_Masks109_g139039 = appendResult73_g139039;
				float clampResult17_g139044 = clamp( (Local_Masks109_g139039).z , 0.0001 , 0.9999 );
				float temp_output_7_0_g139045 = _MainMultiRemap.x;
				float temp_output_10_0_g139045 = ( _MainMultiRemap.y - temp_output_7_0_g139045 );
				half Local_MultiMask78_g139039 = saturate( ( ( clampResult17_g139044 - temp_output_7_0_g139045 ) / ( temp_output_10_0_g139045 + 0.0001 ) ) );
				float lerpResult58_g139039 = lerp( 1.0 , Local_MultiMask78_g139039 , _MainColorMode);
				float4 lerpResult62_g139039 = lerp( _MainColorTwo , _MainColor , lerpResult58_g139039);
				half3 Local_ColorRGB93_g139039 = (lerpResult62_g139039).rgb;
				half3 Local_Albedo139_g139039 = ( Local_AlbedoRGB107_g139039 * Local_ColorRGB93_g139039 );
				float3 In_Albedo3_g139055 = Local_Albedo139_g139039;
				float3 In_AlbedoRaw3_g139055 = Local_Albedo139_g139039;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139049) = _MainNormalTex;
				float localFilterTexture29_g139052 = ( 0.0 );
				SamplerState SamplerDefault29_g139052 = sampler_Linear_Repeat;
				SamplerState SamplerPoint29_g139052 = sampler_Point_Repeat;
				SamplerState SamplerLow29_g139052 = sampler_Linear_Repeat;
				SamplerState SamplerMedium29_g139052 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh29_g139052 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS29_g139052 = SamplerDefault29_g139052;
				#if defined (TVE_FILTER_DEFAULT)
				    SS29_g139052 = SamplerDefault29_g139052;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS29_g139052 = SamplerPoint29_g139052;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS29_g139052 = SamplerLow29_g139052;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS29_g139052 = SamplerMedium29_g139052;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS29_g139052 = SamplerHigh29_g139052;
				#endif
				SamplerState Sampler276_g139049 = SS29_g139052;
				float4 temp_output_37_0_g139049 = Local_Coords180_g139039;
				half4 Coords276_g139049 = temp_output_37_0_g139049;
				half2 TexCoord276_g139049 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139049 = SampleMain( Texture276_g139049 , Sampler276_g139049 , Coords276_g139049 , TexCoord276_g139049 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139049) = _MainNormalTex;
				SamplerState Sampler275_g139049 = SS29_g139052;
				half4 Coords275_g139049 = temp_output_37_0_g139049;
				half2 TexCoord275_g139049 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139049 = SampleExtra( Texture275_g139049 , Sampler275_g139049 , Coords275_g139049 , TexCoord275_g139049 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139049) = _MainNormalTex;
				SamplerState Sampler238_g139049 = SS29_g139052;
				half4 Coords238_g139049 = temp_output_37_0_g139049;
				float3 temp_output_279_0_g139049 = Model_PositionWO222_g139039;
				half3 WorldPosition238_g139049 = temp_output_279_0_g139049;
				half4 localSamplePlanar2D238_g139049 = SamplePlanar2D( Texture238_g139049 , Sampler238_g139049 , Coords238_g139049 , WorldPosition238_g139049 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139049) = _MainNormalTex;
				SamplerState Sampler246_g139049 = SS29_g139052;
				half4 Coords246_g139049 = temp_output_37_0_g139049;
				half3 WorldPosition246_g139049 = temp_output_279_0_g139049;
				float3 temp_output_280_0_g139049 = Model_NormalWS226_g139039;
				half3 WorldNormal246_g139049 = temp_output_280_0_g139049;
				half4 localSamplePlanar3D246_g139049 = SamplePlanar3D( Texture246_g139049 , Sampler246_g139049 , Coords246_g139049 , WorldPosition246_g139049 , WorldNormal246_g139049 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139049) = _MainNormalTex;
				SamplerState Sampler234_g139049 = SS29_g139052;
				float4 Coords234_g139049 = temp_output_37_0_g139049;
				float3 WorldPosition234_g139049 = temp_output_279_0_g139049;
				float4 localSampleStochastic2D234_g139049 = SampleStochastic2D( Texture234_g139049 , Sampler234_g139049 , Coords234_g139049 , WorldPosition234_g139049 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139049) = _MainNormalTex;
				SamplerState Sampler263_g139049 = SS29_g139052;
				half4 Coords263_g139049 = temp_output_37_0_g139049;
				half3 WorldPosition263_g139049 = temp_output_279_0_g139049;
				half3 WorldNormal263_g139049 = temp_output_280_0_g139049;
				half4 localSampleStochastic3D263_g139049 = SampleStochastic3D( Texture263_g139049 , Sampler263_g139049 , Coords263_g139049 , WorldPosition263_g139049 , WorldNormal263_g139049 );
				#if defined( TVE_MAIN_SAMPLE_MAIN_UV )
				float4 staticSwitch193_g139039 = localSampleMain276_g139049;
				#elif defined( TVE_MAIN_SAMPLE_EXTRA_UV )
				float4 staticSwitch193_g139039 = localSampleExtra275_g139049;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_2D )
				float4 staticSwitch193_g139039 = localSamplePlanar2D238_g139049;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_3D )
				float4 staticSwitch193_g139039 = localSamplePlanar3D246_g139049;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch193_g139039 = localSampleStochastic2D234_g139049;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch193_g139039 = localSampleStochastic3D263_g139049;
				#else
				float4 staticSwitch193_g139039 = localSampleMain276_g139049;
				#endif
				half4 Local_NormaTex191_g139039 = staticSwitch193_g139039;
				half4 Normal_Packed45_g139040 = Local_NormaTex191_g139039;
				float2 appendResult58_g139040 = (float2(( (Normal_Packed45_g139040).x * (Normal_Packed45_g139040).w ) , (Normal_Packed45_g139040).y));
				half2 Normal_Default50_g139040 = appendResult58_g139040;
				half2 Normal_ASTC41_g139040 = (Normal_Packed45_g139040).xy;
				#ifdef UNITY_ASTC_NORMALMAP_ENCODING
				float2 staticSwitch38_g139040 = Normal_ASTC41_g139040;
				#else
				float2 staticSwitch38_g139040 = Normal_Default50_g139040;
				#endif
				half2 Normal_NO_DTX544_g139040 = (Normal_Packed45_g139040).wy;
				#ifdef UNITY_NO_DXT5nm
				float2 staticSwitch37_g139040 = Normal_NO_DTX544_g139040;
				#else
				float2 staticSwitch37_g139040 = staticSwitch38_g139040;
				#endif
				float2 temp_output_26_0_g139039 = ( (staticSwitch37_g139040*2.0 + -1.0) * _MainNormalValue );
				float3 ase_worldTangent = i.ase_texcoord7.xyz;
				float3 ase_worldBitangent = i.ase_texcoord8.xyz;
				float3x3 ase_worldToTangent = float3x3(ase_worldTangent,ase_worldBitangent,ase_worldNormal);
				half2 Normal_Planar45_g139041 = temp_output_26_0_g139039;
				float2 break71_g139041 = Normal_Planar45_g139041;
				float3 appendResult72_g139041 = (float3(break71_g139041.x , 0.0 , break71_g139041.y));
				float2 temp_output_205_0_g139039 = (mul( ase_worldToTangent, appendResult72_g139041 )).xy;
				#if defined( TVE_MAIN_SAMPLE_MAIN_UV )
				float2 staticSwitch204_g139039 = temp_output_26_0_g139039;
				#elif defined( TVE_MAIN_SAMPLE_EXTRA_UV )
				float2 staticSwitch204_g139039 = temp_output_26_0_g139039;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_2D )
				float2 staticSwitch204_g139039 = temp_output_205_0_g139039;
				#elif defined( TVE_MAIN_SAMPLE_PLANAR_3D )
				float2 staticSwitch204_g139039 = temp_output_205_0_g139039;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_2D )
				float2 staticSwitch204_g139039 = temp_output_205_0_g139039;
				#elif defined( TVE_MAIN_SAMPLE_STOCHASTIC_3D )
				float2 staticSwitch204_g139039 = temp_output_205_0_g139039;
				#else
				float2 staticSwitch204_g139039 = temp_output_26_0_g139039;
				#endif
				half2 Local_NormalTS108_g139039 = staticSwitch204_g139039;
				float2 In_NormalTS3_g139055 = Local_NormalTS108_g139039;
				float3 appendResult68_g139042 = (float3(Local_NormalTS108_g139039 , 1.0));
				float3 tanToWorld0 = float3( ase_worldTangent.x, ase_worldBitangent.x, ase_worldNormal.x );
				float3 tanToWorld1 = float3( ase_worldTangent.y, ase_worldBitangent.y, ase_worldNormal.y );
				float3 tanToWorld2 = float3( ase_worldTangent.z, ase_worldBitangent.z, ase_worldNormal.z );
				float3 tanNormal74_g139042 = appendResult68_g139042;
				float3 worldNormal74_g139042 = normalize( float3(dot(tanToWorld0,tanNormal74_g139042), dot(tanToWorld1,tanNormal74_g139042), dot(tanToWorld2,tanNormal74_g139042)) );
				half3 Local_NormalWS250_g139039 = worldNormal74_g139042;
				float3 In_NormalWS3_g139055 = Local_NormalWS250_g139039;
				float4 In_Shader3_g139055 = Local_Masks109_g139039;
				float4 In_Emissive3_g139055 = half4(1,1,1,1);
				float3 temp_output_3_0_g139043 = Local_Albedo139_g139039;
				float dotResult20_g139043 = dot( temp_output_3_0_g139043 , float3(0.2126,0.7152,0.0722) );
				half Local_Grayscale110_g139039 = dotResult20_g139043;
				float In_Grayscale3_g139055 = Local_Grayscale110_g139039;
				float clampResult144_g139039 = clamp( saturate( ( Local_Grayscale110_g139039 * 5.0 ) ) , 0.2 , 1.0 );
				half Local_Luminosity145_g139039 = clampResult144_g139039;
				float In_Luminosity3_g139055 = Local_Luminosity145_g139039;
				float In_MultiMask3_g139055 = Local_MultiMask78_g139039;
				float temp_output_187_0_g139039 = (Local_AlbedoTex185_g139039).w;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch236_g139039 = ( temp_output_187_0_g139039 - _MainAlphaClipValue );
				#else
				float staticSwitch236_g139039 = temp_output_187_0_g139039;
				#endif
				half Local_AlphaClip111_g139039 = staticSwitch236_g139039;
				float In_AlphaClip3_g139055 = Local_AlphaClip111_g139039;
				half Local_AlphaFade246_g139039 = (lerpResult62_g139039).a;
				float In_AlphaFade3_g139055 = Local_AlphaFade246_g139039;
				float3 temp_cast_2 = (1.0).xxx;
				float3 In_Translucency3_g139055 = temp_cast_2;
				float In_Transmission3_g139055 = 1.0;
				float In_Thickness3_g139055 = 0.0;
				float In_Diffusion3_g139055 = 0.0;
				Data3_g139055.Dummy = In_Dummy3_g139055;
				Data3_g139055.Albedo = In_Albedo3_g139055;
				Data3_g139055.AlbedoRaw = In_AlbedoRaw3_g139055;
				Data3_g139055.NormalTS = In_NormalTS3_g139055;
				Data3_g139055.NormalWS = In_NormalWS3_g139055;
				Data3_g139055.Shader = In_Shader3_g139055;
				Data3_g139055.Emissive= In_Emissive3_g139055;
				Data3_g139055.MultiMask = In_MultiMask3_g139055;
				Data3_g139055.Grayscale = In_Grayscale3_g139055;
				Data3_g139055.Luminosity = In_Luminosity3_g139055;
				Data3_g139055.AlphaClip = In_AlphaClip3_g139055;
				Data3_g139055.AlphaFade = In_AlphaFade3_g139055;
				Data3_g139055.Translucency = In_Translucency3_g139055;
				Data3_g139055.Transmission = In_Transmission3_g139055;
				Data3_g139055.Thickness = In_Thickness3_g139055;
				Data3_g139055.Diffusion = In_Diffusion3_g139055;
				TVEVisualData DataA25_g139091 = Data3_g139055;
				float localCompData3_g139090 = ( 0.0 );
				TVEVisualData Data3_g139090 = (TVEVisualData)0;
				half4 Dummy946_g139057 = ( ( _LayerCategory + _LayerEnd ) + ( _SecondSampleMode + _SecondCoordMode + _SecondCoordValue ) + ( _SecondMaskSampleMode + _SecondMaskCoordMode + _SecondMaskCoordValue ) + _SecondElementMode + _SecondBakeMode );
				float In_Dummy3_g139090 = Dummy946_g139057.x;
				float localBreakData4_g139069 = ( 0.0 );
				TVEVisualData Data4_g139069 = Data3_g139055;
				float Out_Dummy4_g139069 = 0;
				float3 Out_Albedo4_g139069 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139069 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139069 = float2( 0,0 );
				float3 Out_NormalWS4_g139069 = float3( 0,0,0 );
				float4 Out_Shader4_g139069 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139069 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139069 = 0;
				float Out_Grayscale4_g139069 = 0;
				float Out_Luminosity4_g139069 = 0;
				float Out_AlphaClip4_g139069 = 0;
				float Out_AlphaFade4_g139069 = 0;
				float3 Out_Translucency4_g139069 = float3( 0,0,0 );
				float Out_Transmission4_g139069 = 0;
				float Out_Thickness4_g139069 = 0;
				float Out_Diffusion4_g139069 = 0;
				Out_Dummy4_g139069 = Data4_g139069.Dummy;
				Out_Albedo4_g139069 = Data4_g139069.Albedo;
				Out_AlbedoRaw4_g139069 = Data4_g139069.AlbedoRaw;
				Out_NormalTS4_g139069 = Data4_g139069.NormalTS;
				Out_NormalWS4_g139069 = Data4_g139069.NormalWS;
				Out_Shader4_g139069 = Data4_g139069.Shader;
				Out_Emissive4_g139069= Data4_g139069.Emissive;
				Out_MultiMask4_g139069 = Data4_g139069.MultiMask;
				Out_Grayscale4_g139069 = Data4_g139069.Grayscale;
				Out_Luminosity4_g139069= Data4_g139069.Luminosity;
				Out_AlphaClip4_g139069 = Data4_g139069.AlphaClip;
				Out_AlphaFade4_g139069 = Data4_g139069.AlphaFade;
				Out_Translucency4_g139069 = Data4_g139069.Translucency;
				Out_Transmission4_g139069 = Data4_g139069.Transmission;
				Out_Thickness4_g139069 = Data4_g139069.Thickness;
				Out_Diffusion4_g139069 = Data4_g139069.Diffusion;
				half3 Visual_Albedo527_g139057 = Out_Albedo4_g139069;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139058) = _SecondAlbedoTex;
				float localFilterTexture19_g139086 = ( 0.0 );
				SamplerState SamplerDefault19_g139086 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerPoint19_g139086 = sampler_Point_Repeat;
				SamplerState SamplerLow19_g139086 = sampler_Linear_Repeat;
				SamplerState SamplerMedium19_g139086 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh19_g139086 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS19_g139086 = SamplerDefault19_g139086;
				#if defined (TVE_FILTER_DEFAULT)
				    SS19_g139086 = SamplerDefault19_g139086;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS19_g139086 = SamplerPoint19_g139086;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS19_g139086 = SamplerLow19_g139086;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS19_g139086 = SamplerMedium19_g139086;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS19_g139086 = SamplerHigh19_g139086;
				#endif
				SamplerState Sampler276_g139058 = SS19_g139086;
				half4 Local_LayerCoords790_g139057 = _second_coord_value;
				float4 temp_output_37_0_g139058 = Local_LayerCoords790_g139057;
				half4 Coords276_g139058 = temp_output_37_0_g139058;
				half2 TexCoord276_g139058 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139058 = SampleMain( Texture276_g139058 , Sampler276_g139058 , Coords276_g139058 , TexCoord276_g139058 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139058) = _SecondAlbedoTex;
				SamplerState Sampler275_g139058 = SS19_g139086;
				half4 Coords275_g139058 = temp_output_37_0_g139058;
				half2 TexCoord275_g139058 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139058 = SampleExtra( Texture275_g139058 , Sampler275_g139058 , Coords275_g139058 , TexCoord275_g139058 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139058) = _SecondAlbedoTex;
				SamplerState Sampler238_g139058 = SS19_g139086;
				half4 Coords238_g139058 = temp_output_37_0_g139058;
				TVEModelData Data15_g139089 = Data16_g76840;
				float Out_Dummy15_g139089 = 0;
				float3 Out_PositionWS15_g139089 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139089 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139089 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139089 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139089 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139089 = float3( 0,0,0 );
				float4 Out_VertexData15_g139089 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139089 = float4( 0,0,0,0 );
				Out_Dummy15_g139089 = Data15_g139089.Dummy;
				Out_PositionWS15_g139089 = Data15_g139089.PositionWS;
				Out_PositionWO15_g139089 = Data15_g139089.PositionWO;
				Out_PivotWS15_g139089 = Data15_g139089.PivotWS;
				Out_PivotWO15_g139089 = Data15_g139089.PivotWO;
				Out_NormalWS15_g139089 = Data15_g139089.NormalWS;
				Out_ViewDirWS15_g139089 = Data15_g139089.ViewDirWS;
				Out_VertexData15_g139089 = Data15_g139089.VertexData;
				Out_BoundsData15_g139089 = Data15_g139089.BoundsData;
				half3 Model_PositionWO636_g139057 = Out_PositionWO15_g139089;
				float3 temp_output_279_0_g139058 = Model_PositionWO636_g139057;
				half3 WorldPosition238_g139058 = temp_output_279_0_g139058;
				half4 localSamplePlanar2D238_g139058 = SamplePlanar2D( Texture238_g139058 , Sampler238_g139058 , Coords238_g139058 , WorldPosition238_g139058 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139058) = _SecondAlbedoTex;
				SamplerState Sampler246_g139058 = SS19_g139086;
				half4 Coords246_g139058 = temp_output_37_0_g139058;
				half3 WorldPosition246_g139058 = temp_output_279_0_g139058;
				half3 Model_NormalWS869_g139057 = Out_NormalWS15_g139089;
				float3 temp_output_280_0_g139058 = Model_NormalWS869_g139057;
				half3 WorldNormal246_g139058 = temp_output_280_0_g139058;
				half4 localSamplePlanar3D246_g139058 = SamplePlanar3D( Texture246_g139058 , Sampler246_g139058 , Coords246_g139058 , WorldPosition246_g139058 , WorldNormal246_g139058 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139058) = _SecondAlbedoTex;
				SamplerState Sampler234_g139058 = SS19_g139086;
				float4 Coords234_g139058 = temp_output_37_0_g139058;
				float3 WorldPosition234_g139058 = temp_output_279_0_g139058;
				float4 localSampleStochastic2D234_g139058 = SampleStochastic2D( Texture234_g139058 , Sampler234_g139058 , Coords234_g139058 , WorldPosition234_g139058 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139058) = _SecondAlbedoTex;
				SamplerState Sampler263_g139058 = SS19_g139086;
				half4 Coords263_g139058 = temp_output_37_0_g139058;
				half3 WorldPosition263_g139058 = temp_output_279_0_g139058;
				half3 WorldNormal263_g139058 = temp_output_280_0_g139058;
				half4 localSampleStochastic3D263_g139058 = SampleStochastic3D( Texture263_g139058 , Sampler263_g139058 , Coords263_g139058 , WorldPosition263_g139058 , WorldNormal263_g139058 );
				#if defined( TVE_SECOND_SAMPLE_MAIN_UV )
				float4 staticSwitch693_g139057 = localSampleMain276_g139058;
				#elif defined( TVE_SECOND_SAMPLE_EXTRA_UV )
				float4 staticSwitch693_g139057 = localSampleExtra275_g139058;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_2D )
				float4 staticSwitch693_g139057 = localSamplePlanar2D238_g139058;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_3D )
				float4 staticSwitch693_g139057 = localSamplePlanar3D246_g139058;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch693_g139057 = localSampleStochastic2D234_g139058;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch693_g139057 = localSampleStochastic3D263_g139058;
				#else
				float4 staticSwitch693_g139057 = localSampleMain276_g139058;
				#endif
				half4 Local_AlbedoTex777_g139057 = staticSwitch693_g139057;
				float3 lerpResult716_g139057 = lerp( float3( 1,1,1 ) , (Local_AlbedoTex777_g139057).xyz , _SecondAlbedoValue);
				half3 Local_AlbedoRGB771_g139057 = lerpResult716_g139057;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139060) = _SecondShaderTex;
				float localFilterTexture30_g139088 = ( 0.0 );
				SamplerState SamplerDefault30_g139088 = sampler_Linear_Repeat;
				SamplerState SamplerPoint30_g139088 = sampler_Point_Repeat;
				SamplerState SamplerLow30_g139088 = sampler_Linear_Repeat;
				SamplerState SamplerMedium30_g139088 = sampler_Linear_Repeat;
				SamplerState SamplerHigh30_g139088 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS30_g139088 = SamplerDefault30_g139088;
				#if defined (TVE_FILTER_DEFAULT)
				    SS30_g139088 = SamplerDefault30_g139088;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS30_g139088 = SamplerPoint30_g139088;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS30_g139088 = SamplerLow30_g139088;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS30_g139088 = SamplerMedium30_g139088;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS30_g139088 = SamplerHigh30_g139088;
				#endif
				SamplerState Sampler276_g139060 = SS30_g139088;
				float4 temp_output_37_0_g139060 = Local_LayerCoords790_g139057;
				half4 Coords276_g139060 = temp_output_37_0_g139060;
				half2 TexCoord276_g139060 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139060 = SampleMain( Texture276_g139060 , Sampler276_g139060 , Coords276_g139060 , TexCoord276_g139060 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139060) = _SecondShaderTex;
				SamplerState Sampler275_g139060 = SS30_g139088;
				half4 Coords275_g139060 = temp_output_37_0_g139060;
				half2 TexCoord275_g139060 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139060 = SampleExtra( Texture275_g139060 , Sampler275_g139060 , Coords275_g139060 , TexCoord275_g139060 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139060) = _SecondShaderTex;
				SamplerState Sampler238_g139060 = SS30_g139088;
				half4 Coords238_g139060 = temp_output_37_0_g139060;
				float3 temp_output_279_0_g139060 = Model_PositionWO636_g139057;
				half3 WorldPosition238_g139060 = temp_output_279_0_g139060;
				half4 localSamplePlanar2D238_g139060 = SamplePlanar2D( Texture238_g139060 , Sampler238_g139060 , Coords238_g139060 , WorldPosition238_g139060 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139060) = _SecondShaderTex;
				SamplerState Sampler246_g139060 = SS30_g139088;
				half4 Coords246_g139060 = temp_output_37_0_g139060;
				half3 WorldPosition246_g139060 = temp_output_279_0_g139060;
				float3 temp_output_280_0_g139060 = Model_NormalWS869_g139057;
				half3 WorldNormal246_g139060 = temp_output_280_0_g139060;
				half4 localSamplePlanar3D246_g139060 = SamplePlanar3D( Texture246_g139060 , Sampler246_g139060 , Coords246_g139060 , WorldPosition246_g139060 , WorldNormal246_g139060 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139060) = _SecondShaderTex;
				SamplerState Sampler234_g139060 = SS30_g139088;
				float4 Coords234_g139060 = temp_output_37_0_g139060;
				float3 WorldPosition234_g139060 = temp_output_279_0_g139060;
				float4 localSampleStochastic2D234_g139060 = SampleStochastic2D( Texture234_g139060 , Sampler234_g139060 , Coords234_g139060 , WorldPosition234_g139060 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139060) = _SecondShaderTex;
				SamplerState Sampler263_g139060 = SS30_g139088;
				half4 Coords263_g139060 = temp_output_37_0_g139060;
				half3 WorldPosition263_g139060 = temp_output_279_0_g139060;
				half3 WorldNormal263_g139060 = temp_output_280_0_g139060;
				half4 localSampleStochastic3D263_g139060 = SampleStochastic3D( Texture263_g139060 , Sampler263_g139060 , Coords263_g139060 , WorldPosition263_g139060 , WorldNormal263_g139060 );
				#if defined( TVE_SECOND_SAMPLE_MAIN_UV )
				float4 staticSwitch722_g139057 = localSampleMain276_g139060;
				#elif defined( TVE_SECOND_SAMPLE_EXTRA_UV )
				float4 staticSwitch722_g139057 = localSampleExtra275_g139060;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_2D )
				float4 staticSwitch722_g139057 = localSamplePlanar2D238_g139060;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_3D )
				float4 staticSwitch722_g139057 = localSamplePlanar3D246_g139060;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch722_g139057 = localSampleStochastic2D234_g139060;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch722_g139057 = localSampleStochastic3D263_g139060;
				#else
				float4 staticSwitch722_g139057 = localSampleMain276_g139060;
				#endif
				half4 Local_ShaderTex775_g139057 = staticSwitch722_g139057;
				float lerpResult739_g139057 = lerp( 1.0 , (Local_ShaderTex775_g139057).y , _SecondOcclusionValue);
				float4 appendResult749_g139057 = (float4(( (Local_ShaderTex775_g139057).x * _SecondMetallicValue ) , lerpResult739_g139057 , (Local_ShaderTex775_g139057).z , ( (Local_ShaderTex775_g139057).w * _SecondSmoothnessValue )));
				half4 Local_Masks750_g139057 = appendResult749_g139057;
				float clampResult17_g139064 = clamp( (Local_Masks750_g139057).z , 0.0001 , 0.9999 );
				float temp_output_7_0_g139065 = _SecondMultiRemap.x;
				float temp_output_10_0_g139065 = ( _SecondMultiRemap.y - temp_output_7_0_g139065 );
				float temp_output_765_0_g139057 = saturate( ( ( clampResult17_g139064 - temp_output_7_0_g139065 ) / ( temp_output_10_0_g139065 + 0.0001 ) ) );
				half Local_MultiMask767_g139057 = temp_output_765_0_g139057;
				float lerpResult705_g139057 = lerp( 1.0 , Local_MultiMask767_g139057 , _SecondColorMode);
				float4 lerpResult706_g139057 = lerp( _SecondColorTwo , _SecondColor , lerpResult705_g139057);
				half3 Local_ColorRGB774_g139057 = (lerpResult706_g139057).rgb;
				half3 Local_Albedo768_g139057 = ( Local_AlbedoRGB771_g139057 * Local_ColorRGB774_g139057 );
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g139082 = 2.0;
				#else
				float staticSwitch1_g139082 = 4.594794;
				#endif
				float3 lerpResult985_g139057 = lerp( Local_Albedo768_g139057 , ( Visual_Albedo527_g139057 * Local_Albedo768_g139057 * staticSwitch1_g139082 ) , _SecondBlendAlbedoValue);
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139061) = _SecondMaskTex;
				SamplerState Sampler276_g139061 = sampler_Linear_Repeat;
				half4 Local_BlendCoords813_g139057 = _second_mask_coord_value;
				float4 temp_output_37_0_g139061 = Local_BlendCoords813_g139057;
				half4 Coords276_g139061 = temp_output_37_0_g139061;
				half2 TexCoord276_g139061 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139061 = SampleMain( Texture276_g139061 , Sampler276_g139061 , Coords276_g139061 , TexCoord276_g139061 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139061) = _SecondMaskTex;
				SamplerState Sampler275_g139061 = sampler_Linear_Repeat;
				half4 Coords275_g139061 = temp_output_37_0_g139061;
				half2 TexCoord275_g139061 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139061 = SampleExtra( Texture275_g139061 , Sampler275_g139061 , Coords275_g139061 , TexCoord275_g139061 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139061) = _SecondMaskTex;
				SamplerState Sampler238_g139061 = sampler_Linear_Repeat;
				half4 Coords238_g139061 = temp_output_37_0_g139061;
				float3 temp_output_279_0_g139061 = Model_PositionWO636_g139057;
				half3 WorldPosition238_g139061 = temp_output_279_0_g139061;
				half4 localSamplePlanar2D238_g139061 = SamplePlanar2D( Texture238_g139061 , Sampler238_g139061 , Coords238_g139061 , WorldPosition238_g139061 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139061) = _SecondMaskTex;
				SamplerState Sampler246_g139061 = sampler_Linear_Repeat;
				half4 Coords246_g139061 = temp_output_37_0_g139061;
				half3 WorldPosition246_g139061 = temp_output_279_0_g139061;
				float3 temp_output_280_0_g139061 = Model_NormalWS869_g139057;
				half3 WorldNormal246_g139061 = temp_output_280_0_g139061;
				half4 localSamplePlanar3D246_g139061 = SamplePlanar3D( Texture246_g139061 , Sampler246_g139061 , Coords246_g139061 , WorldPosition246_g139061 , WorldNormal246_g139061 );
				#if defined( TVE_SECOND_MASK_SAMPLE_MAIN_UV )
				float4 staticSwitch817_g139057 = localSampleMain276_g139061;
				#elif defined( TVE_SECOND_MASK_SAMPLE_EXTRA_UV )
				float4 staticSwitch817_g139057 = localSampleExtra275_g139061;
				#elif defined( TVE_SECOND_MASK_SAMPLE_PLANAR_2D )
				float4 staticSwitch817_g139057 = localSamplePlanar2D238_g139061;
				#elif defined( TVE_SECOND_MASK_SAMPLE_PLANAR_3D )
				float4 staticSwitch817_g139057 = localSamplePlanar3D246_g139061;
				#else
				float4 staticSwitch817_g139057 = localSampleMain276_g139061;
				#endif
				half4 Local_MaskTex861_g139057 = staticSwitch817_g139057;
				float clampResult17_g139073 = clamp( (Local_MaskTex861_g139057).z , 0.0001 , 0.9999 );
				float temp_output_7_0_g139072 = _SecondMaskRemap.x;
				float temp_output_10_0_g139072 = ( _SecondMaskRemap.y - temp_output_7_0_g139072 );
				float lerpResult1015_g139057 = lerp( 1.0 , saturate( ( ( clampResult17_g139073 - temp_output_7_0_g139072 ) / ( temp_output_10_0_g139072 + 0.0001 ) ) ) , _SecondMaskValue);
				half Blend_TexMask429_g139057 = lerpResult1015_g139057;
				half3 Visual_NormalWS951_g139057 = Out_NormalWS4_g139069;
				float clampResult17_g139074 = clamp( saturate( (Visual_NormalWS951_g139057).y ) , 0.0001 , 0.9999 );
				float temp_output_7_0_g139075 = _SecondProjRemap.x;
				float temp_output_10_0_g139075 = ( _SecondProjRemap.y - temp_output_7_0_g139075 );
				float lerpResult996_g139057 = lerp( 1.0 , saturate( ( ( clampResult17_g139074 - temp_output_7_0_g139075 ) / ( temp_output_10_0_g139075 + 0.0001 ) ) ) , _SecondProjValue);
				half Blend_ProjMask434_g139057 = lerpResult996_g139057;
				half4 Model_VertexMasks964_g139057 = Out_VertexData15_g139089;
				float4 break965_g139057 = Model_VertexMasks964_g139057;
				float4 break33_g139076 = _second_vert_mode;
				float temp_output_30_0_g139076 = ( break965_g139057.x * break33_g139076.x );
				float temp_output_29_0_g139076 = ( break965_g139057.y * break33_g139076.y );
				float temp_output_31_0_g139076 = ( break965_g139057.z * break33_g139076.z );
				float temp_output_28_0_g139076 = ( temp_output_30_0_g139076 + temp_output_29_0_g139076 + temp_output_31_0_g139076 + ( break965_g139057.w * break33_g139076.w ) );
				float clampResult17_g139070 = clamp( temp_output_28_0_g139076 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139071 = _SecondMeshRemap.x;
				float temp_output_10_0_g139071 = ( _SecondMeshRemap.y - temp_output_7_0_g139071 );
				float lerpResult1017_g139057 = lerp( 1.0 , saturate( ( ( clampResult17_g139070 - temp_output_7_0_g139071 ) / ( temp_output_10_0_g139071 + 0.0001 ) ) ) , _SecondMeshValue);
				float temp_output_6_0_g139084 = lerpResult1017_g139057;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139084 = ( temp_output_6_0_g139084 + ( _SecondMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139084 = temp_output_6_0_g139084;
				#endif
				float temp_output_987_0_g139057 = staticSwitch14_g139084;
				half Blend_VertMask918_g139057 = temp_output_987_0_g139057;
				float localBuildGlobalData374 = ( 0.0 );
				TVEGlobalData Data374 = (TVEGlobalData)0;
				float In_Dummy374 = 0;
				float4 _Vector1 = float4(1,1,1,1);
				float4 In_CoatParams374 = _Vector1;
				float4 In_PaintParams374 = _Vector1;
				float4 In_GlowParams374 = float4( 0,0,0,0 );
				float4 In_AtmoParams374 = _Vector1;
				float4 In_FormParams374 = float4( 0,0,0,0 );
				float4 In_WindParams374 = float4( 0,0,0,0 );
				float4 In_PushParams374 = float4( 0,0,0,0 );
				Data374.Dummy = In_Dummy374;
				Data374.CoatParams = In_CoatParams374;
				Data374.PaintParams = In_PaintParams374;
				Data374.GlowParams = In_GlowParams374;
				Data374.AtmoParams = In_AtmoParams374;
				Data374.FormParams= In_FormParams374;
				Data374.WindParams = In_WindParams374;
				Data374.PushParams = In_PushParams374;
				TVEGlobalData Data15_g139068 = Data374;
				float Out_Dummy15_g139068 = 0;
				float4 Out_CoatParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139068 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139068 = float4( 0,0,0,0 );
				Out_Dummy15_g139068 = Data15_g139068.Dummy;
				Out_CoatParams15_g139068 = Data15_g139068.CoatParams;
				Out_PaintParams15_g139068 = Data15_g139068.PaintParams;
				Out_GlowParams15_g139068 = Data15_g139068.GlowParams;
				Out_AtmoParams15_g139068= Data15_g139068.AtmoParams;
				Out_FadeParams15_g139068= Data15_g139068.FadeParams;
				Out_FormParams15_g139068 = Data15_g139068.FormParams;
				Out_LandParams15_g139068 = Data15_g139068.LandParams;
				Out_WindParams15_g139068 = Data15_g139068.WindParams;
				Out_PushParams15_g139068 = Data15_g139068.PushParams;
				half4 Global_CoatParams975_g139057 = Out_CoatParams15_g139068;
				float lerpResult1013_g139057 = lerp( 1.0 , (Global_CoatParams975_g139057).z , TVE_IsEnabled);
				#ifdef TVE_SECOND_ELEMENT
				float staticSwitch971_g139057 = lerpResult1013_g139057;
				#else
				float staticSwitch971_g139057 = 1.0;
				#endif
				half Blend_GlobalMask972_g139057 = staticSwitch971_g139057;
				float temp_output_432_0_g139057 = ( _SecondIntensityValue * Blend_TexMask429_g139057 * Blend_ProjMask434_g139057 * Blend_VertMask918_g139057 * Blend_GlobalMask972_g139057 );
				float clampResult17_g139078 = clamp( temp_output_432_0_g139057 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139077 = _SecondBlendRemap.x;
				float temp_output_10_0_g139077 = ( _SecondBlendRemap.y - temp_output_7_0_g139077 );
				half Blend_Mask412_g139057 = ( saturate( ( ( clampResult17_g139078 - temp_output_7_0_g139077 ) / ( temp_output_10_0_g139077 + 0.0001 ) ) ) * _SecondBlendIntensityValue );
				float3 lerpResult403_g139057 = lerp( Visual_Albedo527_g139057 , lerpResult985_g139057 , Blend_Mask412_g139057);
				#ifdef TVE_SECOND
				float3 staticSwitch415_g139057 = lerpResult403_g139057;
				#else
				float3 staticSwitch415_g139057 = Visual_Albedo527_g139057;
				#endif
				half3 Final_Albedo601_g139057 = staticSwitch415_g139057;
				float3 In_Albedo3_g139090 = Final_Albedo601_g139057;
				float3 In_AlbedoRaw3_g139090 = Final_Albedo601_g139057;
				half2 Visual_NormalTS529_g139057 = Out_NormalTS4_g139069;
				float2 lerpResult40_g139080 = lerp( float2( 0,0 ) , Visual_NormalTS529_g139057 , _SecondBlendNormalValue);
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139059) = _SecondNormalTex;
				float localFilterTexture29_g139087 = ( 0.0 );
				SamplerState SamplerDefault29_g139087 = sampler_Linear_Repeat;
				SamplerState SamplerPoint29_g139087 = sampler_Point_Repeat;
				SamplerState SamplerLow29_g139087 = sampler_Linear_Repeat;
				SamplerState SamplerMedium29_g139087 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh29_g139087 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS29_g139087 = SamplerDefault29_g139087;
				#if defined (TVE_FILTER_DEFAULT)
				    SS29_g139087 = SamplerDefault29_g139087;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS29_g139087 = SamplerPoint29_g139087;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS29_g139087 = SamplerLow29_g139087;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS29_g139087 = SamplerMedium29_g139087;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS29_g139087 = SamplerHigh29_g139087;
				#endif
				SamplerState Sampler276_g139059 = SS29_g139087;
				float4 temp_output_37_0_g139059 = Local_LayerCoords790_g139057;
				half4 Coords276_g139059 = temp_output_37_0_g139059;
				half2 TexCoord276_g139059 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139059 = SampleMain( Texture276_g139059 , Sampler276_g139059 , Coords276_g139059 , TexCoord276_g139059 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139059) = _SecondNormalTex;
				SamplerState Sampler275_g139059 = SS29_g139087;
				half4 Coords275_g139059 = temp_output_37_0_g139059;
				half2 TexCoord275_g139059 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139059 = SampleExtra( Texture275_g139059 , Sampler275_g139059 , Coords275_g139059 , TexCoord275_g139059 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139059) = _SecondNormalTex;
				SamplerState Sampler238_g139059 = SS29_g139087;
				half4 Coords238_g139059 = temp_output_37_0_g139059;
				float3 temp_output_279_0_g139059 = Model_PositionWO636_g139057;
				half3 WorldPosition238_g139059 = temp_output_279_0_g139059;
				half4 localSamplePlanar2D238_g139059 = SamplePlanar2D( Texture238_g139059 , Sampler238_g139059 , Coords238_g139059 , WorldPosition238_g139059 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139059) = _SecondNormalTex;
				SamplerState Sampler246_g139059 = SS29_g139087;
				half4 Coords246_g139059 = temp_output_37_0_g139059;
				half3 WorldPosition246_g139059 = temp_output_279_0_g139059;
				float3 temp_output_280_0_g139059 = Model_NormalWS869_g139057;
				half3 WorldNormal246_g139059 = temp_output_280_0_g139059;
				half4 localSamplePlanar3D246_g139059 = SamplePlanar3D( Texture246_g139059 , Sampler246_g139059 , Coords246_g139059 , WorldPosition246_g139059 , WorldNormal246_g139059 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139059) = _SecondNormalTex;
				SamplerState Sampler234_g139059 = SS29_g139087;
				float4 Coords234_g139059 = temp_output_37_0_g139059;
				float3 WorldPosition234_g139059 = temp_output_279_0_g139059;
				float4 localSampleStochastic2D234_g139059 = SampleStochastic2D( Texture234_g139059 , Sampler234_g139059 , Coords234_g139059 , WorldPosition234_g139059 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139059) = _SecondNormalTex;
				SamplerState Sampler263_g139059 = SS29_g139087;
				half4 Coords263_g139059 = temp_output_37_0_g139059;
				half3 WorldPosition263_g139059 = temp_output_279_0_g139059;
				half3 WorldNormal263_g139059 = temp_output_280_0_g139059;
				half4 localSampleStochastic3D263_g139059 = SampleStochastic3D( Texture263_g139059 , Sampler263_g139059 , Coords263_g139059 , WorldPosition263_g139059 , WorldNormal263_g139059 );
				#if defined( TVE_SECOND_SAMPLE_MAIN_UV )
				float4 staticSwitch698_g139057 = localSampleMain276_g139059;
				#elif defined( TVE_SECOND_SAMPLE_EXTRA_UV )
				float4 staticSwitch698_g139057 = localSampleExtra275_g139059;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_2D )
				float4 staticSwitch698_g139057 = localSamplePlanar2D238_g139059;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_3D )
				float4 staticSwitch698_g139057 = localSamplePlanar3D246_g139059;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch698_g139057 = localSampleStochastic2D234_g139059;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch698_g139057 = localSampleStochastic3D263_g139059;
				#else
				float4 staticSwitch698_g139057 = localSampleMain276_g139059;
				#endif
				half4 Local_NormalTex776_g139057 = staticSwitch698_g139057;
				half4 Normal_Packed45_g139062 = Local_NormalTex776_g139057;
				float2 appendResult58_g139062 = (float2(( (Normal_Packed45_g139062).x * (Normal_Packed45_g139062).w ) , (Normal_Packed45_g139062).y));
				half2 Normal_Default50_g139062 = appendResult58_g139062;
				half2 Normal_ASTC41_g139062 = (Normal_Packed45_g139062).xy;
				#ifdef UNITY_ASTC_NORMALMAP_ENCODING
				float2 staticSwitch38_g139062 = Normal_ASTC41_g139062;
				#else
				float2 staticSwitch38_g139062 = Normal_Default50_g139062;
				#endif
				half2 Normal_NO_DTX544_g139062 = (Normal_Packed45_g139062).wy;
				#ifdef UNITY_NO_DXT5nm
				float2 staticSwitch37_g139062 = Normal_NO_DTX544_g139062;
				#else
				float2 staticSwitch37_g139062 = staticSwitch38_g139062;
				#endif
				float2 temp_output_724_0_g139057 = ( (staticSwitch37_g139062*2.0 + -1.0) * _SecondNormalValue );
				half2 Normal_Planar45_g139063 = temp_output_724_0_g139057;
				float2 break71_g139063 = Normal_Planar45_g139063;
				float3 appendResult72_g139063 = (float3(break71_g139063.x , 0.0 , break71_g139063.y));
				float2 temp_output_858_0_g139057 = (mul( ase_worldToTangent, appendResult72_g139063 )).xy;
				#if defined( TVE_SECOND_SAMPLE_MAIN_UV )
				float2 staticSwitch727_g139057 = temp_output_724_0_g139057;
				#elif defined( TVE_SECOND_SAMPLE_EXTRA_UV )
				float2 staticSwitch727_g139057 = temp_output_724_0_g139057;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_2D )
				float2 staticSwitch727_g139057 = temp_output_858_0_g139057;
				#elif defined( TVE_SECOND_SAMPLE_PLANAR_3D )
				float2 staticSwitch727_g139057 = temp_output_858_0_g139057;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_2D )
				float2 staticSwitch727_g139057 = temp_output_858_0_g139057;
				#elif defined( TVE_SECOND_SAMPLE_STOCHASTIC_3D )
				float2 staticSwitch727_g139057 = temp_output_858_0_g139057;
				#else
				float2 staticSwitch727_g139057 = temp_output_724_0_g139057;
				#endif
				half2 Local_NormalTS729_g139057 = staticSwitch727_g139057;
				float2 temp_output_36_0_g139080 = ( lerpResult40_g139080 + Local_NormalTS729_g139057 );
				float2 lerpResult405_g139057 = lerp( Visual_NormalTS529_g139057 , temp_output_36_0_g139080 , Blend_Mask412_g139057);
				#ifdef TVE_SECOND
				float2 staticSwitch418_g139057 = lerpResult405_g139057;
				#else
				float2 staticSwitch418_g139057 = Visual_NormalTS529_g139057;
				#endif
				half2 Final_NormalTS612_g139057 = staticSwitch418_g139057;
				float2 In_NormalTS3_g139090 = Final_NormalTS612_g139057;
				float3 appendResult68_g139081 = (float3(Final_NormalTS612_g139057 , 1.0));
				float3 tanNormal74_g139081 = appendResult68_g139081;
				float3 worldNormal74_g139081 = normalize( float3(dot(tanToWorld0,tanNormal74_g139081), dot(tanToWorld1,tanNormal74_g139081), dot(tanToWorld2,tanNormal74_g139081)) );
				half3 Final_NormalWS950_g139057 = worldNormal74_g139081;
				float3 In_NormalWS3_g139090 = Final_NormalWS950_g139057;
				half4 Visual_Shader531_g139057 = Out_Shader4_g139069;
				float4 lerpResult994_g139057 = lerp( Local_Masks750_g139057 , ( Visual_Shader531_g139057 * Local_Masks750_g139057 ) , _SecondBlendShaderValue);
				float4 lerpResult440_g139057 = lerp( Visual_Shader531_g139057 , lerpResult994_g139057 , Blend_Mask412_g139057);
				#ifdef TVE_SECOND
				float4 staticSwitch451_g139057 = lerpResult440_g139057;
				#else
				float4 staticSwitch451_g139057 = Visual_Shader531_g139057;
				#endif
				half4 Final_Masks613_g139057 = staticSwitch451_g139057;
				float4 In_Shader3_g139090 = Final_Masks613_g139057;
				float4 In_Emissive3_g139090 = Out_Emissive4_g139069;
				float3 temp_output_3_0_g139083 = Final_Albedo601_g139057;
				float dotResult20_g139083 = dot( temp_output_3_0_g139083 , float3(0.2126,0.7152,0.0722) );
				half Final_Grayscale615_g139057 = dotResult20_g139083;
				float In_Grayscale3_g139090 = Final_Grayscale615_g139057;
				float clampResult651_g139057 = clamp( saturate( ( Final_Grayscale615_g139057 * 5.0 ) ) , 0.2 , 1.0 );
				half Final_Luminosity652_g139057 = clampResult651_g139057;
				float In_Luminosity3_g139090 = Final_Luminosity652_g139057;
				half Visual_MultiMask547_g139057 = Out_MultiMask4_g139069;
				float lerpResult477_g139057 = lerp( Visual_MultiMask547_g139057 , Local_MultiMask767_g139057 , Blend_Mask412_g139057);
				#ifdef TVE_SECOND
				float staticSwitch482_g139057 = lerpResult477_g139057;
				#else
				float staticSwitch482_g139057 = Visual_MultiMask547_g139057;
				#endif
				half Final_MultiMask572_g139057 = staticSwitch482_g139057;
				float In_MultiMask3_g139090 = Final_MultiMask572_g139057;
				half Visual_AlphaClip559_g139057 = Out_AlphaClip4_g139069;
				float temp_output_718_0_g139057 = (Local_AlbedoTex777_g139057).w;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch932_g139057 = ( temp_output_718_0_g139057 - _SecondAlphaClipValue );
				#else
				float staticSwitch932_g139057 = temp_output_718_0_g139057;
				#endif
				half Local_AlphaClip772_g139057 = staticSwitch932_g139057;
				float lerpResult448_g139057 = lerp( Visual_AlphaClip559_g139057 , Local_AlphaClip772_g139057 , Blend_Mask412_g139057);
				#ifdef TVE_SECOND
				float staticSwitch564_g139057 = lerpResult448_g139057;
				#else
				float staticSwitch564_g139057 = Visual_AlphaClip559_g139057;
				#endif
				half Final_AlphaClip602_g139057 = staticSwitch564_g139057;
				float In_AlphaClip3_g139090 = Final_AlphaClip602_g139057;
				half Visual_AlphaFade588_g139057 = Out_AlphaFade4_g139069;
				half Local_AlphaFade773_g139057 = (lerpResult706_g139057).a;
				float lerpResult604_g139057 = lerp( Visual_AlphaFade588_g139057 , Local_AlphaFade773_g139057 , Blend_Mask412_g139057);
				#ifdef TVE_SECOND
				float staticSwitch608_g139057 = lerpResult604_g139057;
				#else
				float staticSwitch608_g139057 = Visual_AlphaFade588_g139057;
				#endif
				half Final_AlphaFade611_g139057 = staticSwitch608_g139057;
				float In_AlphaFade3_g139090 = Final_AlphaFade611_g139057;
				float3 In_Translucency3_g139090 = Out_Translucency4_g139069;
				float In_Transmission3_g139090 = Out_Transmission4_g139069;
				float In_Thickness3_g139090 = Out_Thickness4_g139069;
				float In_Diffusion3_g139090 = Out_Diffusion4_g139069;
				Data3_g139090.Dummy = In_Dummy3_g139090;
				Data3_g139090.Albedo = In_Albedo3_g139090;
				Data3_g139090.AlbedoRaw = In_AlbedoRaw3_g139090;
				Data3_g139090.NormalTS = In_NormalTS3_g139090;
				Data3_g139090.NormalWS = In_NormalWS3_g139090;
				Data3_g139090.Shader = In_Shader3_g139090;
				Data3_g139090.Emissive= In_Emissive3_g139090;
				Data3_g139090.MultiMask = In_MultiMask3_g139090;
				Data3_g139090.Grayscale = In_Grayscale3_g139090;
				Data3_g139090.Luminosity = In_Luminosity3_g139090;
				Data3_g139090.AlphaClip = In_AlphaClip3_g139090;
				Data3_g139090.AlphaFade = In_AlphaFade3_g139090;
				Data3_g139090.Translucency = In_Translucency3_g139090;
				Data3_g139090.Transmission = In_Transmission3_g139090;
				Data3_g139090.Thickness = In_Thickness3_g139090;
				Data3_g139090.Diffusion = In_Diffusion3_g139090;
				TVEVisualData DataB25_g139091 = Data3_g139090;
				float Alpha25_g139091 = _SecondBakeMode;
				if (Alpha25_g139091 < 0.5 )
				{
				Data25_g139091 = DataA25_g139091;
				}
				else
				{
				Data25_g139091 = DataB25_g139091;
				}
				TVEVisualData DataA25_g139125 = Data25_g139091;
				float localCompData3_g139109 = ( 0.0 );
				TVEVisualData Data3_g139109 = (TVEVisualData)0;
				half4 Dummy944_g139092 = ( ( _DetailCategory + _DetailEnd ) + ( _ThirdSampleMode + _ThirdCoordMode + _ThirdCoordValue ) + ( _ThirdMaskSampleMode + _ThirdMaskCoordMode + _ThirdMaskCoordValue ) + _ThirdElementMode + _ThirdBakeMode );
				float In_Dummy3_g139109 = Dummy944_g139092.x;
				float localBreakData4_g139108 = ( 0.0 );
				TVEVisualData Data4_g139108 = Data25_g139091;
				float Out_Dummy4_g139108 = 0;
				float3 Out_Albedo4_g139108 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139108 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139108 = float2( 0,0 );
				float3 Out_NormalWS4_g139108 = float3( 0,0,0 );
				float4 Out_Shader4_g139108 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139108 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139108 = 0;
				float Out_Grayscale4_g139108 = 0;
				float Out_Luminosity4_g139108 = 0;
				float Out_AlphaClip4_g139108 = 0;
				float Out_AlphaFade4_g139108 = 0;
				float3 Out_Translucency4_g139108 = float3( 0,0,0 );
				float Out_Transmission4_g139108 = 0;
				float Out_Thickness4_g139108 = 0;
				float Out_Diffusion4_g139108 = 0;
				Out_Dummy4_g139108 = Data4_g139108.Dummy;
				Out_Albedo4_g139108 = Data4_g139108.Albedo;
				Out_AlbedoRaw4_g139108 = Data4_g139108.AlbedoRaw;
				Out_NormalTS4_g139108 = Data4_g139108.NormalTS;
				Out_NormalWS4_g139108 = Data4_g139108.NormalWS;
				Out_Shader4_g139108 = Data4_g139108.Shader;
				Out_Emissive4_g139108= Data4_g139108.Emissive;
				Out_MultiMask4_g139108 = Data4_g139108.MultiMask;
				Out_Grayscale4_g139108 = Data4_g139108.Grayscale;
				Out_Luminosity4_g139108= Data4_g139108.Luminosity;
				Out_AlphaClip4_g139108 = Data4_g139108.AlphaClip;
				Out_AlphaFade4_g139108 = Data4_g139108.AlphaFade;
				Out_Translucency4_g139108 = Data4_g139108.Translucency;
				Out_Transmission4_g139108 = Data4_g139108.Transmission;
				Out_Thickness4_g139108 = Data4_g139108.Thickness;
				Out_Diffusion4_g139108 = Data4_g139108.Diffusion;
				half3 Visual_Albedo527_g139092 = Out_Albedo4_g139108;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139093) = _ThirdAlbedoTex;
				float localFilterTexture19_g139121 = ( 0.0 );
				SamplerState SamplerDefault19_g139121 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerPoint19_g139121 = sampler_Point_Repeat;
				SamplerState SamplerLow19_g139121 = sampler_Linear_Repeat;
				SamplerState SamplerMedium19_g139121 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh19_g139121 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS19_g139121 = SamplerDefault19_g139121;
				#if defined (TVE_FILTER_DEFAULT)
				    SS19_g139121 = SamplerDefault19_g139121;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS19_g139121 = SamplerPoint19_g139121;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS19_g139121 = SamplerLow19_g139121;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS19_g139121 = SamplerMedium19_g139121;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS19_g139121 = SamplerHigh19_g139121;
				#endif
				SamplerState Sampler276_g139093 = SS19_g139121;
				half4 Local_LayerCoords790_g139092 = _third_coord_value;
				float4 temp_output_37_0_g139093 = Local_LayerCoords790_g139092;
				half4 Coords276_g139093 = temp_output_37_0_g139093;
				half2 TexCoord276_g139093 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139093 = SampleMain( Texture276_g139093 , Sampler276_g139093 , Coords276_g139093 , TexCoord276_g139093 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139093) = _ThirdAlbedoTex;
				SamplerState Sampler275_g139093 = SS19_g139121;
				half4 Coords275_g139093 = temp_output_37_0_g139093;
				half2 TexCoord275_g139093 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139093 = SampleExtra( Texture275_g139093 , Sampler275_g139093 , Coords275_g139093 , TexCoord275_g139093 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139093) = _ThirdAlbedoTex;
				SamplerState Sampler238_g139093 = SS19_g139121;
				half4 Coords238_g139093 = temp_output_37_0_g139093;
				TVEModelData Data15_g139124 = Data16_g76840;
				float Out_Dummy15_g139124 = 0;
				float3 Out_PositionWS15_g139124 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139124 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139124 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139124 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139124 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139124 = float3( 0,0,0 );
				float4 Out_VertexData15_g139124 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139124 = float4( 0,0,0,0 );
				Out_Dummy15_g139124 = Data15_g139124.Dummy;
				Out_PositionWS15_g139124 = Data15_g139124.PositionWS;
				Out_PositionWO15_g139124 = Data15_g139124.PositionWO;
				Out_PivotWS15_g139124 = Data15_g139124.PivotWS;
				Out_PivotWO15_g139124 = Data15_g139124.PivotWO;
				Out_NormalWS15_g139124 = Data15_g139124.NormalWS;
				Out_ViewDirWS15_g139124 = Data15_g139124.ViewDirWS;
				Out_VertexData15_g139124 = Data15_g139124.VertexData;
				Out_BoundsData15_g139124 = Data15_g139124.BoundsData;
				half3 Model_PositionWO636_g139092 = Out_PositionWO15_g139124;
				float3 temp_output_279_0_g139093 = Model_PositionWO636_g139092;
				half3 WorldPosition238_g139093 = temp_output_279_0_g139093;
				half4 localSamplePlanar2D238_g139093 = SamplePlanar2D( Texture238_g139093 , Sampler238_g139093 , Coords238_g139093 , WorldPosition238_g139093 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139093) = _ThirdAlbedoTex;
				SamplerState Sampler246_g139093 = SS19_g139121;
				half4 Coords246_g139093 = temp_output_37_0_g139093;
				half3 WorldPosition246_g139093 = temp_output_279_0_g139093;
				half3 Model_NormalWS869_g139092 = Out_NormalWS15_g139124;
				float3 temp_output_280_0_g139093 = Model_NormalWS869_g139092;
				half3 WorldNormal246_g139093 = temp_output_280_0_g139093;
				half4 localSamplePlanar3D246_g139093 = SamplePlanar3D( Texture246_g139093 , Sampler246_g139093 , Coords246_g139093 , WorldPosition246_g139093 , WorldNormal246_g139093 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139093) = _ThirdAlbedoTex;
				SamplerState Sampler234_g139093 = SS19_g139121;
				float4 Coords234_g139093 = temp_output_37_0_g139093;
				float3 WorldPosition234_g139093 = temp_output_279_0_g139093;
				float4 localSampleStochastic2D234_g139093 = SampleStochastic2D( Texture234_g139093 , Sampler234_g139093 , Coords234_g139093 , WorldPosition234_g139093 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139093) = _ThirdAlbedoTex;
				SamplerState Sampler263_g139093 = SS19_g139121;
				half4 Coords263_g139093 = temp_output_37_0_g139093;
				half3 WorldPosition263_g139093 = temp_output_279_0_g139093;
				half3 WorldNormal263_g139093 = temp_output_280_0_g139093;
				half4 localSampleStochastic3D263_g139093 = SampleStochastic3D( Texture263_g139093 , Sampler263_g139093 , Coords263_g139093 , WorldPosition263_g139093 , WorldNormal263_g139093 );
				#if defined( TVE_THIRD_SAMPLE_MAIN_UV )
				float4 staticSwitch693_g139092 = localSampleMain276_g139093;
				#elif defined( TVE_THIRD_SAMPLE_EXTRA_UV )
				float4 staticSwitch693_g139092 = localSampleExtra275_g139093;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_2D )
				float4 staticSwitch693_g139092 = localSamplePlanar2D238_g139093;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_3D )
				float4 staticSwitch693_g139092 = localSamplePlanar3D246_g139093;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch693_g139092 = localSampleStochastic2D234_g139093;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch693_g139092 = localSampleStochastic3D263_g139093;
				#else
				float4 staticSwitch693_g139092 = localSampleMain276_g139093;
				#endif
				half4 Local_AlbedoTex777_g139092 = staticSwitch693_g139092;
				float3 lerpResult716_g139092 = lerp( float3( 1,1,1 ) , (Local_AlbedoTex777_g139092).xyz , _ThirdAlbedoValue);
				half3 Local_AlbedoRGB771_g139092 = lerpResult716_g139092;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139095) = _ThirdShaderTex;
				float localFilterTexture30_g139123 = ( 0.0 );
				SamplerState SamplerDefault30_g139123 = sampler_Linear_Repeat;
				SamplerState SamplerPoint30_g139123 = sampler_Point_Repeat;
				SamplerState SamplerLow30_g139123 = sampler_Linear_Repeat;
				SamplerState SamplerMedium30_g139123 = sampler_Linear_Repeat;
				SamplerState SamplerHigh30_g139123 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS30_g139123 = SamplerDefault30_g139123;
				#if defined (TVE_FILTER_DEFAULT)
				    SS30_g139123 = SamplerDefault30_g139123;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS30_g139123 = SamplerPoint30_g139123;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS30_g139123 = SamplerLow30_g139123;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS30_g139123 = SamplerMedium30_g139123;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS30_g139123 = SamplerHigh30_g139123;
				#endif
				SamplerState Sampler276_g139095 = SS30_g139123;
				float4 temp_output_37_0_g139095 = Local_LayerCoords790_g139092;
				half4 Coords276_g139095 = temp_output_37_0_g139095;
				half2 TexCoord276_g139095 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139095 = SampleMain( Texture276_g139095 , Sampler276_g139095 , Coords276_g139095 , TexCoord276_g139095 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139095) = _ThirdShaderTex;
				SamplerState Sampler275_g139095 = SS30_g139123;
				half4 Coords275_g139095 = temp_output_37_0_g139095;
				half2 TexCoord275_g139095 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139095 = SampleExtra( Texture275_g139095 , Sampler275_g139095 , Coords275_g139095 , TexCoord275_g139095 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139095) = _ThirdShaderTex;
				SamplerState Sampler238_g139095 = SS30_g139123;
				half4 Coords238_g139095 = temp_output_37_0_g139095;
				float3 temp_output_279_0_g139095 = Model_PositionWO636_g139092;
				half3 WorldPosition238_g139095 = temp_output_279_0_g139095;
				half4 localSamplePlanar2D238_g139095 = SamplePlanar2D( Texture238_g139095 , Sampler238_g139095 , Coords238_g139095 , WorldPosition238_g139095 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139095) = _ThirdShaderTex;
				SamplerState Sampler246_g139095 = SS30_g139123;
				half4 Coords246_g139095 = temp_output_37_0_g139095;
				half3 WorldPosition246_g139095 = temp_output_279_0_g139095;
				float3 temp_output_280_0_g139095 = Model_NormalWS869_g139092;
				half3 WorldNormal246_g139095 = temp_output_280_0_g139095;
				half4 localSamplePlanar3D246_g139095 = SamplePlanar3D( Texture246_g139095 , Sampler246_g139095 , Coords246_g139095 , WorldPosition246_g139095 , WorldNormal246_g139095 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139095) = _ThirdShaderTex;
				SamplerState Sampler234_g139095 = SS30_g139123;
				float4 Coords234_g139095 = temp_output_37_0_g139095;
				float3 WorldPosition234_g139095 = temp_output_279_0_g139095;
				float4 localSampleStochastic2D234_g139095 = SampleStochastic2D( Texture234_g139095 , Sampler234_g139095 , Coords234_g139095 , WorldPosition234_g139095 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139095) = _ThirdShaderTex;
				SamplerState Sampler263_g139095 = SS30_g139123;
				half4 Coords263_g139095 = temp_output_37_0_g139095;
				half3 WorldPosition263_g139095 = temp_output_279_0_g139095;
				half3 WorldNormal263_g139095 = temp_output_280_0_g139095;
				half4 localSampleStochastic3D263_g139095 = SampleStochastic3D( Texture263_g139095 , Sampler263_g139095 , Coords263_g139095 , WorldPosition263_g139095 , WorldNormal263_g139095 );
				#if defined( TVE_THIRD_SAMPLE_MAIN_UV )
				float4 staticSwitch722_g139092 = localSampleMain276_g139095;
				#elif defined( TVE_THIRD_SAMPLE_EXTRA_UV )
				float4 staticSwitch722_g139092 = localSampleExtra275_g139095;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_2D )
				float4 staticSwitch722_g139092 = localSamplePlanar2D238_g139095;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_3D )
				float4 staticSwitch722_g139092 = localSamplePlanar3D246_g139095;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch722_g139092 = localSampleStochastic2D234_g139095;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch722_g139092 = localSampleStochastic3D263_g139095;
				#else
				float4 staticSwitch722_g139092 = localSampleMain276_g139095;
				#endif
				half4 Local_ShaderTex775_g139092 = staticSwitch722_g139092;
				float lerpResult739_g139092 = lerp( 1.0 , (Local_ShaderTex775_g139092).y , _ThirdOcclusionValue);
				float4 appendResult749_g139092 = (float4(( (Local_ShaderTex775_g139092).x * _ThirdMetallicValue ) , lerpResult739_g139092 , (Local_ShaderTex775_g139092).z , ( (Local_ShaderTex775_g139092).w * _ThirdSmoothnessValue )));
				half4 Local_Masks750_g139092 = appendResult749_g139092;
				float clampResult17_g139098 = clamp( (Local_Masks750_g139092).z , 0.0001 , 0.9999 );
				float temp_output_7_0_g139099 = _ThirdMultiRemap.x;
				float temp_output_10_0_g139099 = ( _ThirdMultiRemap.y - temp_output_7_0_g139099 );
				float temp_output_765_0_g139092 = saturate( ( ( clampResult17_g139098 - temp_output_7_0_g139099 ) / ( temp_output_10_0_g139099 + 0.0001 ) ) );
				half Local_MultiMask767_g139092 = temp_output_765_0_g139092;
				float lerpResult705_g139092 = lerp( 1.0 , Local_MultiMask767_g139092 , _ThirdColorMode);
				float4 lerpResult706_g139092 = lerp( _ThirdColorTwo , _ThirdColor , lerpResult705_g139092);
				half3 Local_ColorRGB774_g139092 = (lerpResult706_g139092).rgb;
				half3 Local_Albedo768_g139092 = ( Local_AlbedoRGB771_g139092 * Local_ColorRGB774_g139092 );
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g139116 = 2.0;
				#else
				float staticSwitch1_g139116 = 4.594794;
				#endif
				float3 lerpResult985_g139092 = lerp( Local_Albedo768_g139092 , ( Visual_Albedo527_g139092 * Local_Albedo768_g139092 * staticSwitch1_g139116 ) , _ThirdBlendAlbedoValue);
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139119) = _ThirdMaskTex;
				SamplerState Sampler276_g139119 = sampler_Linear_Repeat;
				half4 Local_MaskCoords813_g139092 = _third_mask_coord_value;
				float4 temp_output_37_0_g139119 = Local_MaskCoords813_g139092;
				half4 Coords276_g139119 = temp_output_37_0_g139119;
				half2 TexCoord276_g139119 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139119 = SampleMain( Texture276_g139119 , Sampler276_g139119 , Coords276_g139119 , TexCoord276_g139119 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139119) = _ThirdMaskTex;
				SamplerState Sampler275_g139119 = sampler_Linear_Repeat;
				half4 Coords275_g139119 = temp_output_37_0_g139119;
				half2 TexCoord275_g139119 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139119 = SampleExtra( Texture275_g139119 , Sampler275_g139119 , Coords275_g139119 , TexCoord275_g139119 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139119) = _ThirdMaskTex;
				SamplerState Sampler238_g139119 = sampler_Linear_Repeat;
				half4 Coords238_g139119 = temp_output_37_0_g139119;
				float3 temp_output_279_0_g139119 = Model_PositionWO636_g139092;
				half3 WorldPosition238_g139119 = temp_output_279_0_g139119;
				half4 localSamplePlanar2D238_g139119 = SamplePlanar2D( Texture238_g139119 , Sampler238_g139119 , Coords238_g139119 , WorldPosition238_g139119 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139119) = _ThirdMaskTex;
				SamplerState Sampler246_g139119 = sampler_Linear_Repeat;
				half4 Coords246_g139119 = temp_output_37_0_g139119;
				half3 WorldPosition246_g139119 = temp_output_279_0_g139119;
				float3 temp_output_280_0_g139119 = Model_NormalWS869_g139092;
				half3 WorldNormal246_g139119 = temp_output_280_0_g139119;
				half4 localSamplePlanar3D246_g139119 = SamplePlanar3D( Texture246_g139119 , Sampler246_g139119 , Coords246_g139119 , WorldPosition246_g139119 , WorldNormal246_g139119 );
				#if defined( TVE_THIRD_MASK_SAMPLE_MAIN_UV )
				float4 staticSwitch817_g139092 = localSampleMain276_g139119;
				#elif defined( TVE_THIRD_MASK_SAMPLE_EXTRA_UV )
				float4 staticSwitch817_g139092 = localSampleExtra275_g139119;
				#elif defined( TVE_THIRD_MASK_SAMPLE_PLANAR_2D )
				float4 staticSwitch817_g139092 = localSamplePlanar2D238_g139119;
				#elif defined( TVE_THIRD_MASK_SAMPLE_PLANAR_3D )
				float4 staticSwitch817_g139092 = localSamplePlanar3D246_g139119;
				#else
				float4 staticSwitch817_g139092 = localSampleMain276_g139119;
				#endif
				half4 Local_MaskTex861_g139092 = staticSwitch817_g139092;
				float clampResult17_g139103 = clamp( (Local_MaskTex861_g139092).y , 0.0001 , 0.9999 );
				float temp_output_7_0_g139104 = _ThirdMaskRemap.x;
				float temp_output_10_0_g139104 = ( _ThirdMaskRemap.y - temp_output_7_0_g139104 );
				float lerpResult1028_g139092 = lerp( 1.0 , saturate( ( ( clampResult17_g139103 - temp_output_7_0_g139104 ) / ( temp_output_10_0_g139104 + 0.0001 ) ) ) , _ThirdMaskValue);
				half Detail_TexMask429_g139092 = lerpResult1028_g139092;
				half4 Model_VertexMasks960_g139092 = Out_VertexData15_g139124;
				float4 break961_g139092 = Model_VertexMasks960_g139092;
				float4 break33_g139107 = _third_vert_mode;
				float temp_output_30_0_g139107 = ( break961_g139092.x * break33_g139107.x );
				float temp_output_29_0_g139107 = ( break961_g139092.y * break33_g139107.y );
				float temp_output_31_0_g139107 = ( break961_g139092.z * break33_g139107.z );
				float temp_output_28_0_g139107 = ( temp_output_30_0_g139107 + temp_output_29_0_g139107 + temp_output_31_0_g139107 + ( break961_g139092.w * break33_g139107.w ) );
				float clampResult17_g139101 = clamp( temp_output_28_0_g139107 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139102 = _ThirdMeshRemap.x;
				float temp_output_10_0_g139102 = ( _ThirdMeshRemap.y - temp_output_7_0_g139102 );
				float lerpResult1026_g139092 = lerp( 1.0 , saturate( ( ( clampResult17_g139101 - temp_output_7_0_g139102 ) / ( temp_output_10_0_g139102 + 0.0001 ) ) ) , _ThirdMeshValue);
				float temp_output_6_0_g139118 = lerpResult1026_g139092;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139118 = ( temp_output_6_0_g139118 + ( _ThirdMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139118 = temp_output_6_0_g139118;
				#endif
				float temp_output_992_0_g139092 = staticSwitch14_g139118;
				half Blend_VertMask913_g139092 = temp_output_992_0_g139092;
				half3 Visual_NormalWS953_g139092 = Out_NormalWS4_g139108;
				float clampResult17_g139105 = clamp( saturate( (Visual_NormalWS953_g139092).y ) , 0.0001 , 0.9999 );
				float temp_output_7_0_g139106 = _ThirdProjRemap.x;
				float temp_output_10_0_g139106 = ( _ThirdProjRemap.y - temp_output_7_0_g139106 );
				float lerpResult1004_g139092 = lerp( 1.0 , saturate( ( ( clampResult17_g139105 - temp_output_7_0_g139106 ) / ( temp_output_10_0_g139106 + 0.0001 ) ) ) , _ThirdProjValue);
				half Blend_ProjMask912_g139092 = lerpResult1004_g139092;
				TVEGlobalData Data15_g139110 = Data374;
				float Out_Dummy15_g139110 = 0;
				float4 Out_CoatParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139110 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139110 = float4( 0,0,0,0 );
				Out_Dummy15_g139110 = Data15_g139110.Dummy;
				Out_CoatParams15_g139110 = Data15_g139110.CoatParams;
				Out_PaintParams15_g139110 = Data15_g139110.PaintParams;
				Out_GlowParams15_g139110 = Data15_g139110.GlowParams;
				Out_AtmoParams15_g139110= Data15_g139110.AtmoParams;
				Out_FadeParams15_g139110= Data15_g139110.FadeParams;
				Out_FormParams15_g139110 = Data15_g139110.FormParams;
				Out_LandParams15_g139110 = Data15_g139110.LandParams;
				Out_WindParams15_g139110 = Data15_g139110.WindParams;
				Out_PushParams15_g139110 = Data15_g139110.PushParams;
				half4 Global_CoatParams972_g139092 = Out_CoatParams15_g139110;
				float lerpResult1023_g139092 = lerp( 1.0 , (Global_CoatParams972_g139092).y , TVE_IsEnabled);
				#ifdef TVE_THIRD_ELEMENT
				float staticSwitch965_g139092 = lerpResult1023_g139092;
				#else
				float staticSwitch965_g139092 = 1.0;
				#endif
				half Blend_GlobalMask968_g139092 = staticSwitch965_g139092;
				float temp_output_432_0_g139092 = ( _ThirdIntensityValue * Detail_TexMask429_g139092 * Blend_VertMask913_g139092 * Blend_ProjMask912_g139092 * Blend_GlobalMask968_g139092 );
				float clampResult17_g139112 = clamp( temp_output_432_0_g139092 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139111 = _ThirdBlendRemap.x;
				float temp_output_10_0_g139111 = ( _ThirdBlendRemap.y - temp_output_7_0_g139111 );
				half Detail_Mask412_g139092 = ( saturate( ( ( clampResult17_g139112 - temp_output_7_0_g139111 ) / ( temp_output_10_0_g139111 + 0.0001 ) ) ) * _ThirdBlendIntensityValue );
				float3 lerpResult989_g139092 = lerp( Visual_Albedo527_g139092 , lerpResult985_g139092 , Detail_Mask412_g139092);
				#ifdef TVE_THIRD
				float3 staticSwitch415_g139092 = lerpResult989_g139092;
				#else
				float3 staticSwitch415_g139092 = Visual_Albedo527_g139092;
				#endif
				half3 Final_Albedo601_g139092 = staticSwitch415_g139092;
				float3 In_Albedo3_g139109 = Final_Albedo601_g139092;
				float3 In_AlbedoRaw3_g139109 = Final_Albedo601_g139092;
				half2 Visual_NormalTS529_g139092 = Out_NormalTS4_g139108;
				float2 lerpResult40_g139114 = lerp( float2( 0,0 ) , Visual_NormalTS529_g139092 , _ThirdBlendNormalValue);
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139094) = _ThirdNormalTex;
				float localFilterTexture29_g139122 = ( 0.0 );
				SamplerState SamplerDefault29_g139122 = sampler_Linear_Repeat;
				SamplerState SamplerPoint29_g139122 = sampler_Point_Repeat;
				SamplerState SamplerLow29_g139122 = sampler_Linear_Repeat;
				SamplerState SamplerMedium29_g139122 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh29_g139122 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS29_g139122 = SamplerDefault29_g139122;
				#if defined (TVE_FILTER_DEFAULT)
				    SS29_g139122 = SamplerDefault29_g139122;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS29_g139122 = SamplerPoint29_g139122;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS29_g139122 = SamplerLow29_g139122;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS29_g139122 = SamplerMedium29_g139122;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS29_g139122 = SamplerHigh29_g139122;
				#endif
				SamplerState Sampler276_g139094 = SS29_g139122;
				float4 temp_output_37_0_g139094 = Local_LayerCoords790_g139092;
				half4 Coords276_g139094 = temp_output_37_0_g139094;
				half2 TexCoord276_g139094 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139094 = SampleMain( Texture276_g139094 , Sampler276_g139094 , Coords276_g139094 , TexCoord276_g139094 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139094) = _ThirdNormalTex;
				SamplerState Sampler275_g139094 = SS29_g139122;
				half4 Coords275_g139094 = temp_output_37_0_g139094;
				half2 TexCoord275_g139094 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139094 = SampleExtra( Texture275_g139094 , Sampler275_g139094 , Coords275_g139094 , TexCoord275_g139094 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139094) = _ThirdNormalTex;
				SamplerState Sampler238_g139094 = SS29_g139122;
				half4 Coords238_g139094 = temp_output_37_0_g139094;
				float3 temp_output_279_0_g139094 = Model_PositionWO636_g139092;
				half3 WorldPosition238_g139094 = temp_output_279_0_g139094;
				half4 localSamplePlanar2D238_g139094 = SamplePlanar2D( Texture238_g139094 , Sampler238_g139094 , Coords238_g139094 , WorldPosition238_g139094 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139094) = _ThirdNormalTex;
				SamplerState Sampler246_g139094 = SS29_g139122;
				half4 Coords246_g139094 = temp_output_37_0_g139094;
				half3 WorldPosition246_g139094 = temp_output_279_0_g139094;
				float3 temp_output_280_0_g139094 = Model_NormalWS869_g139092;
				half3 WorldNormal246_g139094 = temp_output_280_0_g139094;
				half4 localSamplePlanar3D246_g139094 = SamplePlanar3D( Texture246_g139094 , Sampler246_g139094 , Coords246_g139094 , WorldPosition246_g139094 , WorldNormal246_g139094 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139094) = _ThirdNormalTex;
				SamplerState Sampler234_g139094 = SS29_g139122;
				float4 Coords234_g139094 = temp_output_37_0_g139094;
				float3 WorldPosition234_g139094 = temp_output_279_0_g139094;
				float4 localSampleStochastic2D234_g139094 = SampleStochastic2D( Texture234_g139094 , Sampler234_g139094 , Coords234_g139094 , WorldPosition234_g139094 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139094) = _ThirdNormalTex;
				SamplerState Sampler263_g139094 = SS29_g139122;
				half4 Coords263_g139094 = temp_output_37_0_g139094;
				half3 WorldPosition263_g139094 = temp_output_279_0_g139094;
				half3 WorldNormal263_g139094 = temp_output_280_0_g139094;
				half4 localSampleStochastic3D263_g139094 = SampleStochastic3D( Texture263_g139094 , Sampler263_g139094 , Coords263_g139094 , WorldPosition263_g139094 , WorldNormal263_g139094 );
				#if defined( TVE_THIRD_SAMPLE_MAIN_UV )
				float4 staticSwitch698_g139092 = localSampleMain276_g139094;
				#elif defined( TVE_THIRD_SAMPLE_EXTRA_UV )
				float4 staticSwitch698_g139092 = localSampleExtra275_g139094;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_2D )
				float4 staticSwitch698_g139092 = localSamplePlanar2D238_g139094;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_3D )
				float4 staticSwitch698_g139092 = localSamplePlanar3D246_g139094;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch698_g139092 = localSampleStochastic2D234_g139094;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch698_g139092 = localSampleStochastic3D263_g139094;
				#else
				float4 staticSwitch698_g139092 = localSampleMain276_g139094;
				#endif
				half4 Local_NormalTex776_g139092 = staticSwitch698_g139092;
				half4 Normal_Packed45_g139096 = Local_NormalTex776_g139092;
				float2 appendResult58_g139096 = (float2(( (Normal_Packed45_g139096).x * (Normal_Packed45_g139096).w ) , (Normal_Packed45_g139096).y));
				half2 Normal_Default50_g139096 = appendResult58_g139096;
				half2 Normal_ASTC41_g139096 = (Normal_Packed45_g139096).xy;
				#ifdef UNITY_ASTC_NORMALMAP_ENCODING
				float2 staticSwitch38_g139096 = Normal_ASTC41_g139096;
				#else
				float2 staticSwitch38_g139096 = Normal_Default50_g139096;
				#endif
				half2 Normal_NO_DTX544_g139096 = (Normal_Packed45_g139096).wy;
				#ifdef UNITY_NO_DXT5nm
				float2 staticSwitch37_g139096 = Normal_NO_DTX544_g139096;
				#else
				float2 staticSwitch37_g139096 = staticSwitch38_g139096;
				#endif
				float2 temp_output_724_0_g139092 = ( (staticSwitch37_g139096*2.0 + -1.0) * _ThirdNormalValue );
				half2 Normal_Planar45_g139097 = temp_output_724_0_g139092;
				float2 break71_g139097 = Normal_Planar45_g139097;
				float3 appendResult72_g139097 = (float3(break71_g139097.x , 0.0 , break71_g139097.y));
				float2 temp_output_858_0_g139092 = (mul( ase_worldToTangent, appendResult72_g139097 )).xy;
				#if defined( TVE_THIRD_SAMPLE_MAIN_UV )
				float2 staticSwitch727_g139092 = temp_output_724_0_g139092;
				#elif defined( TVE_THIRD_SAMPLE_EXTRA_UV )
				float2 staticSwitch727_g139092 = temp_output_724_0_g139092;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_2D )
				float2 staticSwitch727_g139092 = temp_output_858_0_g139092;
				#elif defined( TVE_THIRD_SAMPLE_PLANAR_3D )
				float2 staticSwitch727_g139092 = temp_output_858_0_g139092;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_2D )
				float2 staticSwitch727_g139092 = temp_output_858_0_g139092;
				#elif defined( TVE_THIRD_SAMPLE_STOCHASTIC_3D )
				float2 staticSwitch727_g139092 = temp_output_858_0_g139092;
				#else
				float2 staticSwitch727_g139092 = temp_output_724_0_g139092;
				#endif
				half2 Local_NormalTS729_g139092 = staticSwitch727_g139092;
				float2 temp_output_36_0_g139114 = ( lerpResult40_g139114 + Local_NormalTS729_g139092 );
				float2 lerpResult405_g139092 = lerp( Visual_NormalTS529_g139092 , temp_output_36_0_g139114 , Detail_Mask412_g139092);
				#ifdef TVE_THIRD
				float2 staticSwitch418_g139092 = lerpResult405_g139092;
				#else
				float2 staticSwitch418_g139092 = Visual_NormalTS529_g139092;
				#endif
				half2 Final_NormalTS612_g139092 = staticSwitch418_g139092;
				float2 In_NormalTS3_g139109 = Final_NormalTS612_g139092;
				float3 appendResult68_g139115 = (float3(Final_NormalTS612_g139092 , 1.0));
				float3 tanNormal74_g139115 = appendResult68_g139115;
				float3 worldNormal74_g139115 = normalize( float3(dot(tanToWorld0,tanNormal74_g139115), dot(tanToWorld1,tanNormal74_g139115), dot(tanToWorld2,tanNormal74_g139115)) );
				half3 Final_NormalWS956_g139092 = worldNormal74_g139115;
				float3 In_NormalWS3_g139109 = Final_NormalWS956_g139092;
				half4 Visual_Shader531_g139092 = Out_Shader4_g139108;
				float4 lerpResult1000_g139092 = lerp( Local_Masks750_g139092 , ( Visual_Shader531_g139092 * Local_Masks750_g139092 ) , _ThirdBlendShaderValue);
				float4 lerpResult998_g139092 = lerp( Visual_Shader531_g139092 , lerpResult1000_g139092 , Detail_Mask412_g139092);
				#ifdef TVE_THIRD
				float4 staticSwitch451_g139092 = lerpResult998_g139092;
				#else
				float4 staticSwitch451_g139092 = Visual_Shader531_g139092;
				#endif
				half4 Final_Masks613_g139092 = staticSwitch451_g139092;
				float4 In_Shader3_g139109 = Final_Masks613_g139092;
				float4 In_Emissive3_g139109 = Out_Emissive4_g139108;
				float3 temp_output_3_0_g139117 = Final_Albedo601_g139092;
				float dotResult20_g139117 = dot( temp_output_3_0_g139117 , float3(0.2126,0.7152,0.0722) );
				half Final_Grayscale615_g139092 = dotResult20_g139117;
				float In_Grayscale3_g139109 = Final_Grayscale615_g139092;
				float clampResult651_g139092 = clamp( saturate( ( Final_Grayscale615_g139092 * 5.0 ) ) , 0.2 , 1.0 );
				half Final_Luminosity652_g139092 = clampResult651_g139092;
				float In_Luminosity3_g139109 = Final_Luminosity652_g139092;
				half Visual_MultiMask547_g139092 = Out_MultiMask4_g139108;
				float lerpResult477_g139092 = lerp( Visual_MultiMask547_g139092 , Local_MultiMask767_g139092 , Detail_Mask412_g139092);
				#ifdef TVE_THIRD
				float staticSwitch482_g139092 = lerpResult477_g139092;
				#else
				float staticSwitch482_g139092 = Visual_MultiMask547_g139092;
				#endif
				half Final_MultiMask572_g139092 = staticSwitch482_g139092;
				float In_MultiMask3_g139109 = Final_MultiMask572_g139092;
				half Visual_AlphaClip559_g139092 = Out_AlphaClip4_g139108;
				float temp_output_718_0_g139092 = (Local_AlbedoTex777_g139092).w;
				#ifdef TVE_ALPHA_CLIP
				float staticSwitch924_g139092 = ( temp_output_718_0_g139092 - _ThirdAlphaClipValue );
				#else
				float staticSwitch924_g139092 = temp_output_718_0_g139092;
				#endif
				half Local_AlphaClip772_g139092 = staticSwitch924_g139092;
				float lerpResult448_g139092 = lerp( Visual_AlphaClip559_g139092 , Local_AlphaClip772_g139092 , Detail_Mask412_g139092);
				#ifdef TVE_THIRD
				float staticSwitch564_g139092 = lerpResult448_g139092;
				#else
				float staticSwitch564_g139092 = Visual_AlphaClip559_g139092;
				#endif
				half Final_AlphaClip602_g139092 = staticSwitch564_g139092;
				float In_AlphaClip3_g139109 = Final_AlphaClip602_g139092;
				half Visual_AlphaFade588_g139092 = Out_AlphaFade4_g139108;
				half Local_AlphaFade773_g139092 = (lerpResult706_g139092).a;
				float lerpResult604_g139092 = lerp( Visual_AlphaFade588_g139092 , Local_AlphaFade773_g139092 , Detail_Mask412_g139092);
				#ifdef TVE_THIRD
				float staticSwitch608_g139092 = lerpResult604_g139092;
				#else
				float staticSwitch608_g139092 = Visual_AlphaFade588_g139092;
				#endif
				half Final_AlphaFade611_g139092 = staticSwitch608_g139092;
				float In_AlphaFade3_g139109 = Final_AlphaFade611_g139092;
				float3 In_Translucency3_g139109 = Out_Translucency4_g139108;
				float In_Transmission3_g139109 = Out_Transmission4_g139108;
				float In_Thickness3_g139109 = Out_Thickness4_g139108;
				float In_Diffusion3_g139109 = Out_Diffusion4_g139108;
				Data3_g139109.Dummy = In_Dummy3_g139109;
				Data3_g139109.Albedo = In_Albedo3_g139109;
				Data3_g139109.AlbedoRaw = In_AlbedoRaw3_g139109;
				Data3_g139109.NormalTS = In_NormalTS3_g139109;
				Data3_g139109.NormalWS = In_NormalWS3_g139109;
				Data3_g139109.Shader = In_Shader3_g139109;
				Data3_g139109.Emissive= In_Emissive3_g139109;
				Data3_g139109.MultiMask = In_MultiMask3_g139109;
				Data3_g139109.Grayscale = In_Grayscale3_g139109;
				Data3_g139109.Luminosity = In_Luminosity3_g139109;
				Data3_g139109.AlphaClip = In_AlphaClip3_g139109;
				Data3_g139109.AlphaFade = In_AlphaFade3_g139109;
				Data3_g139109.Translucency = In_Translucency3_g139109;
				Data3_g139109.Transmission = In_Transmission3_g139109;
				Data3_g139109.Thickness = In_Thickness3_g139109;
				Data3_g139109.Diffusion = In_Diffusion3_g139109;
				TVEVisualData DataB25_g139125 = Data3_g139109;
				float Alpha25_g139125 = _ThirdBakeMode;
				if (Alpha25_g139125 < 0.5 )
				{
				Data25_g139125 = DataA25_g139125;
				}
				else
				{
				Data25_g139125 = DataB25_g139125;
				}
				TVEVisualData DataA25_g139136 = Data25_g139125;
				float localCompData3_g139128 = ( 0.0 );
				TVEVisualData Data3_g139128 = (TVEVisualData)0;
				half Dummy202_g139126 = ( _OcclusionCategory + _OcclusionEnd + _OcclusionBakeMode );
				float In_Dummy3_g139128 = Dummy202_g139126;
				float localBreakData4_g139127 = ( 0.0 );
				TVEVisualData Data4_g139127 = Data25_g139125;
				float Out_Dummy4_g139127 = 0;
				float3 Out_Albedo4_g139127 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139127 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139127 = float2( 0,0 );
				float3 Out_NormalWS4_g139127 = float3( 0,0,0 );
				float4 Out_Shader4_g139127 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139127 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139127 = 0;
				float Out_Grayscale4_g139127 = 0;
				float Out_Luminosity4_g139127 = 0;
				float Out_AlphaClip4_g139127 = 0;
				float Out_AlphaFade4_g139127 = 0;
				float3 Out_Translucency4_g139127 = float3( 0,0,0 );
				float Out_Transmission4_g139127 = 0;
				float Out_Thickness4_g139127 = 0;
				float Out_Diffusion4_g139127 = 0;
				Out_Dummy4_g139127 = Data4_g139127.Dummy;
				Out_Albedo4_g139127 = Data4_g139127.Albedo;
				Out_AlbedoRaw4_g139127 = Data4_g139127.AlbedoRaw;
				Out_NormalTS4_g139127 = Data4_g139127.NormalTS;
				Out_NormalWS4_g139127 = Data4_g139127.NormalWS;
				Out_Shader4_g139127 = Data4_g139127.Shader;
				Out_Emissive4_g139127= Data4_g139127.Emissive;
				Out_MultiMask4_g139127 = Data4_g139127.MultiMask;
				Out_Grayscale4_g139127 = Data4_g139127.Grayscale;
				Out_Luminosity4_g139127= Data4_g139127.Luminosity;
				Out_AlphaClip4_g139127 = Data4_g139127.AlphaClip;
				Out_AlphaFade4_g139127 = Data4_g139127.AlphaFade;
				Out_Translucency4_g139127 = Data4_g139127.Translucency;
				Out_Transmission4_g139127 = Data4_g139127.Transmission;
				Out_Thickness4_g139127 = Data4_g139127.Thickness;
				Out_Diffusion4_g139127 = Data4_g139127.Diffusion;
				half3 Visual_Albedo127_g139126 = Out_Albedo4_g139127;
				TVEModelData Data15_g139135 = Data16_g76840;
				float Out_Dummy15_g139135 = 0;
				float3 Out_PositionWS15_g139135 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139135 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139135 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139135 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139135 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139135 = float3( 0,0,0 );
				float4 Out_VertexData15_g139135 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139135 = float4( 0,0,0,0 );
				Out_Dummy15_g139135 = Data15_g139135.Dummy;
				Out_PositionWS15_g139135 = Data15_g139135.PositionWS;
				Out_PositionWO15_g139135 = Data15_g139135.PositionWO;
				Out_PivotWS15_g139135 = Data15_g139135.PivotWS;
				Out_PivotWO15_g139135 = Data15_g139135.PivotWO;
				Out_NormalWS15_g139135 = Data15_g139135.NormalWS;
				Out_ViewDirWS15_g139135 = Data15_g139135.ViewDirWS;
				Out_VertexData15_g139135 = Data15_g139135.VertexData;
				Out_BoundsData15_g139135 = Data15_g139135.BoundsData;
				half4 Model_VertexData206_g139126 = Out_VertexData15_g139135;
				float4 break208_g139126 = Model_VertexData206_g139126;
				float4 break33_g139133 = _occlusion_vert_mode;
				float temp_output_30_0_g139133 = ( break208_g139126.x * break33_g139133.x );
				float temp_output_29_0_g139133 = ( break208_g139126.y * break33_g139133.y );
				float temp_output_31_0_g139133 = ( break208_g139126.z * break33_g139133.z );
				float temp_output_28_0_g139133 = ( temp_output_30_0_g139133 + temp_output_29_0_g139133 + temp_output_31_0_g139133 + ( break208_g139126.w * break33_g139133.w ) );
				float temp_output_194_0_g139126 = temp_output_28_0_g139133;
				float clampResult17_g139131 = clamp( temp_output_194_0_g139126 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139130 = _OcclusionMeshRemap.x;
				float temp_output_10_0_g139130 = ( _OcclusionMeshRemap.y - temp_output_7_0_g139130 );
				float temp_output_6_0_g139132 = saturate( ( ( clampResult17_g139131 - temp_output_7_0_g139130 ) / ( temp_output_10_0_g139130 + 0.0001 ) ) );
				#ifdef TVE_REGISTER
				float staticSwitch14_g139132 = ( temp_output_6_0_g139132 + ( _OcclusionMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139132 = temp_output_6_0_g139132;
				#endif
				half Occlusion_Mask82_g139126 = staticSwitch14_g139132;
				float3 lerpResult75_g139126 = lerp( (_OcclusionColorTwo).rgb , (_OcclusionColorOne).rgb , Occlusion_Mask82_g139126);
				float3 lerpResult186_g139126 = lerp( Visual_Albedo127_g139126 , ( Visual_Albedo127_g139126 * lerpResult75_g139126 ) , _OcclusionIntensityValue);
				#ifdef TVE_OCCLUSION
				float3 staticSwitch171_g139126 = lerpResult186_g139126;
				#else
				float3 staticSwitch171_g139126 = Visual_Albedo127_g139126;
				#endif
				half3 Final_Albedo160_g139126 = staticSwitch171_g139126;
				float3 In_Albedo3_g139128 = Final_Albedo160_g139126;
				float3 In_AlbedoRaw3_g139128 = Final_Albedo160_g139126;
				float2 In_NormalTS3_g139128 = Out_NormalTS4_g139127;
				float3 In_NormalWS3_g139128 = Out_NormalWS4_g139127;
				float4 In_Shader3_g139128 = Out_Shader4_g139127;
				float4 In_Emissive3_g139128 = Out_Emissive4_g139127;
				float3 temp_output_3_0_g139129 = Final_Albedo160_g139126;
				float dotResult20_g139129 = dot( temp_output_3_0_g139129 , float3(0.2126,0.7152,0.0722) );
				half Final_Grayscale164_g139126 = dotResult20_g139129;
				float In_Grayscale3_g139128 = Final_Grayscale164_g139126;
				float clampResult180_g139126 = clamp( saturate( ( Final_Grayscale164_g139126 * 5.0 ) ) , 0.2 , 1.0 );
				half Final_Shading181_g139126 = clampResult180_g139126;
				float In_Luminosity3_g139128 = Final_Shading181_g139126;
				float In_MultiMask3_g139128 = Out_MultiMask4_g139127;
				float In_AlphaClip3_g139128 = Out_AlphaClip4_g139127;
				float In_AlphaFade3_g139128 = Out_AlphaFade4_g139127;
				float3 In_Translucency3_g139128 = Out_Translucency4_g139127;
				float In_Transmission3_g139128 = Out_Transmission4_g139127;
				float In_Thickness3_g139128 = Out_Thickness4_g139127;
				float In_Diffusion3_g139128 = Out_Diffusion4_g139127;
				Data3_g139128.Dummy = In_Dummy3_g139128;
				Data3_g139128.Albedo = In_Albedo3_g139128;
				Data3_g139128.AlbedoRaw = In_AlbedoRaw3_g139128;
				Data3_g139128.NormalTS = In_NormalTS3_g139128;
				Data3_g139128.NormalWS = In_NormalWS3_g139128;
				Data3_g139128.Shader = In_Shader3_g139128;
				Data3_g139128.Emissive= In_Emissive3_g139128;
				Data3_g139128.MultiMask = In_MultiMask3_g139128;
				Data3_g139128.Grayscale = In_Grayscale3_g139128;
				Data3_g139128.Luminosity = In_Luminosity3_g139128;
				Data3_g139128.AlphaClip = In_AlphaClip3_g139128;
				Data3_g139128.AlphaFade = In_AlphaFade3_g139128;
				Data3_g139128.Translucency = In_Translucency3_g139128;
				Data3_g139128.Transmission = In_Transmission3_g139128;
				Data3_g139128.Thickness = In_Thickness3_g139128;
				Data3_g139128.Diffusion = In_Diffusion3_g139128;
				TVEVisualData DataB25_g139136 = Data3_g139128;
				float Alpha25_g139136 = _OcclusionBakeMode;
				if (Alpha25_g139136 < 0.5 )
				{
				Data25_g139136 = DataA25_g139136;
				}
				else
				{
				Data25_g139136 = DataB25_g139136;
				}
				TVEVisualData DataA25_g139147 = Data25_g139136;
				float localCompData3_g139140 = ( 0.0 );
				TVEVisualData Data3_g139140 = (TVEVisualData)0;
				half Dummy220_g139137 = ( _GradientCategory + _GradientEnd + _GradientBakeMode );
				float In_Dummy3_g139140 = Dummy220_g139137;
				float localBreakData4_g139139 = ( 0.0 );
				TVEVisualData Data4_g139139 = Data25_g139136;
				float Out_Dummy4_g139139 = 0;
				float3 Out_Albedo4_g139139 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139139 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139139 = float2( 0,0 );
				float3 Out_NormalWS4_g139139 = float3( 0,0,0 );
				float4 Out_Shader4_g139139 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139139 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139139 = 0;
				float Out_Grayscale4_g139139 = 0;
				float Out_Luminosity4_g139139 = 0;
				float Out_AlphaClip4_g139139 = 0;
				float Out_AlphaFade4_g139139 = 0;
				float3 Out_Translucency4_g139139 = float3( 0,0,0 );
				float Out_Transmission4_g139139 = 0;
				float Out_Thickness4_g139139 = 0;
				float Out_Diffusion4_g139139 = 0;
				Out_Dummy4_g139139 = Data4_g139139.Dummy;
				Out_Albedo4_g139139 = Data4_g139139.Albedo;
				Out_AlbedoRaw4_g139139 = Data4_g139139.AlbedoRaw;
				Out_NormalTS4_g139139 = Data4_g139139.NormalTS;
				Out_NormalWS4_g139139 = Data4_g139139.NormalWS;
				Out_Shader4_g139139 = Data4_g139139.Shader;
				Out_Emissive4_g139139= Data4_g139139.Emissive;
				Out_MultiMask4_g139139 = Data4_g139139.MultiMask;
				Out_Grayscale4_g139139 = Data4_g139139.Grayscale;
				Out_Luminosity4_g139139= Data4_g139139.Luminosity;
				Out_AlphaClip4_g139139 = Data4_g139139.AlphaClip;
				Out_AlphaFade4_g139139 = Data4_g139139.AlphaFade;
				Out_Translucency4_g139139 = Data4_g139139.Translucency;
				Out_Transmission4_g139139 = Data4_g139139.Transmission;
				Out_Thickness4_g139139 = Data4_g139139.Thickness;
				Out_Diffusion4_g139139 = Data4_g139139.Diffusion;
				half3 Visual_Albedo127_g139137 = Out_Albedo4_g139139;
				TVEModelData Data15_g139146 = Data16_g76840;
				float Out_Dummy15_g139146 = 0;
				float3 Out_PositionWS15_g139146 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139146 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139146 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139146 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139146 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139146 = float3( 0,0,0 );
				float4 Out_VertexData15_g139146 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139146 = float4( 0,0,0,0 );
				Out_Dummy15_g139146 = Data15_g139146.Dummy;
				Out_PositionWS15_g139146 = Data15_g139146.PositionWS;
				Out_PositionWO15_g139146 = Data15_g139146.PositionWO;
				Out_PivotWS15_g139146 = Data15_g139146.PivotWS;
				Out_PivotWO15_g139146 = Data15_g139146.PivotWO;
				Out_NormalWS15_g139146 = Data15_g139146.NormalWS;
				Out_ViewDirWS15_g139146 = Data15_g139146.ViewDirWS;
				Out_VertexData15_g139146 = Data15_g139146.VertexData;
				Out_BoundsData15_g139146 = Data15_g139146.BoundsData;
				half4 Model_VertexData224_g139137 = Out_VertexData15_g139146;
				float4 break226_g139137 = Model_VertexData224_g139137;
				float4 break33_g139138 = _gradient_vert_mode;
				float temp_output_30_0_g139138 = ( break226_g139137.x * break33_g139138.x );
				float temp_output_29_0_g139138 = ( break226_g139137.y * break33_g139138.y );
				float temp_output_31_0_g139138 = ( break226_g139137.z * break33_g139138.z );
				float temp_output_28_0_g139138 = ( temp_output_30_0_g139138 + temp_output_29_0_g139138 + temp_output_31_0_g139138 + ( break226_g139137.w * break33_g139138.w ) );
				float temp_output_211_0_g139137 = temp_output_28_0_g139138;
				float clampResult17_g139143 = clamp( temp_output_211_0_g139137 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139144 = _GradientMeshRemap.x;
				float temp_output_10_0_g139144 = ( _GradientMeshRemap.y - temp_output_7_0_g139144 );
				float temp_output_6_0_g139145 = saturate( ( ( clampResult17_g139143 - temp_output_7_0_g139144 ) / ( temp_output_10_0_g139144 + 0.0001 ) ) );
				#ifdef TVE_REGISTER
				float staticSwitch14_g139145 = ( temp_output_6_0_g139145 + ( _GradientMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139145 = temp_output_6_0_g139145;
				#endif
				half Gradient_VertMask82_g139137 = staticSwitch14_g139145;
				half Gradient_Mask200_g139137 = Gradient_VertMask82_g139137;
				float3 lerpResult75_g139137 = lerp( (_GradientColorTwo).rgb , (_GradientColorOne).rgb , Gradient_Mask200_g139137);
				float temp_output_162_11_g139137 = Out_MultiMask4_g139139;
				half Visual_MultiMask196_g139137 = temp_output_162_11_g139137;
				float lerpResult190_g139137 = lerp( 1.0 , Visual_MultiMask196_g139137 , _GradientMultiValue);
				half Gradient_MultiMask194_g139137 = lerpResult190_g139137;
				float3 lerpResult186_g139137 = lerp( Visual_Albedo127_g139137 , ( Visual_Albedo127_g139137 * lerpResult75_g139137 ) , ( _GradientIntensityValue * Gradient_MultiMask194_g139137 ));
				#ifdef TVE_GRADIENT
				float3 staticSwitch171_g139137 = lerpResult186_g139137;
				#else
				float3 staticSwitch171_g139137 = Visual_Albedo127_g139137;
				#endif
				half3 Final_Albedo160_g139137 = staticSwitch171_g139137;
				float3 In_Albedo3_g139140 = Final_Albedo160_g139137;
				float3 In_AlbedoRaw3_g139140 = Final_Albedo160_g139137;
				float2 In_NormalTS3_g139140 = Out_NormalTS4_g139139;
				float3 In_NormalWS3_g139140 = Out_NormalWS4_g139139;
				float4 In_Shader3_g139140 = Out_Shader4_g139139;
				float4 In_Emissive3_g139140 = Out_Emissive4_g139139;
				float3 temp_output_3_0_g139141 = Final_Albedo160_g139137;
				float dotResult20_g139141 = dot( temp_output_3_0_g139141 , float3(0.2126,0.7152,0.0722) );
				half Final_Grayscale164_g139137 = dotResult20_g139141;
				float In_Grayscale3_g139140 = Final_Grayscale164_g139137;
				float clampResult180_g139137 = clamp( saturate( ( Final_Grayscale164_g139137 * 5.0 ) ) , 0.2 , 1.0 );
				half Final_Luminosity181_g139137 = clampResult180_g139137;
				float In_Luminosity3_g139140 = Final_Luminosity181_g139137;
				float In_MultiMask3_g139140 = temp_output_162_11_g139137;
				float In_AlphaClip3_g139140 = Out_AlphaClip4_g139139;
				float In_AlphaFade3_g139140 = Out_AlphaFade4_g139139;
				float3 In_Translucency3_g139140 = Out_Translucency4_g139139;
				float In_Transmission3_g139140 = Out_Transmission4_g139139;
				float In_Thickness3_g139140 = Out_Thickness4_g139139;
				float In_Diffusion3_g139140 = Out_Diffusion4_g139139;
				Data3_g139140.Dummy = In_Dummy3_g139140;
				Data3_g139140.Albedo = In_Albedo3_g139140;
				Data3_g139140.AlbedoRaw = In_AlbedoRaw3_g139140;
				Data3_g139140.NormalTS = In_NormalTS3_g139140;
				Data3_g139140.NormalWS = In_NormalWS3_g139140;
				Data3_g139140.Shader = In_Shader3_g139140;
				Data3_g139140.Emissive= In_Emissive3_g139140;
				Data3_g139140.MultiMask = In_MultiMask3_g139140;
				Data3_g139140.Grayscale = In_Grayscale3_g139140;
				Data3_g139140.Luminosity = In_Luminosity3_g139140;
				Data3_g139140.AlphaClip = In_AlphaClip3_g139140;
				Data3_g139140.AlphaFade = In_AlphaFade3_g139140;
				Data3_g139140.Translucency = In_Translucency3_g139140;
				Data3_g139140.Transmission = In_Transmission3_g139140;
				Data3_g139140.Thickness = In_Thickness3_g139140;
				Data3_g139140.Diffusion = In_Diffusion3_g139140;
				TVEVisualData DataB25_g139147 = Data3_g139140;
				float Alpha25_g139147 = _GradientBakeMode;
				if (Alpha25_g139147 < 0.5 )
				{
				Data25_g139147 = DataA25_g139147;
				}
				else
				{
				Data25_g139147 = DataB25_g139147;
				}
				TVEVisualData DataA25_g139172 = Data25_g139147;
				float localCompData3_g139169 = ( 0.0 );
				TVEVisualData Data3_g139169 = (TVEVisualData)0;
				half Dummy205_g139148 = ( _TintingCategory + _TintingEnd + _TintingSpace + _TintingBakeMode );
				float In_Dummy3_g139169 = Dummy205_g139148;
				float localBreakData4_g139170 = ( 0.0 );
				TVEVisualData Data4_g139170 = Data25_g139147;
				float Out_Dummy4_g139170 = 0;
				float3 Out_Albedo4_g139170 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139170 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139170 = float2( 0,0 );
				float3 Out_NormalWS4_g139170 = float3( 0,0,0 );
				float4 Out_Shader4_g139170 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139170 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139170 = 0;
				float Out_Grayscale4_g139170 = 0;
				float Out_Luminosity4_g139170 = 0;
				float Out_AlphaClip4_g139170 = 0;
				float Out_AlphaFade4_g139170 = 0;
				float3 Out_Translucency4_g139170 = float3( 0,0,0 );
				float Out_Transmission4_g139170 = 0;
				float Out_Thickness4_g139170 = 0;
				float Out_Diffusion4_g139170 = 0;
				Out_Dummy4_g139170 = Data4_g139170.Dummy;
				Out_Albedo4_g139170 = Data4_g139170.Albedo;
				Out_AlbedoRaw4_g139170 = Data4_g139170.AlbedoRaw;
				Out_NormalTS4_g139170 = Data4_g139170.NormalTS;
				Out_NormalWS4_g139170 = Data4_g139170.NormalWS;
				Out_Shader4_g139170 = Data4_g139170.Shader;
				Out_Emissive4_g139170= Data4_g139170.Emissive;
				Out_MultiMask4_g139170 = Data4_g139170.MultiMask;
				Out_Grayscale4_g139170 = Data4_g139170.Grayscale;
				Out_Luminosity4_g139170= Data4_g139170.Luminosity;
				Out_AlphaClip4_g139170 = Data4_g139170.AlphaClip;
				Out_AlphaFade4_g139170 = Data4_g139170.AlphaFade;
				Out_Translucency4_g139170 = Data4_g139170.Translucency;
				Out_Transmission4_g139170 = Data4_g139170.Transmission;
				Out_Thickness4_g139170 = Data4_g139170.Thickness;
				Out_Diffusion4_g139170 = Data4_g139170.Diffusion;
				half3 Visual_Albedo139_g139148 = Out_Albedo4_g139170;
				float temp_output_200_12_g139148 = Out_Grayscale4_g139170;
				half Visual_Grayscale150_g139148 = temp_output_200_12_g139148;
				float3 temp_cast_5 = (Visual_Grayscale150_g139148).xxx;
				TVEGlobalData Data15_g139149 = Data374;
				float Out_Dummy15_g139149 = 0;
				float4 Out_CoatParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139149 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139149 = float4( 0,0,0,0 );
				Out_Dummy15_g139149 = Data15_g139149.Dummy;
				Out_CoatParams15_g139149 = Data15_g139149.CoatParams;
				Out_PaintParams15_g139149 = Data15_g139149.PaintParams;
				Out_GlowParams15_g139149 = Data15_g139149.GlowParams;
				Out_AtmoParams15_g139149= Data15_g139149.AtmoParams;
				Out_FadeParams15_g139149= Data15_g139149.FadeParams;
				Out_FormParams15_g139149 = Data15_g139149.FormParams;
				Out_LandParams15_g139149 = Data15_g139149.LandParams;
				Out_WindParams15_g139149 = Data15_g139149.WindParams;
				Out_PushParams15_g139149 = Data15_g139149.PushParams;
				half4 Global_PaintParams209_g139148 = Out_PaintParams15_g139149;
				float temp_output_6_0_g139150 = ( saturate( (Global_PaintParams209_g139148).w ) * TVE_IsEnabled );
				#ifdef TVE_REGISTER
				float staticSwitch14_g139150 = ( temp_output_6_0_g139150 + ( _TintingElementMode * 0.0 ) );
				#else
				float staticSwitch14_g139150 = temp_output_6_0_g139150;
				#endif
				#ifdef TVE_TINTING_ELEMENT
				float staticSwitch283_g139148 = staticSwitch14_g139150;
				#else
				float staticSwitch283_g139148 = 1.0;
				#endif
				half Tinting_GlobalValue285_g139148 = staticSwitch283_g139148;
				float3 lerpResult368_g139148 = lerp( Visual_Albedo139_g139148 , temp_cast_5 , ( Tinting_GlobalValue285_g139148 * _TintingGrayValue ));
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g139168 = 2.0;
				#else
				float staticSwitch1_g139168 = 4.594794;
				#endif
				float3 temp_cast_6 = (1.0).xxx;
				#ifdef TVE_TINTING_ELEMENT
				float3 staticSwitch288_g139148 = (Global_PaintParams209_g139148).xyz;
				#else
				float3 staticSwitch288_g139148 = temp_cast_6;
				#endif
				half3 Tinting_ColorGlobal290_g139148 = staticSwitch288_g139148;
				float temp_output_200_11_g139148 = Out_MultiMask4_g139170;
				half Visual_MultiMask181_g139148 = temp_output_200_11_g139148;
				float lerpResult147_g139148 = lerp( 1.0 , Visual_MultiMask181_g139148 , _TintingMultiValue);
				half Tinting_MutiMask121_g139148 = lerpResult147_g139148;
				half Tinting_TexMask385_g139148 = 1.0;
				float temp_output_200_15_g139148 = Out_Luminosity4_g139170;
				half Visual_Luminosity257_g139148 = temp_output_200_15_g139148;
				float clampResult17_g139162 = clamp( Visual_Luminosity257_g139148 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139161 = _TintingLumaRemap.x;
				float temp_output_10_0_g139161 = ( _TintingLumaRemap.y - temp_output_7_0_g139161 );
				float lerpResult228_g139148 = lerp( 1.0 , saturate( ( ( clampResult17_g139162 - temp_output_7_0_g139161 ) / ( temp_output_10_0_g139161 + 0.0001 ) ) ) , _TintingLumaValue);
				half Tinting_LumaMask153_g139148 = lerpResult228_g139148;
				TVEModelData Data15_g139171 = Data16_g76840;
				float Out_Dummy15_g139171 = 0;
				float3 Out_PositionWS15_g139171 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139171 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139171 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139171 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139171 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139171 = float3( 0,0,0 );
				float4 Out_VertexData15_g139171 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139171 = float4( 0,0,0,0 );
				Out_Dummy15_g139171 = Data15_g139171.Dummy;
				Out_PositionWS15_g139171 = Data15_g139171.PositionWS;
				Out_PositionWO15_g139171 = Data15_g139171.PositionWO;
				Out_PivotWS15_g139171 = Data15_g139171.PivotWS;
				Out_PivotWO15_g139171 = Data15_g139171.PivotWO;
				Out_NormalWS15_g139171 = Data15_g139171.NormalWS;
				Out_ViewDirWS15_g139171 = Data15_g139171.ViewDirWS;
				Out_VertexData15_g139171 = Data15_g139171.VertexData;
				Out_BoundsData15_g139171 = Data15_g139171.BoundsData;
				half4 Model_VertexMasks307_g139148 = Out_VertexData15_g139171;
				float4 break311_g139148 = Model_VertexMasks307_g139148;
				float4 break33_g139154 = _tinting_vert_mode;
				float temp_output_30_0_g139154 = ( break311_g139148.x * break33_g139154.x );
				float temp_output_29_0_g139154 = ( break311_g139148.y * break33_g139154.y );
				float temp_output_31_0_g139154 = ( break311_g139148.z * break33_g139154.z );
				float temp_output_28_0_g139154 = ( temp_output_30_0_g139154 + temp_output_29_0_g139154 + temp_output_31_0_g139154 + ( break311_g139148.w * break33_g139154.w ) );
				float clampResult17_g139160 = clamp( temp_output_28_0_g139154 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139159 = _TintingMeshRemap.x;
				float temp_output_10_0_g139159 = ( _TintingMeshRemap.y - temp_output_7_0_g139159 );
				float lerpResult370_g139148 = lerp( 1.0 , saturate( ( ( clampResult17_g139160 - temp_output_7_0_g139159 ) / ( temp_output_10_0_g139159 + 0.0001 ) ) ) , _TintingMeshValue);
				float temp_output_6_0_g139163 = lerpResult370_g139148;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139163 = ( temp_output_6_0_g139163 + ( _TintingMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139163 = temp_output_6_0_g139163;
				#endif
				float temp_output_333_0_g139148 = staticSwitch14_g139163;
				half Tinting_VertMask309_g139148 = temp_output_333_0_g139148;
				half Tinting_NoiseMask213_g139148 = 1.0;
				half Custom_Mask345_g139148 = 1.0;
				float temp_output_7_0_g139157 = _TintingBlendRemap.x;
				float temp_output_10_0_g139157 = ( _TintingBlendRemap.y - temp_output_7_0_g139157 );
				half Tinting_Mask242_g139148 = ( saturate( ( ( ( _TintingIntensityValue * Tinting_MutiMask121_g139148 * Tinting_TexMask385_g139148 * Tinting_LumaMask153_g139148 * Tinting_VertMask309_g139148 * Tinting_NoiseMask213_g139148 * Tinting_GlobalValue285_g139148 * Custom_Mask345_g139148 ) - temp_output_7_0_g139157 ) / ( temp_output_10_0_g139157 + 0.0001 ) ) ) * TVE_IsEnabled );
				float3 lerpResult90_g139148 = lerp( Visual_Albedo139_g139148 , ( lerpResult368_g139148 * staticSwitch1_g139168 * Tinting_ColorGlobal290_g139148 * (_TintingColor).rgb ) , Tinting_Mask242_g139148);
				#ifdef TVE_TINTING
				float3 staticSwitch286_g139148 = lerpResult90_g139148;
				#else
				float3 staticSwitch286_g139148 = Visual_Albedo139_g139148;
				#endif
				half3 Final_Albedo97_g139148 = staticSwitch286_g139148;
				float3 In_Albedo3_g139169 = Final_Albedo97_g139148;
				float3 In_AlbedoRaw3_g139169 = Out_AlbedoRaw4_g139170;
				float2 In_NormalTS3_g139169 = Out_NormalTS4_g139170;
				float3 In_NormalWS3_g139169 = Out_NormalWS4_g139170;
				float4 In_Shader3_g139169 = Out_Shader4_g139170;
				float4 In_Emissive3_g139169 = Out_Emissive4_g139170;
				float In_Grayscale3_g139169 = temp_output_200_12_g139148;
				float In_Luminosity3_g139169 = temp_output_200_15_g139148;
				float In_MultiMask3_g139169 = temp_output_200_11_g139148;
				float In_AlphaClip3_g139169 = Out_AlphaClip4_g139170;
				float In_AlphaFade3_g139169 = Out_AlphaFade4_g139170;
				float3 In_Translucency3_g139169 = Out_Translucency4_g139170;
				float In_Transmission3_g139169 = Out_Transmission4_g139170;
				float In_Thickness3_g139169 = Out_Thickness4_g139170;
				float In_Diffusion3_g139169 = Out_Diffusion4_g139170;
				Data3_g139169.Dummy = In_Dummy3_g139169;
				Data3_g139169.Albedo = In_Albedo3_g139169;
				Data3_g139169.AlbedoRaw = In_AlbedoRaw3_g139169;
				Data3_g139169.NormalTS = In_NormalTS3_g139169;
				Data3_g139169.NormalWS = In_NormalWS3_g139169;
				Data3_g139169.Shader = In_Shader3_g139169;
				Data3_g139169.Emissive= In_Emissive3_g139169;
				Data3_g139169.MultiMask = In_MultiMask3_g139169;
				Data3_g139169.Grayscale = In_Grayscale3_g139169;
				Data3_g139169.Luminosity = In_Luminosity3_g139169;
				Data3_g139169.AlphaClip = In_AlphaClip3_g139169;
				Data3_g139169.AlphaFade = In_AlphaFade3_g139169;
				Data3_g139169.Translucency = In_Translucency3_g139169;
				Data3_g139169.Transmission = In_Transmission3_g139169;
				Data3_g139169.Thickness = In_Thickness3_g139169;
				Data3_g139169.Diffusion = In_Diffusion3_g139169;
				TVEVisualData DataB25_g139172 = Data3_g139169;
				float Alpha25_g139172 = _TintingBakeMode;
				if (Alpha25_g139172 < 0.5 )
				{
				Data25_g139172 = DataA25_g139172;
				}
				else
				{
				Data25_g139172 = DataB25_g139172;
				}
				TVEVisualData DataA25_g139196 = Data25_g139172;
				float localCompData3_g139176 = ( 0.0 );
				TVEVisualData Data3_g139176 = (TVEVisualData)0;
				half Dummy205_g139173 = ( _DrynessCategory + _DrynessEnd + _DrynessSpace + _DrynessElementMode + _DrynessBakeMode );
				float In_Dummy3_g139176 = Dummy205_g139173;
				float localBreakData4_g139175 = ( 0.0 );
				TVEVisualData Data4_g139175 = Data25_g139172;
				float Out_Dummy4_g139175 = 0;
				float3 Out_Albedo4_g139175 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139175 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139175 = float2( 0,0 );
				float3 Out_NormalWS4_g139175 = float3( 0,0,0 );
				float4 Out_Shader4_g139175 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139175 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139175 = 0;
				float Out_Grayscale4_g139175 = 0;
				float Out_Luminosity4_g139175 = 0;
				float Out_AlphaClip4_g139175 = 0;
				float Out_AlphaFade4_g139175 = 0;
				float3 Out_Translucency4_g139175 = float3( 0,0,0 );
				float Out_Transmission4_g139175 = 0;
				float Out_Thickness4_g139175 = 0;
				float Out_Diffusion4_g139175 = 0;
				Out_Dummy4_g139175 = Data4_g139175.Dummy;
				Out_Albedo4_g139175 = Data4_g139175.Albedo;
				Out_AlbedoRaw4_g139175 = Data4_g139175.AlbedoRaw;
				Out_NormalTS4_g139175 = Data4_g139175.NormalTS;
				Out_NormalWS4_g139175 = Data4_g139175.NormalWS;
				Out_Shader4_g139175 = Data4_g139175.Shader;
				Out_Emissive4_g139175= Data4_g139175.Emissive;
				Out_MultiMask4_g139175 = Data4_g139175.MultiMask;
				Out_Grayscale4_g139175 = Data4_g139175.Grayscale;
				Out_Luminosity4_g139175= Data4_g139175.Luminosity;
				Out_AlphaClip4_g139175 = Data4_g139175.AlphaClip;
				Out_AlphaFade4_g139175 = Data4_g139175.AlphaFade;
				Out_Translucency4_g139175 = Data4_g139175.Translucency;
				Out_Transmission4_g139175 = Data4_g139175.Transmission;
				Out_Thickness4_g139175 = Data4_g139175.Thickness;
				Out_Diffusion4_g139175 = Data4_g139175.Diffusion;
				half3 Visual_Albedo292_g139173 = Out_Albedo4_g139175;
				float temp_output_280_12_g139173 = Out_Grayscale4_g139175;
				half Visual_Grayscale308_g139173 = temp_output_280_12_g139173;
				float3 temp_cast_7 = (Visual_Grayscale308_g139173).xxx;
				TVEGlobalData Data15_g139174 = Data374;
				float Out_Dummy15_g139174 = 0;
				float4 Out_CoatParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139174 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139174 = float4( 0,0,0,0 );
				Out_Dummy15_g139174 = Data15_g139174.Dummy;
				Out_CoatParams15_g139174 = Data15_g139174.CoatParams;
				Out_PaintParams15_g139174 = Data15_g139174.PaintParams;
				Out_GlowParams15_g139174 = Data15_g139174.GlowParams;
				Out_AtmoParams15_g139174= Data15_g139174.AtmoParams;
				Out_FadeParams15_g139174= Data15_g139174.FadeParams;
				Out_FormParams15_g139174 = Data15_g139174.FormParams;
				Out_LandParams15_g139174 = Data15_g139174.LandParams;
				Out_WindParams15_g139174 = Data15_g139174.WindParams;
				Out_PushParams15_g139174 = Data15_g139174.PushParams;
				half4 Global_AtmoParams314_g139173 = Out_AtmoParams15_g139174;
				#ifdef TVE_DRYNESS_ELEMENT
				float staticSwitch351_g139173 = ( (Global_AtmoParams314_g139173).x * TVE_IsEnabled );
				#else
				float staticSwitch351_g139173 = 1.0;
				#endif
				half Dryness_GlobalMask352_g139173 = staticSwitch351_g139173;
				float3 lerpResult485_g139173 = lerp( Visual_Albedo292_g139173 , temp_cast_7 , ( Dryness_GlobalMask352_g139173 * _DrynessGrayValue ));
				half3 hsvTorgb58_g139194 = RGBToHSV( lerpResult485_g139173 );
				half3 hsvTorgb61_g139194 = HSVToRGB( half3(( hsvTorgb58_g139194.x + _DrynessShiftValue ),hsvTorgb58_g139194.y,hsvTorgb58_g139194.z) );
				#ifdef TVE_DRYNESS_SHIFT
				float3 staticSwitch499_g139173 = hsvTorgb61_g139194;
				#else
				float3 staticSwitch499_g139173 = lerpResult485_g139173;
				#endif
				#ifdef UNITY_COLORSPACE_GAMMA
				float staticSwitch1_g139193 = 2.0;
				#else
				float staticSwitch1_g139193 = 4.594794;
				#endif
				half Dryness_TexMask478_g139173 = 1.0;
				float temp_output_280_11_g139173 = Out_MultiMask4_g139175;
				half Visual_MultiMask310_g139173 = temp_output_280_11_g139173;
				float lerpResult283_g139173 = lerp( 1.0 , Visual_MultiMask310_g139173 , _DrynessMultiValue);
				half Dryness_MultiMask302_g139173 = lerpResult283_g139173;
				float temp_output_280_15_g139173 = Out_Luminosity4_g139175;
				half Visual_Luminosity309_g139173 = temp_output_280_15_g139173;
				float clampResult17_g139189 = clamp( Visual_Luminosity309_g139173 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139188 = _DrynessLumaRemap.x;
				float temp_output_10_0_g139188 = ( _DrynessLumaRemap.y - temp_output_7_0_g139188 );
				float lerpResult295_g139173 = lerp( 1.0 , saturate( ( ( clampResult17_g139189 - temp_output_7_0_g139188 ) / ( temp_output_10_0_g139188 + 0.0001 ) ) ) , _DrynessLumaValue);
				half Dryness_LumaMask301_g139173 = lerpResult295_g139173;
				TVEModelData Data15_g139195 = Data16_g76840;
				float Out_Dummy15_g139195 = 0;
				float3 Out_PositionWS15_g139195 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139195 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139195 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139195 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139195 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139195 = float3( 0,0,0 );
				float4 Out_VertexData15_g139195 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139195 = float4( 0,0,0,0 );
				Out_Dummy15_g139195 = Data15_g139195.Dummy;
				Out_PositionWS15_g139195 = Data15_g139195.PositionWS;
				Out_PositionWO15_g139195 = Data15_g139195.PositionWO;
				Out_PivotWS15_g139195 = Data15_g139195.PivotWS;
				Out_PivotWO15_g139195 = Data15_g139195.PivotWO;
				Out_NormalWS15_g139195 = Data15_g139195.NormalWS;
				Out_ViewDirWS15_g139195 = Data15_g139195.ViewDirWS;
				Out_VertexData15_g139195 = Data15_g139195.VertexData;
				Out_BoundsData15_g139195 = Data15_g139195.BoundsData;
				half4 Model_VertexMasks386_g139173 = Out_VertexData15_g139195;
				float4 break375_g139173 = Model_VertexMasks386_g139173;
				float4 break33_g139182 = _dryness_vert_mode;
				float temp_output_30_0_g139182 = ( break375_g139173.x * break33_g139182.x );
				float temp_output_29_0_g139182 = ( break375_g139173.y * break33_g139182.y );
				float temp_output_31_0_g139182 = ( break375_g139173.z * break33_g139182.z );
				float temp_output_28_0_g139182 = ( temp_output_30_0_g139182 + temp_output_29_0_g139182 + temp_output_31_0_g139182 + ( break375_g139173.w * break33_g139182.w ) );
				float clampResult17_g139183 = clamp( temp_output_28_0_g139182 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139184 = _DrynessMeshRemap.x;
				float temp_output_10_0_g139184 = ( _DrynessMeshRemap.y - temp_output_7_0_g139184 );
				float lerpResult452_g139173 = lerp( 1.0 , saturate( ( ( clampResult17_g139183 - temp_output_7_0_g139184 ) / ( temp_output_10_0_g139184 + 0.0001 ) ) ) , _DrynessMeshValue);
				float temp_output_6_0_g139190 = lerpResult452_g139173;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139190 = ( temp_output_6_0_g139190 + ( _DrynessMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139190 = temp_output_6_0_g139190;
				#endif
				float temp_output_448_0_g139173 = staticSwitch14_g139190;
				half Dryness_VertMask378_g139173 = temp_output_448_0_g139173;
				half Dryness_MaskNoise291_g139173 = 1.0;
				half Custom_Mask411_g139173 = 1.0;
				float temp_output_7_0_g139187 = _DrynessBlendRemap.x;
				float temp_output_10_0_g139187 = ( _DrynessBlendRemap.y - temp_output_7_0_g139187 );
				half Dryness_Mask329_g139173 = saturate( ( ( ( _DrynessIntensityValue * Dryness_TexMask478_g139173 * Dryness_MultiMask302_g139173 * Dryness_LumaMask301_g139173 * Dryness_VertMask378_g139173 * Dryness_MaskNoise291_g139173 * Dryness_GlobalMask352_g139173 * Custom_Mask411_g139173 ) - temp_output_7_0_g139187 ) / ( temp_output_10_0_g139187 + 0.0001 ) ) );
				float3 lerpResult336_g139173 = lerp( Visual_Albedo292_g139173 , ( staticSwitch499_g139173 * staticSwitch1_g139193 * (_DrynessColor).rgb ) , Dryness_Mask329_g139173);
				#ifdef TVE_DRYNESS
				float3 staticSwitch356_g139173 = lerpResult336_g139173;
				#else
				float3 staticSwitch356_g139173 = Visual_Albedo292_g139173;
				#endif
				half3 Final_Albedo331_g139173 = staticSwitch356_g139173;
				float3 In_Albedo3_g139176 = Final_Albedo331_g139173;
				float3 In_AlbedoRaw3_g139176 = Out_AlbedoRaw4_g139175;
				float2 In_NormalTS3_g139176 = Out_NormalTS4_g139175;
				float3 In_NormalWS3_g139176 = Out_NormalWS4_g139175;
				half4 Visual_Shader415_g139173 = Out_Shader4_g139175;
				float4 break438_g139173 = Visual_Shader415_g139173;
				float4 appendResult439_g139173 = (float4(break438_g139173.x , break438_g139173.y , break438_g139173.z , ( break438_g139173.w * _DrynessSmoothnessValue )));
				float4 lerpResult427_g139173 = lerp( Visual_Shader415_g139173 , appendResult439_g139173 , Dryness_Mask329_g139173);
				#ifdef TVE_DRYNESS
				float4 staticSwitch426_g139173 = lerpResult427_g139173;
				#else
				float4 staticSwitch426_g139173 = Visual_Shader415_g139173;
				#endif
				half4 Final_Shader433_g139173 = staticSwitch426_g139173;
				float4 In_Shader3_g139176 = Final_Shader433_g139173;
				float4 In_Emissive3_g139176 = Out_Emissive4_g139175;
				float In_Grayscale3_g139176 = temp_output_280_12_g139173;
				float In_Luminosity3_g139176 = temp_output_280_15_g139173;
				float In_MultiMask3_g139176 = temp_output_280_11_g139173;
				float In_AlphaClip3_g139176 = Out_AlphaClip4_g139175;
				float In_AlphaFade3_g139176 = Out_AlphaFade4_g139175;
				float3 In_Translucency3_g139176 = Out_Translucency4_g139175;
				half Visual_Transmission416_g139173 = Out_Transmission4_g139175;
				float lerpResult421_g139173 = lerp( Visual_Transmission416_g139173 , ( Visual_Transmission416_g139173 * _DrynessSubsurfaceValue ) , Dryness_Mask329_g139173);
				#ifdef TVE_DRYNESS
				float staticSwitch418_g139173 = lerpResult421_g139173;
				#else
				float staticSwitch418_g139173 = Visual_Transmission416_g139173;
				#endif
				half Final_Transmission425_g139173 = staticSwitch418_g139173;
				float In_Transmission3_g139176 = Final_Transmission425_g139173;
				float In_Thickness3_g139176 = Out_Thickness4_g139175;
				float In_Diffusion3_g139176 = Out_Diffusion4_g139175;
				Data3_g139176.Dummy = In_Dummy3_g139176;
				Data3_g139176.Albedo = In_Albedo3_g139176;
				Data3_g139176.AlbedoRaw = In_AlbedoRaw3_g139176;
				Data3_g139176.NormalTS = In_NormalTS3_g139176;
				Data3_g139176.NormalWS = In_NormalWS3_g139176;
				Data3_g139176.Shader = In_Shader3_g139176;
				Data3_g139176.Emissive= In_Emissive3_g139176;
				Data3_g139176.MultiMask = In_MultiMask3_g139176;
				Data3_g139176.Grayscale = In_Grayscale3_g139176;
				Data3_g139176.Luminosity = In_Luminosity3_g139176;
				Data3_g139176.AlphaClip = In_AlphaClip3_g139176;
				Data3_g139176.AlphaFade = In_AlphaFade3_g139176;
				Data3_g139176.Translucency = In_Translucency3_g139176;
				Data3_g139176.Transmission = In_Transmission3_g139176;
				Data3_g139176.Thickness = In_Thickness3_g139176;
				Data3_g139176.Diffusion = In_Diffusion3_g139176;
				TVEVisualData DataB25_g139196 = Data3_g139176;
				float Alpha25_g139196 = _DrynessBakeMode;
				if (Alpha25_g139196 < 0.5 )
				{
				Data25_g139196 = DataA25_g139196;
				}
				else
				{
				Data25_g139196 = DataB25_g139196;
				}
				TVEVisualData DataA25_g139233 = Data25_g139196;
				float localCompData3_g139200 = ( 0.0 );
				TVEVisualData Data3_g139200 = (TVEVisualData)0;
				half Dummy594_g139197 = ( _OverlayCategory + _OverlayEnd + _OverlaySpace + _OverlayElementMode + _OverlayBakeMode );
				float In_Dummy3_g139200 = Dummy594_g139197;
				float localBreakData4_g139198 = ( 0.0 );
				TVEVisualData Data4_g139198 = Data25_g139196;
				float Out_Dummy4_g139198 = 0;
				float3 Out_Albedo4_g139198 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139198 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139198 = float2( 0,0 );
				float3 Out_NormalWS4_g139198 = float3( 0,0,0 );
				float4 Out_Shader4_g139198 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139198 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139198 = 0;
				float Out_Grayscale4_g139198 = 0;
				float Out_Luminosity4_g139198 = 0;
				float Out_AlphaClip4_g139198 = 0;
				float Out_AlphaFade4_g139198 = 0;
				float3 Out_Translucency4_g139198 = float3( 0,0,0 );
				float Out_Transmission4_g139198 = 0;
				float Out_Thickness4_g139198 = 0;
				float Out_Diffusion4_g139198 = 0;
				Out_Dummy4_g139198 = Data4_g139198.Dummy;
				Out_Albedo4_g139198 = Data4_g139198.Albedo;
				Out_AlbedoRaw4_g139198 = Data4_g139198.AlbedoRaw;
				Out_NormalTS4_g139198 = Data4_g139198.NormalTS;
				Out_NormalWS4_g139198 = Data4_g139198.NormalWS;
				Out_Shader4_g139198 = Data4_g139198.Shader;
				Out_Emissive4_g139198= Data4_g139198.Emissive;
				Out_MultiMask4_g139198 = Data4_g139198.MultiMask;
				Out_Grayscale4_g139198 = Data4_g139198.Grayscale;
				Out_Luminosity4_g139198= Data4_g139198.Luminosity;
				Out_AlphaClip4_g139198 = Data4_g139198.AlphaClip;
				Out_AlphaFade4_g139198 = Data4_g139198.AlphaFade;
				Out_Translucency4_g139198 = Data4_g139198.Translucency;
				Out_Transmission4_g139198 = Data4_g139198.Transmission;
				Out_Thickness4_g139198 = Data4_g139198.Thickness;
				Out_Diffusion4_g139198 = Data4_g139198.Diffusion;
				half3 Visual_Albedo127_g139197 = Out_Albedo4_g139198;
				float3 temp_output_622_0_g139197 = (_OverlayColor).rgb;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139201) = _OverlayAlbedoTex;
				float localFilterTexture19_g139230 = ( 0.0 );
				SamplerState SamplerDefault19_g139230 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerPoint19_g139230 = sampler_Point_Repeat;
				SamplerState SamplerLow19_g139230 = sampler_Linear_Repeat;
				SamplerState SamplerMedium19_g139230 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh19_g139230 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS19_g139230 = SamplerDefault19_g139230;
				#if defined (TVE_FILTER_DEFAULT)
				    SS19_g139230 = SamplerDefault19_g139230;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS19_g139230 = SamplerPoint19_g139230;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS19_g139230 = SamplerLow19_g139230;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS19_g139230 = SamplerMedium19_g139230;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS19_g139230 = SamplerHigh19_g139230;
				#endif
				SamplerState Sampler238_g139201 = SS19_g139230;
				float4 temp_output_6_0_g139202 = _overlay_coord_value;
				#ifdef TVE_REGISTER
				float4 staticSwitch14_g139202 = ( temp_output_6_0_g139202 + ( ( _OverlaySampleMode + _OverlayCoordMode + _OverlayCoordValue ) * float4( 0,0,0,0 ) ) );
				#else
				float4 staticSwitch14_g139202 = temp_output_6_0_g139202;
				#endif
				half4 Overlay_Coords639_g139197 = staticSwitch14_g139202;
				float4 temp_output_37_0_g139201 = Overlay_Coords639_g139197;
				half4 Coords238_g139201 = temp_output_37_0_g139201;
				TVEModelData Data15_g139232 = Data16_g76840;
				float Out_Dummy15_g139232 = 0;
				float3 Out_PositionWS15_g139232 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139232 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139232 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139232 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139232 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139232 = float3( 0,0,0 );
				float4 Out_VertexData15_g139232 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139232 = float4( 0,0,0,0 );
				Out_Dummy15_g139232 = Data15_g139232.Dummy;
				Out_PositionWS15_g139232 = Data15_g139232.PositionWS;
				Out_PositionWO15_g139232 = Data15_g139232.PositionWO;
				Out_PivotWS15_g139232 = Data15_g139232.PivotWS;
				Out_PivotWO15_g139232 = Data15_g139232.PivotWO;
				Out_NormalWS15_g139232 = Data15_g139232.NormalWS;
				Out_ViewDirWS15_g139232 = Data15_g139232.ViewDirWS;
				Out_VertexData15_g139232 = Data15_g139232.VertexData;
				Out_BoundsData15_g139232 = Data15_g139232.BoundsData;
				half3 Model_PositionWO602_g139197 = Out_PositionWO15_g139232;
				float3 temp_output_279_0_g139201 = Model_PositionWO602_g139197;
				half3 WorldPosition238_g139201 = temp_output_279_0_g139201;
				half4 localSamplePlanar2D238_g139201 = SamplePlanar2D( Texture238_g139201 , Sampler238_g139201 , Coords238_g139201 , WorldPosition238_g139201 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139201) = _OverlayAlbedoTex;
				SamplerState Sampler246_g139201 = SS19_g139230;
				half4 Coords246_g139201 = temp_output_37_0_g139201;
				half3 WorldPosition246_g139201 = temp_output_279_0_g139201;
				half3 Model_NormalWS712_g139197 = Out_NormalWS15_g139232;
				float3 temp_output_280_0_g139201 = Model_NormalWS712_g139197;
				half3 WorldNormal246_g139201 = temp_output_280_0_g139201;
				half4 localSamplePlanar3D246_g139201 = SamplePlanar3D( Texture246_g139201 , Sampler246_g139201 , Coords246_g139201 , WorldPosition246_g139201 , WorldNormal246_g139201 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139201) = _OverlayAlbedoTex;
				SamplerState Sampler234_g139201 = SS19_g139230;
				float4 Coords234_g139201 = temp_output_37_0_g139201;
				float3 WorldPosition234_g139201 = temp_output_279_0_g139201;
				float4 localSampleStochastic2D234_g139201 = SampleStochastic2D( Texture234_g139201 , Sampler234_g139201 , Coords234_g139201 , WorldPosition234_g139201 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139201) = _OverlayAlbedoTex;
				SamplerState Sampler263_g139201 = SS19_g139230;
				half4 Coords263_g139201 = temp_output_37_0_g139201;
				half3 WorldPosition263_g139201 = temp_output_279_0_g139201;
				half3 WorldNormal263_g139201 = temp_output_280_0_g139201;
				half4 localSampleStochastic3D263_g139201 = SampleStochastic3D( Texture263_g139201 , Sampler263_g139201 , Coords263_g139201 , WorldPosition263_g139201 , WorldNormal263_g139201 );
				#if defined( TVE_OVERLAY_SAMPLE_PLANAR_2D )
				float4 staticSwitch676_g139197 = localSamplePlanar2D238_g139201;
				#elif defined( TVE_OVERLAY_SAMPLE_PLANAR_3D )
				float4 staticSwitch676_g139197 = localSamplePlanar3D246_g139201;
				#elif defined( TVE_OVERLAY_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch676_g139197 = localSampleStochastic2D234_g139201;
				#elif defined( TVE_OVERLAY_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch676_g139197 = localSampleStochastic3D263_g139201;
				#else
				float4 staticSwitch676_g139197 = localSamplePlanar2D238_g139201;
				#endif
				half3 Overlay_Albedo526_g139197 = (staticSwitch676_g139197).xyz;
				#ifdef TVE_OVERLAY_TEX
				float3 staticSwitch578_g139197 = ( temp_output_622_0_g139197 * Overlay_Albedo526_g139197 );
				#else
				float3 staticSwitch578_g139197 = temp_output_622_0_g139197;
				#endif
				float3 temp_output_6_0_g139208 = staticSwitch578_g139197;
				#ifdef TVE_REGISTER
				float3 staticSwitch14_g139208 = ( temp_output_6_0_g139208 + ( _OverlayTextureMode * 0.0 ) );
				#else
				float3 staticSwitch14_g139208 = temp_output_6_0_g139208;
				#endif
				float3 temp_cast_8 = (0.0).xxx;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139205) = _OverlayGlitterTexRT;
				SamplerState Sampler246_g139205 = sampler_Linear_Repeat;
				float4 appendResult863_g139197 = (float4(_OverlayGlitterTillingValue , _OverlayGlitterTillingValue , 0.0 , 0.0));
				float4 temp_output_37_0_g139205 = appendResult863_g139197;
				half4 Coords246_g139205 = temp_output_37_0_g139205;
				float3 temp_output_279_0_g139205 = Model_PositionWO602_g139197;
				half3 WorldPosition246_g139205 = temp_output_279_0_g139205;
				float3 temp_output_280_0_g139205 = Model_NormalWS712_g139197;
				half3 WorldNormal246_g139205 = temp_output_280_0_g139205;
				half4 localSamplePlanar3D246_g139205 = SamplePlanar3D( Texture246_g139205 , Sampler246_g139205 , Coords246_g139205 , WorldPosition246_g139205 , WorldNormal246_g139205 );
				half Overlay_GlitterTex854_g139197 = (localSamplePlanar3D246_g139205).x;
				half3 Model_PositionWS879_g139197 = Out_PositionWS15_g139232;
				#ifdef TVE_OVERLAY_GLITTER
				float3 staticSwitch868_g139197 = ( _OverlayGlitterIntensityValue * (_OverlayGlitterColor).rgb * Overlay_GlitterTex854_g139197 * ( 1.0 - saturate( ( distance( _WorldSpaceCameraPos , Model_PositionWS879_g139197 ) / _OverlayGlitterDistValue ) ) ) );
				#else
				float3 staticSwitch868_g139197 = temp_cast_8;
				#endif
				half3 Overlay_GlitterColor865_g139197 = staticSwitch868_g139197;
				half Overlay_TexMask908_g139197 = 1.0;
				float3 temp_output_739_21_g139197 = Out_NormalWS4_g139198;
				half3 Visual_NormalWS749_g139197 = temp_output_739_21_g139197;
				float clampResult17_g139218 = clamp( saturate( (Visual_NormalWS749_g139197).y ) , 0.0001 , 0.9999 );
				float temp_output_7_0_g139217 = _OverlayProjRemap.x;
				float temp_output_10_0_g139217 = ( _OverlayProjRemap.y - temp_output_7_0_g139217 );
				float lerpResult842_g139197 = lerp( 1.0 , saturate( ( ( clampResult17_g139218 - temp_output_7_0_g139217 ) / ( temp_output_10_0_g139217 + 0.0001 ) ) ) , _OverlayProjValue);
				half Overlay_MaskProj457_g139197 = lerpResult842_g139197;
				float temp_output_739_15_g139197 = Out_Luminosity4_g139198;
				half Visual_Luminosity654_g139197 = temp_output_739_15_g139197;
				float clampResult17_g139220 = clamp( Visual_Luminosity654_g139197 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139219 = _OverlayLumaRemap.x;
				float temp_output_10_0_g139219 = ( _OverlayLumaRemap.y - temp_output_7_0_g139219 );
				float lerpResult587_g139197 = lerp( 1.0 , saturate( ( ( clampResult17_g139220 - temp_output_7_0_g139219 ) / ( temp_output_10_0_g139219 + 0.0001 ) ) ) , _OverlayLumaValue);
				half Overlay_MaskLuma438_g139197 = lerpResult587_g139197;
				half4 Model_VertexMasks791_g139197 = Out_VertexData15_g139232;
				float4 break792_g139197 = Model_VertexMasks791_g139197;
				float4 break33_g139210 = _overlay_vert_mode;
				float temp_output_30_0_g139210 = ( break792_g139197.x * break33_g139210.x );
				float temp_output_29_0_g139210 = ( break792_g139197.y * break33_g139210.y );
				float temp_output_31_0_g139210 = ( break792_g139197.z * break33_g139210.z );
				float temp_output_28_0_g139210 = ( temp_output_30_0_g139210 + temp_output_29_0_g139210 + temp_output_31_0_g139210 + ( break792_g139197.w * break33_g139210.w ) );
				float clampResult17_g139211 = clamp( temp_output_28_0_g139210 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139214 = _OverlayMeshRemap.x;
				float temp_output_10_0_g139214 = ( _OverlayMeshRemap.y - temp_output_7_0_g139214 );
				float lerpResult881_g139197 = lerp( 1.0 , saturate( ( ( clampResult17_g139211 - temp_output_7_0_g139214 ) / ( temp_output_10_0_g139214 + 0.0001 ) ) ) , _OverlayMeshValue);
				float temp_output_6_0_g139221 = lerpResult881_g139197;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139221 = ( temp_output_6_0_g139221 + ( _OverlayMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139221 = temp_output_6_0_g139221;
				#endif
				float temp_output_831_0_g139197 = staticSwitch14_g139221;
				half Overlay_VertMask801_g139197 = temp_output_831_0_g139197;
				half Overlay_MaskNoise427_g139197 = 1.0;
				TVEGlobalData Data15_g139199 = Data374;
				float Out_Dummy15_g139199 = 0;
				float4 Out_CoatParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139199 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139199 = float4( 0,0,0,0 );
				Out_Dummy15_g139199 = Data15_g139199.Dummy;
				Out_CoatParams15_g139199 = Data15_g139199.CoatParams;
				Out_PaintParams15_g139199 = Data15_g139199.PaintParams;
				Out_GlowParams15_g139199 = Data15_g139199.GlowParams;
				Out_AtmoParams15_g139199= Data15_g139199.AtmoParams;
				Out_FadeParams15_g139199= Data15_g139199.FadeParams;
				Out_FormParams15_g139199 = Data15_g139199.FormParams;
				Out_LandParams15_g139199 = Data15_g139199.LandParams;
				Out_WindParams15_g139199 = Data15_g139199.WindParams;
				Out_PushParams15_g139199 = Data15_g139199.PushParams;
				half4 Global_AtmoParams516_g139197 = Out_AtmoParams15_g139199;
				#ifdef TVE_OVERLAY_ELEMENT
				float staticSwitch705_g139197 = ( (Global_AtmoParams516_g139197).z * TVE_IsEnabled );
				#else
				float staticSwitch705_g139197 = 1.0;
				#endif
				half Overlay_MaskGlobal429_g139197 = staticSwitch705_g139197;
				half Custom_Mask646_g139197 = 1.0;
				float temp_output_7_0_g139226 = _OverlayBlendRemap1.x;
				float temp_output_10_0_g139226 = ( _OverlayBlendRemap1.y - temp_output_7_0_g139226 );
				half Overlay_Mask494_g139197 = saturate( ( ( ( _OverlayIntensityValue * Overlay_TexMask908_g139197 * Overlay_MaskProj457_g139197 * Overlay_MaskLuma438_g139197 * Overlay_VertMask801_g139197 * Overlay_MaskNoise427_g139197 * Overlay_MaskGlobal429_g139197 * Custom_Mask646_g139197 ) - temp_output_7_0_g139226 ) / ( temp_output_10_0_g139226 + 0.0001 ) ) );
				float3 lerpResult467_g139197 = lerp( Visual_Albedo127_g139197 , ( staticSwitch14_g139208 + Overlay_GlitterColor865_g139197 ) , Overlay_Mask494_g139197);
				#ifdef TVE_OVERLAY
				float3 staticSwitch577_g139197 = lerpResult467_g139197;
				#else
				float3 staticSwitch577_g139197 = Visual_Albedo127_g139197;
				#endif
				half3 Final_Albedo493_g139197 = staticSwitch577_g139197;
				float3 In_Albedo3_g139200 = Final_Albedo493_g139197;
				float3 In_AlbedoRaw3_g139200 = Out_AlbedoRaw4_g139198;
				half2 Visual_NormalTS535_g139197 = Out_NormalTS4_g139198;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture238_g139203) = _OverlayNormalTex;
				float localFilterTexture29_g139231 = ( 0.0 );
				SamplerState SamplerDefault29_g139231 = sampler_Linear_Repeat;
				SamplerState SamplerPoint29_g139231 = sampler_Point_Repeat;
				SamplerState SamplerLow29_g139231 = sampler_Linear_Repeat;
				SamplerState SamplerMedium29_g139231 = sampler_Linear_Repeat_Aniso8;
				SamplerState SamplerHigh29_g139231 = sampler_Linear_Repeat_Aniso8;
				SamplerState SS29_g139231 = SamplerDefault29_g139231;
				#if defined (TVE_FILTER_DEFAULT)
				    SS29_g139231 = SamplerDefault29_g139231;
				#endif
				#if defined (TVE_FILTER_POINT)
				    SS29_g139231 = SamplerPoint29_g139231;
				#endif
				#if defined (TVE_FILTER_LOW)
				    SS29_g139231 = SamplerLow29_g139231;
				#endif
				#if defined (TVE_FILTER_MEDIUM)
				    SS29_g139231 = SamplerMedium29_g139231;
				#endif
				#if defined (TVE_FILTER_HIGH)
				    SS29_g139231 = SamplerHigh29_g139231;
				#endif
				SamplerState Sampler238_g139203 = SS29_g139231;
				float4 temp_output_37_0_g139203 = Overlay_Coords639_g139197;
				half4 Coords238_g139203 = temp_output_37_0_g139203;
				float3 temp_output_279_0_g139203 = Model_PositionWO602_g139197;
				half3 WorldPosition238_g139203 = temp_output_279_0_g139203;
				half4 localSamplePlanar2D238_g139203 = SamplePlanar2D( Texture238_g139203 , Sampler238_g139203 , Coords238_g139203 , WorldPosition238_g139203 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture246_g139203) = _OverlayNormalTex;
				SamplerState Sampler246_g139203 = SS29_g139231;
				half4 Coords246_g139203 = temp_output_37_0_g139203;
				half3 WorldPosition246_g139203 = temp_output_279_0_g139203;
				float3 temp_output_280_0_g139203 = Model_NormalWS712_g139197;
				half3 WorldNormal246_g139203 = temp_output_280_0_g139203;
				half4 localSamplePlanar3D246_g139203 = SamplePlanar3D( Texture246_g139203 , Sampler246_g139203 , Coords246_g139203 , WorldPosition246_g139203 , WorldNormal246_g139203 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture234_g139203) = _OverlayNormalTex;
				SamplerState Sampler234_g139203 = SS29_g139231;
				float4 Coords234_g139203 = temp_output_37_0_g139203;
				float3 WorldPosition234_g139203 = temp_output_279_0_g139203;
				float4 localSampleStochastic2D234_g139203 = SampleStochastic2D( Texture234_g139203 , Sampler234_g139203 , Coords234_g139203 , WorldPosition234_g139203 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture263_g139203) = _OverlayNormalTex;
				SamplerState Sampler263_g139203 = SS29_g139231;
				half4 Coords263_g139203 = temp_output_37_0_g139203;
				half3 WorldPosition263_g139203 = temp_output_279_0_g139203;
				half3 WorldNormal263_g139203 = temp_output_280_0_g139203;
				half4 localSampleStochastic3D263_g139203 = SampleStochastic3D( Texture263_g139203 , Sampler263_g139203 , Coords263_g139203 , WorldPosition263_g139203 , WorldNormal263_g139203 );
				#if defined( TVE_OVERLAY_SAMPLE_PLANAR_2D )
				float4 staticSwitch686_g139197 = localSamplePlanar2D238_g139203;
				#elif defined( TVE_OVERLAY_SAMPLE_PLANAR_3D )
				float4 staticSwitch686_g139197 = localSamplePlanar3D246_g139203;
				#elif defined( TVE_OVERLAY_SAMPLE_STOCHASTIC_2D )
				float4 staticSwitch686_g139197 = localSampleStochastic2D234_g139203;
				#elif defined( TVE_OVERLAY_SAMPLE_STOCHASTIC_3D )
				float4 staticSwitch686_g139197 = localSampleStochastic3D263_g139203;
				#else
				float4 staticSwitch686_g139197 = localSamplePlanar2D238_g139203;
				#endif
				half4 Normal_Packed45_g139207 = staticSwitch686_g139197;
				float2 appendResult58_g139207 = (float2(( (Normal_Packed45_g139207).x * (Normal_Packed45_g139207).w ) , (Normal_Packed45_g139207).y));
				half2 Normal_Default50_g139207 = appendResult58_g139207;
				half2 Normal_ASTC41_g139207 = (Normal_Packed45_g139207).xy;
				#ifdef UNITY_ASTC_NORMALMAP_ENCODING
				float2 staticSwitch38_g139207 = Normal_ASTC41_g139207;
				#else
				float2 staticSwitch38_g139207 = Normal_Default50_g139207;
				#endif
				half2 Normal_NO_DTX544_g139207 = (Normal_Packed45_g139207).wy;
				#ifdef UNITY_NO_DXT5nm
				float2 staticSwitch37_g139207 = Normal_NO_DTX544_g139207;
				#else
				float2 staticSwitch37_g139207 = staticSwitch38_g139207;
				#endif
				half2 Normal_Planar45_g139206 = (staticSwitch37_g139207*2.0 + -1.0);
				float2 break71_g139206 = Normal_Planar45_g139206;
				float3 appendResult72_g139206 = (float3(break71_g139206.x , 0.0 , break71_g139206.y));
				half2 Overlay_Normal528_g139197 = (mul( ase_worldToTangent, appendResult72_g139206 )).xy;
				#ifdef TVE_OVERLAY_TEX
				float2 staticSwitch579_g139197 = Overlay_Normal528_g139197;
				#else
				float2 staticSwitch579_g139197 = Visual_NormalTS535_g139197;
				#endif
				float2 lerpResult551_g139197 = lerp( Visual_NormalTS535_g139197 , ( staticSwitch579_g139197 * _OverlayNormalValue ) , Overlay_Mask494_g139197);
				#ifdef TVE_OVERLAY
				float2 staticSwitch583_g139197 = lerpResult551_g139197;
				#else
				float2 staticSwitch583_g139197 = Visual_NormalTS535_g139197;
				#endif
				half2 Final_NormalTS499_g139197 = staticSwitch583_g139197;
				float2 In_NormalTS3_g139200 = Final_NormalTS499_g139197;
				float3 In_NormalWS3_g139200 = temp_output_739_21_g139197;
				half4 Visual_Masks536_g139197 = Out_Shader4_g139198;
				float4 appendResult585_g139197 = (float4(0.0 , 1.0 , 0.0 , _OverlaySmoothnessValue));
				float4 lerpResult584_g139197 = lerp( Visual_Masks536_g139197 , appendResult585_g139197 , Overlay_Mask494_g139197);
				#ifdef TVE_OVERLAY
				float4 staticSwitch586_g139197 = lerpResult584_g139197;
				#else
				float4 staticSwitch586_g139197 = Visual_Masks536_g139197;
				#endif
				half4 Final_Masks482_g139197 = staticSwitch586_g139197;
				float4 In_Shader3_g139200 = Final_Masks482_g139197;
				float4 In_Emissive3_g139200 = Out_Emissive4_g139198;
				float temp_output_739_12_g139197 = Out_Grayscale4_g139198;
				float In_Grayscale3_g139200 = temp_output_739_12_g139197;
				float In_Luminosity3_g139200 = temp_output_739_15_g139197;
				float In_MultiMask3_g139200 = Out_MultiMask4_g139198;
				float In_AlphaClip3_g139200 = Out_AlphaClip4_g139198;
				float In_AlphaFade3_g139200 = Out_AlphaFade4_g139198;
				float3 In_Translucency3_g139200 = Out_Translucency4_g139198;
				half Visual_Transmission699_g139197 = Out_Transmission4_g139198;
				float lerpResult746_g139197 = lerp( Visual_Transmission699_g139197 , ( Visual_Transmission699_g139197 * _OverlaySubsurfaceValue ) , ( Overlay_VertMask801_g139197 * Overlay_MaskNoise427_g139197 * Overlay_MaskGlobal429_g139197 ));
				#ifdef TVE_OVERLAY
				float staticSwitch703_g139197 = lerpResult746_g139197;
				#else
				float staticSwitch703_g139197 = Visual_Transmission699_g139197;
				#endif
				half Final_Transmission702_g139197 = staticSwitch703_g139197;
				float In_Transmission3_g139200 = Final_Transmission702_g139197;
				float In_Thickness3_g139200 = Out_Thickness4_g139198;
				float In_Diffusion3_g139200 = Out_Diffusion4_g139198;
				Data3_g139200.Dummy = In_Dummy3_g139200;
				Data3_g139200.Albedo = In_Albedo3_g139200;
				Data3_g139200.AlbedoRaw = In_AlbedoRaw3_g139200;
				Data3_g139200.NormalTS = In_NormalTS3_g139200;
				Data3_g139200.NormalWS = In_NormalWS3_g139200;
				Data3_g139200.Shader = In_Shader3_g139200;
				Data3_g139200.Emissive= In_Emissive3_g139200;
				Data3_g139200.MultiMask = In_MultiMask3_g139200;
				Data3_g139200.Grayscale = In_Grayscale3_g139200;
				Data3_g139200.Luminosity = In_Luminosity3_g139200;
				Data3_g139200.AlphaClip = In_AlphaClip3_g139200;
				Data3_g139200.AlphaFade = In_AlphaFade3_g139200;
				Data3_g139200.Translucency = In_Translucency3_g139200;
				Data3_g139200.Transmission = In_Transmission3_g139200;
				Data3_g139200.Thickness = In_Thickness3_g139200;
				Data3_g139200.Diffusion = In_Diffusion3_g139200;
				TVEVisualData DataB25_g139233 = Data3_g139200;
				float Alpha25_g139233 = _OverlayBakeMode;
				if (Alpha25_g139233 < 0.5 )
				{
				Data25_g139233 = DataA25_g139233;
				}
				else
				{
				Data25_g139233 = DataB25_g139233;
				}
				TVEVisualData DataA25_g139263 = Data25_g139233;
				float localCompData3_g139236 = ( 0.0 );
				TVEVisualData Data3_g139236 = (TVEVisualData)0;
				half Dummy594_g139234 = ( _WetnessCategory + _WetnessEnd + _WetnessElementMode + _WetnessBakeMode );
				float In_Dummy3_g139236 = Dummy594_g139234;
				float localBreakData4_g139235 = ( 0.0 );
				TVEVisualData Data4_g139235 = Data25_g139233;
				float Out_Dummy4_g139235 = 0;
				float3 Out_Albedo4_g139235 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139235 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139235 = float2( 0,0 );
				float3 Out_NormalWS4_g139235 = float3( 0,0,0 );
				float4 Out_Shader4_g139235 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139235 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139235 = 0;
				float Out_Grayscale4_g139235 = 0;
				float Out_Luminosity4_g139235 = 0;
				float Out_AlphaClip4_g139235 = 0;
				float Out_AlphaFade4_g139235 = 0;
				float3 Out_Translucency4_g139235 = float3( 0,0,0 );
				float Out_Transmission4_g139235 = 0;
				float Out_Thickness4_g139235 = 0;
				float Out_Diffusion4_g139235 = 0;
				Out_Dummy4_g139235 = Data4_g139235.Dummy;
				Out_Albedo4_g139235 = Data4_g139235.Albedo;
				Out_AlbedoRaw4_g139235 = Data4_g139235.AlbedoRaw;
				Out_NormalTS4_g139235 = Data4_g139235.NormalTS;
				Out_NormalWS4_g139235 = Data4_g139235.NormalWS;
				Out_Shader4_g139235 = Data4_g139235.Shader;
				Out_Emissive4_g139235= Data4_g139235.Emissive;
				Out_MultiMask4_g139235 = Data4_g139235.MultiMask;
				Out_Grayscale4_g139235 = Data4_g139235.Grayscale;
				Out_Luminosity4_g139235= Data4_g139235.Luminosity;
				Out_AlphaClip4_g139235 = Data4_g139235.AlphaClip;
				Out_AlphaFade4_g139235 = Data4_g139235.AlphaFade;
				Out_Translucency4_g139235 = Data4_g139235.Translucency;
				Out_Transmission4_g139235 = Data4_g139235.Transmission;
				Out_Thickness4_g139235 = Data4_g139235.Thickness;
				Out_Diffusion4_g139235 = Data4_g139235.Diffusion;
				half3 Visual_Albedo127_g139234 = Out_Albedo4_g139235;
				TVEGlobalData Data15_g139240 = Data374;
				float Out_Dummy15_g139240 = 0;
				float4 Out_CoatParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139240 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139240 = float4( 0,0,0,0 );
				Out_Dummy15_g139240 = Data15_g139240.Dummy;
				Out_CoatParams15_g139240 = Data15_g139240.CoatParams;
				Out_PaintParams15_g139240 = Data15_g139240.PaintParams;
				Out_GlowParams15_g139240 = Data15_g139240.GlowParams;
				Out_AtmoParams15_g139240= Data15_g139240.AtmoParams;
				Out_FadeParams15_g139240= Data15_g139240.FadeParams;
				Out_FormParams15_g139240 = Data15_g139240.FormParams;
				Out_LandParams15_g139240 = Data15_g139240.LandParams;
				Out_WindParams15_g139240 = Data15_g139240.WindParams;
				Out_PushParams15_g139240 = Data15_g139240.PushParams;
				half4 Global_AtmoParams516_g139234 = Out_AtmoParams15_g139240;
				#ifdef TVE_WETNESS_ELEMENT
				float staticSwitch663_g139234 = ( (Global_AtmoParams516_g139234).y * TVE_IsEnabled );
				#else
				float staticSwitch663_g139234 = 1.0;
				#endif
				half Global_Wetness429_g139234 = staticSwitch663_g139234;
				half Wetness_Value1042_g139234 = ( _WetnessIntensityValue * Global_Wetness429_g139234 );
				half Wetness_VertMask1024_g139234 = 1.0;
				float temp_output_1043_0_g139234 = ( Wetness_Value1042_g139234 * Wetness_VertMask1024_g139234 );
				half Wetness_Mask866_g139234 = temp_output_1043_0_g139234;
				half Water_VertMask1094_g139234 = 1.0;
				half4 Visual_Masks536_g139234 = Out_Shader4_g139235;
				float lerpResult1013_g139234 = lerp( 1.0 , (Visual_Masks536_g139234).z , _WetnessWaterBaseValue);
				half Water_HeightMask782_g139234 = lerpResult1013_g139234;
				float clampResult17_g139249 = clamp( ( ( _WetnessWaterIntensityValue * Wetness_Mask866_g139234 * Water_VertMask1094_g139234 ) - Water_HeightMask782_g139234 ) , 0.0001 , 0.9999 );
				float temp_output_7_0_g139250 = _WetnessWaterBlendRemap.x;
				float temp_output_10_0_g139250 = ( _WetnessWaterBlendRemap.y - temp_output_7_0_g139250 );
				TVEModelData Data15_g139248 = Data16_g76840;
				float Out_Dummy15_g139248 = 0;
				float3 Out_PositionWS15_g139248 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139248 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139248 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139248 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139248 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139248 = float3( 0,0,0 );
				float4 Out_VertexData15_g139248 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139248 = float4( 0,0,0,0 );
				Out_Dummy15_g139248 = Data15_g139248.Dummy;
				Out_PositionWS15_g139248 = Data15_g139248.PositionWS;
				Out_PositionWO15_g139248 = Data15_g139248.PositionWO;
				Out_PivotWS15_g139248 = Data15_g139248.PivotWS;
				Out_PivotWO15_g139248 = Data15_g139248.PivotWO;
				Out_NormalWS15_g139248 = Data15_g139248.NormalWS;
				Out_ViewDirWS15_g139248 = Data15_g139248.ViewDirWS;
				Out_VertexData15_g139248 = Data15_g139248.VertexData;
				Out_BoundsData15_g139248 = Data15_g139248.BoundsData;
				half3 Model_NormalWS798_g139234 = Out_NormalWS15_g139248;
				float temp_output_786_0_g139234 = saturate( (Model_NormalWS798_g139234).y );
				half Wetness_ProjMask790_g139234 = temp_output_786_0_g139234;
				half Water_Mask760_g139234 = ( saturate( ( ( clampResult17_g139249 - temp_output_7_0_g139250 ) / ( temp_output_10_0_g139250 + 0.0001 ) ) ) * Wetness_ProjMask790_g139234 );
				float3 lerpResult918_g139234 = lerp( Visual_Albedo127_g139234 , ( Visual_Albedo127_g139234 * (_WetnessWaterColor).rgb ) , Water_Mask760_g139234);
				#ifdef TVE_WETNESS_WATER
				float3 staticSwitch946_g139234 = lerpResult918_g139234;
				#else
				float3 staticSwitch946_g139234 = Visual_Albedo127_g139234;
				#endif
				float3 lerpResult768_g139234 = lerp( staticSwitch946_g139234 , ( staticSwitch946_g139234 * staticSwitch946_g139234 ) , _WetnessContrastValue);
				float3 lerpResult651_g139234 = lerp( Visual_Albedo127_g139234 , lerpResult768_g139234 , Wetness_Mask866_g139234);
				#ifdef TVE_WETNESS
				float3 staticSwitch577_g139234 = lerpResult651_g139234;
				#else
				float3 staticSwitch577_g139234 = Visual_Albedo127_g139234;
				#endif
				half3 Final_Albedo493_g139234 = staticSwitch577_g139234;
				float3 In_Albedo3_g139236 = Final_Albedo493_g139234;
				float3 In_AlbedoRaw3_g139236 = Out_AlbedoRaw4_g139235;
				half2 Visual_Normal535_g139234 = Out_NormalTS4_g139235;
				#ifdef TVE_WETNESS
				float2 staticSwitch774_g139234 = Visual_Normal535_g139234;
				#else
				float2 staticSwitch774_g139234 = Visual_Normal535_g139234;
				#endif
				half2 Final_Normal499_g139234 = staticSwitch774_g139234;
				float2 In_NormalTS3_g139236 = Final_Normal499_g139234;
				float3 In_NormalWS3_g139236 = Out_NormalWS4_g139235;
				float4 break658_g139234 = Visual_Masks536_g139234;
				float temp_output_935_0_g139234 = ( Wetness_Mask866_g139234 * _WetnessSmoothnessValue );
				float lerpResult941_g139234 = lerp( temp_output_935_0_g139234 , 2.0 , Water_Mask760_g139234);
				#ifdef TVE_WETNESS_WATER
				float staticSwitch959_g139234 = lerpResult941_g139234;
				#else
				float staticSwitch959_g139234 = temp_output_935_0_g139234;
				#endif
				float4 appendResult661_g139234 = (float4(break658_g139234.x , break658_g139234.y , break658_g139234.z , saturate( ( break658_g139234.w + staticSwitch959_g139234 ) )));
				#ifdef TVE_WETNESS
				float4 staticSwitch586_g139234 = appendResult661_g139234;
				#else
				float4 staticSwitch586_g139234 = Visual_Masks536_g139234;
				#endif
				half4 Final_Masks482_g139234 = staticSwitch586_g139234;
				float4 In_Shader3_g139236 = Final_Masks482_g139234;
				float4 In_Emissive3_g139236 = Out_Emissive4_g139235;
				float In_Grayscale3_g139236 = Out_Grayscale4_g139235;
				float In_Luminosity3_g139236 = Out_Luminosity4_g139235;
				float In_MultiMask3_g139236 = Out_MultiMask4_g139235;
				float In_AlphaClip3_g139236 = Out_AlphaClip4_g139235;
				float In_AlphaFade3_g139236 = Out_AlphaFade4_g139235;
				float3 In_Translucency3_g139236 = Out_Translucency4_g139235;
				float In_Transmission3_g139236 = Out_Transmission4_g139235;
				float In_Thickness3_g139236 = Out_Thickness4_g139235;
				float In_Diffusion3_g139236 = Out_Diffusion4_g139235;
				Data3_g139236.Dummy = In_Dummy3_g139236;
				Data3_g139236.Albedo = In_Albedo3_g139236;
				Data3_g139236.AlbedoRaw = In_AlbedoRaw3_g139236;
				Data3_g139236.NormalTS = In_NormalTS3_g139236;
				Data3_g139236.NormalWS = In_NormalWS3_g139236;
				Data3_g139236.Shader = In_Shader3_g139236;
				Data3_g139236.Emissive= In_Emissive3_g139236;
				Data3_g139236.MultiMask = In_MultiMask3_g139236;
				Data3_g139236.Grayscale = In_Grayscale3_g139236;
				Data3_g139236.Luminosity = In_Luminosity3_g139236;
				Data3_g139236.AlphaClip = In_AlphaClip3_g139236;
				Data3_g139236.AlphaFade = In_AlphaFade3_g139236;
				Data3_g139236.Translucency = In_Translucency3_g139236;
				Data3_g139236.Transmission = In_Transmission3_g139236;
				Data3_g139236.Thickness = In_Thickness3_g139236;
				Data3_g139236.Diffusion = In_Diffusion3_g139236;
				TVEVisualData DataB25_g139263 = Data3_g139236;
				float Alpha25_g139263 = _WetnessBakeMode;
				if (Alpha25_g139263 < 0.5 )
				{
				Data25_g139263 = DataA25_g139263;
				}
				else
				{
				Data25_g139263 = DataB25_g139263;
				}
				TVEVisualData DataA25_g139277 = Data25_g139263;
				float localCompData3_g139265 = ( 0.0 );
				TVEVisualData Data3_g139265 = (TVEVisualData)0;
				half Dummy594_g139264 = ( _CutoutCategory + _CutoutEnd + _CutoutElementMode + _CutoutBakeMode );
				float In_Dummy3_g139265 = Dummy594_g139264;
				float localBreakData4_g139275 = ( 0.0 );
				TVEVisualData Data4_g139275 = Data25_g139263;
				float Out_Dummy4_g139275 = 0;
				float3 Out_Albedo4_g139275 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139275 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139275 = float2( 0,0 );
				float3 Out_NormalWS4_g139275 = float3( 0,0,0 );
				float4 Out_Shader4_g139275 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139275 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139275 = 0;
				float Out_Grayscale4_g139275 = 0;
				float Out_Luminosity4_g139275 = 0;
				float Out_AlphaClip4_g139275 = 0;
				float Out_AlphaFade4_g139275 = 0;
				float3 Out_Translucency4_g139275 = float3( 0,0,0 );
				float Out_Transmission4_g139275 = 0;
				float Out_Thickness4_g139275 = 0;
				float Out_Diffusion4_g139275 = 0;
				Out_Dummy4_g139275 = Data4_g139275.Dummy;
				Out_Albedo4_g139275 = Data4_g139275.Albedo;
				Out_AlbedoRaw4_g139275 = Data4_g139275.AlbedoRaw;
				Out_NormalTS4_g139275 = Data4_g139275.NormalTS;
				Out_NormalWS4_g139275 = Data4_g139275.NormalWS;
				Out_Shader4_g139275 = Data4_g139275.Shader;
				Out_Emissive4_g139275= Data4_g139275.Emissive;
				Out_MultiMask4_g139275 = Data4_g139275.MultiMask;
				Out_Grayscale4_g139275 = Data4_g139275.Grayscale;
				Out_Luminosity4_g139275= Data4_g139275.Luminosity;
				Out_AlphaClip4_g139275 = Data4_g139275.AlphaClip;
				Out_AlphaFade4_g139275 = Data4_g139275.AlphaFade;
				Out_Translucency4_g139275 = Data4_g139275.Translucency;
				Out_Transmission4_g139275 = Data4_g139275.Transmission;
				Out_Thickness4_g139275 = Data4_g139275.Thickness;
				Out_Diffusion4_g139275 = Data4_g139275.Diffusion;
				float3 In_Albedo3_g139265 = Out_Albedo4_g139275;
				float3 In_AlbedoRaw3_g139265 = Out_AlbedoRaw4_g139275;
				float2 In_NormalTS3_g139265 = Out_NormalTS4_g139275;
				float3 In_NormalWS3_g139265 = Out_NormalWS4_g139275;
				float4 In_Shader3_g139265 = Out_Shader4_g139275;
				float4 In_Emissive3_g139265 = Out_Emissive4_g139275;
				float In_Grayscale3_g139265 = Out_Grayscale4_g139275;
				float In_Luminosity3_g139265 = Out_Luminosity4_g139275;
				float temp_output_836_11_g139264 = Out_MultiMask4_g139275;
				float In_MultiMask3_g139265 = temp_output_836_11_g139264;
				half Visual_AlphaClip667_g139264 = Out_AlphaClip4_g139275;
				TVEGlobalData Data15_g139274 = Data374;
				float Out_Dummy15_g139274 = 0;
				float4 Out_CoatParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139274 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139274 = float4( 0,0,0,0 );
				Out_Dummy15_g139274 = Data15_g139274.Dummy;
				Out_CoatParams15_g139274 = Data15_g139274.CoatParams;
				Out_PaintParams15_g139274 = Data15_g139274.PaintParams;
				Out_GlowParams15_g139274 = Data15_g139274.GlowParams;
				Out_AtmoParams15_g139274= Data15_g139274.AtmoParams;
				Out_FadeParams15_g139274= Data15_g139274.FadeParams;
				Out_FormParams15_g139274 = Data15_g139274.FormParams;
				Out_LandParams15_g139274 = Data15_g139274.LandParams;
				Out_WindParams15_g139274 = Data15_g139274.WindParams;
				Out_PushParams15_g139274 = Data15_g139274.PushParams;
				half4 Global_FadeParams516_g139264 = Out_FadeParams15_g139274;
				#ifdef TVE_CUTOUT_ELEMENT
				float staticSwitch663_g139264 = saturate( (Global_FadeParams516_g139264).w );
				#else
				float staticSwitch663_g139264 = 1.0;
				#endif
				half Local_GlobalMask429_g139264 = staticSwitch663_g139264;
				float lerpResult811_g139264 = lerp( 1.0 , Visual_AlphaClip667_g139264 , _CutoutAlphaValue);
				half Local_AlphaMask814_g139264 = lerpResult811_g139264;
				TVEModelData Data15_g139276 = Data16_g76840;
				float Out_Dummy15_g139276 = 0;
				float3 Out_PositionWS15_g139276 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139276 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139276 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139276 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139276 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139276 = float3( 0,0,0 );
				float4 Out_VertexData15_g139276 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139276 = float4( 0,0,0,0 );
				Out_Dummy15_g139276 = Data15_g139276.Dummy;
				Out_PositionWS15_g139276 = Data15_g139276.PositionWS;
				Out_PositionWO15_g139276 = Data15_g139276.PositionWO;
				Out_PivotWS15_g139276 = Data15_g139276.PivotWS;
				Out_PivotWO15_g139276 = Data15_g139276.PivotWO;
				Out_NormalWS15_g139276 = Data15_g139276.NormalWS;
				Out_ViewDirWS15_g139276 = Data15_g139276.ViewDirWS;
				Out_VertexData15_g139276 = Data15_g139276.VertexData;
				Out_BoundsData15_g139276 = Data15_g139276.BoundsData;
				half3 Model_PositionWO602_g139264 = Out_PositionWO15_g139276;
				float4 tex3DNode60_g139268 = SAMPLE_TEXTURE3D( _NoiseTex3D, sampler_Linear_Repeat, ( Model_PositionWO602_g139264 * ( _CutoutNoiseTillingValue * 0.01 ) ) );
				float lerpResult673_g139264 = lerp( 1.0 , tex3DNode60_g139268.r , _CutoutNoiseValue);
				half Local_NoiseMask678_g139264 = lerpResult673_g139264;
				half4 Model_VertexMasks752_g139264 = Out_VertexData15_g139276;
				float4 break755_g139264 = Model_VertexMasks752_g139264;
				float4 break33_g139266 = _cutout_vert_mode;
				float temp_output_30_0_g139266 = ( break755_g139264.x * break33_g139266.x );
				float temp_output_29_0_g139266 = ( break755_g139264.y * break33_g139266.y );
				float temp_output_31_0_g139266 = ( break755_g139264.z * break33_g139266.z );
				float temp_output_28_0_g139266 = ( temp_output_30_0_g139266 + temp_output_29_0_g139266 + temp_output_31_0_g139266 + ( break755_g139264.w * break33_g139266.w ) );
				float clampResult17_g139267 = clamp( temp_output_28_0_g139266 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139270 = _CutoutMeshRemap.x;
				float temp_output_10_0_g139270 = ( _CutoutMeshRemap.y - temp_output_7_0_g139270 );
				float lerpResult820_g139264 = lerp( 1.0 , saturate( ( ( clampResult17_g139267 - temp_output_7_0_g139270 ) / ( temp_output_10_0_g139270 + 0.0001 ) ) ) , _CutoutMeshValue);
				float temp_output_6_0_g139271 = lerpResult820_g139264;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139271 = ( temp_output_6_0_g139271 + ( _CutoutMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139271 = temp_output_6_0_g139271;
				#endif
				float temp_output_801_0_g139264 = staticSwitch14_g139271;
				half Local_VertMask766_g139264 = temp_output_801_0_g139264;
				half Visual_MultiMask671_g139264 = temp_output_836_11_g139264;
				float lerpResult683_g139264 = lerp( 1.0 , Visual_MultiMask671_g139264 , _CutoutMultiValue);
				half Local_MultiMask685_g139264 = lerpResult683_g139264;
				float lerpResult728_g139264 = lerp( Visual_AlphaClip667_g139264 , min( Visual_AlphaClip667_g139264 , ( -0.001 - ( ( _CutoutIntensityValue * Local_GlobalMask429_g139264 ) - ( Local_AlphaMask814_g139264 * Local_NoiseMask678_g139264 * Local_VertMask766_g139264 ) ) ) ) , Local_MultiMask685_g139264);
				half Local_AlphaClip784_g139264 = lerpResult728_g139264;
				#ifdef TVE_CUTOUT
				float staticSwitch577_g139264 = Local_AlphaClip784_g139264;
				#else
				float staticSwitch577_g139264 = Visual_AlphaClip667_g139264;
				#endif
				half Final_AlphaClip795_g139264 = staticSwitch577_g139264;
				float In_AlphaClip3_g139265 = Final_AlphaClip795_g139264;
				float In_AlphaFade3_g139265 = Out_AlphaFade4_g139275;
				float3 In_Translucency3_g139265 = Out_Translucency4_g139275;
				float In_Transmission3_g139265 = Out_Transmission4_g139275;
				float In_Thickness3_g139265 = Out_Thickness4_g139275;
				float In_Diffusion3_g139265 = Out_Diffusion4_g139275;
				Data3_g139265.Dummy = In_Dummy3_g139265;
				Data3_g139265.Albedo = In_Albedo3_g139265;
				Data3_g139265.AlbedoRaw = In_AlbedoRaw3_g139265;
				Data3_g139265.NormalTS = In_NormalTS3_g139265;
				Data3_g139265.NormalWS = In_NormalWS3_g139265;
				Data3_g139265.Shader = In_Shader3_g139265;
				Data3_g139265.Emissive= In_Emissive3_g139265;
				Data3_g139265.MultiMask = In_MultiMask3_g139265;
				Data3_g139265.Grayscale = In_Grayscale3_g139265;
				Data3_g139265.Luminosity = In_Luminosity3_g139265;
				Data3_g139265.AlphaClip = In_AlphaClip3_g139265;
				Data3_g139265.AlphaFade = In_AlphaFade3_g139265;
				Data3_g139265.Translucency = In_Translucency3_g139265;
				Data3_g139265.Transmission = In_Transmission3_g139265;
				Data3_g139265.Thickness = In_Thickness3_g139265;
				Data3_g139265.Diffusion = In_Diffusion3_g139265;
				TVEVisualData DataB25_g139277 = Data3_g139265;
				float Alpha25_g139277 = _CutoutBakeMode;
				if (Alpha25_g139277 < 0.5 )
				{
				Data25_g139277 = DataA25_g139277;
				}
				else
				{
				Data25_g139277 = DataB25_g139277;
				}
				TVEVisualData Data4_g139287 = Data25_g139277;
				float Out_Dummy4_g139287 = 0;
				float3 Out_Albedo4_g139287 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139287 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139287 = float2( 0,0 );
				float3 Out_NormalWS4_g139287 = float3( 0,0,0 );
				float4 Out_Shader4_g139287 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139287 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139287 = 0;
				float Out_Grayscale4_g139287 = 0;
				float Out_Luminosity4_g139287 = 0;
				float Out_AlphaClip4_g139287 = 0;
				float Out_AlphaFade4_g139287 = 0;
				float3 Out_Translucency4_g139287 = float3( 0,0,0 );
				float Out_Transmission4_g139287 = 0;
				float Out_Thickness4_g139287 = 0;
				float Out_Diffusion4_g139287 = 0;
				Out_Dummy4_g139287 = Data4_g139287.Dummy;
				Out_Albedo4_g139287 = Data4_g139287.Albedo;
				Out_AlbedoRaw4_g139287 = Data4_g139287.AlbedoRaw;
				Out_NormalTS4_g139287 = Data4_g139287.NormalTS;
				Out_NormalWS4_g139287 = Data4_g139287.NormalWS;
				Out_Shader4_g139287 = Data4_g139287.Shader;
				Out_Emissive4_g139287= Data4_g139287.Emissive;
				Out_MultiMask4_g139287 = Data4_g139287.MultiMask;
				Out_Grayscale4_g139287 = Data4_g139287.Grayscale;
				Out_Luminosity4_g139287= Data4_g139287.Luminosity;
				Out_AlphaClip4_g139287 = Data4_g139287.AlphaClip;
				Out_AlphaFade4_g139287 = Data4_g139287.AlphaFade;
				Out_Translucency4_g139287 = Data4_g139287.Translucency;
				Out_Transmission4_g139287 = Data4_g139287.Transmission;
				Out_Thickness4_g139287 = Data4_g139287.Thickness;
				Out_Diffusion4_g139287 = Data4_g139287.Diffusion;
				float3 temp_output_297_0_g139278 = Out_Albedo4_g139287;
				float3 In_Albedo3_g139288 = temp_output_297_0_g139278;
				float3 temp_output_297_23_g139278 = Out_AlbedoRaw4_g139287;
				float3 In_AlbedoRaw3_g139288 = temp_output_297_23_g139278;
				float2 In_NormalTS3_g139288 = Out_NormalTS4_g139287;
				float3 In_NormalWS3_g139288 = Out_NormalWS4_g139287;
				float4 In_Shader3_g139288 = Out_Shader4_g139287;
				float4 temp_cast_9 = (0.0).xxxx;
				half4 Visual_Emissive255_g139278 = Out_Emissive4_g139287;
				TVEModelData Data15_g139291 = Data16_g76840;
				float Out_Dummy15_g139291 = 0;
				float3 Out_PositionWS15_g139291 = float3( 0,0,0 );
				float3 Out_PositionWO15_g139291 = float3( 0,0,0 );
				float3 Out_PivotWS15_g139291 = float3( 0,0,0 );
				float3 Out_PivotWO15_g139291 = float3( 0,0,0 );
				float3 Out_NormalWS15_g139291 = float3( 0,0,0 );
				float3 Out_ViewDirWS15_g139291 = float3( 0,0,0 );
				float4 Out_VertexData15_g139291 = float4( 0,0,0,0 );
				float4 Out_BoundsData15_g139291 = float4( 0,0,0,0 );
				Out_Dummy15_g139291 = Data15_g139291.Dummy;
				Out_PositionWS15_g139291 = Data15_g139291.PositionWS;
				Out_PositionWO15_g139291 = Data15_g139291.PositionWO;
				Out_PivotWS15_g139291 = Data15_g139291.PivotWS;
				Out_PivotWO15_g139291 = Data15_g139291.PivotWO;
				Out_NormalWS15_g139291 = Data15_g139291.NormalWS;
				Out_ViewDirWS15_g139291 = Data15_g139291.ViewDirWS;
				Out_VertexData15_g139291 = Data15_g139291.VertexData;
				Out_BoundsData15_g139291 = Data15_g139291.BoundsData;
				half4 Model_VertexMasks216_g139278 = Out_VertexData15_g139291;
				float4 break251_g139278 = Model_VertexMasks216_g139278;
				float4 break33_g139282 = _emissive_vert_mode;
				float temp_output_30_0_g139282 = ( break251_g139278.x * break33_g139282.x );
				float temp_output_29_0_g139282 = ( break251_g139278.y * break33_g139282.y );
				float temp_output_31_0_g139282 = ( break251_g139278.z * break33_g139282.z );
				float temp_output_28_0_g139282 = ( temp_output_30_0_g139282 + temp_output_29_0_g139282 + temp_output_31_0_g139282 + ( break251_g139278.w * break33_g139282.w ) );
				float clampResult17_g139281 = clamp( temp_output_28_0_g139282 , 0.0001 , 0.9999 );
				float temp_output_7_0_g139285 = _EmissiveMeshRemap.x;
				float temp_output_10_0_g139285 = ( _EmissiveMeshRemap.y - temp_output_7_0_g139285 );
				float lerpResult303_g139278 = lerp( 1.0 , saturate( ( ( clampResult17_g139281 - temp_output_7_0_g139285 ) / ( temp_output_10_0_g139285 + 0.0001 ) ) ) , _EmissiveMeshValue);
				float temp_output_6_0_g139290 = lerpResult303_g139278;
				#ifdef TVE_REGISTER
				float staticSwitch14_g139290 = ( temp_output_6_0_g139290 + ( _EmissiveMeshMode * 0.0 ) );
				#else
				float staticSwitch14_g139290 = temp_output_6_0_g139290;
				#endif
				float temp_output_263_0_g139278 = staticSwitch14_g139290;
				half Emissive_MeshMask221_g139278 = temp_output_263_0_g139278;
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture276_g139280) = _EmissiveMaskTex;
				SamplerState Sampler276_g139280 = sampler_Linear_Repeat;
				float4 temp_output_6_0_g139286 = _emissive_coord_value;
				#ifdef TVE_REGISTER
				float4 staticSwitch14_g139286 = ( temp_output_6_0_g139286 + ( ( _EmissiveSampleMode + _EmissiveCoordMode + _EmissiveCoordValue ) * float4( 0,0,0,0 ) ) );
				#else
				float4 staticSwitch14_g139286 = temp_output_6_0_g139286;
				#endif
				half4 Emissive_Coords167_g139278 = staticSwitch14_g139286;
				float4 temp_output_37_0_g139280 = Emissive_Coords167_g139278;
				half4 Coords276_g139280 = temp_output_37_0_g139280;
				half2 TexCoord276_g139280 = i.ase_texcoord.xyz.xy;
				half4 localSampleMain276_g139280 = SampleMain( Texture276_g139280 , Sampler276_g139280 , Coords276_g139280 , TexCoord276_g139280 );
				UNITY_DECLARE_TEX2D_NOSAMPLER(Texture275_g139280) = _EmissiveMaskTex;
				SamplerState Sampler275_g139280 = sampler_Linear_Repeat;
				half4 Coords275_g139280 = temp_output_37_0_g139280;
				half2 TexCoord275_g139280 = i.ase_texcoord1.xy;
				half4 localSampleExtra275_g139280 = SampleExtra( Texture275_g139280 , Sampler275_g139280 , Coords275_g139280 , TexCoord275_g139280 );
				#if defined( TVE_EMISSIVE_SAMPLE_MAIN_UV )
				float4 staticSwitch176_g139278 = localSampleMain276_g139280;
				#elif defined( TVE_EMISSIVE_SAMPLE_EXTRA_UV )
				float4 staticSwitch176_g139278 = localSampleExtra275_g139280;
				#else
				float4 staticSwitch176_g139278 = localSampleMain276_g139280;
				#endif
				half4 Emissive_MaskTex201_g139278 = staticSwitch176_g139278;
				float clampResult17_g139284 = clamp( (Emissive_MaskTex201_g139278).x , 0.0001 , 0.9999 );
				float temp_output_7_0_g139283 = _EmissiveMaskRemap.x;
				float temp_output_10_0_g139283 = ( _EmissiveMaskRemap.y - temp_output_7_0_g139283 );
				float lerpResult302_g139278 = lerp( 1.0 , saturate( ( ( clampResult17_g139284 - temp_output_7_0_g139283 ) / ( temp_output_10_0_g139283 + 0.0001 ) ) ) , _EmissiveMaskValue);
				half Emissive_Mask103_g139278 = lerpResult302_g139278;
				float temp_output_279_0_g139278 = ( Emissive_MeshMask221_g139278 * Emissive_Mask103_g139278 );
				float3 appendResult293_g139278 = (float3(temp_output_279_0_g139278 , temp_output_279_0_g139278 , temp_output_279_0_g139278));
				half3 Local_EmissiveColor278_g139278 = appendResult293_g139278;
				float3 temp_cast_10 = (1.0).xxx;
				TVEGlobalData Data15_g139279 = Data374;
				float Out_Dummy15_g139279 = 0;
				float4 Out_CoatParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_PaintParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_GlowParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_AtmoParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_FadeParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_FormParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_LandParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_WindParams15_g139279 = float4( 0,0,0,0 );
				float4 Out_PushParams15_g139279 = float4( 0,0,0,0 );
				Out_Dummy15_g139279 = Data15_g139279.Dummy;
				Out_CoatParams15_g139279 = Data15_g139279.CoatParams;
				Out_PaintParams15_g139279 = Data15_g139279.PaintParams;
				Out_GlowParams15_g139279 = Data15_g139279.GlowParams;
				Out_AtmoParams15_g139279= Data15_g139279.AtmoParams;
				Out_FadeParams15_g139279= Data15_g139279.FadeParams;
				Out_FormParams15_g139279 = Data15_g139279.FormParams;
				Out_LandParams15_g139279 = Data15_g139279.LandParams;
				Out_WindParams15_g139279 = Data15_g139279.WindParams;
				Out_PushParams15_g139279 = Data15_g139279.PushParams;
				half4 Global_GlowParams179_g139278 = Out_GlowParams15_g139279;
				float3 lerpResult299_g139278 = lerp( float3( 1,1,1 ) , (Global_GlowParams179_g139278).xyz , TVE_IsEnabled);
				#ifdef TVE_EMISSIVE_ELEMENT
				float3 staticSwitch228_g139278 = lerpResult299_g139278;
				#else
				float3 staticSwitch228_g139278 = temp_cast_10;
				#endif
				half3 Emissive_GlobalMask248_g139278 = staticSwitch228_g139278;
				half3 Visual_AlbedoRaw306_g139278 = temp_output_297_23_g139278;
				float3 lerpResult307_g139278 = lerp( float3( 1,1,1 ) , Visual_AlbedoRaw306_g139278 , _EmissiveColorMode);
				half3 Local_EmissiveValue88_g139278 = ( _EmissiveIntensityValue * (_EmissiveColor).rgb * Emissive_GlobalMask248_g139278 * lerpResult307_g139278 );
				half3 Emissive_Blend260_g139278 = ( ( (Visual_Emissive255_g139278).xyz * Local_EmissiveColor278_g139278 ) * Local_EmissiveValue88_g139278 );
				float3 temp_output_3_0_g139292 = Emissive_Blend260_g139278;
				float temp_output_15_0_g139292 = _emissive_power_value;
				float3 temp_output_23_0_g139292 = ( temp_output_3_0_g139292 * temp_output_15_0_g139292 );
				half Local_EmissiveMask294_g139278 = temp_output_279_0_g139278;
				float4 appendResult295_g139278 = (float4(temp_output_23_0_g139292 , Local_EmissiveMask294_g139278));
				#ifdef TVE_EMISSIVE
				float4 staticSwitch129_g139278 = appendResult295_g139278;
				#else
				float4 staticSwitch129_g139278 = temp_cast_9;
				#endif
				half4 Final_Emissive184_g139278 = staticSwitch129_g139278;
				float4 In_Emissive3_g139288 = Final_Emissive184_g139278;
				float In_Grayscale3_g139288 = Out_Grayscale4_g139287;
				float In_Luminosity3_g139288 = Out_Luminosity4_g139287;
				float temp_output_297_11_g139278 = Out_MultiMask4_g139287;
				float In_MultiMask3_g139288 = temp_output_297_11_g139278;
				float In_AlphaClip3_g139288 = Out_AlphaClip4_g139287;
				float In_AlphaFade3_g139288 = Out_AlphaFade4_g139287;
				float3 In_Translucency3_g139288 = Out_Translucency4_g139287;
				float In_Transmission3_g139288 = Out_Transmission4_g139287;
				float In_Thickness3_g139288 = Out_Thickness4_g139287;
				float In_Diffusion3_g139288 = Out_Diffusion4_g139287;
				Data3_g139288.Dummy = In_Dummy3_g139288;
				Data3_g139288.Albedo = In_Albedo3_g139288;
				Data3_g139288.AlbedoRaw = In_AlbedoRaw3_g139288;
				Data3_g139288.NormalTS = In_NormalTS3_g139288;
				Data3_g139288.NormalWS = In_NormalWS3_g139288;
				Data3_g139288.Shader = In_Shader3_g139288;
				Data3_g139288.Emissive= In_Emissive3_g139288;
				Data3_g139288.MultiMask = In_MultiMask3_g139288;
				Data3_g139288.Grayscale = In_Grayscale3_g139288;
				Data3_g139288.Luminosity = In_Luminosity3_g139288;
				Data3_g139288.AlphaClip = In_AlphaClip3_g139288;
				Data3_g139288.AlphaFade = In_AlphaFade3_g139288;
				Data3_g139288.Translucency = In_Translucency3_g139288;
				Data3_g139288.Transmission = In_Transmission3_g139288;
				Data3_g139288.Thickness = In_Thickness3_g139288;
				Data3_g139288.Diffusion = In_Diffusion3_g139288;
				TVEVisualData Data4_g139296 = Data3_g139288;
				float Out_Dummy4_g139296 = 0;
				float3 Out_Albedo4_g139296 = float3( 0,0,0 );
				float3 Out_AlbedoRaw4_g139296 = float3( 0,0,0 );
				float2 Out_NormalTS4_g139296 = float2( 0,0 );
				float3 Out_NormalWS4_g139296 = float3( 0,0,0 );
				float4 Out_Shader4_g139296 = float4( 0,0,0,0 );
				float4 Out_Emissive4_g139296 = float4( 0,0,0,0 );
				float Out_MultiMask4_g139296 = 0;
				float Out_Grayscale4_g139296 = 0;
				float Out_Luminosity4_g139296 = 0;
				float Out_AlphaClip4_g139296 = 0;
				float Out_AlphaFade4_g139296 = 0;
				float3 Out_Translucency4_g139296 = float3( 0,0,0 );
				float Out_Transmission4_g139296 = 0;
				float Out_Thickness4_g139296 = 0;
				float Out_Diffusion4_g139296 = 0;
				Out_Dummy4_g139296 = Data4_g139296.Dummy;
				Out_Albedo4_g139296 = Data4_g139296.Albedo;
				Out_AlbedoRaw4_g139296 = Data4_g139296.AlbedoRaw;
				Out_NormalTS4_g139296 = Data4_g139296.NormalTS;
				Out_NormalWS4_g139296 = Data4_g139296.NormalWS;
				Out_Shader4_g139296 = Data4_g139296.Shader;
				Out_Emissive4_g139296= Data4_g139296.Emissive;
				Out_MultiMask4_g139296 = Data4_g139296.MultiMask;
				Out_Grayscale4_g139296 = Data4_g139296.Grayscale;
				Out_Luminosity4_g139296= Data4_g139296.Luminosity;
				Out_AlphaClip4_g139296 = Data4_g139296.AlphaClip;
				Out_AlphaFade4_g139296 = Data4_g139296.AlphaFade;
				Out_Translucency4_g139296 = Data4_g139296.Translucency;
				Out_Transmission4_g139296 = Data4_g139296.Transmission;
				Out_Thickness4_g139296 = Data4_g139296.Thickness;
				Out_Diffusion4_g139296 = Data4_g139296.Diffusion;
				float4 appendResult92_g139294 = (float4(Out_Albedo4_g139296 , 1.0));
				
				float3 appendResult123_g139294 = (float3(Out_NormalTS4_g139296 , 1.0));
				float3 temp_output_13_0_g139299 = appendResult123_g139294;
				float3 temp_output_33_0_g139299 = ( temp_output_13_0_g139299 * _render_normal );
				float3 switchResult12_g139299 = (((ase_vface>0)?(temp_output_13_0_g139299):(temp_output_33_0_g139299)));
				float3 tanNormal127_g139294 = switchResult12_g139299;
				float3 worldNormal127_g139294 = normalize( float3(dot(tanToWorld0,tanNormal127_g139294), dot(tanToWorld1,tanNormal127_g139294), dot(tanToWorld2,tanNormal127_g139294)) );
				float eyeDepth = i.ase_texcoord.w;
				float temp_output_51_0_g139298 = ( -1.0 / UNITY_MATRIX_P[2].z );
				float4 appendResult94_g139294 = (float4((worldNormal127_g139294*0.5 + 0.5) , ( ( eyeDepth + temp_output_51_0_g139298 ) / temp_output_51_0_g139298 )));
				
				float4 break98_g139294 = Out_Shader4_g139296;
				#ifdef TVE_EMISSIVE
				float staticSwitch128_g139294 = Out_Emissive4_g139296.w;
				#else
				float staticSwitch128_g139294 = break98_g139294.y;
				#endif
				float temp_output_110_11_g139294 = Out_MultiMask4_g139296;
				float4 appendResult99_g139294 = (float4(break98_g139294.x , staticSwitch128_g139294 , temp_output_110_11_g139294 , break98_g139294.w));
				
				float4 appendResult118_g139294 = (float4(i.ase_color.r , i.ase_color.g , temp_output_110_11_g139294 , break98_g139294.w));
				

				outGBuffer0 = appendResult92_g139294;
				outGBuffer1 = appendResult94_g139294;
				outGBuffer2 = appendResult99_g139294;
				outGBuffer3 = appendResult118_g139294;
				outGBuffer4 = i.ase_color;
				outGBuffer5 = 0;
				outGBuffer6 = 0;
				outGBuffer7 = 0;
				float alpha = Out_AlphaClip4_g139296;
				clip( alpha );
				outDepth = i.pos.z;
			}
			ENDCG
		}
	}
	
	
	Fallback Off
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.Vector4Node;408;-4992,0;Inherit;False;Constant;_Vector1;Vector 1;14;0;Create;True;0;0;0;False;0;False;1,1,1,1;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;323;-6016,0;Inherit;False;Block Model;32;;76826;7ad7765e793a6714babedee0033c36e9;14,289,1,240,1,290,1,291,1,181,0,183,0,185,0,188,0,190,0,184,0,192,0,189,0,300,0,193,0;10;102;FLOAT3;0,0,0;False;163;FLOAT3;0,0,0;False;186;FLOAT3;0,0,0;False;187;FLOAT3;0,0,0;False;166;FLOAT3;0,0,0;False;164;FLOAT3;0,0,0;False;301;FLOAT3;0,0,0;False;167;FLOAT4;0,0,0,0;False;172;FLOAT4;0,0,0,0;False;175;FLOAT4;0,0,0,0;False;2;OBJECT;128;OBJECT;314
Node;AmplifyShaderEditor.CustomExpressionNode;374;-4736,0;Inherit;False;Data.Dummy = In_Dummy@$Data.CoatParams = In_CoatParams@$Data.PaintParams = In_PaintParams@$Data.GlowParams = In_GlowParams@$Data.AtmoParams = In_AtmoParams@$Data.FormParams= In_FormParams@$Data.WindParams = In_WindParams@$Data.PushParams = In_PushParams@$$$$$$;1;Call;9;True;Data;OBJECT;(TVEGlobalData)0;Out;TVEGlobalData;Half;False;True;In_Dummy;FLOAT;0;In;;Half;False;True;In_CoatParams;FLOAT4;0,0,0,0;In;;Half;False;True;In_PaintParams;FLOAT4;0,0,0,0;In;;Half;False;True;In_GlowParams;FLOAT4;0,0,0,0;In;;Half;False;True;In_AtmoParams;FLOAT4;0,0,0,0;In;;Half;False;True;In_FormParams;FLOAT4;0,0,0,0;In;;Half;False;True;In_WindParams;FLOAT4;0,0,0,0;In;;Half;False;True;In_PushParams;FLOAT4;0,0,0,0;In;;Half;False;BuildGlobalData;False;False;0;;False;10;0;FLOAT;0;False;1;OBJECT;(TVEGlobalData)0;False;2;FLOAT;0;False;3;FLOAT4;0,0,0,0;False;4;FLOAT4;0,0,0,0;False;5;FLOAT4;0,0,0,0;False;6;FLOAT4;0,0,0,0;False;7;FLOAT4;0,0,0,0;False;8;FLOAT4;0,0,0,0;False;9;FLOAT4;0,0,0,0;False;2;FLOAT;0;OBJECT;2
Node;AmplifyShaderEditor.RegisterLocalVarNode;334;-5696,0;Half;False;Model Data;-1;True;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;335;-3712,0;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;327;-4416,0;Half;False;Global Data;-1;True;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;336;-3456,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;414;-3456,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.FunctionNode;491;-3456,0;Inherit;False;Block Main;42;;139039;b04cfed9a7b4c0841afdb49a38c282c5;5,65,1,136,1,41,1,133,1,40,1;1;225;OBJECT;0,0,0,0;False;1;OBJECT;106
Node;AmplifyShaderEditor.RangedFloatNode;398;-3136,256;Half;False;Property;_SecondBakeMode;_SecondBakeMode;352;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;492;-3200,128;Inherit;False;Block Layer;68;;139057;5f6a6b9e0b5515744bf8e48a9ccead1b;7,986,1,1008,0,709,1,726,1,748,1,747,1,746,1;3;585;OBJECT;0,0,0,0;False;633;OBJECT;0,0,0,0;False;974;OBJECT;0,0,0,0;False;1;OBJECT;552
Node;AmplifyShaderEditor.GetLocalVarNode;338;-2816,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.FunctionNode;409;-2816,0;Inherit;False;If Visual Data;-1;;139091;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;384;-2816,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;401;-2496,256;Half;False;Property;_ThirdBakeMode;_ThirdBakeMode;353;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;493;-2560,128;Inherit;False;Block Detail;115;;139092;a5b52fdec7b855a4fba859a90e837892;7,1013,0,990,1,709,1,726,1,748,1,747,1,746,1;3;585;OBJECT;0,0,0,0;False;633;OBJECT;0,0,0,0;False;971;OBJECT;0,0,0,0;False;1;OBJECT;552
Node;AmplifyShaderEditor.FunctionNode;418;-2176,0;Inherit;False;If Visual Data;-1;;139125;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;343;-2176,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;432;-1888,256;Half;False;Property;_OcclusionBakeMode;_OcclusionBakeMode;354;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;494;-1888,128;Inherit;False;Block Occlusion;160;;139126;ec16733ec52362048954a75640fbe560;1,210,1;2;144;OBJECT;0,0,0,0;False;204;OBJECT;0,0,0,0;False;1;OBJECT;116
Node;AmplifyShaderEditor.FunctionNode;431;-1536,0;Inherit;False;If Visual Data;-1;;139136;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;393;-1536,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;434;-1216,256;Half;False;Property;_GradientBakeMode;_GradientBakeMode;355;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;495;-1248,128;Inherit;False;Block Gradient;172;;139137;1f0cb348753541648acbe7a6adce694e;1,228,1;2;144;OBJECT;0,0,0,0;False;222;OBJECT;0,0,0,0;False;1;OBJECT;116
Node;AmplifyShaderEditor.FunctionNode;433;-896,0;Inherit;False;If Visual Data;-1;;139147;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;447;-896,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;445;-896,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;448;-576,256;Half;False;Property;_TintingBakeMode;_TintingBakeMode;357;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;496;-640,128;Inherit;False;Block Tinting;185;;139148;9f39e156ea8d89e4997ea2a1e194137e;7,352,1,400,0,334,1,336,1,339,1,355,0,344,0;4;198;OBJECT;0,0,0,0;False;223;OBJECT;0,0,0,0;False;207;OBJECT;0,0,0,0;False;346;FLOAT;1;False;1;OBJECT;204
Node;AmplifyShaderEditor.FunctionNode;446;-256,0;Inherit;False;If Visual Data;-1;;139172;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;451;-256,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;452;-256,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;450;64,256;Half;False;Property;_DrynessBakeMode;_DrynessBakeMode;358;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;497;0,128;Inherit;False;Block Dryness;214;;139173;f05795de23f951c45bb73c8b4321e4b7;7,398,1,482,0,400,1,403,1,405,1,442,0,410,0;4;279;OBJECT;0,0,0,0;False;297;OBJECT;0,0,0,0;False;281;OBJECT;0,0,0,0;False;409;FLOAT;1;False;1;OBJECT;346
Node;AmplifyShaderEditor.FunctionNode;449;384,0;Inherit;False;If Visual Data;-1;;139196;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;455;384,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;456;384,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;454;704,256;Half;False;Property;_OverlayBakeMode;_OverlayBakeMode;359;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;498;640,128;Inherit;False;Block Overlay;247;;139197;8ae9c8285a7817844a51243251284d21;9,821,1,819,1,813,1,910,0,826,1,823,1,828,1,844,0,447,0;4;572;OBJECT;0,0,0,0;False;596;OBJECT;0,0,0,0;False;600;OBJECT;0,0,0,0;False;445;FLOAT;1;False;1;OBJECT;566
Node;AmplifyShaderEditor.FunctionNode;453;1024,0;Inherit;False;If Visual Data;-1;;139233;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;460;1024,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;459;1024,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.RangedFloatNode;458;1344,256;Half;False;Property;_WetnessBakeMode;_WetnessBakeMode;360;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;499;1280,128;Inherit;False;Block Wetness;293;;139234;52c5a1f52507fc44e926833b126e7855;8,850,1,1075,0,857,1,945,1,930,1,1092,0,851,0,1107,0;3;572;OBJECT;0,0,0,0;False;596;OBJECT;0,0,0,0;False;600;OBJECT;0,0,0,0;False;1;OBJECT;566
Node;AmplifyShaderEditor.FunctionNode;457;1664,0;Inherit;False;If Visual Data;-1;;139263;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;463;1664,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;464;1664,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.FunctionNode;500;1920,128;Inherit;False;Block Cutout;334;;139264;866b4a5fe67e7f34085520e1bb5be2b7;5,775,1,777,1,815,1,779,1,817,1;3;572;OBJECT;0,0,0,0;False;596;OBJECT;0,0,0,0;False;600;OBJECT;0,0,0,0;False;1;OBJECT;566
Node;AmplifyShaderEditor.RangedFloatNode;461;1984,256;Half;False;Property;_CutoutBakeMode;_CutoutBakeMode;356;0;Fetch;True;0;0;0;False;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;462;2304,0;Inherit;False;If Visual Data;-1;;139277;947a79bd19d4b8e46835240e033f082b;0;3;3;OBJECT;0;False;17;OBJECT;0;False;19;FLOAT;0;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;477;2304,128;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;478;2304,192;Inherit;False;327;Global Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.FunctionNode;503;2688,0;Inherit;False;Block Emissive;0;;139278;64497f287b9096b43b688b52b4a0bf20;5,282,0,273,1,275,1,264,1,267,1;3;146;OBJECT;0,0,0,0;False;148;OBJECT;0,0,0,0;False;178;OBJECT;0,0,0,0;False;1;OBJECT;183
Node;AmplifyShaderEditor.RegisterLocalVarNode;370;3136,0;Half;False;Visual Data;-1;True;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;391;3712,0;Inherit;False;370;Visual Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.GetLocalVarNode;397;3712,64;Inherit;False;334;Model Data;1;0;OBJECT;;False;1;OBJECT;0
Node;AmplifyShaderEditor.FunctionNode;286;4320,-128;Inherit;False;Base Compile;-1;;139293;e67c8238031dbf04ab79a5d4d63d1b4f;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;506;3968,0;Inherit;False;Block Baker Output;24;;139294;5fadd80fe4bec3e42b1e9ce050e0c79b;0;1;17;OBJECT;;False;6;FLOAT4;90;FLOAT4;96;FLOAT4;97;FLOAT4;113;COLOR;117;FLOAT;93
Node;AmplifyShaderEditor.RangedFloatNode;268;3712,-128;Half;False;Property;_RenderCull;_RenderCull;361;1;[HideInInspector];Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;507;3904,-128;Half;False;Property;_RenderNormal;_RenderNormal;246;1;[HideInInspector];Create;True;0;0;0;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;199;4320,0;Float;False;True;-1;2;;100;18;Hidden/BOXOPHOBIC/The Visual Engine/Helpers/Impostors Baker;f53051a8190f7044fa936bd7fbe116c1;True;Unlit;0;0;Unlit;10;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;2;True;_RenderCull;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;True;5;False;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;0;;0;0;Standard;0;0;1;True;False;;True;0
WireConnection;374;3;408;0
WireConnection;374;4;408;0
WireConnection;374;6;408;0
WireConnection;334;0;323;128
WireConnection;327;0;374;2
WireConnection;491;225;335;0
WireConnection;492;585;491;106
WireConnection;492;633;336;0
WireConnection;492;974;414;0
WireConnection;409;3;491;106
WireConnection;409;17;492;552
WireConnection;409;19;398;0
WireConnection;493;585;409;0
WireConnection;493;633;338;0
WireConnection;493;971;384;0
WireConnection;418;3;409;0
WireConnection;418;17;493;552
WireConnection;418;19;401;0
WireConnection;494;144;418;0
WireConnection;494;204;343;0
WireConnection;431;3;418;0
WireConnection;431;17;494;116
WireConnection;431;19;432;0
WireConnection;495;144;431;0
WireConnection;495;222;393;0
WireConnection;433;3;431;0
WireConnection;433;17;495;116
WireConnection;433;19;434;0
WireConnection;496;198;433;0
WireConnection;496;223;447;0
WireConnection;496;207;445;0
WireConnection;446;3;433;0
WireConnection;446;17;496;204
WireConnection;446;19;448;0
WireConnection;497;279;446;0
WireConnection;497;297;451;0
WireConnection;497;281;452;0
WireConnection;449;3;446;0
WireConnection;449;17;497;346
WireConnection;449;19;450;0
WireConnection;498;572;449;0
WireConnection;498;596;455;0
WireConnection;498;600;456;0
WireConnection;453;3;449;0
WireConnection;453;17;498;566
WireConnection;453;19;454;0
WireConnection;499;572;453;0
WireConnection;499;596;459;0
WireConnection;499;600;460;0
WireConnection;457;3;453;0
WireConnection;457;17;499;566
WireConnection;457;19;458;0
WireConnection;500;572;457;0
WireConnection;500;596;463;0
WireConnection;500;600;464;0
WireConnection;462;3;457;0
WireConnection;462;17;500;566
WireConnection;462;19;461;0
WireConnection;503;146;462;0
WireConnection;503;148;477;0
WireConnection;503;178;478;0
WireConnection;370;0;503;183
WireConnection;506;17;391;0
WireConnection;199;0;506;90
WireConnection;199;1;506;96
WireConnection;199;2;506;97
WireConnection;199;3;506;113
WireConnection;199;4;506;117
WireConnection;199;8;506;93
ASEEND*/
//CHKSM=3AEC3A2D49543BC36BBDD5DA4B2DF0C2F893FDFF