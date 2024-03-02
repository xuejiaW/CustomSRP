#ifndef CUSTOM_UNLIT_PASS_INCLUDED

#define CUSTOM_UNLIT_PASS_INCLUDED

#include "../Custom RP/ShaderLibrary/Common.hlsl"

CBUFFER_START(UnityPerMaterial)
    float4 m_BaseColor;
CBUFFER_END

float4 UnlitPassVertex(float3 positionOS: POSITION) : SV_POSITION {
    float3 positionWS = TransformObjectToWorld(positionOS.xyz);
    return TransformWorldToHClip(positionWS);
}

float4 UnlitPassFragment():SV_TARGET {
    return m_BaseColor;
}

#endif
