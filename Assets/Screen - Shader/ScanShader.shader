Shader "Custom/NeoMatrixScanEffect"
{
    Properties
    {
        _Color ("Base Color", Color) = (0,0,0,1) // dark blue
        _MainTex ("Texture", 2D) = "white" {}
        _ScanSpeed ("Scan Speed", Float) = 1
        _ScanWidth ("Scan Width", Float) = 0.05 // Grid Effect
        _GridSize ("Grid Size", Float) = 10 // GridSize
    }
    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;          
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            fixed4 _Color; // Use _Color instead of Color
            float _ScanSpeed;
            float _ScanWidth;
            float _GridSize;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Use built-in _Time variable provided by Unity
                float scanX = abs(frac(_Time.y * _ScanSpeed + i.uv.x * _GridSize) - 0.5) / _ScanWidth;
                float scanY = abs(frac(_Time.y * _ScanSpeed + i.uv.y * _GridSize) - 0.5) / _ScanWidth;
                
                // Combine the two scan effects
                float scan = min(scanX, scanY);

                // Set the color to dark blue and overlay the scan effect
                fixed4 texColor = tex2D(_MainTex, i.uv);
                fixed4 baseColor = fixed4(0.0, 0.0, 0.5, 1.0); // Dark blue color
                fixed4 scanColor = fixed4(0.0, 1.0, 1.0, 1.0); // Cyan color for lines
                
                // Return the final color
                return lerp(baseColor, scanColor, saturate(1 - scan));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
