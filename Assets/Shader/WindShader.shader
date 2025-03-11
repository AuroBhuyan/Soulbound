Shader "Unlit/WindShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _WindStrength ("Wind Strength", Float) = 0.2
        _WindSpeed ("Wind Speed", Float) = 1.0
        _WindDirection ("Wind Direction", Vector) = (1, 0, 0, 0)
        _Cutoff ("Alpha Cutoff", Range(0, 1)) = 0.5  // NEW
    }
    SubShader
    {
        Tags { "RenderType"="TransparentCutout" }  // Changed for transparency
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma target 3.0  // Required for discard function

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _WindStrength;
            float _WindSpeed;
            float4 _WindDirection;
            float _Cutoff;  // NEW

            v2f vert (appdata v)
            {
                v2f o;

                // Wind Effect Calculation
                float wave = sin(_Time.y * _WindSpeed + v.vertex.x * 0.5) * _WindStrength;
                v.vertex.x += wave * _WindDirection.x;
                v.vertex.z += wave * _WindDirection.z;

                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                
                // Alpha Clipping for cleaner edges
                if (col.a < _Cutoff) discard;

                UNITY_APPLY_FOG(i.fogCoord, col);
                return col;
            }
            ENDCG
        }
    }
}
