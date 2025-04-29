Shader "VFXHidden/Tornado"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _CameraRotation ("Camera Rotation", Vector) = (1., 1., 1.)
        _Resolution ("Screen Resolution", Vector) = (1., 1., 1.)
        _MainLightDirection ("Light Direction", Vector) = (1., 1., 1.)
        _MainLightColor ("Light Color", Vector) = (1., 1., 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
        Cull Off
        ZWrite On
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #include "AutoLight.cginc"

            #define TAU 6.283185307

            #define MAX_DISTANCE 250.
            #define MAX_STEPS 200.
            #define EPSILON .001

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            struct CappedCone {
                float3 Position;
                float Radius;
                float Height;
                float3 Color;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float3 _CameraRotation;
            float3 _Resolution;
            float3 _MainLightDirection;
            float4 _MainLightColor;

            float3x3 GetRotationX(float angle) {
                float c = cos(angle);
                float s = sin(angle);
                return float3x3(
                    1, 0, 0,
                    0, c, -s,
                    0, s, c
                );
            }

            float3x3 GetRotationY(float angle) {
                float c = cos(angle);
                float s = sin(angle);
                return float3x3(
                    c, 0,  s,
                    0, 1,  0,
                   -s, 0,  c
                );
            }

            float3x3 GetRotationZ(float angle) {
                float c = cos(angle);
                float s = sin(angle);
                return float3x3(
                    c, -s, 0,
                    s,  c, 0,
                    0,  0, 1
                );
            }

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.worldPos = UnityObjectToWorldDir(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            float CappedConeSDF(float3 thePoint, float3 theCenter, float radius, float height) {
                float distance = length(theCenter.xz - thePoint.xz) - ((thePoint.y - theCenter.y) * radius);
                distance = max(distance, length(thePoint - theCenter) - height);
                return distance;
            }

            float Scene(float3 thePoint) {
                CappedCone cone;
                cone.Position = float3(0, 0, 0);
                cone.Radius = .5;
                cone.Height = 30.;

                float distance = CappedConeSDF(thePoint, cone.Position, cone.Radius, cone.Height);

                return distance;
            }

            float3 calcNormal(in float3 thePoint) {
                const float2 h = float2(.00001, 0);
                return normalize(
                    float3(
                        Scene(thePoint + h.xyy) - Scene(thePoint - h.xyy),
                        Scene(thePoint + h.yxy) - Scene(thePoint - h.yxyx),
                        Scene(thePoint + h.yyx) - Scene(thePoint - h.yyx)
                    )
                );
            } 

            fixed4 frag (v2f i) : SV_Target
            {
                float2 uv = (2. * i.vertex.xy - _Resolution.xy) / min(_Resolution.x, _Resolution.y);

                
                uv *= .6;

                float3 color = float3(0, 0, 0);
                float distance = 0.;
                float calcDist = 0.;
                float3 rayOrigin = _WorldSpaceCameraPos;
                float3 normalizedCamera = _CameraRotation;
                float3x3 rotation = mul(mul(GetRotationY(normalizedCamera.y), GetRotationZ(normalizedCamera.z)), GetRotationX(normalizedCamera.x));
                float3 rayDirection = normalize(mul(rotation, float3(uv, 1.)));

                for (float i = 0.; i < MAX_STEPS; i++) {

                    float3 Point = rayOrigin + rayDirection * distance;

                    calcDist = Scene(Point);
                    float3 normal = calcNormal(Point);

                    color += clamp(-calcDist, -.01, 1.) * max(dot(calcNormal(Point), -_MainLightDirection), 0.1) * .01;

                    distance += max(calcDist, EPSILON + .01);
                }
                // sample the texture
                fixed4 col = float4(color, length(color));

                if (col.a < .1) {
                    discard;
                }
                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}