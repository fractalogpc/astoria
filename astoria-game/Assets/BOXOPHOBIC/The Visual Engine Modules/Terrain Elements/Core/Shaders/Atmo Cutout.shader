// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Visual Engine/Elements/Atmo Cutout (Layered)"
{
	Properties
	{
		[StyledMessage(Info, Use the Cutout elements to reduce the leaves amount or the alpha treshold. Useful to create winter sceneries or dead forests and dissolve effects. Make sure the element size matches your terrain., 0,15)]_ElementMessage("Element Message", Float) = 0
		[HideInInspector]_IsVersion("_IsVersion", Float) = 2040
		[HideInInspector]_IsElementShader("_IsElementShader", Float) = 1
		[StyledCategory(Render Settings, true, 0, 10)]_RenderCategory("[ Render Category ]", Float) = 1
		_ElementIntensity("Render Intensity", Range( 0 , 1)) = 1
		[StyledMessage(Info, When using a higher Layer number the Global Volume will create more render textures to render the elements. Try using fewer layers when possible., _ElementLayerMessage, 1, 10, 10)]_ElementLayerMessage("Render Layer Message", Float) = 0
		[StyledMessage(Warning, When using all layers the Global Volume will create one render texture for each layer to render the elements. Try using fewer layers when possible., _ElementLayerWarning, 1, 10, 10)]_ElementLayerWarning("Render Layer Warning", Float) = 0
		[StyledMask(Atmo Layers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_ElementLayerMask("Render Layer", Float) = 1
		[Enum(Multiply,0,Additive,1)]_ElementBlendA("Render Blend", Float) = 0
		[StyledSpace(10)]_ElementEnd("[ Element End ]", Float) = 1
		[StyledCategory(Control Settings, true, 0, 10)]_ControlCategory("[ Control Category ]", Float) = 1
		[NoScaleOffset][StyledTextureSingleLine]_ControlTex1("Control 01", 2D) = "black" {}
		[HDR][Gamma][NoScaleOffset][StyledTextureSingleLine]_ControlTex2("Control 02", 2D) = "black" {}
		[HDR][Gamma][NoScaleOffset][StyledTextureSingleLine]_ControlTex3("Control 03", 2D) = "black" {}
		[HDR][Gamma][NoScaleOffset][StyledTextureSingleLine]_ControlTex4("Control 04", 2D) = "black" {}
		[Space(10)][StyledRemapSlider]_ControlMaskRemap("Control Remap", Vector) = (0,1,0,0)
		[StyledSpace(10)]_ControlEnd("[ Control End ]", Float) = 1
		[StyledCategory(Layer Settings, true, 0, 10)]_LayerCategory("[ Layer Category ]", Float) = 1
		_LayerValue1("Layer 01", Range( 0 , 1)) = 1
		_LayerValue2("Layer 02", Range( 0 , 1)) = 1
		_LayerValue3("Layer 03", Range( 0 , 1)) = 1
		_LayerValue4("Layer 04", Range( 0 , 1)) = 1
		[Space(10)]_LayerValue5("Layer 05", Range( 0 , 1)) = 1
		_LayerValue6("Layer 06", Range( 0 , 1)) = 1
		_LayerValue7("Layer 07", Range( 0 , 1)) = 1
		_LayerValue8("Layer 08", Range( 0 , 1)) = 1
		[Space(10)]_LayerValue9("Layer 09", Range( 0 , 1)) = 1
		_LayerValue10("Layer 10", Range( 0 , 1)) = 1
		_LayerValue11("Layer 11", Range( 0 , 1)) = 1
		_LayerValue12("Layer 12", Range( 0 , 1)) = 1
		[Space(10)]_LayerValue13("Layer 13", Range( 0 , 1)) = 1
		_LayerValue14("Layer 14", Range( 0 , 1)) = 1
		_LayerValue15("Layer 15", Range( 0 , 1)) = 1
		_LayerValue16("Layer 16", Range( 0 , 1)) = 1
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
		[HideInInspector]_render_src("_render_src", Float) = 2
		[HideInInspector]_render_dst("_render_dst", Float) = 0

	}
	
	SubShader
	{
		
		
		Tags { "RenderType"="Transparent" "Queue"="Transparent" "DisableBatching"="True" }
	LOD 100

		
		Pass
		{
			
			Name "VolumePass"
			
			CGINCLUDE
			#pragma target 3.0
			ENDCG
			Blend One Zero, [_render_src] [_render_dst]
			AlphaToMask Off
			Cull Back
			ColorMask A
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
			uniform half _render_src;
			uniform half _render_dst;
			uniform half _ElementMessage;
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
			uniform sampler2D _ControlTex1;
			uniform half _LayerValue1;
			uniform half _LayerValue2;
			uniform half _LayerValue3;
			uniform half _LayerValue4;
			uniform sampler2D _ControlTex2;
			uniform half _LayerValue5;
			uniform half _LayerValue6;
			uniform half _LayerValue7;
			uniform half _LayerValue8;
			uniform sampler2D _ControlTex3;
			uniform half _LayerValue9;
			uniform half _LayerValue10;
			uniform half _LayerValue11;
			uniform half _LayerValue12;
			uniform sampler2D _ControlTex4;
			uniform half _LayerValue13;
			uniform half _LayerValue14;
			uniform half _LayerValue15;
			uniform half _LayerValue16;
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
			uniform half _ElementBlendA;

			
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
				half Control_Min296_g22333 = _ControlMaskRemap.x;
				half temp_output_7_0_g22336 = Control_Min296_g22333;
				half4 temp_cast_0 = (temp_output_7_0_g22336).xxxx;
				half Control_Max299_g22333 = _ControlMaskRemap.y;
				half temp_output_10_0_g22336 = ( Control_Max299_g22333 - temp_output_7_0_g22336 );
				half4 ControlTex_1123_g22333 = saturate( ( ( tex2D( _ControlTex1, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_0 ) / ( temp_output_10_0_g22336 + 0.0001 ) ) );
				half4 weightedBlendVar138_g22333 = ControlTex_1123_g22333;
				half weightedBlend138_g22333 = ( weightedBlendVar138_g22333.x*_LayerValue1 + weightedBlendVar138_g22333.y*_LayerValue2 + weightedBlendVar138_g22333.z*_LayerValue3 + weightedBlendVar138_g22333.w*_LayerValue4 );
				half Terrain_Values_1141_g22333 = weightedBlend138_g22333;
				half temp_output_7_0_g22337 = Control_Min296_g22333;
				half4 temp_cast_1 = (temp_output_7_0_g22337).xxxx;
				half temp_output_10_0_g22337 = ( Control_Max299_g22333 - temp_output_7_0_g22337 );
				half4 ControlTex_2124_g22333 = saturate( ( ( tex2D( _ControlTex2, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_1 ) / ( temp_output_10_0_g22337 + 0.0001 ) ) );
				half4 weightedBlendVar147_g22333 = ControlTex_2124_g22333;
				half weightedBlend147_g22333 = ( weightedBlendVar147_g22333.x*_LayerValue5 + weightedBlendVar147_g22333.y*_LayerValue6 + weightedBlendVar147_g22333.z*_LayerValue7 + weightedBlendVar147_g22333.w*_LayerValue8 );
				half Terrain_Values_2148_g22333 = weightedBlend147_g22333;
				half temp_output_7_0_g22335 = Control_Min296_g22333;
				half4 temp_cast_2 = (temp_output_7_0_g22335).xxxx;
				half temp_output_10_0_g22335 = ( Control_Max299_g22333 - temp_output_7_0_g22335 );
				half4 ControlTex_3307_g22333 = saturate( ( ( tex2D( _ControlTex3, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_2 ) / ( temp_output_10_0_g22335 + 0.0001 ) ) );
				half4 weightedBlendVar366_g22333 = ControlTex_3307_g22333;
				half weightedBlend366_g22333 = ( weightedBlendVar366_g22333.x*_LayerValue9 + weightedBlendVar366_g22333.y*_LayerValue10 + weightedBlendVar366_g22333.z*_LayerValue11 + weightedBlendVar366_g22333.w*_LayerValue12 );
				half Terrain_Values_3365_g22333 = weightedBlend366_g22333;
				half temp_output_7_0_g22334 = Control_Min296_g22333;
				half4 temp_cast_3 = (temp_output_7_0_g22334).xxxx;
				half temp_output_10_0_g22334 = ( Control_Max299_g22333 - temp_output_7_0_g22334 );
				half4 ControlTex_4322_g22333 = saturate( ( ( tex2D( _ControlTex4, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_3 ) / ( temp_output_10_0_g22334 + 0.0001 ) ) );
				half4 weightedBlendVar367_g22333 = ControlTex_4322_g22333;
				half weightedBlend367_g22333 = ( weightedBlendVar367_g22333.x*_LayerValue13 + weightedBlendVar367_g22333.y*_LayerValue14 + weightedBlendVar367_g22333.z*_LayerValue15 + weightedBlendVar367_g22333.w*_LayerValue16 );
				half Terrain_Values_4368_g22333 = weightedBlend367_g22333;
				half Terrain_Values153_g22333 = ( Terrain_Values_1141_g22333 + Terrain_Values_2148_g22333 + Terrain_Values_3365_g22333 + Terrain_Values_4368_g22333 );
				half TVE_SeasonOptions_X66_g22333 = TVE_SeasonOption.x;
				half Additional_Value_Winter90_g22333 = _AdditionalValue1;
				half Additional_Value_Spring88_g22333 = _AdditionalValue2;
				half temp_output_7_0_g22346 = _SeasonRemap.x;
				half temp_output_10_0_g22346 = ( _SeasonRemap.y - temp_output_7_0_g22346 );
				half TVE_SeasonLerp57_g22333 = saturate( saturate( ( ( TVE_SeasonLerp - temp_output_7_0_g22346 ) / temp_output_10_0_g22346 ) ) );
				half lerpResult195_g22333 = lerp( Additional_Value_Winter90_g22333 , Additional_Value_Spring88_g22333 , TVE_SeasonLerp57_g22333);
				half TVE_SeasonOptions_Y64_g22333 = TVE_SeasonOption.y;
				half Additional_Value_Summer80_g22333 = _AdditionalValue3;
				half lerpResult197_g22333 = lerp( Additional_Value_Spring88_g22333 , Additional_Value_Summer80_g22333 , TVE_SeasonLerp57_g22333);
				half TVE_SeasonOptions_Z61_g22333 = TVE_SeasonOption.z;
				half Additional_Value_Autumn101_g22333 = _AdditionalValue4;
				half lerpResult192_g22333 = lerp( Additional_Value_Summer80_g22333 , Additional_Value_Autumn101_g22333 , TVE_SeasonLerp57_g22333);
				half TVE_SeasonOptions_W62_g22333 = TVE_SeasonOption.w;
				half lerpResult205_g22333 = lerp( Additional_Value_Autumn101_g22333 , Additional_Value_Winter90_g22333 , TVE_SeasonLerp57_g22333);
				half Element_Inflience461_g22333 = ( ( TVE_SeasonOptions_X66_g22333 * lerpResult195_g22333 ) + ( TVE_SeasonOptions_Y64_g22333 * lerpResult197_g22333 ) + ( TVE_SeasonOptions_Z61_g22333 * lerpResult192_g22333 ) + ( TVE_SeasonOptions_W62_g22333 * lerpResult205_g22333 ) );
				half temp_output_7_0_g22349 = 1.0;
				half temp_output_10_0_g22349 = ( _ElementVolumeFadeValue - temp_output_7_0_g22349 );
				half lerpResult18_g22347 = lerp( 1.0 , saturate( ( ( saturate( ( distance( WorldPosition , (TVE_RenderBasePositionR).xyz ) / (TVE_RenderBasePositionR).w ) ) - temp_output_7_0_g22349 ) / temp_output_10_0_g22349 ) ) , _ElementVolumeFadeMode);
				half Element_Intensity235_g22333 = ( _ElementIntensity * Element_Inflience461_g22333 * lerpResult18_g22347 );
				half lerpResult285_g22333 = lerp( 1.0 , Terrain_Values153_g22333 , Element_Intensity235_g22333);
				half Element_BlendA433_g22333 = _ElementBlendA;
				half lerpResult444_g22333 = lerp( lerpResult285_g22333 , ( Terrain_Values153_g22333 * Element_Intensity235_g22333 ) , Element_BlendA433_g22333);
				half4 appendResult287_g22333 = (half4(0.0 , 0.0 , 0.0 , lerpResult444_g22333));
				
				
				finalColor = appendResult287_g22333;
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
			Offset 0 , 0
			
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
			uniform half _render_src;
			uniform half _render_dst;
			uniform half _ElementMessage;
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
			uniform sampler2D _ControlTex1;
			uniform half _LayerValue1;
			uniform half _LayerValue2;
			uniform half _LayerValue3;
			uniform half _LayerValue4;
			uniform sampler2D _ControlTex2;
			uniform half _LayerValue5;
			uniform half _LayerValue6;
			uniform half _LayerValue7;
			uniform half _LayerValue8;
			uniform sampler2D _ControlTex3;
			uniform half _LayerValue9;
			uniform half _LayerValue10;
			uniform half _LayerValue11;
			uniform half _LayerValue12;
			uniform sampler2D _ControlTex4;
			uniform half _LayerValue13;
			uniform half _LayerValue14;
			uniform half _LayerValue15;
			uniform half _LayerValue16;
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
				half Control_Min296_g22333 = _ControlMaskRemap.x;
				half temp_output_7_0_g22336 = Control_Min296_g22333;
				half4 temp_cast_0 = (temp_output_7_0_g22336).xxxx;
				half Control_Max299_g22333 = _ControlMaskRemap.y;
				half temp_output_10_0_g22336 = ( Control_Max299_g22333 - temp_output_7_0_g22336 );
				half4 ControlTex_1123_g22333 = saturate( ( ( tex2D( _ControlTex1, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_0 ) / ( temp_output_10_0_g22336 + 0.0001 ) ) );
				half4 weightedBlendVar138_g22333 = ControlTex_1123_g22333;
				half weightedBlend138_g22333 = ( weightedBlendVar138_g22333.x*_LayerValue1 + weightedBlendVar138_g22333.y*_LayerValue2 + weightedBlendVar138_g22333.z*_LayerValue3 + weightedBlendVar138_g22333.w*_LayerValue4 );
				half Terrain_Values_1141_g22333 = weightedBlend138_g22333;
				half temp_output_7_0_g22337 = Control_Min296_g22333;
				half4 temp_cast_1 = (temp_output_7_0_g22337).xxxx;
				half temp_output_10_0_g22337 = ( Control_Max299_g22333 - temp_output_7_0_g22337 );
				half4 ControlTex_2124_g22333 = saturate( ( ( tex2D( _ControlTex2, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_1 ) / ( temp_output_10_0_g22337 + 0.0001 ) ) );
				half4 weightedBlendVar147_g22333 = ControlTex_2124_g22333;
				half weightedBlend147_g22333 = ( weightedBlendVar147_g22333.x*_LayerValue5 + weightedBlendVar147_g22333.y*_LayerValue6 + weightedBlendVar147_g22333.z*_LayerValue7 + weightedBlendVar147_g22333.w*_LayerValue8 );
				half Terrain_Values_2148_g22333 = weightedBlend147_g22333;
				half temp_output_7_0_g22335 = Control_Min296_g22333;
				half4 temp_cast_2 = (temp_output_7_0_g22335).xxxx;
				half temp_output_10_0_g22335 = ( Control_Max299_g22333 - temp_output_7_0_g22335 );
				half4 ControlTex_3307_g22333 = saturate( ( ( tex2D( _ControlTex3, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_2 ) / ( temp_output_10_0_g22335 + 0.0001 ) ) );
				half4 weightedBlendVar366_g22333 = ControlTex_3307_g22333;
				half weightedBlend366_g22333 = ( weightedBlendVar366_g22333.x*_LayerValue9 + weightedBlendVar366_g22333.y*_LayerValue10 + weightedBlendVar366_g22333.z*_LayerValue11 + weightedBlendVar366_g22333.w*_LayerValue12 );
				half Terrain_Values_3365_g22333 = weightedBlend366_g22333;
				half temp_output_7_0_g22334 = Control_Min296_g22333;
				half4 temp_cast_3 = (temp_output_7_0_g22334).xxxx;
				half temp_output_10_0_g22334 = ( Control_Max299_g22333 - temp_output_7_0_g22334 );
				half4 ControlTex_4322_g22333 = saturate( ( ( tex2D( _ControlTex4, ( 1.0 - i.ase_texcoord1.xy ) ) - temp_cast_3 ) / ( temp_output_10_0_g22334 + 0.0001 ) ) );
				half4 weightedBlendVar367_g22333 = ControlTex_4322_g22333;
				half weightedBlend367_g22333 = ( weightedBlendVar367_g22333.x*_LayerValue13 + weightedBlendVar367_g22333.y*_LayerValue14 + weightedBlendVar367_g22333.z*_LayerValue15 + weightedBlendVar367_g22333.w*_LayerValue16 );
				half Terrain_Values_4368_g22333 = weightedBlend367_g22333;
				half Terrain_Values153_g22333 = ( Terrain_Values_1141_g22333 + Terrain_Values_2148_g22333 + Terrain_Values_3365_g22333 + Terrain_Values_4368_g22333 );
				half3 temp_cast_4 = (Terrain_Values153_g22333).xxx;
				half3 Input_Color94_g22342 = temp_cast_4;
				half3 break68_g22342 = Input_Color94_g22342;
				half clampResult80_g22342 = clamp( max( max( break68_g22342.x , break68_g22342.y ) , break68_g22342.z ) , 0.1 , 10000.0 );
				half4 color563_g22333 = IsGammaSpace() ? half4(0.5,0.5,0.5,0) : half4(0.2140411,0.2140411,0.2140411,0);
				half3 Input_Tint76_g22342 = (color563_g22333).rgb;
				half3 Element_Valid47_g22342 = ( clampResult80_g22342 * Input_Tint76_g22342 );
				half TVE_SeasonOptions_X66_g22333 = TVE_SeasonOption.x;
				half Additional_Value_Winter90_g22333 = _AdditionalValue1;
				half Additional_Value_Spring88_g22333 = _AdditionalValue2;
				half temp_output_7_0_g22346 = _SeasonRemap.x;
				half temp_output_10_0_g22346 = ( _SeasonRemap.y - temp_output_7_0_g22346 );
				half TVE_SeasonLerp57_g22333 = saturate( saturate( ( ( TVE_SeasonLerp - temp_output_7_0_g22346 ) / temp_output_10_0_g22346 ) ) );
				half lerpResult195_g22333 = lerp( Additional_Value_Winter90_g22333 , Additional_Value_Spring88_g22333 , TVE_SeasonLerp57_g22333);
				half TVE_SeasonOptions_Y64_g22333 = TVE_SeasonOption.y;
				half Additional_Value_Summer80_g22333 = _AdditionalValue3;
				half lerpResult197_g22333 = lerp( Additional_Value_Spring88_g22333 , Additional_Value_Summer80_g22333 , TVE_SeasonLerp57_g22333);
				half TVE_SeasonOptions_Z61_g22333 = TVE_SeasonOption.z;
				half Additional_Value_Autumn101_g22333 = _AdditionalValue4;
				half lerpResult192_g22333 = lerp( Additional_Value_Summer80_g22333 , Additional_Value_Autumn101_g22333 , TVE_SeasonLerp57_g22333);
				half TVE_SeasonOptions_W62_g22333 = TVE_SeasonOption.w;
				half lerpResult205_g22333 = lerp( Additional_Value_Autumn101_g22333 , Additional_Value_Winter90_g22333 , TVE_SeasonLerp57_g22333);
				half Element_Inflience461_g22333 = ( ( TVE_SeasonOptions_X66_g22333 * lerpResult195_g22333 ) + ( TVE_SeasonOptions_Y64_g22333 * lerpResult197_g22333 ) + ( TVE_SeasonOptions_Z61_g22333 * lerpResult192_g22333 ) + ( TVE_SeasonOptions_W62_g22333 * lerpResult205_g22333 ) );
				half temp_output_7_0_g22349 = 1.0;
				half temp_output_10_0_g22349 = ( _ElementVolumeFadeValue - temp_output_7_0_g22349 );
				half lerpResult18_g22347 = lerp( 1.0 , saturate( ( ( saturate( ( distance( WorldPosition , (TVE_RenderBasePositionR).xyz ) / (TVE_RenderBasePositionR).w ) ) - temp_output_7_0_g22349 ) / temp_output_10_0_g22349 ) ) , _ElementVolumeFadeMode);
				half Element_Intensity235_g22333 = ( _ElementIntensity * Element_Inflience461_g22333 * lerpResult18_g22347 );
				half Input_Alpha48_g22342 = Element_Intensity235_g22333;
				half4 appendResult58_g22342 = (half4(Element_Valid47_g22342 , Input_Alpha48_g22342));
				
				
				finalColor = appendResult58_g22342;
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
Node;AmplifyShaderEditor.FunctionNode;121;-640,-1024;Inherit;False;Element Atmo;1;;22330;825a873635c1c404399c06fe163ce11a;0;0;1;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;113;-640,-1280;Inherit;False;Property;_render_src;_render_src;78;1;[HideInInspector];Create;True;0;0;0;True;0;False;2;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;114;-480,-1280;Inherit;False;Property;_render_dst;_render_dst;79;1;[HideInInspector];Create;True;0;0;0;True;0;False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;115;-64,-1408;Inherit;False;Element Compile;-1;;22332;5302407fc6d65554791e558e67d59358;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;104;-640,-1408;Half;False;Property;_ElementMessage;Element Message;0;0;Create;True;0;0;0;True;1;StyledMessage(Info, Use the Cutout elements to reduce the leaves amount or the alpha treshold. Useful to create winter sceneries or dead forests and dissolve effects. Make sure the element size matches your terrain., 0,15);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;122;-384,-1024;Inherit;False;Element Terrain;10;;22333;a84c2b02263ac4b42be9eb75f696cb74;6,222,1,558,1,557,3,225,3,582,1,589,1;1;609;FLOAT;1;False;2;FLOAT4;230;FLOAT4;550
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;116;-64,-1024;Half;False;True;-1;2;TVEShaderGUIElement;100;16;BOXOPHOBIC/The Visual Engine/Elements/Atmo Cutout (Layered);f4f273c3bb6262d4396be458405e60f9;True;VolumePass;0;0;VolumePass;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;DisableBatching=True=DisableBatching;False;False;0;True;True;0;1;False;;0;False;;1;0;True;_render_src;0;True;_render_dst;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;True;True;False;False;False;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;0;False;;True;False;0;False;;0;False;;False;True;2;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;2;True;True;False;;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;117;-64,-896;Float;False;False;-1;2;ASEMaterialInspector;100;16;New Amplify Shader;f4f273c3bb6262d4396be458405e60f9;True;VisualPass;0;1;VisualPass;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;False;False;0;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;0;False;;True;True;0;False;;0;False;;False;True;2;False;0;;0;0;Standard;0;False;0
WireConnection;122;609;121;4
WireConnection;116;0;122;230
WireConnection;117;0;122;550
ASEEND*/
//CHKSM=802024D8FAAA6D9B295B3A0C704E011169530FB5