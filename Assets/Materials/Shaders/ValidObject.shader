Shader "Custom/Valid"
{
    Properties
    {
        _ObjectColor("Object Color", Color) = (1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM

        #pragma surface surf Lambert
        #pragma target 3.5

        struct Input {
            float2 uv_MainTex;
        };
            
        fixed3 _ObjectColor;

        void surf(Input IN, inout SurfaceOutput o) {
            o.Albedo = _ObjectColor;
        }

        ENDCG
    }
    Fallback "Diffuse"
}
