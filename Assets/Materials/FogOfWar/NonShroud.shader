Shader "Custom/NonShroud"
{
    Properties
    {
        _MainTex("Main Texture",2D) = "white"{}
        _MaskTex("Mask Texture",2D) = "white"{}
        _TintColor("Tint Color",Color) = (0,0,0,0)
    }

    CGINCLUDE
    #include "UnityCustomRenderTexture.cginc"

    sampler2D _MaskTex;
    sampler2D _MainTex;

    fixed4 _TintColor;


    half4 frag(v2f_customrendertexture i) : SV_Target
    {
        float2 uv = i.globalTexcoord;

        half4 col = tex2D(_MainTex, uv);
        half4 lastCol = tex2D(_SelfTexture2D, uv);
        fixed4 mask = tex2D(_MaskTex, uv);

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

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "Update"
            CGPROGRAM
            #pragma vertex CustomRenderTextureVertexShader
            #pragma fragment frag
            ENDCG
        }
    }
}