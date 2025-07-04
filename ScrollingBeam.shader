Shader "Custom/ScrollingBeam"
{
    Properties
    {
        _MainTex("Beam Texture", 2D) = "white" {}
        _ScrollSpeed("Scroll Speed", Float) = 1
        _TintColor("Tint Color", Color) = (0, 1, 1, 1)
    }

    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        LOD 100

        Blend One One
        ZWrite Off
        Cull Off

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _ScrollSpeed;
            float4 _TintColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.position = TransformObjectToHClip(v.vertex.xyz);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            float4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;
                uv.y += _Time.y * _ScrollSpeed;

                float4 tex = tex2D(_MainTex, uv);
                return tex * _TintColor;
            }

            ENDHLSL
        }
    }

    FallBack "Unlit/Transparent"
}
