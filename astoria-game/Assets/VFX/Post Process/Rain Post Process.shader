Shader "Hidden/Shader/RainPostProcess"
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
    #include "Packages/com.unity.render-pipelines.high-definition/Runtime/ShaderLibrary/ShaderVariables.hlsl"

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

    float2 hash2(float2 seed) { //as far as I can tell,to make a hash you basically just add a bunch of numbers
        float2 hash = frac(seed.yx * float2(1.1042, 1.10523)); // to try and make everything as jumbled as possible
        hash += dot(hash, hash.yx - 222.6234); // Then you fract() it to get it in the 0 - 1 range
        hash *= frac(hash + .99340184015);
        hash -= dot(frac(hash), hash);
        hash = frac(hash.yx - hash.xx * 12.);
        hash *= 1.; // this makes the results smaller, so the two spheres don't get too far apart
        return hash;
    }

    float SmoothUnion(float point1, float point2, float strength) {
        float temp = clamp(.5 + .5 * (point2 - point1) / strength, 0., 1.);
        return lerp(point2, point1, temp) - strength * temp * (1. - temp);
    }

    // List of properties to control your post process effect
    float _Intensity;
    float3 _RainDrops[20];
    TEXTURE2D_X(_MainTex);

    float3 CalculateNormal(in float2 r, in float2 uv) {
        float3 normal;

        float2 point1 = (r - hash2(r - (length(r.xx) * .12))) - uv;
        float2 point2 = (r + hash2(r + (length(r.xy) * .15))) - uv;
        float2 point3 = (r - hash2(r + (length(r.yy) * .09))) - uv;
        float2 point4 = (r + hash2(r - (length(r.yx) * .13))) - uv;

        float normal1 = SmoothUnion(abs(length(point1)), abs(length(point2)), .5);
        float normal2 = SmoothUnion(abs(length(point3)), abs(length(point4)), .5);

        normal.xy = SmoothUnion(normal1, normal2, .5) * (r - uv);

        normal.z = cos(asin(length(normal.xy)));

        if (length(normal.xy) > 1.0) {
            normal = float3(0, 0, 0);
        }
        return normal;
    }

    float4 CustomPostProcess(Varyings input) : SV_Target
    {
        UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

        // Note that if HDUtils.DrawFullScreen is not used to render the post process, you don't need to call ClampAndScaleUVForBilinearPostProcessTexture.
        
        float2 uv = ((2. * input.positionCS - _ScreenParams.xy) / min(_ScreenParams.x, _ScreenParams.y)) * 15.;

        //float3 sourceColor = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, ClampAndScaleUVForBilinearPostProcessTexture(input.texcoord.xy)).xyz;

        float3 color;
        for (int i = 0; i < 20; i++) {
            color += CalculateNormal(_RainDrops[i].xy, uv) * _RainDrops[i].z;
        }

        uv = input.texcoord;
        uv -= refract(float3(0, 0, -1), color, 1. / 1.3).xy * .1;

        float4 view = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, uv);
        float4 scene = SAMPLE_TEXTURE2D_X(_MainTex, s_linear_clamp_sampler, input.texcoord);
        view.xyz += clamp(dot(color, float3(.2, .4, .3)) * .1, 0., .4);

        view = lerp(scene, view, .2);
        
        return view;
    }

    ENDHLSL

    SubShader
    {
        Tags{ "RenderPipeline" = "HDRenderPipeline" }
        Pass
        {
            Name "Rain Post Process"

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
