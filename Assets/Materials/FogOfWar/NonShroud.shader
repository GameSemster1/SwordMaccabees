Shader "Custom/NonShroud"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LastTex ("Last Frame's Texture", 2D) = "white" {}
        _MaskTex("Mask Texture",2D) = "white" {}
        _TintColor("Tint Color",Color) = (0,0,0,0)
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MaskTex;
            sampler2D _MainTex;
            sampler2D _LastTex;

            fixed4 _TintColor;

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                fixed4 lastCol = tex2D(_LastTex, i.uv);

                fixed4 mask = tex2D(_MaskTex, i.uv);

                // col.a = max(0, 1 - mask.a);

                if (mask.a >= 0.9)
                {
                    lastCol = col;

                    lastCol.r = lerp(lastCol.r, _TintColor.r, _TintColor.a);
                    lastCol.g = lerp(lastCol.g, _TintColor.g, _TintColor.a);
                    lastCol.b = lerp(lastCol.b, _TintColor.b, _TintColor.a);
                }

                return lastCol;
            }
            ENDCG
        }
    }
}