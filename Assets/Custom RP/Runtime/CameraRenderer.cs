using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private ScriptableRenderContext m_RenderContext = default;
    private Camera m_Camera = null;

    private const string k_BufferName = "Render Camera";
    private CommandBuffer m_Buffer = new() {name = k_BufferName};
    private CullingResults m_CullingResults = default;

    public void Render(ScriptableRenderContext renderContext, Camera camera)
    {
        m_RenderContext = renderContext;
        m_Camera = camera;

        if (!Cull()) return;

        Setup();
        DrawVisibleGeometry();
        Submit();
    }

    private void Setup()
    {
        m_RenderContext.SetupCameraProperties(m_Camera);
        m_Buffer.ClearRenderTarget(true, true, Color.clear);

        m_Buffer.BeginSample(k_BufferName);
        ExecuteCommandBuffer();
    }

    private void DrawVisibleGeometry() { m_RenderContext.DrawSkybox(m_Camera); }

    private void Submit()
    {
        m_Buffer.EndSample(k_BufferName);
        ExecuteCommandBuffer();

        m_RenderContext.Submit();
    }

    private void ExecuteCommandBuffer()
    {
        m_RenderContext.ExecuteCommandBuffer(m_Buffer);
        m_Buffer.Clear();
    }

    private bool Cull()
    {
        if (!m_Camera.TryGetCullingParameters(out ScriptableCullingParameters cullingParameters)) return false;

        m_CullingResults = m_RenderContext.Cull(ref cullingParameters);
        return true;
    }
}