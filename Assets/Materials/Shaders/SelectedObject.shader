Shader "Custom/Selected"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _ObjectColor("Object Color", Color) = (1, 1, 1)
        _SelectedColor("Selected Color", Color) = (1, 0.5, 0)
    }
        SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        CGPROGRAM

        #pragma surface surf Lambert
        #pragma target 3.5

        struct Input {
            float2 uv_MainTex;
        };

        sampler2D _MainTex;
        fixed3 _ObjectColor;
        fixed3 _SelectedColor;

        void surf(Input IN, inout SurfaceOutput o) {
            float texLerp = tex2D(_MainTex, IN.uv_MainTex);
            
            o.Albedo = lerp(_SelectedColor, _ObjectColor, texLerp);
        }

        ENDCG
    }
        Fallback "Diffuse"
}
