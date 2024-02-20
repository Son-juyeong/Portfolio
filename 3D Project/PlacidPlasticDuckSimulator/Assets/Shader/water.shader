Shader "Custom/water"
{
    Properties
    {
        _Color ("Water Color", Color) = (1,1,1,1)
        _SpecCol ("Specular Color", Color) = (1,1,1,1)
        _BumpMap ("Normal Map", 2D)="bump"{}
        _NormalMapSpeed("NormalMap Speed", Range(0, 10)) = 1
        _WaveSpeed("Wave Speed", Range(0, 10)) = 1
        _Amplitude("Amplitude", Range(0, 50))=1
        _SpecInten ("Specular Intencity", Range(0, 100)) = 1
        _SpecPow ("Specular Power", Range(0, 1)) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        GrabPass{}

        CGPROGRAM
        #pragma surface surf _WaterShader vertex:vert

        #pragma target 3.0

        sampler2D _GrabTexture;
        sampler2D _BumpMap;

        fixed4 _Color;
        fixed4 _SpecCol;
        float _NormalMapSpeed;
        float _WaveSpeed;
        float _Amplitude;
        float _SpecInten;
        float _SpecPow;

        void vert(inout appdata_full v){
            float moveY1 = sin(v.vertex.x+_Time.y*_WaveSpeed)*_Amplitude*0.02;
            float moveY2 = sin(v.vertex.z+_Time.y*_WaveSpeed)*_Amplitude*0.02;

            float moveY3 = sin((v.vertex.x+_Time.y*_WaveSpeed)*1.5)*_Amplitude*0.02;
            float moveY4 = sin((v.vertex.z+_Time.y*_WaveSpeed)*1.5)*_Amplitude*0.02;

            float moveY5 = sin((v.vertex.x+_Time.y*_WaveSpeed)*(-0.5))*_Amplitude*0.02;
            float moveY6 = sin((v.vertex.z+_Time.y*_WaveSpeed)*(-0.5))*_Amplitude*0.02;

            v.vertex.y+=(moveY1+moveY2+moveY3+moveY4+moveY5+moveY6)/6;   
        }

        struct Input
        {
            float3 viewDir;
            float4 screenPos;
            float2 uv_BumpMap;
        };
        
        void surf (Input IN, inout SurfaceOutput o)
        {
            fixed4 c = _Color;
            float2 uvBump = float2(IN.uv_BumpMap.x*0.9,IN.uv_BumpMap.y*0.2);
            float3 x1 = UnpackNormal(tex2D(_BumpMap, float2(uvBump.x+_Time.x*_NormalMapSpeed, uvBump.y)));
            float3 x2 = UnpackNormal(tex2D(_BumpMap, float2(uvBump.x-_Time.x*_NormalMapSpeed, uvBump.y)));
            float3 y1 = UnpackNormal(tex2D(_BumpMap, float2(uvBump.x, uvBump.y+_Time.x*_NormalMapSpeed)));
            float3 y2 = UnpackNormal(tex2D(_BumpMap, float2(uvBump.x, uvBump.y-_Time.x*_NormalMapSpeed)));
            float3 normal = (x1+x2+y1+y2)/4;
            o.Normal = normal;
            float rim = saturate(dot(o.Normal, IN.viewDir));
            float lerpRim=pow((1-rim)*0.5, 2);
            float3 scrPos = IN.screenPos.xyz/(IN.screenPos.w+0.00001f);
            float4 grab = tex2D(_GrabTexture, scrPos.xy);
            o.Emission = lerp(grab.rgb, c.rgb, lerpRim+0.2);
            o.Emission+=saturate(pow(rim, 15)-0.1);
            //o.Emission = rim+0.3;
        }

        float4 Lighting_WaterShader(SurfaceOutput s, float3 lightDir, float3 viewDir, float atten){
            float4 result = (0,0,0,1);
            float3 h = normalize(lightDir+viewDir);
            float3 spec = saturate(dot(h, s.Normal));
            spec = pow(spec, 50*_SpecInten)*_SpecCol;
            result.rgb = spec*_SpecPow;
            return result;
            //return float4(0,0,0,1);
        }
        ENDCG
    }
    FallBack "Diffuse"            
}
