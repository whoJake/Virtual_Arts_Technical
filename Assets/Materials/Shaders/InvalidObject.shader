Shader "Custom/Invalid"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white"{}
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
            float2 uv;  //INTERNAL
        };
            
        sampler2D _MainTex;
        fixed4 _FlashColor;
        float _FlashPeriod;

        void surf(Input IN, inout SurfaceOutput o) {

            float lerpval = (sin(_Time.y / _FlashPeriod) + 1) / 2;
            fixed4 texCol = tex2D(_MainTex, IN.uv);

            o.Albedo = lerp(texCol, _FlashColor, lerpval);
        }

        ENDCG
    }
    Fallback "Diffuse"
}
