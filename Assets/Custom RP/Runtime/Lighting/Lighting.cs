using Unity.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string k_BufferName = "Lighting";
    private CommandBuffer m_Buffer = new() {name = k_BufferName};
    private CullingResults m_CullingResults = default;

    private const int k_MaxDirLightCount = 4;

    private static int s_DirLightCountId = Shader.PropertyToID("_DirectionalLightCount");
    private static int s_DirLightColorId = Shader.PropertyToID("_DirectionalLightColors");
    private static int s_DirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirections");

    private static Vector4[] s_DirLightColors = new Vector4[k_MaxDirLightCount];
    private static Vector4[] s_DirLightDirections = new Vector4[k_MaxDirLightCount];

    public void Setup(ScriptableRenderContext context, CullingResults cullingResult)
    {
        m_CullingResults = cullingResult;
        m_Buffer.BeginSample(k_BufferName);
        SetupLights();
        m_Buffer.EndSample(k_BufferName);
        context.ExecuteCommandBuffer(m_Buffer);
        m_Buffer.Clear();
    }

    private void SetupLights()
    {
        NativeArray<VisibleLight> visibleLights = m_CullingResults.visibleLights;
        int setDirLightCount = 0;
        for (int i = 0; i != visibleLights.Length; ++i)
        {
            VisibleLight visibleLight = visibleLights[i];
            if (visibleLight.lightType != LightType.Directional) continue;

            SetDirectionalLight(setDirLightCount++, visibleLight);

            if (setDirLightCount >= k_MaxDirLightCount) break;
        }

        m_Buffer.SetGlobalInt(s_DirLightCountId, visibleLights.Length);
        m_Buffer.SetGlobalVectorArray(s_DirLightColorId, s_DirLightColors);
        m_Buffer.SetGlobalVectorArray(s_DirLightDirectionId, s_DirLightDirections);
    }

    private void SetDirectionalLight(int index, in VisibleLight visibleLight)
    {
        s_DirLightColors[index] = visibleLight.finalColor;
        s_DirLightDirections[index] = -visibleLight.localToWorldMatrix.GetColumn(2);
    }
}