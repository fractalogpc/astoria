// Made with Amplify Shader Editor v1.9.6.3
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "BOXOPHOBIC/The Visual Engine/Elements/Land Height (Additive)"
{
	Properties
	{
		[StyledMessage(Info, Use the Height Additive elements to offset to the height of the Land elements., 0, 15)]_ElementMessage("Element Message", Float) = 0
		[HideInInspector]_IsVersion("_IsVersion", Float) = 2040
		[HideInInspector]_IsElementShader("_IsElementShader", Float) = 1
		[StyledCategory(Render Settings, true, 0, 10)]_RenderCategory("[ Render Category ]", Float) = 1
		_ElementIntensity("Render Intensity", Range( 0 , 1)) = 1
		[StyledMessage(Info, When using a higher Layer number the Global Volume will create more render textures to render the elements. Try using fewer layers when possible., _ElementLayerMessage, 1, 10, 10)]_ElementLayerMessage("Render Layer Message", Float) = 0
		[StyledMessage(Warning, When using all layers the Global Volume will create one render texture for each layer to render the elements. Try using fewer layers when possible., _ElementLayerWarning, 1, 10, 10)]_ElementLayerWarning("Render Layer Warning", Float) = 0
		[StyledMask(Form Layers, Default 0 Layer_1 1 Layer_2 2 Layer_3 3 Layer_4 4 Layer_5 5 Layer_6 6 Layer_7 7 Layer_8 8, 0, 0)]_ElementLayerMask("Render Layer", Float) = 1
		[StyledSpace(10)]_EndRender("[ End Render ]", Float) = 1
		[StyledCategory(Element Settings, true, 0, 10)]_CategoryElement("[ Category Element ]", Float) = 1
		[NoScaleOffset][StyledTextureSingleLine]_MainTex("Element Texture", 2D) = "white" {}
		[Enum(Main UV,0,Planar,1)][Space(10)]_ElementUVsMode("Element Sampling", Float) = 0
		[StyledVector(9)]_MainUVs("Element UV Value", Vector) = (1,1,0,0)
		[StyledRemapSlider(_MainTexAlphaMinValue, _MainTexAlphaMaxValue, 0, 1)]_MainTexAlphaRemap("Element Alpha", Vector) = (0,0,0,0)
		[HideInInspector]_MainTexAlphaMinValue("Element Alpha Min", Range( 0 , 1)) = 0
		[HideInInspector]_MainTexAlphaMaxValue("Element Alpha Max", Range( 0 , 1)) = 1
		[Space(10)]_HeightOffsetValue("Height Offset", Float) = 0
		[StyledSpace(10)]_EndElement("[ End Element ]", Float) = 1
		[StyledCategory(Fading Settings, true, 0, 10)]_CategoryFade("[ Category Fade ]", Float) = 1
		[Enum(Off,0,On,1)]_ElementVolumeFadeMode("Bounds Fade", Float) = 0
		_ElementVolumeFadeValue("Bounds Fade Blend", Range( 0 , 1)) = 0.75
		[StyledSpace(10)]_EndFade("[ End Fade ]", Float) = 1

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
			Blend One One
			AlphaToMask Off
			Cull Back
			ColorMask B
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
			uniform half4 _MainTexAlphaRemap;
			uniform half _CategoryElement;
			uniform half _CategoryFade;
			uniform half _EndElement;
			uniform half _EndRender;
			uniform half _EndFade;
			uniform half _ElementMessage;
			uniform float _HeightOffsetValue;
			uniform sampler2D _MainTex;
			uniform half _ElementUVsMode;
			uniform half4 _MainUVs;
			uniform half _MainTexAlphaMinValue;
			uniform half _MainTexAlphaMaxValue;
			uniform float _ElementIntensity;
			uniform half4 TVE_RenderBasePositionR;
			uniform float _ElementVolumeFadeValue;
			uniform float _ElementVolumeFadeMode;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (v.vertex).xyz, 1 )).xyz;
				float2 appendResult232 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 lerpResult228 = lerp( ( 1.0 - v.ase_texcoord.xy ) , appendResult232 , _ElementUVsMode);
				float2 vertexToFrag11_g23057 = ( ( lerpResult228 * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord1.xy = vertexToFrag11_g23057;
				
				
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
				half Element_Offset212 = _HeightOffsetValue;
				float2 vertexToFrag11_g23057 = i.ase_texcoord1.xy;
				half2 UV238 = vertexToFrag11_g23057;
				half4 MainTex_RGBA199 = tex2D( _MainTex, UV238 );
				float clampResult17_g23058 = clamp( (MainTex_RGBA199).a , 0.0001 , 0.9999 );
				float temp_output_7_0_g23071 = _MainTexAlphaMinValue;
				float temp_output_10_0_g23071 = ( _MainTexAlphaMaxValue - temp_output_7_0_g23071 );
				half Element_Remap_A209 = saturate( ( ( clampResult17_g23058 - temp_output_7_0_g23071 ) / ( temp_output_10_0_g23071 + 0.0001 ) ) );
				float temp_output_7_0_g23061 = 1.0;
				float temp_output_10_0_g23061 = ( _ElementVolumeFadeValue - temp_output_7_0_g23061 );
				float lerpResult18_g23059 = lerp( 1.0 , saturate( ( ( saturate( ( distance( WorldPosition , (TVE_RenderBasePositionR).xyz ) / (TVE_RenderBasePositionR).w ) ) - temp_output_7_0_g23061 ) / temp_output_10_0_g23061 ) ) , _ElementVolumeFadeMode);
				half Element_Alpha182 = ( _ElementIntensity * lerpResult18_g23059 );
				float4 appendResult169 = (float4(0.0 , 0.0 , ( Element_Offset212 * Element_Remap_A209 * Element_Alpha182 ) , 0.0));
				
				
				finalColor = appendResult169;
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
			uniform half4 _MainTexAlphaRemap;
			uniform half _CategoryElement;
			uniform half _CategoryFade;
			uniform half _EndElement;
			uniform half _EndRender;
			uniform half _EndFade;
			uniform half _ElementMessage;
			uniform float _HeightOffsetValue;
			uniform sampler2D _MainTex;
			uniform half _ElementUVsMode;
			uniform half4 _MainUVs;
			uniform half _MainTexAlphaMinValue;
			uniform half _MainTexAlphaMaxValue;
			uniform float _ElementIntensity;
			uniform half4 TVE_RenderBasePositionR;
			uniform float _ElementVolumeFadeValue;
			uniform float _ElementVolumeFadeMode;

			
			v2f vert ( appdata v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
				UNITY_TRANSFER_INSTANCE_ID(v, o);

				float3 ase_worldPos = mul(unity_ObjectToWorld, float4( (v.vertex).xyz, 1 )).xyz;
				float2 appendResult232 = (float2(ase_worldPos.x , ase_worldPos.z));
				float2 lerpResult228 = lerp( ( 1.0 - v.ase_texcoord.xy ) , appendResult232 , _ElementUVsMode);
				float2 vertexToFrag11_g23057 = ( ( lerpResult228 * (_MainUVs).xy ) + (_MainUVs).zw );
				o.ase_texcoord1.xy = vertexToFrag11_g23057;
				
				
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
				half Element_Offset212 = _HeightOffsetValue;
				float3 temp_cast_0 = (saturate( Element_Offset212 )).xxx;
				half3 Input_Color94_g23073 = temp_cast_0;
				float3 break68_g23073 = Input_Color94_g23073;
				float clampResult80_g23073 = clamp( max( max( break68_g23073.x , break68_g23073.y ) , break68_g23073.z ) , 0.1 , 10000.0 );
				float4 color185 = IsGammaSpace() ? float4(0,0.2,1,0) : float4(0,0.03310476,1,0);
				half3 Input_Tint76_g23073 = (color185).rgb;
				half3 Element_Valid47_g23073 = ( clampResult80_g23073 * Input_Tint76_g23073 );
				float2 vertexToFrag11_g23057 = i.ase_texcoord1.xy;
				half2 UV238 = vertexToFrag11_g23057;
				half4 MainTex_RGBA199 = tex2D( _MainTex, UV238 );
				float clampResult17_g23058 = clamp( (MainTex_RGBA199).a , 0.0001 , 0.9999 );
				float temp_output_7_0_g23071 = _MainTexAlphaMinValue;
				float temp_output_10_0_g23071 = ( _MainTexAlphaMaxValue - temp_output_7_0_g23071 );
				half Element_Remap_A209 = saturate( ( ( clampResult17_g23058 - temp_output_7_0_g23071 ) / ( temp_output_10_0_g23071 + 0.0001 ) ) );
				float temp_output_7_0_g23061 = 1.0;
				float temp_output_10_0_g23061 = ( _ElementVolumeFadeValue - temp_output_7_0_g23061 );
				float lerpResult18_g23059 = lerp( 1.0 , saturate( ( ( saturate( ( distance( WorldPosition , (TVE_RenderBasePositionR).xyz ) / (TVE_RenderBasePositionR).w ) ) - temp_output_7_0_g23061 ) / temp_output_10_0_g23061 ) ) , _ElementVolumeFadeMode);
				half Element_Alpha182 = ( _ElementIntensity * lerpResult18_g23059 );
				half Input_Alpha48_g23073 = ( Element_Remap_A209 * Element_Alpha182 );
				float4 appendResult58_g23073 = (float4(Element_Valid47_g23073 , Input_Alpha48_g23073));
				
				
				finalColor = appendResult58_g23073;
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
Node;AmplifyShaderEditor.WorldPosInputsNode;229;-2048,-896;Inherit;False;0;4;FLOAT3;0;FLOAT;1;FLOAT;2;FLOAT;3
Node;AmplifyShaderEditor.TexCoordVertexDataNode;230;-2048,-1024;Inherit;False;0;2;0;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.OneMinusNode;231;-1856,-1024;Inherit;False;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.DynamicAppendNode;232;-1856,-896;Inherit;False;FLOAT2;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector4Node;234;-1664,-896;Half;False;Property;_MainUVs;Element UV Value;14;0;Create;False;0;0;0;False;1;StyledVector(9);False;1,1,0,0;1,1,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;237;-2048,-736;Half;False;Property;_ElementUVsMode;Element Sampling;13;1;[Enum];Create;False;0;2;Main UV;0;Planar;1;0;False;1;Space(10);False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.LerpOp;228;-1664,-1024;Inherit;False;3;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;236;-1440,-896;Inherit;False;FLOAT2;0;1;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;226;-1280,-1024;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SwizzleNode;235;-1440,-816;Inherit;False;FLOAT2;2;3;2;3;1;0;FLOAT4;0,0,0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;227;-1088,-1024;Inherit;False;2;2;0;FLOAT2;0,0;False;1;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FunctionNode;233;-896,-1024;Inherit;False;Per Vertex;-1;;23057;db7dd586c7d3fd34786fd504127455fc;0;1;3;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;238;-704,-1024;Half;False;UV;-1;True;1;0;FLOAT2;0,0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.GetLocalVarNode;239;-2048,-512;Inherit;False;238;UV;1;0;OBJECT;;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;200;-1664,-512;Inherit;True;Property;_MainTex;Element Texture;12;1;[NoScaleOffset];Create;False;0;0;0;False;1;StyledTextureSingleLine;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;8;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;6;FLOAT;0;False;7;SAMPLERSTATE;;False;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.RegisterLocalVarNode;199;-944,-512;Half;False;MainTex_RGBA;-1;True;1;0;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.GetLocalVarNode;205;-2048,0;Inherit;False;199;MainTex_RGBA;1;0;OBJECT;;False;1;COLOR;0
Node;AmplifyShaderEditor.SwizzleNode;208;-1792,0;Inherit;False;FLOAT;3;1;2;3;1;0;COLOR;0,0,0,0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;204;-1792,192;Half;False;Property;_MainTexAlphaMaxValue;Element Alpha Max;17;1;[HideInInspector];Create;False;0;0;0;False;0;False;1;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;206;-1792,128;Half;False;Property;_MainTexAlphaMinValue;Element Alpha Min;16;1;[HideInInspector];Create;False;0;0;0;False;0;False;0;0;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;210;-1536,0;Inherit;False;Math Clamp;-1;;23058;be0e6188e535d474483310546a0d9e78;0;1;6;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;174;-2048,576;Inherit;False;Element Fade Volume;21;;23059;4935729172cdadd45b9b14c3fa9c1db4;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;225;-2048,512;Inherit;False;Element Form;1;;23069;bc58488265c2ed6408843a733b1a9124;0;0;1;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;211;-1280,0;Inherit;False;Math Remap;-1;;23071;5eda8a2bb94ebef4ab0e43d19291cd8b;1,14,0;3;6;FLOAT;0;False;7;FLOAT;0;False;8;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;170;-1408,512;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;194;-2048,896;Inherit;False;Property;_HeightOffsetValue;Height Offset;18;0;Create;False;0;0;0;False;1;Space(10);False;0;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;182;-944,512;Half;False;Element_Alpha;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;209;-960,0;Half;False;Element_Remap_A;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RegisterLocalVarNode;212;-944,896;Half;False;Element_Offset;-1;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;213;256,-640;Inherit;False;212;Element_Offset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;216;256,-512;Inherit;False;182;Element_Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;217;256,-576;Inherit;False;209;Element_Remap_A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SaturateNode;187;512,-640;Inherit;False;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;218;512,-576;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;185;256,-448;Inherit;False;Constant;_Color1;Color 1;15;0;Create;True;0;0;0;False;0;False;0,0.2,1,0;0,0,0,0;True;True;0;6;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4;FLOAT3;5
Node;AmplifyShaderEditor.FunctionNode;167;-1344,-1536;Inherit;False;Element Compile;-1;;23072;5302407fc6d65554791e558e67d59358;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.Vector4Node;207;-2048,128;Half;False;Property;_MainTexAlphaRemap;Element Alpha;15;0;Create;False;0;0;0;True;1;StyledRemapSlider(_MainTexAlphaMinValue, _MainTexAlphaMaxValue, 0, 1);False;0,0,0,0;0,0,0,0;0;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FunctionNode;180;1024,-640;Inherit;False;Element Visuals;-1;;23073;78cf0f00cfd72824e84597f2db54a76e;1,64,1;3;59;FLOAT3;0,0,0;False;117;FLOAT;0;False;77;COLOR;1,1,1,1;False;1;FLOAT4;0
Node;AmplifyShaderEditor.DynamicAppendNode;169;1024,-1024;Inherit;False;FLOAT4;4;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;3;FLOAT;0;False;1;FLOAT4;0
Node;AmplifyShaderEditor.GetLocalVarNode;176;256,-1024;Inherit;False;212;Element_Offset;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;215;512,-1024;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;214;256,-960;Inherit;False;209;Element_Remap_A;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.GetLocalVarNode;183;256,-896;Inherit;False;182;Element_Alpha;1;0;OBJECT;;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;220;-1792,-1536;Half;False;Property;_CategoryElement;[ Category Element ];11;0;Create;True;0;0;0;True;1;StyledCategory(Element Settings, true, 0, 10);False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;221;-1584,-1536;Half;False;Property;_CategoryFade;[ Category Fade ];20;0;Create;True;0;0;0;True;1;StyledCategory(Fading Settings, true, 0, 10);False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;223;-1792,-1472;Half;False;Property;_EndElement;[ End Element ];19;0;Create;True;0;0;0;True;1;StyledSpace(10);False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;224;-1792,-1408;Half;False;Property;_EndRender;[ End Render ];10;0;Create;True;0;0;0;True;1;StyledSpace(10);False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;222;-1584,-1472;Half;False;Property;_EndFade;[ End Fade ];24;0;Create;True;0;0;0;True;1;StyledSpace(10);False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;112;-2048,-1536;Half;False;Property;_ElementMessage;Element Message;0;0;Create;True;0;0;0;True;1;StyledMessage(Info, Use the Height Additive elements to offset to the height of the Land elements., 0, 15);False;0;0;1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;178;1472,-640;Float;False;False;-1;2;ASEMaterialInspector;100;16;New Amplify Shader;f4f273c3bb6262d4396be458405e60f9;True;VisualPass;0;1;VisualPass;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;RenderType=Opaque=RenderType;False;False;0;True;True;2;5;False;;10;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;False;True;True;True;True;True;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;0;False;;True;False;0;False;;0;False;;False;True;2;False;0;;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;177;1472,-1024;Float;False;True;-1;2;TVEShaderGUIElement;100;16;BOXOPHOBIC/The Visual Engine/Elements/Land Height (Additive);f4f273c3bb6262d4396be458405e60f9;True;VolumePass;0;0;VolumePass;2;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderType=Transparent=RenderType;Queue=Transparent=Queue=0;DisableBatching=True=DisableBatching;False;False;0;True;True;4;1;False;;1;False;;0;1;False;;0;False;;True;0;False;;0;False;;False;False;False;False;False;False;False;False;False;True;0;False;;False;True;0;False;;True;True;False;False;True;False;0;False;;False;False;False;False;False;False;False;True;False;0;False;;255;False;;255;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;0;False;;True;True;2;False;;True;0;False;;True;False;0;False;;0;False;;False;True;2;False;0;;0;0;Standard;1;Vertex Position,InvertActionOnDeselection;1;0;0;2;True;True;False;;False;0
WireConnection;231;0;230;0
WireConnection;232;0;229;1
WireConnection;232;1;229;3
WireConnection;228;0;231;0
WireConnection;228;1;232;0
WireConnection;228;2;237;0
WireConnection;236;0;234;0
WireConnection;226;0;228;0
WireConnection;226;1;236;0
WireConnection;235;0;234;0
WireConnection;227;0;226;0
WireConnection;227;1;235;0
WireConnection;233;3;227;0
WireConnection;238;0;233;0
WireConnection;200;1;239;0
WireConnection;199;0;200;0
WireConnection;208;0;205;0
WireConnection;210;6;208;0
WireConnection;211;6;210;0
WireConnection;211;7;206;0
WireConnection;211;8;204;0
WireConnection;170;0;225;4
WireConnection;170;1;174;0
WireConnection;182;0;170;0
WireConnection;209;0;211;0
WireConnection;212;0;194;0
WireConnection;187;0;213;0
WireConnection;218;0;217;0
WireConnection;218;1;216;0
WireConnection;180;59;187;0
WireConnection;180;117;218;0
WireConnection;180;77;185;0
WireConnection;169;2;215;0
WireConnection;215;0;176;0
WireConnection;215;1;214;0
WireConnection;215;2;183;0
WireConnection;178;0;180;0
WireConnection;177;0;169;0
ASEEND*/
//CHKSM=818FCCCF52A498CBC56F3F3FFF86314FC2941612