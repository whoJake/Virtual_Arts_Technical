Shader "Custom/FresnelInvalid"
{
    Properties
    {
        _ObjectColor("Object Color", Color) = (1, 1, 1)
        _FlashColor("Flash Color", Color) = (1, 0, 0)
        _FlashPeriod("Flash Period", Float) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM

        #pragma surface surf Lambert
        #pragma target 3.5

        struct Input {
            float3 viewDir;
            float3 worldNormal;
        };
            
        fixed3 _ObjectColor;
        fixed3 _FlashColor;
        float _FlashPeriod;

        void surf(Input IN, inout SurfaceOutput o) {
            float fresnel = dot(IN.viewDir, IN.worldNormal);
            if (fresnel < 0.6) fresnel = 0;
            else fresnel = 1;
            
            fixed3 flashColor = lerp(_FlashColor, _ObjectColor, fresnel);

            float lerpval = (sin(_Time.y / _FlashPeriod) + 1) / 2;
            o.Albedo = lerp(_ObjectColor, flashColor, lerpval);
        }

        ENDCG
    }
    Fallback "Diffuse"
}
