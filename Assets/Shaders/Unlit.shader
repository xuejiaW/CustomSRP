Shader "Custom RP/Unlit"
{

    Properties
    {
        m_BaseColor("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        [Enum(UnityEngine.Rendering.BlendMode)] m_SrcBlend("Src Blend", Float) = 1
        [Enum(UnityEngine.Rendering.BlendMode)] m_DstBlend("Dst Blend", Float) = 0
    }

    SubShader
    {
        Pass
        {
            Blend [m_SrcBlend] [m_DstBlend]
            HLSLPROGRAM
            #pragma  multi_compile_instancing
            #pragma vertex UnlitPassVertex
            #pragma fragment UnlitPassFragment
            #include "UnlitPass.hlsl"
            ENDHLSL
        }
    }
}