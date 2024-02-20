Shader "Custom/ground"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        #pragma surface surf _NoLight
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
        };

        fixed4 _Color;

        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Emission = c.rgb;
            o.Alpha = c.a;
        }
        float4 Lighting_NoLight(SurfaceOutput s, float3 lightDir, float atten){
            return float4(0,0,0,1);
        }

        ENDCG
    }
    FallBack "Diffuse"
}
