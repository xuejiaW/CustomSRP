using UnityEngine;
using UnityEngine.Rendering;

public class Lighting
{
    private const string k_BufferName = "Lighting";
    private CommandBuffer m_Buffer = new() {name = k_BufferName};

    private static int s_DirLightColorId = Shader.PropertyToID("_DirectionalLightColor");
    private static int s_DirLightDirectionId = Shader.PropertyToID("_DirectionalLightDirection");

    public void Setup(ScriptableRenderContext context)
    {
        m_Buffer.BeginSample(k_BufferName);
        SetDirectionalLight();
        m_Buffer.EndSample(k_BufferName);
        context.ExecuteCommandBuffer(m_Buffer);
        m_Buffer.Clear();
    }

    private void SetDirectionalLight()
    {
        // Scene's main directional light
        Light light = RenderSettings.sun;
        m_Buffer.SetGlobalVector(s_DirLightColorId, light.color.linear * light.intensity);
        m_Buffer.SetGlobalVector(s_DirLightDirectionId, -light.transform.forward);
    }
}