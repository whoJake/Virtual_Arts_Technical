Shader "Custom/Fresnel"
{
    Properties
    {
        _ObjectColor("Object Color", Color) = (1, 1, 1)
        _SelectionColor("Selection Color", Color) = (0, 0, 1)
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
        fixed3 _SelectionColor;

        void surf(Input IN, inout SurfaceOutput o) {
            float fresnel = dot(IN.viewDir, IN.worldNormal);
            if (fresnel < 0.4) fresnel = 0;
            else fresnel = 1;

            o.Albedo = lerp(_SelectionColor, _ObjectColor, fresnel);
        }

        ENDCG
    }
    Fallback "Diffuse"
}
