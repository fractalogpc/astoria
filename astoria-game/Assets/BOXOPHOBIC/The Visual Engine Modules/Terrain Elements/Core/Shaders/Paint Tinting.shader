// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Visual Engine/Elements/Paint Tinting (Layered)"
{
	Properties
	{
		[StyledMessage(Info, Use the Tinting elements to add color tinting to the vegetation assets. Make sure the element size matches your terrain., 0,15)]_ElementMessage("Element Message", Float) = 0
		[HideInInspector]_IsVersion("_IsVersion", Float) = 2040
		[HideInInspector]_IsElementShader("_IsElementShader", Float) = 1
		[StyledCategory(Render Settings, true, 0, 10)]_RenderCategory("[ Render Category ]", Float) = 1
		_ElementIntensity("Render Intensity", Range( 0 , 1)) = 1
		[StyledMessage(Info, When using a higher Layer number the Global Volume will create more render textures to render the elements. Try using fewer layers when possible., _ElementLayerMessage, 1, 10, 10)]_ElementLayerMessage("Render Layer Message", Float) = 0
		[StyledMessage(Warning, When using all layers the Global Volume will create one render texture for each layer to render the elements. Try using fewer layers when possible., _ElementLayerWarning, 1, 10, 10)]_ElementLayerWarning("Render Layer Warning", Float) = 0
		[StyledMask(Paint Layers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_ElementLayerMask("Render Layer", Float) = 1
		[StyledSpace(10)]_ElementEnd("[ Element End ]", Float) = 1
		[StyledCategory(Control Settings, true, 0, 10)]_ControlCategory("[ Control Category ]", Float) = 1
		[NoScaleOffset][StyledTextureSingleLine]_ControlTex1("Control 01", 2D) = "black" {}
		[HDR][Gamma][NoScaleOffset][StyledTextureSingleLine]_ControlTex2("Control 02", 2D) = "black" {}
		[HDR][Gamma][NoScaleOffset][StyledTextureSingleLine]_ControlTex3("Control 03", 2D) = "black" {}
		[HDR][Gamma][NoScaleOffset][StyledTextureSingleLine]_ControlTex4("Control 04", 2D) = "black" {}
		[Space(10)][StyledRemapSlider]_ControlMaskRemap("Control Remap", Vector) = (0,1,0,0)
		[StyledSpace(10)]_ControlEnd("[ Control End ]", Float) = 1
		[StyledCategory(Layer Settings, true, 0, 10)]_LayerCategory("[ Layer Category ]", Float) = 1
		[HDR][Gamma]_LayerColor1("Layer 01", Color) = (0.5,0.5,0.5,1)
		[HDR][Gamma]_LayerColor2("Layer 02", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor3("Layer 03", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor4("Layer 04", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma][Space(10)]_LayerColor5("Layer 05", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor6("Layer 06", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor7("Layer 07", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor8("Layer 08", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma][Space(10)]_LayerColor9("Layer 09", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor10("Layer 10", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor11("Layer 11", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor12("Layer 12", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma][Space(10)]_LayerColor13("Layer 13", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor14("Layer 14", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor15("Layer 15", Color) = (0.5019608,0.5019608,0.5019608,1)
		[HDR][Gamma]_LayerColor16("Layer 16", Color) = (0.5019608,0.5019608,0.5019608,1)
		[StyledSpace(10)]_LayerEnd("[ Layer End ]", Float) = 1
		[StyledCategory(Season Settings, true, 0, 10)]_SeasonCategory("[ Season Category ]", Float) = 1
		_AdditionalValue1("Winter Value", Range( 0 , 1)) = 1
		_AdditionalValue2("Spring Value", Range( 0 , 1)) = 1
		_AdditionalValue3("Summer Value", Range( 0 , 1)) = 1
		_AdditionalValue4("Autumn Value", Range( 0 , 1)) = 1
		[Space(10)][StyledRemapSlider]_SeasonRemap("Seasons Curve", Vector) = (0,1,0,0)
		[StyledSpace(10)]_SeasonsEnd("[ Seasons End ]", Float) = 1
		[StyledCategory(Fading Settings, true, 0, 10)]_FadeCategory("[ Fade Category ]", Float) = 1
		[Enum(Off,0,On,1)]_ElementVolumeFadeMode("Bounds Fade", Float) = 0
		_ElementVolumeFadeValue("Bounds Fade Blend", Range( 0 , 1)) = 0.75
		[StyledSpace(10)]_FadeEnd("[ Fade End ]", Float) = 1
		[HideInInspector]_render_colormask("_render_colormask", Float) = 15

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching"="False" }
	LOD 100

		
		Pass
		{
			
			Name "VolumePass"
			
			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaToMask Off
			Cull Back
			ColorMask RGBA
			ZWrite Off
			ZTest LEqual
			
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half _IsVersion;
			uniform half _IsElementShader;
			uniform half _RenderCategory;
			uniform half _ElementLayerMessage;
			uniform half _ElementLayerWarning;
			uniform half _ElementLayerMask;
			uniform half _render_colormask;
			uniform half _ControlCategory;
			uniform half _LayerCategory;
			uniform half _LayerEnd;
			uniform half _SeasonsEnd;
			uniform half _FadeEnd;
			uniform half _ControlEnd;
			uniform half _ElementEnd;
			uniform half4 _SeasonRemap;
			uniform half _SeasonCategory;
			uniform half _FadeCategory;
			uniform half4 _ControlMaskRemap;
			uniform half _ElementMessage;
			uniform sampler2D _ControlTex1;
			uniform half4 _LayerColor1;
			uniform half4 _LayerColor2;
			uniform half4 _LayerColor3;
			uniform half4 _LayerColor4;
			uniform sampler2D _ControlTex2;
			uniform half4 _LayerColor5;
			uniform half4 _LayerColor6;
			uniform half4 _LayerColor7;
			uniform half4 _LayerColor8;
			uniform sampler2D _ControlTex3;
			uniform half4 _LayerColor9;
			uniform half4 _LayerColor10;
			uniform half4 _LayerColor11;
			uniform half4 _LayerColor12;
			uniform sampler2D _ControlTex4;
			uniform half4 _LayerColor13;
			uniform half4 _LayerColor14;
			uniform half4 _LayerColor15;
			uniform half4 _LayerColor16;
			uniform half _ElementIntensity;
			uniform half4 TVE_SeasonOption;
			uniform half _AdditionalValue1;
			uniform half _AdditionalValue2;
			uniform half TVE_SeasonLerp;
			uniform half _AdditionalValue3;
			uniform half _AdditionalValue4;
			uniform half4 TVE_RenderBasePositionR;
			uniform half _ElementVolumeFadeValue;
			uniform half _ElementVolumeFadeMode;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				half Control_Min296_g22461 = _ControlMaskRemap.x;
				half temp_output_7_0_g22464 = Control_Min296_g22461;
				half4 temp_cast_0 = (temp_output_7_0_g22464).xxxx;
				half Control_Max299_g22461 = _ControlMaskRemap.y;
				half temp_output_10_0_g22464 = ( Control_Max299_g22461 - temp_output_7_0_g22464 );
				half4 ControlTex_1123_g22461 = saturate( ( ( tex2D( _ControlTex1, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_0 ) / ( temp_output_10_0_g22464 + 0.0001 ) ) );
				half4 weightedBlendVar16_g22461 = ControlTex_1123_g22461;
				half4 weightedBlend16_g22461 = ( weightedBlendVar16_g22461.x*_LayerColor1 + weightedBlendVar16_g22461.y*_LayerColor2 + weightedBlendVar16_g22461.z*_LayerColor3 + weightedBlendVar16_g22461.w*_LayerColor4 );
				half4 Terrain_Colors_118_g22461 = weightedBlend16_g22461;
				half temp_output_7_0_g22465 = Control_Min296_g22461;
				half4 temp_cast_1 = (temp_output_7_0_g22465).xxxx;
				half temp_output_10_0_g22465 = ( Control_Max299_g22461 - temp_output_7_0_g22465 );
				half4 ControlTex_2124_g22461 = saturate( ( ( tex2D( _ControlTex2, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_1 ) / ( temp_output_10_0_g22465 + 0.0001 ) ) );
				half4 weightedBlendVar15_g22461 = ControlTex_2124_g22461;
				half4 weightedBlend15_g22461 = ( weightedBlendVar15_g22461.x*_LayerColor5 + weightedBlendVar15_g22461.y*_LayerColor6 + weightedBlendVar15_g22461.z*_LayerColor7 + weightedBlendVar15_g22461.w*_LayerColor8 );
				half4 Terrain_Colors_217_g22461 = weightedBlend15_g22461;
				half temp_output_7_0_g22463 = Control_Min296_g22461;
				half4 temp_cast_2 = (temp_output_7_0_g22463).xxxx;
				half temp_output_10_0_g22463 = ( Control_Max299_g22461 - temp_output_7_0_g22463 );
				half4 ControlTex_3307_g22461 = saturate( ( ( tex2D( _ControlTex3, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_2 ) / ( temp_output_10_0_g22463 + 0.0001 ) ) );
				half4 weightedBlendVar339_g22461 = ControlTex_3307_g22461;
				half4 weightedBlend339_g22461 = ( weightedBlendVar339_g22461.x*_LayerColor9 + weightedBlendVar339_g22461.y*_LayerColor10 + weightedBlendVar339_g22461.z*_LayerColor11 + weightedBlendVar339_g22461.w*_LayerColor12 );
				half4 Terrain_Colors_3340_g22461 = weightedBlend339_g22461;
				half temp_output_7_0_g22462 = Control_Min296_g22461;
				half4 temp_cast_3 = (temp_output_7_0_g22462).xxxx;
				half temp_output_10_0_g22462 = ( Control_Max299_g22461 - temp_output_7_0_g22462 );
				half4 ControlTex_4322_g22461 = saturate( ( ( tex2D( _ControlTex4, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_3 ) / ( temp_output_10_0_g22462 + 0.0001 ) ) );
				half4 weightedBlendVar344_g22461 = ControlTex_4322_g22461;
				half4 weightedBlend344_g22461 = ( weightedBlendVar344_g22461.x*_LayerColor13 + weightedBlendVar344_g22461.y*_LayerColor14 + weightedBlendVar344_g22461.z*_LayerColor15 + weightedBlendVar344_g22461.w*_LayerColor16 );
				half4 Terrain_Colors_4350_g22461 = weightedBlend344_g22461;
				half4 Terrain_colors32_g22461 = ( Terrain_Colors_118_g22461 + Terrain_Colors_217_g22461 + Terrain_Colors_3340_g22461 + Terrain_Colors_4350_g22461 );
				half TVE_SeasonOptions_X66_g22461 = TVE_SeasonOption.x;
				half Additional_Value_Winter90_g22461 = _AdditionalValue1;
				half Additional_Value_Spring88_g22461 = _AdditionalValue2;
				half temp_output_7_0_g22474 = _SeasonRemap.x;
				half temp_output_10_0_g22474 = ( _SeasonRemap.y - temp_output_7_0_g22474 );
				half TVE_SeasonLerp57_g22461 = saturate( saturate( ( ( TVE_SeasonLerp - temp_output_7_0_g22474 ) / temp_output_10_0_g22474 ) ) );
				half lerpResult195_g22461 = lerp( Additional_Value_Winter90_g22461 , Additional_Value_Spring88_g22461 , TVE_SeasonLerp57_g22461);
				half TVE_SeasonOptions_Y64_g22461 = TVE_SeasonOption.y;
				half Additional_Value_Summer80_g22461 = _AdditionalValue3;
				half lerpResult197_g22461 = lerp( Additional_Value_Spring88_g22461 , Additional_Value_Summer80_g22461 , TVE_SeasonLerp57_g22461);
				half TVE_SeasonOptions_Z61_g22461 = TVE_SeasonOption.z;
				half Additional_Value_Autumn101_g22461 = _AdditionalValue4;
				half lerpResult192_g22461 = lerp( Additional_Value_Summer80_g22461 , Additional_Value_Autumn101_g22461 , TVE_SeasonLerp57_g22461);
				half TVE_SeasonOptions_W62_g22461 = TVE_SeasonOption.w;
				half lerpResult205_g22461 = lerp( Additional_Value_Autumn101_g22461 , Additional_Value_Winter90_g22461 , TVE_SeasonLerp57_g22461);
				half Element_Inflience461_g22461 = ( ( TVE_SeasonOptions_X66_g22461 * lerpResult195_g22461 ) + ( TVE_SeasonOptions_Y64_g22461 * lerpResult197_g22461 ) + ( TVE_SeasonOptions_Z61_g22461 * lerpResult192_g22461 ) + ( TVE_SeasonOptions_W62_g22461 * lerpResult205_g22461 ) );
				half temp_output_7_0_g22477 = 1.0;
				half temp_output_10_0_g22477 = ( _ElementVolumeFadeValue - temp_output_7_0_g22477 );
				half lerpResult18_g22475 = lerp( 1.0 , saturate( ( ( saturate( ( distance( WorldPosition , (TVE_RenderBasePositionR).xyz ) / (TVE_RenderBasePositionR).w ) ) - temp_output_7_0_g22477 ) / temp_output_10_0_g22477 ) ) , _ElementVolumeFadeMode);
				half Element_Intensity235_g22461 = ( _ElementIntensity * Element_Inflience461_g22461 * lerpResult18_g22475 );
				half4 appendResult410_g22461 = (half4((Terrain_colors32_g22461).rgb , ( Terrain_colors32_g22461.a * Element_Intensity235_g22461 )));
				
				
				finalColor = appendResult410_g22461;
				return finalColor;
			}
			ENDCG
		}
		
		
		Pass
		{
			
			Name "VisualPass"
			
			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend SrcAlpha OneMinusSrcAlpha
			AlphaToMask Off
			Cull Back
			ColorMask RGBA
			ZWrite Off
			ZTest LEqual
			
			CGPROGRAM

			

			#ifndef UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX
			//only defining to not throw compilation error over Unity 5.5
			#define UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input)
			#endif
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile_instancing
			#include "UnityCG.cginc"
			#define ASE_NEEDS_FRAG_WORLD_POSITION


			struct appdata
			{
				float4 vertex : POSITION;
				float4 color : COLOR;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			
			struct v2f
			{
				float4 vertex : SV_POSITION;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 worldPos : TEXCOORD0;
				#endif
				float4 ase_texcoord1 : TEXCOORD1;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			uniform half _IsVersion;
			uniform half _IsElementShader;
			uniform half _RenderCategory;
			uniform half _ElementLayerMessage;
			uniform half _ElementLayerWarning;
			uniform half _ElementLayerMask;
			uniform half _render_colormask;
			uniform half _ControlCategory;
			uniform half _LayerCategory;
			uniform half _LayerEnd;
			uniform half _SeasonsEnd;
			uniform half _FadeEnd;
			uniform half _ControlEnd;
			uniform half _ElementEnd;
			uniform half4 _SeasonRemap;
			uniform half _SeasonCategory;
			uniform half _FadeCategory;
			uniform half4 _ControlMaskRemap;
			uniform half _ElementMessage;
			uniform sampler2D _ControlTex1;
			uniform half4 _LayerColor1;
			uniform half4 _LayerColor2;
			uniform half4 _LayerColor3;
			uniform half4 _LayerColor4;
			uniform sampler2D _ControlTex2;
			uniform half4 _LayerColor5;
			uniform half4 _LayerColor6;
			uniform half4 _LayerColor7;
			uniform half4 _LayerColor8;
			uniform sampler2D _ControlTex3;
			uniform half4 _LayerColor9;
			uniform half4 _LayerColor10;
			uniform half4 _LayerColor11;
			uniform half4 _LayerColor12;
			uniform sampler2D _ControlTex4;
			uniform half4 _LayerColor13;
			uniform half4 _LayerColor14;
			uniform half4 _LayerColor15;
			uniform half4 _LayerColor16;
			uniform half _ElementIntensity;
			uniform half4 TVE_SeasonOption;
			uniform half _AdditionalValue1;
			uniform half _AdditionalValue2;
			uniform half TVE_SeasonLerp;
			uniform half _AdditionalValue3;
			uniform half _AdditionalValue4;
			uniform half4 TVE_RenderBasePositionR;
			uniform half _ElementVolumeFadeValue;
			uniform half _ElementVolumeFadeMode;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				o.ase_texcoord1.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord1.zw = 0;
				float3 vertexValue = float3(0, 0, 0);
				#if ASE_ABSOLUTE_VERTEX_POS
				vertexValue = v.vertex.xyz;
				#endif
				vertexValue = vertexValue;
				#if ASE_ABSOLUTE_VERTEX_POS
				v.vertex.xyz = vertexValue;
				#else
				v.vertex.xyz += vertexValue;
				#endif
				o.vertex = UnityObjectToClipPos(v.vertex);

				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
				#endif
				return o;
			}
			
			fixed4 frag (v2f i ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);
				fixed4 finalColor;
				#ifdef ASE_NEEDS_FRAG_WORLD_POSITION
				float3 WorldPosition = i.worldPos;
				#endif
				half Control_Min296_g22461 = _ControlMaskRemap.x;
				half temp_output_7_0_g22464 = Control_Min296_g22461;
				half4 temp_cast_0 = (temp_output_7_0_g22464).xxxx;
				half Control_Max299_g22461 = _ControlMaskRemap.y;
				half temp_output_10_0_g22464 = ( Control_Max299_g22461 - temp_output_7_0_g22464 );
				half4 ControlTex_1123_g22461 = saturate( ( ( tex2D( _ControlTex1, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_0 ) / ( temp_output_10_0_g22464 + 0.0001 ) ) );
				half4 weightedBlendVar16_g22461 = ControlTex_1123_g22461;
				half4 weightedBlend16_g22461 = ( weightedBlendVar16_g22461.x*_LayerColor1 + weightedBlendVar16_g22461.y*_LayerColor2 + weightedBlendVar16_g22461.z*_LayerColor3 + weightedBlendVar16_g22461.w*_LayerColor4 );
				half4 Terrain_Colors_118_g22461 = weightedBlend16_g22461;
				half temp_output_7_0_g22465 = Control_Min296_g22461;
				half4 temp_cast_1 = (temp_output_7_0_g22465).xxxx;
				half temp_output_10_0_g22465 = ( Control_Max299_g22461 - temp_output_7_0_g22465 );
				half4 ControlTex_2124_g22461 = saturate( ( ( tex2D( _ControlTex2, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_1 ) / ( temp_output_10_0_g22465 + 0.0001 ) ) );
				half4 weightedBlendVar15_g22461 = ControlTex_2124_g22461;
				half4 weightedBlend15_g22461 = ( weightedBlendVar15_g22461.x*_LayerColor5 + weightedBlendVar15_g22461.y*_LayerColor6 + weightedBlendVar15_g22461.z*_LayerColor7 + weightedBlendVar15_g22461.w*_LayerColor8 );
				half4 Terrain_Colors_217_g22461 = weightedBlend15_g22461;
				half temp_output_7_0_g22463 = Control_Min296_g22461;
				half4 temp_cast_2 = (temp_output_7_0_g22463).xxxx;
				half temp_output_10_0_g22463 = ( Control_Max299_g22461 - temp_output_7_0_g22463 );
				half4 ControlTex_3307_g22461 = saturate( ( ( tex2D( _ControlTex3, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_2 ) / ( temp_output_10_0_g22463 + 0.0001 ) ) );
				half4 weightedBlendVar339_g22461 = ControlTex_3307_g22461;
				half4 weightedBlend339_g22461 = ( weightedBlendVar339_g22461.x*_LayerColor9 + weightedBlendVar339_g22461.y*_LayerColor10 + weightedBlendVar339_g22461.z*_LayerColor11 + weightedBlendVar339_g22461.w*_LayerColor12 );
				half4 Terrain_Colors_3340_g22461 = weightedBlend339_g22461;
				half temp_output_7_0_g22462 = Control_Min296_g22461;
				half4 temp_cast_3 = (temp_output_7_0_g22462).xxxx;
				half temp_output_10_0_g22462 = ( Control_Max299_g22461 - temp_output_7_0_g22462 );
				half4 ControlTex_4322_g22461 = saturate( ( ( tex2D( _ControlTex4, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_3 ) / ( temp_output_10_0_g22462 + 0.0001 ) ) );
				half4 weightedBlendVar344_g22461 = ControlTex_4322_g22461;
				half4 weightedBlend344_g22461 = ( weightedBlendVar344_g22461.x*_LayerColor13 + weightedBlendVar344_g22461.y*_LayerColor14 + weightedBlendVar344_g22461.z*_LayerColor15 + weightedBlendVar344_g22461.w*_LayerColor16 );
				half4 Terrain_Colors_4350_g22461 = weightedBlend344_g22461;
				half4 Terrain_colors32_g22461 = ( Terrain_Colors_118_g22461 + Terrain_Colors_217_g22461 + Terrain_Colors_3340_g22461 + Terrain_Colors_4350_g22461 );
				half3 Input_Color94_g22466 = (Terrain_colors32_g22461).rgb;
				half3 Element_Valid47_g22466 = Input_Color94_g22466;
				half TVE_SeasonOptions_X66_g22461 = TVE_SeasonOption.x;
				half Additional_Value_Winter90_g22461 = _AdditionalValue1;
				half Additional_Value_Spring88_g22461 = _AdditionalValue2;
				half temp_output_7_0_g22474 = _SeasonRemap.x;
				half temp_output_10_0_g22474 = ( _SeasonRemap.y - temp_output_7_0_g22474 );
				half TVE_SeasonLerp57_g22461 = saturate( saturate( ( ( TVE_SeasonLerp - temp_output_7_0_g22474 ) / temp_output_10_0_g22474 ) ) );
				half lerpResult195_g22461 = lerp( Additional_Value_Winter90_g22461 , Additional_Value_Spring88_g22461 , TVE_SeasonLerp57_g22461);
				half TVE_SeasonOptions_Y64_g22461 = TVE_SeasonOption.y;
				half Additional_Value_Summer80_g22461 = _AdditionalValue3;
				half lerpResult197_g22461 = lerp( Additional_Value_Spring88_g22461 , Additional_Value_Summer80_g22461 , TVE_SeasonLerp57_g22461);
				half TVE_SeasonOptions_Z61_g22461 = TVE_SeasonOption.z;
				half Additional_Value_Autumn101_g22461 = _AdditionalValue4;
				half lerpResult192_g22461 = lerp( Additional_Value_Summer80_g22461 , Additional_Value_Autumn101_g22461 , TVE_SeasonLerp57_g22461);
				half TVE_SeasonOptions_W62_g22461 = TVE_SeasonOption.w;
				half lerpResult205_g22461 = lerp( Additional_Value_Autumn101_g22461 , Additional_Value_Winter90_g22461 , TVE_SeasonLerp57_g22461);
				half Element_Inflience461_g22461 = ( ( TVE_SeasonOptions_X66_g22461 * lerpResult195_g22461 ) + ( TVE_SeasonOptions_Y64_g22461 * lerpResult197_g22461 ) + ( TVE_SeasonOptions_Z61_g22461 * lerpResult192_g22461 ) + ( TVE_SeasonOptions_W62_g22461 * lerpResult205_g22461 ) );
				half temp_output_7_0_g22477 = 1.0;
				half temp_output_10_0_g22477 = ( _ElementVolumeFadeValue - temp_output_7_0_g22477 );
				half lerpResult18_g22475 = lerp( 1.0 , saturate( ( ( saturate( ( distance( WorldPosition , (TVE_RenderBasePositionR).xyz ) / (TVE_RenderBasePositionR).w ) ) - temp_output_7_0_g22477 ) / temp_output_10_0_g22477 ) ) , _ElementVolumeFadeMode);
				half Element_Intensity235_g22461 = ( _ElementIntensity * Element_Inflience461_g22461 * lerpResult18_g22475 );
				half Input_Alpha48_g22466 = ( Terrain_colors32_g22461.a * Element_Intensity235_g22461 );
				half4 appendResult58_g22466 = (half4(Element_Valid47_g22466 , Input_Alpha48_g22466));
				
				
				finalColor = appendResult58_g22466;
				return finalColor;
			}
			ENDCG
		}
	}
	CustomEditor "TVEShaderGUIElement"
	
	Fallback Off
}
/*ASEBEGIN
Version=19603
Node;AmplifyShaderEditor.FunctionNode;207;-640,-512;Inherit;False;Element Paint;1;;22458;5810d2854679b554b88f8bb18ff3bfa0;0;0;1;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;161;-640,-640;Half;False;Property;_render_colormask;_render_colormask;78;1;[HideInInspector];Create;True;0;0;0;True;0;False;15;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;200;-64,-768;Inherit;False;Element Compile;-1;;22460;5302407fc6d65554791e558e67d59358;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;205;-384,-512;Inherit;False;Element Terrain;10;;22461;a84c2b02263ac4b42be9eb75f696cb74;6,222,0,558,0,557,0,225,0,582,1,589,1;1;609;FLOAT;1;False;2;FLOAT4;230;FLOAT4;550
Node;AmplifyShaderEditor.RangedFloatNode;100;-640,-768;Half;False;Property;_ElementMessage;Element Message;0;0;Create;True;0;0;0;True;1;StyledMessage(Info, Use the Tinting elements to add color tinting to the vegetation assets. Make sure the element size matches your terrain., 0,15);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;201;-64,-512;Half;False;True;-1;2;TVEShaderGUIElement;100;16;BOXOPHOBIC/The Visual Engine/Elements/Paint Tinting (Layered);f4f273c3bb6262d4396be458405e60f9;True;VolumePass;0;0;VolumePass;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;DisableBatching=False=DisableBatching;False;False;0;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;0;False;;True;False;0;False;;0;False;;False;True;2;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;2;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;202;-64,-384;Float;False;False;-1;2;ASEMaterialInspector;100;16;New Amplify Shader;f4f273c3bb6262d4396be458405e60f9;True;VisualPass;0;1;VisualPass;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;False;False;0;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;0;False;;True;False;0;False;;0;False;;False;True;2;False;0;;0;0;Standard;0;False;0
WireConnection;205;609;207;4
WireConnection;201;0;205;230
WireConnection;202;0;205;550
ASEEND*/
//CHKSM=BDB4C4F5B40653515566E85D53B62F24A60B9D5A