#ifndef CUSTOM_LIT_PASS_INCLUDED

#define CUSTOM_LIT_PASS_INCLUDED

#include "../Custom RP/ShaderLibrary/Common.hlsl"

TEXTURE2D(m_BaseMap);
SAMPLER(sampler_m_BaseMap);

UNITY_INSTANCING_BUFFER_START(UnityPerMaterial)
    UNITY_DEFINE_INSTANCED_PROP(float4, m_BaseColor)
    UNITY_DEFINE_INSTANCED_PROP(float4, m_BaseMap_ST)
    UNITY_DEFINE_INSTANCED_PROP(float, m_Cutoff)
UNITY_INSTANCING_BUFFER_END(UnityPerMaterial)

struct Attributes {
    float3 positionOS: POSITION;
    float3 normalOS: NORMAL;
    float2 baseUV : TEXCOORD0;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

struct Varyings {
    float4 positionCS : SV_POSITION;
    float3 normalWS: VAR_NORMAL;
    float2 baseUV : VAR_BASE_UV;
    UNITY_VERTEX_INPUT_INSTANCE_ID
};

Varyings LitPassVertex(Attributes input) {
    Varyings output;
    UNITY_SETUP_INSTANCE_ID(input)
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    float3 positionWS = TransformObjectToWorld(input.positionOS.xyz);
    output.positionCS = TransformWorldToHClip(positionWS);
    output.normalWS = TransformObjectToWorldNormal(input.normalOS);

    float4 baseST = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, m_BaseMap_ST);
    output.baseUV = input.baseUV * baseST.xy + baseST.zw;

    return output;
}

float4 LitPassFragment(Varyings input) : SV_TARGET {
    UNITY_SETUP_INSTANCE_ID(input)

    float4 baseMap = SAMPLE_TEXTURE2D(m_BaseMap, sampler_m_BaseMap, input.baseUV);
    float4 color = UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, m_BaseColor);
    float4 base = baseMap * color;
    #if defined(_CLIPPING)
        clip(base.a - UNITY_ACCESS_INSTANCED_PROP(UnityPerMaterial, m_Cutoff));
    #endif
    base.rgb = abs(length(input.normalWS) - 1.0) * 10.0;
    return base;
}

#endif
