using UnityEngine;
using UnityEngine.Rendering;

public class CameraRenderer
{
    private ScriptableRenderContext m_RenderContext = default;
    private Camera m_Camera = null;

    private const string k_BufferName = "Render Camera";
    private CommandBuffer m_Buffer = new() {name = k_BufferName};
    private CullingResults m_CullingResults = default;

    private static ShaderTagId s_UnlitShaderTagId = new("SRPDefaultUnlit");

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

    private void DrawVisibleGeometry()
    {
        var sortingSettings = new SortingSettings(m_Camera) {criteria = SortingCriteria.CommonOpaque};
        var drawingSettings = new DrawingSettings(s_UnlitShaderTagId, sortingSettings);
        var filteringSettings = new FilteringSettings(RenderQueueRange.opaque);
        m_RenderContext.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);

        m_RenderContext.DrawSkybox(m_Camera);

        sortingSettings.criteria = SortingCriteria.CommonTransparent;
        drawingSettings.sortingSettings = sortingSettings;
        filteringSettings.renderQueueRange = RenderQueueRange.transparent;
        m_RenderContext.DrawRenderers(m_CullingResults, ref drawingSettings, ref filteringSettings);
    }

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