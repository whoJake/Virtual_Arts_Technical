Shader "Custom/Invalid"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"{}
        _ObjectColor("Object Color", Color) = (1, 1, 1)
        _FlashColor("Flash Color", Color) = (1, 0, 0)
        _FlashPeriod ("Flash Period", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        CGPROGRAM

        #pragma surface surf Lambert
        #pragma target 3.5

        struct Input {
            float2 uv_MainTex;  //INTERNAL
        };
            
        sampler2D _MainTex;
        fixed3 _ObjectColor;
        fixed3 _FlashColor;
        float _FlashPeriod;

        void surf(Input IN, inout SurfaceOutput o) {
            float texLerp = tex2D(_MainTex, IN.uv_MainTex).r;
            if (texLerp == 1) {
                o.Albedo = _ObjectColor;
                return;
            }

            float lerpval = (sin(_Time.y / _FlashPeriod) + 1) / 2;
            o.Albedo = lerp(_ObjectColor, _FlashColor, lerpval);
        }

        ENDCG
    }
    Fallback "Diffuse"
}
