Shader "Custom/SanityShade"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _VignetteIntensity ("Vignette Intensity", Range(0, 1)) = 0.5
        _GrainIntensity ("Grain Intensity", Range(0, 1)) = 0.0
        _MaskRadius ("Mask Radius", Range(0, 1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" "IgnoreProjector" = "True" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _VignetteIntensity;
            float _GrainIntensity;
            float _MaskRadius;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 pos : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float randomNoise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float2 uv = i.uv;

                // Vignette effect
                float dist = distance(uv, float2(0.5, 0.5));
                float vignette = 1.0 - smoothstep(0.25, 0.7, dist + _VignetteIntensity);

                // Grain effect
                float grain = _GrainIntensity * (randomNoise(uv) - 0.5);

                // Mask effect to reveal only the center at low sanity
                float mask = smoothstep(_MaskRadius, 0.0, dist);

                // Sample the main texture
                fixed4 col = tex2D(_MainTex, uv);

                // Apply vignette, grain, and mask effects
                col.rgb = col.rgb * vignette + grain;
                col.rgb *= mask;

                // Ensure transparency is applied for overlay
                col.a = vignette * mask;

                return col;
            }
            ENDCG
        }
    }
}
