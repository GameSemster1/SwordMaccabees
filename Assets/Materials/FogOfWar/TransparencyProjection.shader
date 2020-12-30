Shader "Custom/TransparencyProjection"
{
    Properties
    {
        _PrevTexture ("Previous Texture", 2D) = "white" {}
        _CurrTexture ("Current Texture", 2D) = "white" {}

        _MainTexture ("Main Texture",2D) = "white"{}
        _FakeMainTexture ("Fake Main Texture",2D) = "white"{}

        _Color ("Color", Color) = (0, 0, 0, 0)
        //        _Alpha ("Alpha", float) = 0
        _Blend("Blend", Float) = 0
    }
    SubShader
    {
        Tags
        {
            "Queue"="Transparent+100"
        } // to cover other transparent non-z-write things

        Pass
        {
            ZWrite Off
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Equal

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float4 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };


            float4x4 unity_Projector;
            sampler2D _CurrTexture;
            sampler2D _PrevTexture;
            fixed4 _Color;
            float _Blend;

            sampler2D _MainTexture;
            sampler2D _FakeMainTexture;

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = mul(unity_Projector, v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float aPrev = tex2Dproj(_PrevTexture, i.uv).a;
                float aCurr = tex2Dproj(_CurrTexture, i.uv).a;

                fixed4 col = tex2Dproj(_MainTexture, i.uv);


                fixed a = lerp(aPrev, aCurr, _Blend);

                // col = col * (_Color);

                col.r = lerp(col.r, _Color.r, _Color.a);
                col.g = lerp(col.g, _Color.g, _Color.a);
                col.b = lerp(col.b, _Color.b, _Color.a);

                // col.r = min(col.r + _Color.r * _Color.a, 1);
                // col.g = min(col.g + _Color.g * _Color.a, 1);
                // col.b = min(col.b + _Color.b * _Color.a, 1);

                // weird things happen to minimap if alpha value gets negative
                col.a = max(0, 1 - a);
                return col;
            }
            ENDCG
        }
    }
}