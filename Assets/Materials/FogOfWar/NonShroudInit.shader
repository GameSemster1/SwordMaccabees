Shader "Custom/NonShroudInit"
{
    CGINCLUDE
    #include "UnityCustomRenderTexture.cginc"
    #pragma target 3.5

    half4 frag(v2f_init_customrendertexture i) : SV_Target
    {
        return half4(0, 0, 0, 0);
    }
    ENDCG

    SubShader
    {
        Cull Off ZWrite Off ZTest Always
        Pass
        {
            Name "Init"
            CGPROGRAM
            #pragma vertex InitCustomRenderTextureVertexShader
            #pragma fragment frag
            ENDCG
        }
    }
}