Shader "Custom RP/UnlitTextureCutoff"
{

    Properties
    {
        m_BaseMap("Texture", 2D) = "white" {}
        m_BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        m_Cutoff("Alpha Cutoff", Range(0.0, 1.0)) = 0.5
        [Toggle(_CLIPPING)] m_Clipping ("Alpha Clipping", Float) = 0

        [Enum(UnityEngine.Rendering.BlendMode)] m_SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] m_DstBlend("Dst Blend", Float) = 0
        [Enum(Off,0,On,1)] m_ZWrite ("Z Write", Float) = 1
    }

    SubShader
    {
        Pass
        {
            Blend [m_SrcBlend] [m_DstBlend]
            ZWrite [m_ZWrite]

            HLSLPROGRAM
            #pragma shader_feature _CLIPPING
            #pragma  multi_compile_instancing
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #include "UnlitTransparentTextureCutoffPass.hlsl"
            ENDHLSL
        }
    }
}