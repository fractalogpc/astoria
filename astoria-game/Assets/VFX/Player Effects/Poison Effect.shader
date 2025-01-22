Shader "Hidden/Shader/PoisonEffect"
{
    Properties
    {
        // This property is necessary to make the CommandBuffer.Blit bind the source texture to _MainTex
        _MainTex("Main Texture", 2DArray) = "grey" {}
    }

    HLSLINCLUDE

    #pragma target 4.5
    #pragma only_renderers d3d11 playstation xboxone xboxseries vulkan metal switch

    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Common.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/FXAA.hlsl"
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/PostProcessing/Shaders/RTUpscale.hlsl"

    struct Attributes
    {
        uint vertexID : SV_VertexID;
        UNITY_VERTEX_INPUT_INSTANCE_ID
    };

    struct Varyings
    {
        float4 positionCS : SV_POSITION;
        float2 texcoord   : TEXCOORD0;
        UNITY_VERTEX_OUTPUT_STEREO
    };

    Varyings Vert(Attributes input)
    {
        Varyings output;
        UNITY_SETUP_INSTANCE_ID(input);
        UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);
        output.positionCS = GetFullScreenTriangleVertexPosition(input.vertexID);
        output.texcoord = GetFullScreenTriangleTexCoord(input.vertexID);
        return output;
    }

    // List of properties to control your post process effect
    int _Toggle;
    int _Loops;
    float _Step;
    float _StepAmount;
    float _Speed;
    float4 _Tint;
    float _MixAmount;
    float _ChromAbbOffset;
    TEXTURE2D_X(_MainTex);

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        // Note that if HDUtils.DrawFullScreen is not used to render the post process, you don't need to call ClampAndScaleUVForBilinearPostProcessTexture.

        float4 sourceColor = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, ClampAndScaleUVForBilinearPostProcessTexture(input.texcoord.xy));

        // Apply greyscale effect
        float2 uv = input.texcoord;
        float2 centeredUV = (2. * input.positionCS - _ScreenParams.xy) / min(_ScreenParams.x, _ScreenParams.y);

        float4 color;
        color = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, uv);
        for (int i = 0; i < _Loops; i++)
        {
            color += SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, uv + (centeredUV * -length(centeredUV * (((sin(_Time * _Speed) + 1.) * _Step) + (float(i) * _StepAmount)))));
        }
        color /= float(_Loops + 1);

        color.x += SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, uv + float2(_ChromAbbOffset, 0.)).x;
        color.y += SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, uv + float2(0., _ChromAbbOffset)).y;
        color.z += SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, uv + (centeredUV * -length(centeredUV * _ChromAbbOffset))).z;

        color /= 2.;

        color *= _Tint;

        color = lerp(sourceColor, color, _MixAmount);
        
        return color;
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Poison Effect"

            ZWrite Off
            ZTest Always
            Blend Off
            Cull Off

            HLSLPROGRAM
                #pragma fragment CustomPostProcess
                #pragma vertex Vert
            ENDHLSL
        }
    }
    Fallback Off
}
