Shader "Custom/ThermalVisionShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _HeatMap ("Heat Map", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            sampler2D _MainTex;
            sampler2D _HeatMap;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uvHeat : TEXCOORD1;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.uvHeat = v.uv;
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                half4 mainTex = tex2D(_MainTex, i.uv);
                half4 heatTex = tex2D(_HeatMap, i.uvHeat);

                float temperature = heatTex.r; // Assume red channel represents temperature
                float3 thermalColor = lerp(float3(0, 0, 1), float3(1, 0, 0), temperature);

                return half4(thermalColor, 1.0);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
